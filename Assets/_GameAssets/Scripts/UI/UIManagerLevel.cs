using UnityEngine;

public class UIManagerLevel : MonoBehaviour
{
    [SerializeField]
    private PopupAnimScript _uiResultPanel;
    
    private void OnEnable()
    {
        this.RegisterListener(EventID.OnShowResult,OnShowResult);
        this.RegisterListener(EventID.OnResetStep,OnResetStep);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnShowResult,OnShowResult);
        EventDispatcher.Instance.RemoveListener(EventID.OnResetStep,OnResetStep);
    }

    private void OnResetStep(object obj)
    {
        if (_uiResultPanel)
        {
            _uiResultPanel.ClosePopup();
        }
    }


    private void OnShowResult(object obj)
    {
        if(obj == null || _uiResultPanel == null) return;
        _uiResultPanel.OpenPopup();
        if (_uiResultPanel.TryGetComponent(out UIResultPanelScript uiResultPanelScript))
        {
            uiResultPanelScript.OnShowPanel((int)obj);
        }
    }

    private void Awake()
    {
        _uiResultPanel.ClosePopup();
    }
    
}