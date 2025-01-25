using Cinemachine.Examples;
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

	// dir, moveSpeed
	public event Action<Vector2, float> OnPlayerMove;

	private void Awake()
	{
		_movement = GetComponent<MovementRigidbody2D>();
		_playerAnimator = GetComponentInChildren<PlayerAnimator>();
	}

	private void Start()
	{
		currentScene = SceneManager.GetActiveScene().name;
		Debug.Log(currentScene);
	}

	private void Update()
	{
		if (isMoving == true || currentScene != "IntroScene")
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

            OnPlayerMove?.Invoke(new Vector2(x, 0), _movement.GetMoveSpeed());

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
