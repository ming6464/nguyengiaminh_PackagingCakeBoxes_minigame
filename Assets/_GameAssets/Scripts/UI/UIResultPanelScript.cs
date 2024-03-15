using UnityEngine;

public class UIResultPanelScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _startsOn;

    [SerializeField]
    private GameObject _levelFailedObj;
    
    [SerializeField]
    private GameObject _levelCompletedObj;
    
    public void Home_button_on_click()
    {
        this.PostEvent(EventID.OnGoHomeScene);
    }

    public void Reset_button_on_click()
    {
        this.PostEvent(EventID.OnResetLevel);
    }
    
    public void Next_button_on_click()
    {
        this.PostEvent(EventID.OnNextLevel);
    }

    public void OnShowPanel(int startOnCount)
    {
        LoadStar(startOnCount);

        if (_levelFailedObj)
        {
            _levelFailedObj.SetActive(startOnCount < 1);
        }
        
        if (_levelCompletedObj)
        {
            _levelCompletedObj.SetActive(startOnCount >= 1);
        }
    }

    private void LoadStar(int startOnCount)
    {
        for (int i = 0; i < _startsOn.Length; i++)
        {
            _startsOn[i].SetActive(startOnCount > i);
        }
    }
}
