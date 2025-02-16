using UnityEngine;
using UnityEngine.Playables;

public class UI_NoticePopup : UI_Popup
{
	[SerializeField] private PlayableDirector _playableDirector;
	[SerializeField] private Material[] _material;
	private MeshRenderer _meshRenderer;
	private GameObject _book;

	public override void Init()
	{
		base.Init();
		_playableDirector = GameObject.Find("TimeLineEnd").GetComponent<PlayableDirector>();
		_meshRenderer = GameObject.Find("Quad").GetComponent<MeshRenderer>();
		_meshRenderer.material = _material[0];
		_book = GameObject.FindGameObjectWithTag("Book");
		_book.SetActive(false);
	}

	public void CheckNotice(bool isOn)
	{
		_playableDirector.Play();
		Managers.UI.ClosePopupUI(this);
	}

	private void OnDestroy()
	{
		_meshRenderer.material = _material[1];
		_book.SetActive(true);
	}
}
