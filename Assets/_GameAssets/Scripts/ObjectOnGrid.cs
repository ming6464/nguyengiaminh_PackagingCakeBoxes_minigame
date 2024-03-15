using UnityEngine;
using UnityEngine.UI;

public class ObjectOnGrid : MonoBehaviour
{
    public Vector2 GridPosition;
    public bool IsGift;
    public float DistanceRayCheck = 0.5f;
    public int IndexStepObject;
    public bool IsDead;
    public bool IsMoving => m_isMovement;
    public bool IsChecked;
    
    [SerializeField]
    private Image _imageObject;
    
    [SerializeField]
    private Collider2D _collider2D;
    
    private Vector3 m_nextPosition;
    private bool m_isFinishSetupMap;
    private MoveKey m_swipeKeyNew;
    private bool m_isMovement;
    private bool m_finishStep;
    private float m_speed = 15f;
    private bool m_skipFrame;
    private void Awake()
    {
        if (_imageObject)
        {
            _imageObject.enabled = false;
        }
        
        if (_collider2D)
        {
            _collider2D.enabled = false;
        }
    }


    private void OnEnable()
    {
        if (EventDispatcher.Instance)
        {
            this.RegisterListener(EventID.OnMove,OnMove);
            this.RegisterListener(EventID.FinishSetupMap,OnFinishSetupMap);
        }
        
    }
    
    private void OnDisable()
    {
        if (EventDispatcher.Instance)
        {
            EventDispatcher.Instance.RemoveListener(EventID.OnMove,OnMove);
            EventDispatcher.Instance.RemoveListener(EventID.FinishSetupMap,OnFinishSetupMap);
        }
    }

    private void Start()
    {
        if (GameConfig.Instance)
        {
            m_speed = GameConfig.Instance.MoveSpeedObjectGrid;
        }
    }

    private void OnFinishSetupMap(object obj)
    {
        if(m_isFinishSetupMap) return;
        if(MapScript.Instance == null) return;
        GridInfo gridInfo = MapScript.Instance.GetGridInfo(GridPosition);
        if (gridInfo == null)
        {
            return;
        }

        m_nextPosition = gridInfo.ObjectTf.position;
        transform.position = m_nextPosition;
        m_isFinishSetupMap = true;
        m_finishStep = true;
        this.PostEvent(EventID.OnSetUpStep,transform);
        m_isMovement = false;
        if (_imageObject)
        {
            _imageObject.enabled = true;
        }

        if (_collider2D)
        {
            _collider2D.enabled = true;
        }
        
    }

    private void Update()
    {
        if(!m_isFinishSetupMap) return;
        if (transform.position != m_nextPosition)
        {
            m_isMovement = true;
            if (m_skipFrame)
            {
                m_skipFrame = false;
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, m_nextPosition, Time.deltaTime * m_speed);
        }
        else
        {
            m_isMovement = false;
            if (!m_finishStep)
            {
                m_finishStep = true;
                if (IsDead)
                {
                    gameObject.SetActive(false);
                    this.PostEvent(EventID.OnObjectStepDead,IndexStepObject);
                }
                this.PostEvent(EventID.OnSaveStep,new MessageStep{Index = IndexStepObject,GridPosition = GridPosition});
            }
        }
    }

    private void LateUpdate()
    {
        if (!IsDead && m_isMovement)
        {
            Transform objTf = CheckRay();
            if (objTf != null)
            {
                LoadPositionCollider(objTf);
            }
        }
    }

    private Transform CheckRay()
    {
        Vector3 directRay;
        switch (m_swipeKeyNew)
        {
            case MoveKey.Up:
                directRay = transform.up;
                break;
            case MoveKey.Down:
                directRay = Quaternion.Euler(0,0,180) * transform.up;
                break;
            case MoveKey.Right:
                directRay = transform.right;
                break;
            default:
                directRay = Quaternion.Euler(0,0,180) * transform.right;
                break;
        }
        foreach (RaycastHit2D raycastHit2D in Physics2D.RaycastAll(transform.position, directRay, DistanceRayCheck))
        {
            Transform rayTf = raycastHit2D.transform;
            if(rayTf.GetInstanceID() == transform.GetInstanceID()) continue;
            return rayTf;
        }

        return null;
    }

    public void SetPosition(StepPosition stepPosition)
    {
        m_nextPosition = stepPosition.Position;
        transform.position = m_nextPosition;
        GridPosition = stepPosition.GridPosition;
        m_isMovement = false;
        m_finishStep = true;
        if (_imageObject)
        {
            _imageObject.enabled = true;
        }
        
        if (_collider2D)
        {
            _collider2D.enabled = true;
        }
        IsDead = false;
    }

    public void OnDead()
    {
        if(IsDead) return;
        IsDead = true;
        
        if (_collider2D)
        {
            _collider2D.enabled = false;
        }
        if (_imageObject)
        {
            _imageObject.enabled = false;
        }
        
        this.PostEvent(EventID.OnReducedCake);
        
        if (m_finishStep)
        {
            gameObject.SetActive(false);
            this.PostEvent(EventID.OnObjectStepDead,IndexStepObject);
        }

        if (VFXScript.Instance)
        {
            VFXScript.Instance.PlayVfx(transform.position);
        }
    }
    
    private void OnMove(object obj)
    {
        if(!m_isFinishSetupMap || IsDead || obj == null || MapScript.Instance == null) return;
        MoveKey key = (MoveKey)obj;
        m_swipeKeyNew = key;
        GridInfo gridInfo = MapScript.Instance.GetNextGridInfo(GridPosition, m_swipeKeyNew);
        if (gridInfo == null)
        {
            return;
        }

        IsChecked = false;
        m_finishStep = false;
        if (gridInfo.ObjectTf.position != transform.position)
        {
            Transform objTf = CheckRay();
            if (objTf != null && objTf.CompareTag("Obstacle"))
            {
                return;
            }
            GridPosition = gridInfo.PositionGrid;
            m_nextPosition = gridInfo.ObjectTf.position;
            m_skipFrame = true;
        }
        
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position,transform.up * DistanceRayCheck,Color.green);
        Debug.DrawRay(transform.position,Quaternion.Euler(0,0,180) * transform.up * DistanceRayCheck,Color.green);
        Debug.DrawRay(transform.position,transform.right * DistanceRayCheck,Color.green);
        Debug.DrawRay(transform.position, Quaternion.Euler(0,0,180) * transform.right * DistanceRayCheck,Color.green);
    }
    private void LoadPositionCollider(Transform other)
    {
        if(!m_isMovement || MapScript.Instance == null) return;

        Transform otherTf = other.transform;
        
        if (!other.CompareTag("Obstacle"))
        {
            if (otherTf.TryGetComponent(out ObjectOnGrid objectOnGrid))
            {
                if (objectOnGrid.IsMoving)
                {
                    return;
                }
                
                if (m_swipeKeyNew is MoveKey.Up)
                {
                    if (IsGift && !IsChecked)
                    {
                        return;
                    }
                    
                }else if (m_swipeKeyNew is MoveKey.Down)
                {
                    if (other.CompareTag("Gift") && !objectOnGrid.IsChecked)
                    {
                        return;
                    }
                }
            }
        }
        
        GridInfo gridInfo = MapScript.Instance.GetGridInfoFromPosition(other.transform.position);
        if(gridInfo == null) return;
        Vector2 positionGrid = gridInfo.PositionGrid;
        switch (m_swipeKeyNew)
        {
            case MoveKey.Up:
                positionGrid.y -= 1;
                break;
            case MoveKey.Down:
                positionGrid.y += 1;
                break;
            case MoveKey.Right:
                positionGrid.x -= 1;
                break;
            default:
                positionGrid.x += 1;
                break;
        }

        gridInfo = MapScript.Instance.GetGridInfo(positionGrid);
        if(gridInfo == null) return;
        m_nextPosition = gridInfo.ObjectTf.position;
        GridPosition = gridInfo.PositionGrid;
        transform.position = m_nextPosition;
    }
}