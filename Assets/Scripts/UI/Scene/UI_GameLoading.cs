using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GameLoading : UI_Scene
{
    private Animator _animator;
    private LoadingGameSceneData _data;

    [SerializeField] private UI_TodoList _todoList;
    [SerializeField] private TMP_Text _curDateText;
    [SerializeField] private TMP_Text _totalDateText;
    [SerializeField] private TMP_Text _worldNameText;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetTrigger("StartAnimation");
        _data = Managers.DB.GetLoadingGameSceneData(Managers.World.CurrentWorldType);
    }

    private void Start()
    {
        _totalDateText.text = "";
        _worldNameText.text = "";
        StartCoroutine(AnimateDateRange($"{_data.startDate} ~ {_data.endDate}", 1f));
    }

    IEnumerator AnimateDateRange(string range, float totalTime)
    {
        // 입력된 문자열을 파싱해서 시작/끝 날짜 가져오기
        string[] parts = range.Split('~');
        string[] startParts = parts[0].Trim().Split('.');
        string[] endParts = parts[1].Trim().Split('.');

        int startYear = int.Parse(startParts[0]);
        int startMonth = int.Parse(startParts[1]);
        int endYear = int.Parse(endParts[0]);
        int endMonth = int.Parse(endParts[1]);

        DateTime currentDate = new DateTime(startYear, startMonth, 1);
        DateTime endDate = new DateTime(endYear, endMonth, 1);
        float timePerMonth = totalTime / (endDate.Year * 12 + endDate.Month - (currentDate.Year * 12 + currentDate.Month));

        while (currentDate <= endDate)
        {
            int totalDays = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            float elapsedTime = 0f;
            int currentDay = 1;

            _curDateText.text = $"{currentDate.Year}.{currentDate.Month:D2}"; // "YYYY.MM" 초기 표시

            // 해당 월의 1일부터 마지막 일까지 증가
            while (elapsedTime < timePerMonth)
            {
                elapsedTime += Time.deltaTime;
                int newDay = Mathf.Clamp(Mathf.FloorToInt((elapsedTime / timePerMonth) * totalDays), 1, totalDays);

                if (newDay != currentDay)
                {
                    currentDay = newDay;
                    _curDateText.text = $"{currentDate.Year}.{currentDate.Month:D2}.{currentDay:D2}";
                }

                yield return null;
            }

            // 다음 달로 이동
            currentDate = currentDate.AddMonths(1);
        }

        // 마지막 년.월 표시
        _curDateText.text = $"{endDate.Year}.{endDate.Month:D2}";
    }

    public void OnAnimationEnd()
    {
        StartCoroutine(ShowWorldNameText());
    }

    private IEnumerator ShowWorldNameText()
    {
        _worldNameText.gameObject.SetActive(true);
        yield return StartCoroutine(_worldNameText.CoTypingEffect(_data.worldName, 0.1f));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(_totalDateText.CoTypingEffect($"{_data.startDate} ~ {_data.endDate}", 0.1f));
        yield return new WaitForSeconds(1f);
        ShowToDoList();
    }

    private void ShowToDoList()
    {
        _todoList.gameObject.SetActive(true);
        _todoList.Init(_data);
    }
}
