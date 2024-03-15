using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public Vector2 GridPosition;
    private bool m_finishStepUpMap;
    
    private void OnEnable()
    {
        if (EventDispatcher.Instance)
        {
            this.RegisterListener(EventID.FinishSetupMap,OnFinishSetupMap);
        }
        
    }
    
    private void OnDisable()
    {
        if (EventDispatcher.Instance)
        {
            EventDispatcher.Instance.RemoveListener(EventID.FinishSetupMap,OnFinishSetupMap);
        }
    }
    
    private void OnFinishSetupMap(object obj)
    {
        if(m_finishStepUpMap) return;
        if(MapScript.Instance == null) return;
        GridInfo gridInfo = MapScript.Instance.GetGridInfo(GridPosition);
        if (gridInfo == null)
        {
            return;
        }
        transform.position = gridInfo.ObjectTf.position;
        m_finishStepUpMap = true;
    }
    
}