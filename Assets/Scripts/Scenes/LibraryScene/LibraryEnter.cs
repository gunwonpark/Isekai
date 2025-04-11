using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Playables;

public class LibraryEnter : MonoBehaviour
{
	public float zoomDuration = 2f;
	public float targetZoom = 4f;
	public float delayBeforeLoad = 2f;

	public PlayableDirector timeline;
	private PlayerController playerController;

	private Camera mainCamera;
	private float originalZoom;

	// 트리거 충돌 시점의 플레이어 위치를 저장할 변수
	private Vector3 triggerPlayerPosition;

	private void Start()
	{
		mainCamera = Camera.main;
		playerController = FindAnyObjectByType<PlayerController>();
		timeline = FindAnyObjectByType<PlayableDirector>();
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
			// 트리거에 부딪힌 시점의 플레이어 위치 저장
			triggerPlayerPosition = collision.transform.position;
			playerController.isMoving = false;
			Debug.Log(triggerPlayerPosition);
			StartCoroutine(HandleSceneTransition());
		}
	}

	private IEnumerator HandleSceneTransition()
	{
		if (timeline != null)
		{
			timeline.Play();
			while (timeline.state == PlayState.Playing)
			{
				yield return null;
			}
		}
		else
		{
			Debug.LogWarning("타임라인 참조가 없습니다.");
		}

		PixelPerfectCamera pixelPerfectCamera = mainCamera.GetComponent<PixelPerfectCamera>();
		if (pixelPerfectCamera != null)
			pixelPerfectCamera.enabled = false;

		Vector3 startPos = mainCamera.transform.position;

		
		// 저장된 충돌 시점의 플레이어 위치를 기준으로 하드코딩
		Vector3 endPos = new Vector3(
			24.78f,
			-0.48f,
			-10f
			);

		float startZoom = originalZoom;
		float elapsed = 0f;

		while (elapsed < zoomDuration)
		{
			float t = elapsed / zoomDuration;
			mainCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
			mainCamera.orthographicSize = Mathf.Lerp(startZoom, targetZoom, t);
			elapsed += Time.deltaTime;
			yield return null;
		}

		mainCamera.transform.position = endPos;
		mainCamera.orthographicSize = targetZoom;

		yield return new WaitForSeconds(delayBeforeLoad);

		Managers.Scene.LoadScene(Scene.LibraryScene);
	}
}
