using UnityEngine;

public class UIManagerHome : MonoBehaviour
{
    [SerializeField]
    private GameObject _menuLevelPanel;
    [SerializeField]
    private GameObject _homePanel;

    private void OnEnable()
    {
        this.RegisterListener(EventID.OnOpenMenuLevelPanel,OnOpenMenuLevelPanel);
        this.RegisterListener(EventID.OnOpenHomePanel,OnOpenHomePanel);
        this.RegisterListener(EventID.OnNextLevel,OnNextLevel);
        this.RegisterListener(EventID.OnOpenQuestionPanel,OnOpenQuestionPanel);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnOpenMenuLevelPanel,OnOpenMenuLevelPanel);
        EventDispatcher.Instance.RemoveListener(EventID.OnOpenHomePanel,OnOpenHomePanel);
        EventDispatcher.Instance.RemoveListener(EventID.OnNextLevel,OnNextLevel);
        EventDispatcher.Instance.RemoveListener(EventID.OnOpenQuestionPanel,OnOpenQuestionPanel);
    }

    private void OnOpenQuestionPanel(object obj)
    {
        Debug.Log("OnOpenQuestionPanel");
    }

    private void OnNextLevel(object obj)
    {
        Debug.Log("OnNextLevel");
    }

    private void Start()
    {
        OnOpenHomePanel(null);
    }

    private void OnOpenHomePanel(object obj)
    {
        OpenPanel(_homePanel);
    }

    private void OnOpenMenuLevelPanel(object obj)
    {
        OpenPanel(_menuLevelPanel);
    }

    private void OpenPanel(GameObject panel)
    {
        _menuLevelPanel.SetActive(false);
        _homePanel.SetActive(false);
        
        panel.SetActive(true);
    }
}
