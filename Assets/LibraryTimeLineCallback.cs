using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class LibraryTimeLineCallback : MonoBehaviour
{
	[SerializeField] Material material;
	[SerializeField] MeshRenderer meshRenderer;
	[SerializeField] SpriteRenderer[] mouseRenderers;
	[SerializeField] GameObject[] books;

    PlayableDirector playableDirector;

	private void Awake()
	{
		playableDirector = GetComponent<PlayableDirector>();
	}

	private void OnEnable()
	{
		playableDirector.stopped += LibrayLightSwitch;
	}

	private void OnDisable()
	{
		playableDirector.stopped -= LibrayLightSwitch;
	}

    public void LibrayLightSwitch(PlayableDirector director)
    {
        meshRenderer.material = material;
		BookSwitch();
		StartCoroutine(FingerTwinkleCoroutine());
    }

	private IEnumerator FingerTwinkleCoroutine()
	{
		while (true)
		{
			mouseRenderers[0].enabled = !mouseRenderers[0].enabled;
			mouseRenderers[1].enabled = !mouseRenderers[1].enabled;
			mouseRenderers[2].enabled = !mouseRenderers[2].enabled;
			mouseRenderers[3].enabled = !mouseRenderers[3].enabled;

			yield return new WaitForSeconds(0.8f);
		}
	}

	private void BookSwitch()
	{
		WorldType currentWorldType = Managers.World.CurrentWorldType;

		switch (currentWorldType)
		{
			case WorldType.Vinter:
				books[0].SetActive(true);
				break;
			case WorldType.Chaumm:
				books[1].SetActive(true);
				break;
			case WorldType.Gang:
				books[2].SetActive(true);
				break;
			case WorldType.Pelmanus:
				books[3].SetActive(true);
				break;
		}
	}	
}
