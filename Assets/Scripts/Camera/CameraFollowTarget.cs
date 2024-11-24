using UnityEngine;

public class CameraFollowTarget: MonoBehaviour
{
	[SerializeField] private Transform _target;
	[SerializeField] private bool x, y, z;

	private float _offsetY;

	private void Awake()
	{
		_offsetY = Mathf.Abs(transform.position.y - _target.position.y);
	}

	private void LateUpdate()
	{
		transform.position = new Vector3((x ? _target.position.x : transform.position.x),
										 (y ? _target.position.y + _offsetY : transform.position.y),
										 (z ? _target.position.z : transform.position.z));
	}
}
