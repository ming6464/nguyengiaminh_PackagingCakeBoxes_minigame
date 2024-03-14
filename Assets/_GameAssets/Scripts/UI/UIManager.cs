using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _menuLevelPanel;
    [SerializeField]
    private GameObject _homePanel;

    private void OnEnable()
    {
        this.RegisterListener(EventID.OnOpenMenuLevelPanel,OnOpenMenuLevelPanel);
        this.RegisterListener(EventID.OnOpenHomePanel,OnOpenHomePanel);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnOpenMenuLevelPanel,OnOpenMenuLevelPanel);
        EventDispatcher.Instance.RemoveListener(EventID.OnOpenHomePanel,OnOpenHomePanel);
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
