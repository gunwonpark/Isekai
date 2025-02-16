using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BookPopup : UI_Popup
{
    [SerializeField] private Image _letterImage;
    [SerializeField] private TMP_Text _letterText;
	[SerializeField] private TMP_Text _TitleText;
	[SerializeField] private Button _AnyClick;
	[SerializeField] Material[] _material;
	private MeshRenderer _meshRenderer;
	private GameObject _book;

	public override void Init()
	{
		base.Init();
		SetText();

		_meshRenderer = GameObject.Find("Quad").GetComponent<MeshRenderer>();
		_meshRenderer.material = _material[0];
		_book = GameObject.FindGameObjectWithTag("Book");
		_book.SetActive(false);
	}

	public void ClosePopup()
	{
		Managers.UI.ClosePopupUI(this);
	}

	private void SetText()
	{
		WorldType currentWorldType = Managers.World.CurrentWorldType;

		switch (currentWorldType)
		{
			case WorldType.Vinter:
				SetVinter();
				break;
			case WorldType.Chaumm:
				SetChaumm();
				break;
			case WorldType.Gang:
				SetGang();
				break;
			case WorldType.Pelmanus:
				SetPelmanus();
				break;
		}
	}

	private void SetVinter()
	{
		_TitleText.text = "Title\r\n\r\n빈터발트 공작";
		_letterText.text = "어린 나이에 정령 친화력을 발현하며 놀라운 능력을 인정받아 이른 시기에 공작위를 이어받았다. 그의 가문은 \"빈터발트\"로 명문 가문으로 이름 높으며 뛰어난 혈통과 교육을 자랑한다.";
	}

	private void SetChaumm()
	{
		_TitleText.text = "Title\r\n\r\n차은유";
		_letterText.text = "이 배우의 외모는 단지 아름답다는 표현이 부족할 정도로 강렬한 인상을 남긴다. 사람들은 그의 무대 위에서의 모습에 열광하며, 그의 이름은 곧 예술 그 자체를 의미한다.";
	}

	private void SetGang()
	{
		_TitleText.text = "Title\r\n\r\n갱그릴";
		_letterText.text = "인간과 고블린의 특성을 모두 지닌 독특한 존재이다. 인간의 세련된 지혜와 고블린의 뛰어난 생존력을 겸비해 누구도 흉내낼 수 없는 독창적인 매력을 발산한다.";
	}

	private void SetPelmanus()
	{
		_TitleText.text = "Title\r\n\r\n펠마누스";
		_letterText.text = "그가 태어난 순간, 세상은 경외심으로 물들었다. 그는 죽어가는 사람조차 살려낼 수 있는 신비로운 능력을 지니고 있어, 수많은 이들이 그의 이름을 외우며 숭배했다.";
	}

	private void OnDestroy()
	{
		_meshRenderer.material = _material[1];
		_book.SetActive(true);
	}
}
