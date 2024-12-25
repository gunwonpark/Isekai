using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	private MovementRigidbody2D _movement;
	private PlayerAnimator _playerAnimator;
	public bool isMoving = false;

	private string currentScene;

	private void Awake()
	{
		_movement = GetComponent<MovementRigidbody2D>();
		_playerAnimator = GetComponentInChildren<PlayerAnimator>();
	}

	private void Start()
	{
		currentScene = SceneManager.GetActiveScene().name;
	}

	private void Update()
	{
		if (isMoving == true || currentScene != "01GameScene")
		{
			float x = 0f;

			if (Input.GetKey(KeyCode.LeftArrow))
			{
				x = -1f;
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				x = 1f;
			}
			UpdateMove(x);
			_playerAnimator.UpdateAnimation(x);
		}
		else
		{
			UpdateMove(0);
			_playerAnimator.UpdateAnimation(0);
		}
	}

	private void UpdateMove(float x)
	{
		_movement.MoveTo(x);
	}
}
