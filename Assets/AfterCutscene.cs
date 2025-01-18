using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterCutscene : MonoBehaviour
{
	private PlayerController _playerController;

	void Start()
    {
		_playerController = FindObjectOfType<PlayerController>();
		Managers.Resource.Instantiate("Item/TrigerEnter");
		_playerController.isMoving = true;
	}
}
