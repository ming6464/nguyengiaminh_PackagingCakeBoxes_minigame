using TMPro;
using UnityEngine;

public class UITopPanelScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _timeText;

    private void OnEnable()
    {
        this.RegisterListener(EventID.OnChangeTime,OnChangeTime);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnChangeTime,OnChangeTime);
    }

    private void OnChangeTime(object obj)
    {
        if(obj == null || _timeText == null) return;
        int timeSecondTotal = (int)obj;
        int timeMinute = timeSecondTotal / 60;
        int timeSecond = timeSecondTotal % 60;
        string textTime = "";
        if (timeMinute < 10)
        {
            textTime = $"0{timeMinute}";
        }
        else
        {
            textTime = timeMinute.ToString();
        }

        textTime += " : ";
        
        if (timeSecond < 10)
        {
            textTime += $"0{timeSecond}";
        }
        else
        {
            textTime += timeSecond.ToString();
        }
        
        _timeText.text = textTime;
    }

    public void Home_button_on_click()
    {
         this.PostEvent(EventID.OnGoHomeScene);
    }

    public void Reset_button_on_click()
    {
        this.PostEvent(EventID.OnResetLevel);
    }
}
