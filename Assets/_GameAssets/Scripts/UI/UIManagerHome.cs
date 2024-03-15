using UnityEngine;

public class UIManagerHome : MonoBehaviour
{
    [SerializeField]
    private PopupAnimScript _menuLevelPanel;
    [SerializeField]
    private PopupAnimScript _homePanel;
    [SerializeField]
    private PopupAnimScript _questionPanel;

    private void OnEnable()
    {
        this.RegisterListener(EventID.OnOpenMenuLevelPanel,OnOpenMenuLevelPanel);
        this.RegisterListener(EventID.OnOpenHomePanel,OnOpenHomePanel);
        this.RegisterListener(EventID.OnOpenQuestionPanel,OnOpenQuestionPanel);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnOpenMenuLevelPanel,OnOpenMenuLevelPanel);
        EventDispatcher.Instance.RemoveListener(EventID.OnOpenHomePanel,OnOpenHomePanel);
        EventDispatcher.Instance.RemoveListener(EventID.OnOpenQuestionPanel,OnOpenQuestionPanel);
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
