using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
	public Scene SceneType { get; protected set; } = Scene.Unknown;
	
	private void Awake()
	{
		Init();
	}

	/// <summary>
	/// Awake시점에서 초기화 예정
	/// </summary>
	protected virtual void Init()
	{
		// if you have initialize somthing
		// TODO
	}

	/// <summary>
	/// Scene의 전환이 일어 날 시 자동으로 호출
	/// </summary>
	public abstract void Clear();
}
