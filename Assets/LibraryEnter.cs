using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryEnter : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			Managers.Scene.LoadScene(Scene.LibraryScene);
		}
	}
}
