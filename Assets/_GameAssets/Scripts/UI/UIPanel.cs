using TMPro;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _timeText;
    
    [SerializeField]
    private GameObject _backStepButton;
    
    private void OnEnable()
    {
        this.RegisterListener(EventID.OnChangeTime,OnChangeTime);
        this.RegisterListener(EventID.UpdateBackStepButton,UpdateBackStepButton);

    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnChangeTime,OnChangeTime);
        EventDispatcher.Instance.RemoveListener(EventID.UpdateBackStepButton,UpdateBackStepButton);

    }

    private void UpdateBackStepButton(object obj)
    {
        if(obj == null) return;
        if(_backStepButton) _backStepButton.SetActive((bool)obj);
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
        if(!GameManager.Instance || GameManager.Instance.IsFinishGame) return;
        this.PostEvent(EventID.OnResetStep);
    }

    public void Back_step_button_on_click()
    {
        if(!GameManager.Instance || GameManager.Instance.IsFinishGame) return;
        this.PostEvent(EventID.OnBackStep);
    }
}
