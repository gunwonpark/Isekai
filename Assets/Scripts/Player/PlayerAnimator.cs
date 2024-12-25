using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	private Animator _animator;
	private MovementRigidbody2D _movement;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_movement = GetComponentInParent<MovementRigidbody2D>();
	}

	public void UpdateAnimation(float x)
	{
		if (x != 0)
		{
			SpriteFlipX(x);
		}

		_animator.SetFloat("Speed", Mathf.Abs(x) * Mathf.Abs(_movement.Velocity.x));
	}

	private void SpriteFlipX(float x)
	{
		transform.localScale = new Vector3((x < 0 ? -1f : 1f), 1f, 1f);
	}
}
