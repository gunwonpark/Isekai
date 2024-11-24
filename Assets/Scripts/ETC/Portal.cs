using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
	public string targetSceneName;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Managers.Scene.LoadScene((Scene)System.Enum.Parse(typeof(Scene), targetSceneName));
		}
	}
}
