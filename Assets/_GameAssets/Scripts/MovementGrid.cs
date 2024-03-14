using UnityEngine;

public class MovementGrid : MonoBehaviour
{
    public float StepDistance = 30f;
    public Vector2 GridPosition;
    public bool IsGift;
    public bool IsObstacle;
    public float DistanceRayCheck = 0.5f;
    
    private Vector3 m_nextPosition;
    private bool IsFinishSetupMap;
    private SwipeKey m_swipeKeyNew;
    private bool m_isMovement;
    
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
        IsFinishSetupMap = true;
    }

    private void Update()
    {
        if(IsObstacle || !IsFinishSetupMap) return;
        if (transform.position != m_nextPosition)
        {
            m_isMovement = true;
            transform.position = Vector3.MoveTowards(transform.position, m_nextPosition, Time.deltaTime * StepDistance);
        }
        else m_isMovement = false;
    }

    private void LateUpdate()
    {
        if(IsObstacle || !IsFinishSetupMap) return;
        if (m_isMovement)
        {
            Vector3 directRay;
            switch (m_swipeKeyNew)
            {
                case SwipeKey.Up:
                    directRay = transform.up;
                    break;
                case SwipeKey.Down:
                    directRay = Quaternion.Euler(0,0,180) * transform.up;
                    break;
                case SwipeKey.Right:
                    directRay = transform.right;
                    break;
                default:
                    directRay = Quaternion.Euler(0,0,180) * transform.right;
                    break;
            }
            
            Debug.DrawRay(transform.position,directRay * DistanceRayCheck,Color.red);

            foreach (RaycastHit2D raycastHit2D in Physics2D.RaycastAll(transform.position, directRay, DistanceRayCheck))
            {
                Transform rayTf = raycastHit2D.transform;
                if(rayTf.GetInstanceID() == transform.GetInstanceID()) continue;
                LoadPositionCollider(rayTf);
            }
            
        }
    }

    private void OnSwipe(object obj)
    {
        if(IsObstacle || obj == null || MapScript.Instance == null) return;
        SwipeKey key = (SwipeKey)obj;
        m_swipeKeyNew = key;
        GridInfo gridInfo = MapScript.Instance.GetNextGridInfo(GridPosition, m_swipeKeyNew);
        if (gridInfo == null)
        {
            return;
        }
        
        GridPosition = gridInfo.PositionGrid;
        m_nextPosition = gridInfo.ObjectTf.position;
    }

    private void LoadPositionCollider(Transform other)
    {
        if(!m_isMovement || MapScript.Instance == null) return;
        if (m_swipeKeyNew is SwipeKey.Up or SwipeKey.Down)
        {
            if (IsGift)
            {
                if(other.CompareTag("Cake") && transform.position.y < other.transform.position.y) return;
            }
            else
            {
                if(other.CompareTag("Gift") && transform.position.y > other.transform.position.y) return;
            }
        }
        
        GridInfo gridInfo = MapScript.Instance.GetGridInfoFromPosition(other.transform.position);
        if(gridInfo == null) return;
        Vector2 positionGrid = gridInfo.PositionGrid;
        switch (m_swipeKeyNew)
        {
            case SwipeKey.Up:
                positionGrid.y -= 1;
                break;
            case SwipeKey.Down:
                positionGrid.y += 1;
                break;
            case SwipeKey.Right:
                positionGrid.x -= 1;
                break;
            default:
                positionGrid.x += 1;
                break;
        }

        gridInfo = MapScript.Instance.GetGridInfo(positionGrid);
        m_nextPosition = gridInfo.ObjectTf.position;
        GridPosition = gridInfo.PositionGrid;
        transform.position = m_nextPosition;
    }
}