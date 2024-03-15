using UnityEngine;

public class UIManagerLevel : MonoBehaviour
{
    [SerializeField]
    private GameObject _uiResultPanel;
    
    private void OnEnable()
    {
        this.RegisterListener(EventID.OnShowResult,OnShowResult);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnShowResult,OnShowResult);
    }

    private void Awake()
    {
        _uiResultPanel.SetActive(false);
    }

    private void OnShowResult(object obj)
    {
        if(obj == null || _uiResultPanel == null) return;
        _uiResultPanel.SetActive(true);
        if (_uiResultPanel.TryGetComponent(out UIResultPanelScript uiResultPanelScript))
        {
            uiResultPanelScript.OnShowPanel((int)obj);
        }
    }
}