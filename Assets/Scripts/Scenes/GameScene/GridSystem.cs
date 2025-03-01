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

public class GridSystem : MonoBehaviour
{
    public Vector2 referenceSize = new Vector2(5, 5);
    public Transform referenceTransform;
    public int maxGridCount = 9;
    public List<GridInfo> gridInfos = new List<GridInfo>();
    public List<int> randomIndexList = new List<int>();

    [ContextMenu("Test")]
    public void Test()
    {
        Init(referenceTransform);
        Debug.Log("Test");
    }

    public void Init(Transform referenceTransform)
    {
        this.referenceTransform = referenceTransform;

        Vector2 mainPosition = referenceTransform.position;
        
        // y좌표 초기화
        mainPosition.y = 0;

        // 그리드 갯수 초기화
        gridInfos = new List<GridInfo>(maxGridCount);
        for(int i = 0; i < maxGridCount; i++)
        {
            gridInfos.Add(new GridInfo());
        }

        if (gridInfos.Count == 0) return;

        //랜덤 인덱스 초기화
        randomIndexList = Enumerable.Range(0, gridInfos.Count).ToList();

        for (int i = 0; i < gridInfos.Count; i++)
        {
            gridInfos[i].Size = referenceSize;
        }


        int mid = gridInfos.Count / 2;


        gridInfos[mid].Position = mainPosition;

        for (int i = mid - 1; i >= 0; i--)
        {
            gridInfos[i].Position = gridInfos[i + 1].Position - new Vector2(gridInfos[i].Size.x, 0);
        }

        for (int i = mid + 1; i < gridInfos.Count; i++)
        {
            gridInfos[i].Position = gridInfos[i - 1].Position + new Vector2(gridInfos[i].Size.x, 0);
        }
    }

    /// <summary>
    /// 빈 공간이 없을시 0, 0 좌표에 생성한다
    /// </summary>
    /// <returns></returns>
    public bool TryGetEmptyPosition(out Vector2 position)
    {
        int index = FindEmptyRandomIndex();
        if (index == -1)
        {
            Debug.Log("모든 공간이 찼습니다.");
            position = Vector2.zero;
            return false;
        }

        position = GetRealGridPosition(index);
        return true;
    }

    // 플레이어의 위치를 기준으로 충돌하지 않는 공간에 오브젝트 생성
    private int FindEmptyRandomIndex()
    {
        randomIndexList.Shuffle();

        for (int i = 0; i < randomIndexList.Count; i++)
        {
            int index = randomIndexList[i];

            if (Physics2D.OverlapBox(GetRealGridPosition(index), gridInfos[index].Size, 0, LayerMask.GetMask("UI")) == null)
            {
                return index;
            }
        }

        // 모든 공간이 차있을 경우
        // 리스트를 동적으로 늘리며 공간을 확장한다.
        // 현재 코드에서는 Init()으로 크기를 늘릴수 있다.

        return -1;
    }

    private Vector2 GetRealGridPosition(int index)
    {        
        return gridInfos[index].Position + new Vector2(referenceTransform.position.x, 0);
    }

    private void OnDrawGizmos()
    {
        foreach (var gridInfo in gridInfos)
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
}