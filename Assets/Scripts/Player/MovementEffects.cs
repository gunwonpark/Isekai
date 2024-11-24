using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementEffects : MonoBehaviour
{
	private MovementRigidbody2D _movement;

	[SerializeField] private ParticleSystem _footStepEffect;
	[SerializeField] private ParticleSystem.EmissionModule _footEmission;

	private void Awake()
	{
		_movement = GetComponentInParent<MovementRigidbody2D>();
		_footEmission = _footStepEffect.emission;
	}

	private void Update()
	{
		if (_movement.Velocity.x != 0)
		{
			_footEmission.rateOverTime = 30;
		}
		else
		{
			_footEmission.rateOverTime = 0;
		}
	}
}
