using UnityEngine;

public class UIManagerLevel : MonoBehaviour
{
    [SerializeField]
    private PopupAnimScript _uiResultPanel;

    [SerializeField]
    private GameObject _UI;
    
    private void OnEnable()
    {
        this.RegisterListener(EventID.OnShowResult,OnShowResult);
        this.RegisterListener(EventID.OnResetStep,OnResetStep);
        this.RegisterListener(EventID.OnGoHomeScene,DelayDisableUi);
        this.RegisterListener(EventID.OnPlayNextLevel,DelayDisableUi);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnShowResult,OnShowResult);
        EventDispatcher.Instance.RemoveListener(EventID.OnResetStep,OnResetStep);
        EventDispatcher.Instance.RemoveListener(EventID.OnGoHomeScene,DelayDisableUi);
        EventDispatcher.Instance.RemoveListener(EventID.OnPlayNextLevel,OnResetStep);
    }

    private void DelayDisableUi(object obj)
    {
        Invoke(nameof(DisableUI),.1f);
    }

    private void DisableUI()
    {
        if(_UI) _UI.SetActive(false);
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