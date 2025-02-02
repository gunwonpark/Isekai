using UnityEngine;
using UnityEngine.Playables;

public class UI_NoticePopup : UI_Popup
{
	[SerializeField] PlayableDirector _playableDirector;

	public override void Init()
	{
		base.Init();
		_playableDirector = GameObject.Find("TimeLineEnd").GetComponent<PlayableDirector>();
	}

	public void CheckNotice(bool isOn)
	{

		_playableDirector.Play();
		Managers.UI.ClosePopupUI(this);

	}
}
