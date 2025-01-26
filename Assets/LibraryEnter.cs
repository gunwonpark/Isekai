using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class LibraryEnter : MonoBehaviour
{
	public float zoomDuration = 2f;      
	public float targetZoom = 4f;        

	public float delayBeforeLoad = 2f;  

	private Camera mainCamera;
	private float originalZoom;

	private Transform playerTransform;

	private void Start()
	{
		mainCamera = Camera.main;
		if (mainCamera != null)
		{
			mainCamera.orthographic = true;

			originalZoom = mainCamera.orthographicSize;
		}
		else
		{
			Debug.LogError("메인 카메라를 찾을 수 없습니다!");
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			playerTransform = collision.transform;
			StartCoroutine(HandleSceneTransition());
		}
	}

	private IEnumerator HandleSceneTransition()
	{
		if (mainCamera == null || playerTransform == null) yield break;

		float elapsed = 0f;

		PixelPerfectCamera pixelPerfectCamera = mainCamera.GetComponent<PixelPerfectCamera>();
		pixelPerfectCamera.enabled = false;

		Vector3 startPos = mainCamera.transform.position;

		float offsetX = 0f;
		float offsetY = 1f;

		Vector3 endPos = new Vector3(
			playerTransform.position.x + offsetX,
			playerTransform.position.y + offsetY,
			startPos.z
		);

		float startZoom = originalZoom;

		while (elapsed < zoomDuration)
		{
			float t = elapsed / zoomDuration;

			mainCamera.transform.position = Vector3.Lerp(startPos, endPos, t);

			mainCamera.orthographicSize = Mathf.Lerp(startZoom, targetZoom, t);

			Debug.Log($"[Zooming] Elapsed: {elapsed:F2}, Size: {mainCamera.orthographicSize:F2}");

			elapsed += Time.deltaTime;
			yield return null;
		}

		mainCamera.transform.position = endPos;
		mainCamera.orthographicSize = targetZoom;

		yield return new WaitForSeconds(delayBeforeLoad);

		Managers.Scene.LoadScene(Scene.LibraryScene);
	}
}
