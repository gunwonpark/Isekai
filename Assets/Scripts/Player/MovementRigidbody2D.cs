using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRigidbody2D : MonoBehaviour
{
	//[Header("LayerMask")]
	//[SerializeField] private LayerMask _groundChechLayer;

	[Header("Move")]
	[SerializeField] private float _moveSpeed = 2.5f;

	//[Header("Jump")]
	//[SerializeField] private float _jumpForce = 13.0f;
	[SerializeField] private float _gravityScale = 3.5f;

	

	private Vector2 _collisionSize;
	private Vector2 _footPosition;

	private Rigidbody2D _rigid2D;
	private Collider2D _collider2D;

	//public bool IsGrounded { get; private set; } = false;

	public Vector2 Velocity => _rigid2D.velocity;

	private void Awake()
	{
		_rigid2D = GetComponent<Rigidbody2D>();
		_collider2D = GetComponent<Collider2D>();
		_rigid2D.gravityScale = _gravityScale;
	}

	//private void Update()
	//{
	//	UpdateCollision();
	//}


	public void MoveTo(float x)
	{
		if (Managers.Happy.Happiness > 0 && Managers.Happy.Happiness <= 20)
		{
			_moveSpeed = 3.0f;
		}
		else if (Managers.Happy.Happiness > 20 && Managers.Happy.Happiness <= 50)
		{
			_moveSpeed = 5.0f;
		}
		else
		{
			_moveSpeed = 8.0f;
		}

		if (x != 0) x = Mathf.Sign(x);

		_rigid2D.velocity = new Vector2(x * _moveSpeed, _rigid2D.velocity.y);
	}

	public void SetMoveSpeed(float speed)
	{
		_moveSpeed = speed;
	}

	//private void UpdateCollision()
	//{
	//	Bounds bounds = _collider2D.bounds;

	//	_collisionSize = new Vector2((bounds.max.x - bounds.min.x) * 0.5f, 0.1f);
	//	_footPosition = new Vector2(bounds.center.x, bounds.min.y);

	//	IsGrounded = Physics2D.OverlapBox(_footPosition, _collisionSize, 0, _groundChechLayer);		
	//}

	//public void Jump()

	//{
	//	if (IsGrounded == true)
	//	{
	//		_rigid2D.velocity = new Vector2(_rigid2D.velocity.x, _jumpForce);
	//	}
	//}
}
