using System;
using UnityEngine;

public class MapScript : Singleton<MapScript>
{
    [Header("Info Grid")]
    [SerializeField]
    private GridInfo[] _gridInfos;

    [SerializeField]
    private byte _gridXMin;
    [SerializeField]
    private byte _gridXMax;

    [SerializeField]
    private byte _gridYMin;
    [SerializeField]
    private byte _gridYMax;

    [SerializeField]
    private LayerMask _obstacleLayerMask;
    
    private void Start()
    {
        Invoke(nameof(DelayPostSetUpMap),.1f);
    }

    private void DelayPostSetUpMap()
    {
        this.PostEvent(EventID.FinishSetupMap);
    }

    public GridInfo GetNextGridInfo(Vector2 positionGrid,SwipeKey key)
    {
        switch (key)
        {
            case SwipeKey.Up:
                positionGrid.y = _gridYMax;
                break;
            case SwipeKey.Down:
                positionGrid.y = _gridYMin;
                break;
            case SwipeKey.Right:
                positionGrid.x = _gridXMax;
                break;
            default:
                positionGrid.x = _gridXMin;
                break;
            
        }
        
        return GetGridInfo(positionGrid);
    }

    public GridInfo GetGridInfoFromPosition(Vector3 position)
    {
        foreach (GridInfo grid in _gridInfos)
        {
            if (grid.ObjectTf.position == position) return grid;
        }

        return null;
    }
    

    public GridInfo GetGridInfo(Vector2 positionGrid)
    {
        foreach (GridInfo grid in _gridInfos)
        {
            if (grid.PositionGrid == positionGrid) return grid;
        }

        return null;
    }
    
}

[Serializable]
public class GridInfo
{
    public Transform ObjectTf;
    public Vector2 PositionGrid;
}

