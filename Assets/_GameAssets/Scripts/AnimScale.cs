using UnityEngine;

public class AnimScale : MonoBehaviour
{
    public Vector3 ScaleStart;
    public Vector3 ScaleEnd;
    public float TimeScale;
    public Transform ObjTfScale;
    public float TargetThreshold;
    
    private Vector3 m_velocityScale;
    private Vector3 m_targetScale;
    
    private void OnEnable()
    {
        if (ObjTfScale)
        {
            ObjTfScale.localScale = ScaleStart;
        }
        
        m_targetScale = ScaleEnd;
    }

    private void Update()
    {
        if (ObjTfScale)
        {
            ObjTfScale.localScale = Vector3.SmoothDamp(ObjTfScale.localScale ,m_targetScale,ref m_velocityScale,TimeScale);
            if (Vector3.Distance(ObjTfScale.localScale, m_targetScale) <= TargetThreshold)
            {
                if (m_targetScale == ScaleStart)
                {
                    m_targetScale = ScaleEnd;
                }
                else
                {
                    m_targetScale = ScaleStart;
                }

                m_velocityScale = Vector3.zero;
            }
        }
    }
}
