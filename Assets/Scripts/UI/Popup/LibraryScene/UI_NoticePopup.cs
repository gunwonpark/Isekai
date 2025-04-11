using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class UI_NoticePopup : UI_Popup
{
    [SerializeField] protected Toggle _checkToggle;
    [SerializeField] protected Image _fadeImage;
    [SerializeField] protected RectTransform _backgroundParent;
    [SerializeField] protected TMP_Text _noticeText;

    protected Canvas _canvas;
    protected LibraryScene _libraryScene;
    protected LibraryBook _book;

    protected int _popupIndex = 0;
    protected bool _canHandle = true;
    protected Vector2 _position;

    public override void Init()
    {
        base.Init();
        _libraryScene = Managers.Scene.CurrentScene as LibraryScene;


        _libraryScene.DisableBookSelect();
        

        _checkToggle.onValueChanged.AddListener(OnCheckToggleIsOn);

        // postprocessing 효과를 위한 카메라 설정
        _canvas = GetComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = Camera.main;
    }

    public virtual void Init(LibraryBook book)
    {
        _book = book;
    }

    public virtual void Init(int index, bool canHandle, Vector2 position)
    {
        _canHandle = canHandle;
        _popupIndex = index;
        _position = position;
        _backgroundParent.anchoredPosition = position;
    }

    protected virtual void Update()
    {
        if (!_canHandle) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePopup();
        }
    }

    protected virtual void ClosePopup()
    {
        _libraryScene.EnableBookSelect();
        if (_book != null)
        {
            _book.SetCanClicked();
            _book.StartFingerBlink();
        }
        Managers.UI.ClosePopupUI(this);
    }

    public virtual void OnCheckToggleIsOn(bool isOn)
    {
        if (!isOn) return;

        // 각 월드 타입별 처리를 하위 클래스에서 구현
        ProcessWorldInteraction();
    }

    protected virtual void ProcessWorldInteraction()
    {
        _libraryScene.PlayEndTimeLine();
        Managers.UI.CloseAllPopupUI();
        Managers.Scene.LoadScene(Scene.GameScene);
    }

    public void SetActiveFalse()
    {
        _noticeText.gameObject.SetActive(false);
        _checkToggle.gameObject.SetActive(false);
    }
 //   private void GangrilSequence()
 //   {
 //       _libraryScene.PlayEndTimeLine();
 //       // 1. 화면이 점점 까매진다
 //       // 2. 까만화면에 들어오면 1초뒤에 새로운 Notice창이 생성된다
 //       // 3. Notice창의 크기는 0.7, 1.0, 1.3 순으로 되어있으며 3번까지 Notice창이 생성된다
 //       // 3번째의 체크표시에는 갱그릴 세계에 진입하게 된다.
 //       Sequence sequence = DOTween.Sequence();
 //       sequence.AppendInterval(2f);
 //       sequence.AppendCallback(() => _fadeImage.gameObject.SetActive(true));
 //       sequence.Append(_fadeImage.DOFade(1, 2f).SetEase(Ease.Linear));
 //       sequence.AppendInterval(1f);
 //       sequence.OnComplete(() =>
 //       {
 //           Managers.UI.ClosePopupUI(this);
 //           Managers.UI.MakeSubItem<UI_GangrilNotice>().Init(1);
 //       });
 //       sequence.Play();
 //   }

 //   private float minSpawnTime = 0.1f; // 최소 스폰 속도
 //   private float startSpawnTime = 1.0f; // 시작 속도
 //   private float acceleration = 0.8f; // 가속도 (1보다 작으면 점점 빨라짐)
 //   private float popupWidth, popupHeight;

 //   private float currentSpawnTime;
	//private float spawnX, spawnY;
 //   private float screenWidth, screenHeight;
 //   private float leftTopX, leftTopY;

 //   private float _xOffset = 70;
 //   private float _yOffset = 50;

 //   private UI_NoticePopup _lastPopup;
 //   private void MakeInfinityPopup(Vector3 startPosition)
 //   {
 //       // 50 -50 씩 아래로 팝업을 무한으로 생성하는데 스크린 범위를 넘어가면 왼쪽 위부터 다시 오른쪽 아래로 생성한다
 //       // 처음에는 천천히 생성했다가 점점 빠르게 생성되고 다음에는 일정한 속도로 계속 생성한다
 //       screenWidth = Screen.width;
 //       screenHeight = Screen.height;

 //       spawnX = startPosition.x;
 //       spawnY = startPosition.y;

 //       currentSpawnTime = startSpawnTime;

 //       popupWidth = _backgroundParent.GetComponent<RectTransform>().rect.width;
 //       popupHeight = _backgroundParent.GetComponent<RectTransform>().rect.height;

 //       GetFirstPosition();

 //       StartCoroutine(InfinityPopupCoroutine());
 //   }

 //   /// <summary>
 //   /// 초기 위치 초기화
 //   /// </summary>
 //   private void GetFirstPosition()
 //   {
 //       leftTopX = -popupWidth / 2;
 //       leftTopY = popupHeight / 2;

 //       while (leftTopX >= -(screenWidth / 2) && leftTopY <= screenHeight / 2)
 //       {
 //           leftTopX -= _xOffset;
 //           leftTopY += _yOffset;
 //       }

 //       leftTopX += _xOffset;
 //       leftTopY -= _yOffset;
 //       leftTopX += popupWidth / 2;
 //       leftTopY -= popupHeight / 2;
 //   }

 //   private IEnumerator InfinityPopupCoroutine()
 //   {
 //       int spawnCount = 0;
 //       int maxSpawnCount = 20;

 //       while (true)
 //       {
 //           LibraryScene scene = Managers.Scene.CurrentScene as LibraryScene;
 //           scene.StopColorConversion();
 //           scene.ColorConversion(Mathf.Max(currentSpawnTime * 0.5f, 0.15f));
 //           SpawnPopup();

 //           yield return new WaitForSeconds(currentSpawnTime);

 //           // 가속 적용 (최소 속도보다 크면 계속 감소)
 //           if (currentSpawnTime > minSpawnTime)
 //           {
 //               currentSpawnTime *= acceleration;
 //               if (currentSpawnTime < minSpawnTime)
 //                   currentSpawnTime = minSpawnTime;
 //           }

 //           spawnCount++;
 //           if (spawnCount >= maxSpawnCount)
 //           {
 //               break;
 //           }
 //       }

 //       StartCoroutine(_lastPopup.BlackOutAndSetText());
 //   }

 //   public void SetActiveFalse()
 //   {
 //       _backgroundParent.gameObject.GetComponent<Image>().sprite = _changeSprite;
 //       _noticeText.gameObject.SetActive(false);
 //       _checkToggle.gameObject.SetActive(false);
 //   }
 //   private IEnumerator BlackOutAndSetText()
 //   {
 //       _fadeImage.gameObject.SetActive(true);
 //       _fadeImage.color = new Color(0, 0, 0, 1);
 //       yield return new WaitForSeconds(2f);

 //       yield return StartCoroutine(_warningText.CoTypingEffect("거기서 당장 나와", 0.5f, true));
 //       yield return new WaitForSeconds(2f);

 //       Managers.Scene.LoadScene(Scene.GameScene);
 //   }

 //   /// <summary>
 //   /// 팝업 생성
 //   /// 특정 위치에 생성
 //   /// </summary>
 //   private void SpawnPopup()
 //   {
 //       Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);
 //       UI_NoticePopup popup = Managers.UI.ShowPopupUI<UI_NoticePopup>();
      
 //       // 다음 위치 계산
 //       spawnX += _xOffset;
 //       spawnY -= _yOffset;
 //       if (spawnX + (popupWidth / 2) > screenWidth / 2 || spawnY + -(popupHeight / 2) > screenHeight / 2)
 //       {
 //           _lastPopup.ChangeSprite();

 //           _lastPopup = null;
 //           spawnX = leftTopX;
 //           spawnY = leftTopY;
 //       }

 //       popup.Init(_popupIndex + 1, false, new Vector3(spawnX, spawnY));

 //       if (_lastPopup != null)
 //       {
 //           _lastPopup.SetActiveFalse();
 //       }

 //       _lastPopup = popup;
 //   }

 //   private void ChangeSprite()
 //   {
 //       _checkToggle.onValueChanged.RemoveAllListeners();
 //       _checkToggle.isOn = true;
 //   }
}
