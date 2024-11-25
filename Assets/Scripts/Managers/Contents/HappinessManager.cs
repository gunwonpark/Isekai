using System;
using UnityEngine;

public class HappinessManager
{
	private float _happiness = 50;
	private readonly float _maxHappiness = 100;
	private readonly float _minHappiness = 0;

	public event Action<float> OnHappinessChanged;

	public float Happiness
	{
		get { return _happiness; }
		set
		{
			_happiness = Mathf.Clamp(value, _minHappiness, _maxHappiness);
			UpdateHappinessEffects();

			OnHappinessChanged?.Invoke(_happiness);
			Debug.Log($"Happiness: {_happiness}");

			//if (_happiness <= 100)
			//{
			//	Managers.Scene.LoadScene(Scene.EndingScene);
			//}
		}
	}
	public float MaxHappiness { get { return _maxHappiness; } }
	public float MinHappiness { get { return _minHappiness; } }

	public void Init()
	{
		UpdateHappinessEffects();
		OnHappinessChanged?.Invoke(_happiness); // 초기화 시 이벤트 호출
	}

	private void UpdateHappinessEffects()
	{
		// 배경 색상 업데이트 등 추가 효과
		// Managers.UI.UpdateBackgroundColor(_happiness);
	}

	public void ChangeHappiness(float amount)
	{
		Happiness += amount;
	}
}
