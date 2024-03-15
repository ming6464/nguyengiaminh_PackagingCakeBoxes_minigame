using System;
using System.Collections.Generic;
using UnityEngine;

public class StepGameManager : MonoBehaviour
{
    public int CurrentObjAlive;
    public List<int> ObjAlivePerStep;

    private int m_objDeadOnStep;
    private int m_objSaveStep;
    private List<StepObjectInfo> m_stepObjectInfos;

    private void Awake()
    {
        ObjAlivePerStep = new List<int>();
    }

    private void OnEnable()
    {
        this.RegisterListener(EventID.OnBackStep,OnBackStep);
        this.RegisterListener(EventID.OnResetStep,OnResetStep);
        this.RegisterListener(EventID.OnSetUpStep,OnSetUpStep);
        this.RegisterListener(EventID.OnSaveStep,OnSaveStep);
        this.RegisterListener(EventID.OnObjectStepDead,OnObjectStepDead);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnBackStep,OnBackStep);
        EventDispatcher.Instance.RemoveListener(EventID.OnResetStep,OnResetStep);
        EventDispatcher.Instance.RemoveListener(EventID.OnSetUpStep,OnSetUpStep);
        EventDispatcher.Instance.RemoveListener(EventID.OnSaveStep,OnSaveStep);
        EventDispatcher.Instance.RemoveListener(EventID.OnObjectStepDead,OnObjectStepDead);
    }

    private void OnObjectStepDead(object obj)
    {
        if(obj == null) return;
        m_stepObjectInfos[(int)obj].ObjectDead(GetCurrentStep());
        m_objDeadOnStep++;
    }

    private int GetCurrentStep()
    {
        return ObjAlivePerStep.Count - 1;
    }

    private void OnSetUpStep(object obj)
    {
        if(obj == null) return;
        if (m_stepObjectInfos == null) m_stepObjectInfos = new List<StepObjectInfo>();
        Transform objTf = (Transform)obj;
        StepObjectInfo stepInfo = new StepObjectInfo(objTf,m_stepObjectInfos.Count);
        stepInfo.SaveStep(objTf.GetComponent<ObjectOnGrid>().GridPosition);
        m_stepObjectInfos.Add(stepInfo);
        CurrentObjAlive++;
        if (ObjAlivePerStep.Count == 0)
        {
            ObjAlivePerStep.Add(0);
        }
        ObjAlivePerStep[0] = CurrentObjAlive;
    }

    private void OnSaveStep(object obj)
    {
        if(obj == null) return;
        MessageStep messageStep = (MessageStep)obj;
        if(messageStep.Index >= m_stepObjectInfos.Count) return;
        m_stepObjectInfos[messageStep.Index].SaveStep(messageStep.GridPosition);
        m_objSaveStep++;
        if (m_objSaveStep == CurrentObjAlive)
        {
            FinishStep();
        }
    }

    private void FinishStep()
    {
        CurrentObjAlive -= m_objDeadOnStep;
        ObjAlivePerStep.Add(CurrentObjAlive);
        this.PostEvent(EventID.OnFinishStep);
        m_objDeadOnStep = 0;
        m_objSaveStep = 0;
        this.PostEvent(EventID.UpdateBackStepButton,true);
    }

    private void Start()
    {
        this.PostEvent(EventID.UpdateBackStepButton,false);
    }

    private void OnResetStep(object obj)
    {
        if(GetCurrentStep() <= 0) return;
        CurrentObjAlive = ObjAlivePerStep[0];
        ObjAlivePerStep = new List<int>();
        ObjAlivePerStep.Add(CurrentObjAlive);
        foreach (StepObjectInfo step in m_stepObjectInfos)
        {
            step.ResetStep();
        }
        this.PostEvent(EventID.UpdateBackStepButton,false);
        this.PostEvent(EventID.OnUpdateCakeAlive,CurrentObjAlive - 1);
    }

    private void OnBackStep(object obj)
    {
        if(GetCurrentStep() <= 0) return;
        ObjAlivePerStep.RemoveAt(ObjAlivePerStep.Count - 1);
        CurrentObjAlive = ObjAlivePerStep[GetCurrentStep()];
        foreach (StepObjectInfo step in m_stepObjectInfos)
        {
            step.BackStep(GetCurrentStep());
        }
        this.PostEvent(EventID.UpdateBackStepButton,GetCurrentStep()  > 0);
        this.PostEvent(EventID.OnUpdateCakeAlive,CurrentObjAlive - 1);
        
    }
}

[Serializable]
public class StepObjectInfo
{
    
    
    private Transform m_objTf;
    private List<StepPosition> m_steps;
    private ObjectOnGrid mObjectOnScript;
    
    public StepObjectInfo(Transform objTf,int index)
    {
        m_steps = new List<StepPosition>();
        m_objTf = objTf;
        mObjectOnScript = m_objTf.GetComponent<ObjectOnGrid>();
        mObjectOnScript.IndexStepObject = index;
    }

    public void SaveStep(Vector2 GridPosition)
    {
        if(m_objTf == null || !m_objTf.gameObject.activeSelf) return;
        m_steps.Add(new StepPosition{Position = m_objTf.position,GridPosition = GridPosition});
    }

    public void BackStep(int step)
    {
        if(m_steps.Count == 0 || m_steps.Count <= step) return;
        m_objTf.gameObject.SetActive(true);
        mObjectOnScript.SetPosition(m_steps[step]);
        while (m_steps.Count > step + 1)
        {
            m_steps.RemoveAt(m_steps.Count - 1);
        }
    }

    public void ResetStep()
    {
        if(m_steps.Count == 0) return;
        Vector2 gridPosition = m_steps[0].GridPosition;
        mObjectOnScript.SetPosition(m_steps[0]);
        m_objTf.gameObject.SetActive(true);
        m_steps = new List<StepPosition>();
        SaveStep(gridPosition);
    }

    public void ObjectDead(int currentStep)
    {
        if (currentStep < 0)
        {
            return;
        }
        while (m_steps.Count > currentStep + 1)
        {
            m_steps.RemoveAt(m_steps.Count - 1);
        }
    }
    
}
public class StepPosition
{
    public Vector3 Position;
    public Vector2 GridPosition;
}

public class MessageStep
{
    public int Index;
    public Vector2 GridPosition;
}
