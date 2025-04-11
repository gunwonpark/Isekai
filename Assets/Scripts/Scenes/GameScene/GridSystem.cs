using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
public class GridInfo
{
    public Vector2 Position;
    public float sizeX;
    public float sizeY;

    public Vector2 Size
    {
        get
        {
            return new Vector2(sizeX, sizeY);
        }
        set
        {
            sizeX = value.x;
            sizeY = value.y;
        }
    }
}

/// <summary>
/// 1. 영역 전개 및 영역에 대한 오브젝트 충돌 확인
/// 2. 가능한 영역위치 반환
/// </summary>
public class GridSystem : MonoBehaviour
{
    [SerializeField] private Vector2 _referenceSize = new Vector2(5, 5);       // 그리드하나의 크기
    [SerializeField] private Transform _referenceTransform;                    // 그리드의 기준이 되는 오브젝트
    [SerializeField] private int _maxGridCount = 9;                            // 그리드 총 개수 
    [SerializeField] private List<GridInfo> _gridInfos = new List<GridInfo>(); // 그리드 정보 리스트
    private List<int> _randomIndexList = new List<int>();                      // 랜덤 위치 반환 리스트

    public void Init(Transform referenceTransform)
    {
        this._referenceTransform = referenceTransform;

        // 그리드 갯수 초기화
        _gridInfos = new List<GridInfo>(_maxGridCount);
        for (int i = 0; i < _maxGridCount; i++)
        {
            _gridInfos.Add(new GridInfo());
        }

        if (_gridInfos.Count == 0) return;

        //랜덤 인덱스 초기화
        _randomIndexList = Enumerable.Range(0, _gridInfos.Count).ToList();

        for (int i = 0; i < _gridInfos.Count; i++)
        {
            _gridInfos[i].Size = _referenceSize;
        }

        int mid = _gridInfos.Count / 2;

        // 그리드 위치 설정
        Vector2 mainPosition = _referenceTransform.position;
        mainPosition.y = 0;
        _gridInfos[mid].Position = mainPosition;

        // 좌
        for (int i = mid - 1; i >= 0; i--)
        {
            _gridInfos[i].Position = _gridInfos[i + 1].Position - new Vector2(_gridInfos[i].Size.x, 0);
        }

        // 우
        for (int i = mid + 1; i < _gridInfos.Count; i++)
        {
            _gridInfos[i].Position = _gridInfos[i - 1].Position + new Vector2(_gridInfos[i].Size.x, 0);
        }
    }

    /// <summary>
    /// 빈 공간이 없을시 0, 0 을 반환한다
    /// </summary>
    public bool TryGetEmptyPosition(out Vector2 position)
    {
        int index = FindEmptyRandomIndex();
        if (index == -1)
        {
            position = Vector2.zero;
            return false;
        }

        position = GetGridWorldPosition(index);
        return true;
    }

    /// <summary>
    /// 플레이어의 위치를 기준으로 충돌하지 않는 공간에 오브젝트 생성
    /// 모든 공간이 차있을 경우생성하지 않는다.
    /// </summary>
    /// <returns></returns>
    // 
    private int FindEmptyRandomIndex()
    {
        _randomIndexList.Shuffle();

        for (int i = 0; i < _randomIndexList.Count; i++)
        {
            int index = _randomIndexList[i];

            // 플레이어가 이동하기 때문에 이를 통한 확인이 필요
            // 성능에 문제가 생길시 생성된 minigame의 위치를 저장후 이를 통해 판단
            if (Physics2D.OverlapBox(GetGridWorldPosition(index), _gridInfos[index].Size, 0, LayerMask.GetMask("UI")) == null)
            {
                return index;
            }
        }

        return -1;
    }

    // 그리드의 월드세계 좌표 반환
    private Vector2 GetGridWorldPosition(int index)
    {
        return _gridInfos[index].Position + new Vector2(_referenceTransform.position.x, 0);
    }

    #region 디버깅
    private void OnDrawGizmos()
    {
        foreach (var gridInfo in _gridInfos)
        {
            DrawGrid(gridInfo);
        }

        DrawCamera();
    }

    private void DrawGrid(GridInfo gridInfo)
    {
        Vector3 position = new Vector3(gridInfo.Position.x, gridInfo.Position.y, 0);
        Vector3 size = new Vector3(gridInfo.Size.x, gridInfo.Size.y, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(position, size);
    }

    private void DrawCamera()
    {
        Gizmos.color = Color.red;
        float height = Camera.main.orthographicSize * 2f;
        float width = height * Camera.main.aspect;
        Gizmos.DrawWireCube(Camera.main.transform.position, new Vector3(width - 0.1f, height - 0.1f, 0.1f));
    }
    #endregion
}
