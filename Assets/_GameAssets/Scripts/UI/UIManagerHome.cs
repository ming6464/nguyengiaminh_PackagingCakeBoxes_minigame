using System;
using UnityEngine;

public class UIManagerHome : MonoBehaviour
{
    [SerializeField]
    private PopupAnimScript _menuLevelPanel;
    [SerializeField]
    private PopupAnimScript _homePanel;
    [SerializeField]
    private PopupAnimScript _questionPanel;

    [SerializeField]
    private LoadOpenPanel _loadOpenPanel;
    
    [SerializeField]
    private GameObject _UI;

    private void Awake()
    {
        bool check = false;
        if (Utils.FIRST_OPEN_APP)
        {
            Utils.FIRST_OPEN_APP = false;
            check = true;
        }
        if(_loadOpenPanel)_loadOpenPanel.gameObject.SetActive(check);
    }

    private void OnEnable()
    {
        this.RegisterListener(EventID.OnOpenMenuLevelPanel,OnOpenMenuLevelPanel);
        this.RegisterListener(EventID.OnOpenHomePanel,OnOpenHomePanel);
        this.RegisterListener(EventID.OnOpenQuestionPanel,OnOpenQuestionPanel);
        this.RegisterListener(EventID.OnPlayLevel,DelayDisableUi);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnOpenMenuLevelPanel,OnOpenMenuLevelPanel);
        EventDispatcher.Instance.RemoveListener(EventID.OnOpenHomePanel,OnOpenHomePanel);
        EventDispatcher.Instance.RemoveListener(EventID.OnOpenQuestionPanel,OnOpenQuestionPanel);
        EventDispatcher.Instance.RemoveListener(EventID.OnPlayLevel,DelayDisableUi);
    }
    
    
    private void DelayDisableUi(object obj)
    {
        Invoke(nameof(DisableUI),.1f);
    }

    private void DisableUI()
    {
        if(_UI) _UI.SetActive(false);
    }

    private void OnOpenQuestionPanel(object obj)
    {
        OpenPanel(_questionPanel);
    }
    private void Start()
    {
        OnOpenHomePanel(null);
        if (_menuLevelPanel.TryGetComponent(out MenuLevelPanelScript menuLevelPanelScript))
        {
            menuLevelPanelScript.LoadData();
        }
    }

    private void OnOpenHomePanel(object obj)
    {
        OpenPanel(_homePanel);
    }

    private void OnOpenMenuLevelPanel(object obj)
    {
        OpenPanel(_menuLevelPanel);
    }

    private void OpenPanel(PopupAnimScript popup)
    {
        _menuLevelPanel.ClosePopup();
        _homePanel.ClosePopup();
        _questionPanel.ClosePopup();
        
        popup.OpenPopup();
    }
}
