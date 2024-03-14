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

    public GridInfo MoveOnGrid(Transform obj, Vector2 positionGrid, SwipeKey key, bool isGift = false)
    {
        Vector3 directRay;

        int number = 0;
        bool check = false;
        Vector2 reverseDirection = Vector2.zero;
        
        switch (key)
        {
            case SwipeKey.Up:
                directRay = obj.up;
                positionGrid.y = _gridYMax;
                reverseDirection = Vector2.down;
                break;
            case SwipeKey.Down:
                directRay = Quaternion.Euler(0,0,180f) * obj.up;
                positionGrid.y = _gridYMin;
                reverseDirection = Vector2.up;
                break;
            case SwipeKey.Right:
                directRay = obj.right;
                positionGrid.x = _gridXMax;
                reverseDirection = Vector2.left;
                break;
            default:
                directRay = Quaternion.Euler(0,0,180f) * obj.right;
                positionGrid.x = _gridXMin;
                reverseDirection = Vector2.right;
                break;
        }

        foreach (RaycastHit2D raycastHit2D in Physics2D.RaycastAll(obj.position, directRay, 20f,_obstacleLayerMask))
        {
            Transform tf = raycastHit2D.transform;
            
            if(tf.GetInstanceID() == obj.GetInstanceID()) continue;

            if (!CompareTag("Obstacle"))
            {
                if (isGift && !check)
                {
                    if (tf.position.y > obj.position.y)
                    {
                        check = true;
                        continue;
                    }
                }
                else if (tf.CompareTag("Gift"))
                {
                    if(obj.position.y > tf.position.y) continue;
                }
            }

            number++;
        }
        
        positionGrid += number * reverseDirection;
        
        return GetGridInfo(positionGrid);
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

