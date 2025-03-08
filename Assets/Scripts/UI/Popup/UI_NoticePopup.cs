using UnityEngine;
using UnityEngine.Playables;

public class UI_NoticePopup : UI_Popup
{
	[SerializeField] private PlayableDirector _playableDirector;
	[SerializeField] private Material[] _material;
	private MeshRenderer _meshRenderer;
	private SpriteClickHandler _book;

	public override void Init()
	{
		base.Init();
		_playableDirector = GameObject.Find("TimeLineEnd").GetComponent<PlayableDirector>();
		_meshRenderer = GameObject.Find("Quad").GetComponent<MeshRenderer>();
		_meshRenderer.material = _material[0];
	}

	public void Init(SpriteClickHandler book)
	{
		_book = book;
	}

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
            _book.SetCanClicked();
            _book.StartBlink();
            Managers.UI.ClosePopupUI(this);
        }
    }

    public void CheckNotice(bool isOn)
	{
		_playableDirector.Play();
		Managers.UI.ClosePopupUI(this);
	}

	private void OnDestroy()
	{
		_meshRenderer.material = _material[1];
	}
}
