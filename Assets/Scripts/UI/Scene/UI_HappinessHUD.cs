using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class HappinessHUD : UI_Scene
{
	[SerializeField] private Image _gaugeBarImage;
	[SerializeField] private Image _happyImage;
	[SerializeField] List<Sprite> _happySprites;
    [SerializeField] private Volume _volume;
    [SerializeField] private ColorAdjustments _color;

	public static HappinessHUD Instance { get; private set; }

	private List<Color> _colors;

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
        _volume.profile.TryGet(out _color);
    }

	public override void Init()
	{
		base.Init();

		Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
		canvas.planeDistance = 1;

        // not used
        _colors = new List<Color>()
		{
			new Color32(254,215,0, 255),
			new Color32(198,145,43,255),
			new Color32(170, 120, 56, 255),
			new Color32(142,100,62, 255),
			new Color32(85,64,56,255),
			new Color32(58,51,45,255),
			new Color32(35,32,26,255),
			new Color32(23,21,15,255),
			new Color32(18,18,11,255),
			new Color32(1, 0, 0, 255)
		};
		
        _gaugeBarImage.fillAmount = Managers.Happy.Happiness / Managers.Happy.MaxHappiness;
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
        _gaugeBarImage.fillAmount = happiness / Managers.Happy.MaxHappiness;

		

		if (happiness >= 80f)
		{
			_happyImage.sprite = _happySprites[0];
		}
		else if(happiness >= 60f)
        {
            _happyImage.sprite = _happySprites[1];
        }
        else if (happiness >= 40f)
        {
            _happyImage.sprite = _happySprites[2];
        }
        else if (happiness >= 20f)
        {
            _happyImage.sprite = _happySprites[3];
        }
        else
        {
            _happyImage.sprite = _happySprites[4];
        }


		if (_color != null)
            _color.saturation.Override(-(1f - Managers.Happy.Happiness / Managers.Happy.MaxHappiness) * 100);

    }

    public void ChangeHappiness(float amount)
	{
		Managers.Happy.ChangeHappiness(amount);
    }
}
