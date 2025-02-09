using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class LibraryTimeLineCallback : MonoBehaviour
{
	[SerializeField] Material material;
	[SerializeField] MeshRenderer meshRenderer;
	[SerializeField] SpriteRenderer[] spriteRenderers;

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
		StartCoroutine(FingerTwinkleCoroutine());
    }

	private IEnumerator FingerTwinkleCoroutine()
	{
		while (true)
		{
			spriteRenderers[0].enabled = !spriteRenderers[0].enabled;
			spriteRenderers[1].enabled = !spriteRenderers[1].enabled;
			spriteRenderers[2].enabled = !spriteRenderers[2].enabled;
			spriteRenderers[3].enabled = !spriteRenderers[3].enabled;

			yield return new WaitForSeconds(0.8f);
		}
	}
}
