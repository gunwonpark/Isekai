using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HappinessHUD : UI_Scene
{
	[SerializeField] private Slider _happinessSlider;
	[SerializeField] private Image _gaugeBarImage;

	public static HappinessHUD Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
		}
		else
		{
			Destroy(gameObject); // 이미 존재하면 중복 생성 방지
			return;
		}
	}

	public override void Init()
	{
		base.Init();

		_happinessSlider.value = Managers.Happy.Happiness / Managers.Happy.MaxHappiness;
		UpdateHappinessUI(Managers.Happy.Happiness);		
	}

	private void OnEnable()
	{
		Managers.Happy.OnHappinessChanged += UpdateHappinessUI;
	}

	private void OnDisable()
	{
		Managers.Happy.OnHappinessChanged -= UpdateHappinessUI;
	}

	public void UpdateHappinessUI(float happiness)
	{
		if (happiness >= 90f && happiness <= 100f)
			_gaugeBarImage.sprite = Resources.Load<Sprite>("Textures/Gauge/gauge_10");
		else if (happiness >= 80f && happiness < 90f)
			_gaugeBarImage.sprite = Resources.Load<Sprite>("Textures/Gauge/gauge_9");
		else if (happiness >= 70f && happiness < 80f)
			_gaugeBarImage.sprite = Resources.Load<Sprite>("Textures/Gauge/gauge_7");
		else if (happiness >= 60f && happiness < 70f)
			_gaugeBarImage.sprite = Resources.Load<Sprite>("Textures/Gauge/gauge_6");
		else if (happiness >= 50f && happiness < 60f)
			_gaugeBarImage.sprite = Resources.Load<Sprite>("Textures/Gauge/gauge_5");
		else if (happiness >= 40f && happiness < 50f)
			_gaugeBarImage.sprite = Resources.Load<Sprite>("Textures/Gauge/gauge_4");
		else if (happiness >= 30f && happiness < 40f)
			_gaugeBarImage.sprite = Resources.Load<Sprite>("Textures/Gauge/gauge_3");
		else if (happiness >= 20f && happiness < 30f)
			_gaugeBarImage.sprite = Resources.Load<Sprite>("Textures/Gauge/gauge_2");
		else
			_gaugeBarImage.sprite = Resources.Load<Sprite>("Textures/Gauge/gauge_1");

		if (_happinessSlider != null)
			_happinessSlider.value = happiness / Managers.Happy.MaxHappiness;

		//Debug.Log($"Happiness Updated: {happiness}");
	}

	public void ChangeHappiness(float amount)
	{
		Managers.Happy.ChangeHappiness(amount);
	}
}
