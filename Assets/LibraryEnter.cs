using System.Collections;
using UnityEngine;

public class LibraryEnter : MonoBehaviour
{
	// 줌인 설정 (OrthographicSize 사용)
	public float zoomDuration = 1f;      // 줌인 애니메이션 지속 시간
	public float targetZoom = 3f;        // 목표 Orthographic Size (작을수록 많이 줌인)

	// 씬 전환 지연 시간
	public float delayBeforeLoad = 2f;   // 씬 전환 전 대기 시간

	private Camera mainCamera;
	private float originalZoom;

	// 플레이어 위치를 받아오기 위한 변수
	private Transform playerTransform;

	// 카메라 오프셋
	public float offsetX = 2f;
	public float offsetY = 1000000f;  // 새로 추가한 y축 오프셋

	private void Start()
	{
		mainCamera = Camera.main;
		if (mainCamera != null)
		{
			// 혹시 카메라가 Perspective 모드라면 Orthographic 모드로 전환
			mainCamera.orthographic = true;

			// 기존 orthographicSize 값 저장
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
			// 트리거에 들어온 Collider를 통해 플레이어 transform 확보
			playerTransform = collision.transform;
			StartCoroutine(HandleSceneTransition());
		}
	}

	private IEnumerator HandleSceneTransition()
	{
		if (mainCamera == null || playerTransform == null) yield break;

		float elapsed = 0f;

		// 카메라 시작 위치
		Vector3 startPos = mainCamera.transform.position;
		// 목표 위치: x축과 y축으로 각각 오프셋 적용
		Vector3 endPos = new Vector3(
			playerTransform.position.x + offsetX,
			playerTransform.position.y + offsetY,
			startPos.z
		);

		float startZoom = originalZoom;

		// 카메라 이동 및 줌인
		while (elapsed < zoomDuration)
		{
			float t = elapsed / zoomDuration;

			// 카메라 위치 이동
			mainCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
			// Orthographic Size 조절 (작아질수록 줌인)
			mainCamera.orthographicSize = Mathf.Lerp(startZoom, targetZoom, t);

			elapsed += Time.deltaTime;
			yield return null;
		}

		// 마지막 보정
		mainCamera.transform.position = endPos;
		mainCamera.orthographicSize = targetZoom;

		// 추가 지연 시간
		yield return new WaitForSeconds(delayBeforeLoad);

		// 씬 전환
		Managers.Scene.LoadScene(Scene.LibraryScene);

		// (선택 사항) 씬 전환 후 카메라 원래대로 복귀
		// elapsed = 0f;
		// while (elapsed < zoomDuration)
		// {
		//     float t = elapsed / zoomDuration;
		//     mainCamera.transform.position = Vector3.Lerp(endPos, startPos, t);
		//     mainCamera.orthographicSize = Mathf.Lerp(targetZoom, startZoom, t);
		//     elapsed += Time.deltaTime;
		//     yield return null;
		// }
		//
		// mainCamera.transform.position = startPos;
		// mainCamera.orthographicSize = startZoom;
	}
}
