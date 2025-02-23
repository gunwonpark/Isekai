using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class RealGameScene : MonoBehaviour
{
    [SerializeField] private RealGameFactory _realGameFactory;
    [SerializeField] private PlayableDirector _timeline;
    [SerializeField] private PlayableDirector _enterTimeline;
    [SerializeField] private Transform _player;
    [SerializeField] private Image _fadeImage;

    private IEnumerator Start()
    {
        Managers.Sound.Play("realWorldBgm", Sound.Bgm);
        yield return PlayEnterTimeline();  
        yield return new WaitForSeconds(3f); // 3초 대기

        // 게임시작
        RealWorldInfo realWorldInfo = new FirstWorldInfo();
        Init(realWorldInfo);
    }

    private IEnumerator PlayEnterTimeline()
    {
        if (_enterTimeline != null)
        {
            _enterTimeline.Play();
            yield return new WaitUntil(() => _enterTimeline.state != PlayState.Playing);
        }
    }
    public void Init(RealWorldInfo realWorldInfo)
    {
        _realGameFactory.Init(realWorldInfo);
        _realGameFactory.OnGameEnd += RealGameFactory_OnGameEnd;

       

        RealGameSceneData realGameSceneData = Managers.DB.GetRealGameSceneData(Managers.World.CurrentWorldType);

        Camera.main.GetComponent<FlowCamera>().SetFollowSpeed(realGameSceneData.cameraSpeed);
        Camera.main.GetComponent<FlowCamera>().StartFlow();
    }

    private void RealGameFactory_OnGameEnd(bool isWin)
    {
        Camera.main.GetComponent<FlowCamera>().StopFlow();
        Debug.Log("EndGame");
        //주인공은 오른쪽으로 이동하고 카메라는 고정
        //화면은 천천히 faded out되고 fade in 되면서 도서관 씬으로 이동
        //상관없이 도서관 씬으로 이동
        PlayerController playerController = _player.GetComponent<PlayerController>();
        playerController.isMoving = false;

        StartCoroutine(CoWaitAndStart());
    }

    private void AdjustTimelinePosition()
    {
        StartCoroutine(CoFadeOut());
        Vector3 startPos = _player.position;

        foreach (var track in _timeline.playableAsset.outputs)
        {
            if (track.streamName == "PlayerPosition")
            {
                AnimationTrack animationTrack = (AnimationTrack)track.sourceObject;
                animationTrack.position = startPos;
            }
        }
    }

    private IEnumerator CoWaitAndStart()
    {
        yield return new WaitForSeconds(2.0f);
        AdjustTimelinePosition();
        _timeline.Play();
    }

    private IEnumerator CoFadeOut()
    {
        yield return new WaitForSeconds(3.0f);

        float fadeOutTime = 1.0f;
        float currentTime = 0.0f;
        Color color = _fadeImage.color;

        while (currentTime < fadeOutTime)
        {
            currentTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, currentTime / fadeOutTime);
            _fadeImage.color = color;
            yield return null;
        }

        Managers.World.MoveNextWorld();

        Managers.Scene.LoadScene(Scene.LibraryScene);

        yield return null;
    }

    [ContextMenu("EndGame")]
    public void EndGame()
    {
        RealGameFactory_OnGameEnd(true);
    }
}
