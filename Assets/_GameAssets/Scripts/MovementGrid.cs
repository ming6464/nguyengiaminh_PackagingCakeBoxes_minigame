using UnityEngine;

public class MovementGrid : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public Vector2 GridPosition;
    public bool IsGift;
    public bool IsObstacle;
    
    private Vector3 m_nextPosition;
    
    
    private void OnEnable()
    {
        if (EventDispatcher.Instance)
        {
            this.RegisterListener(EventID.OnSwipe,OnSwipe);
            this.RegisterListener(EventID.FinishSetupMap,OnFinishSetupMap);
        }
        
    }
    
    private void OnDisable()
    {
        if (EventDispatcher.Instance)
        {
            EventDispatcher.Instance.RemoveListener(EventID.OnSwipe,OnSwipe);
            EventDispatcher.Instance.RemoveListener(EventID.FinishSetupMap,OnFinishSetupMap);
        }
    }
    private void OnFinishSetupMap(object obj)
    {
        if(MapScript.Instance == null) return;
        GridInfo gridInfo = MapScript.Instance.GetGridInfo(GridPosition);
        if (gridInfo == null)
        {
            return;
        }

        m_nextPosition = gridInfo.ObjectTf.position;
        transform.position = m_nextPosition;
    }

    private void Update()
    {
        if(IsObstacle) return;
        transform.position = Vector3.Lerp(transform.position, m_nextPosition, Time.deltaTime * MoveSpeed);
    }

    private void OnSwipe(object obj)
    {
        if(IsObstacle || obj == null) return;
        SwipeKey key = (SwipeKey)obj;
        if(MapScript.Instance == null) return;
        GridInfo gridInfo = MapScript.Instance.MoveOnGrid(transform,GridPosition, key,IsGift);
        if (gridInfo == null)
        {
            return;
        }

        GridPosition = gridInfo.PositionGrid;
        m_nextPosition = gridInfo.ObjectTf.position;
    }
}