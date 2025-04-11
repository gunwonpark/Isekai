using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class UI_GangrilNoticePopup : UI_NoticePopup
{
    [SerializeField] private TMP_Text _gangText;

    protected override void ProcessWorldInteraction()
    {
        PlayGangrilSequence();
    }

    private void PlayGangrilSequence()
    {
        _libraryScene.PlayEndTimeLine();

        // 1. 화면이 점점 까매진다
        // 2. 까만화면에 들어오면 1초뒤에 새로운 Notice창이 생성된다
        // 3. Notice창의 크기는 0.7, 1.0, 1.3 순으로 되어있으며 3번까지 Notice창이 생성된다
        // 3번째의 체크표시에는 갱그릴 세계에 진입하게 된다.
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(2f);
        sequence.AppendCallback(() => _fadeImage.gameObject.SetActive(true));
        sequence.Append(_fadeImage.DOFade(1, 2f).SetEase(Ease.Linear));
        sequence.AppendInterval(1f);
        sequence.OnComplete(() =>
        {
            Managers.UI.ClosePopupUI(this);
            Managers.UI.MakeSubItem<UI_GangrilNotice>().Init(1);
        });
        sequence.Play();
    }
}
