using UnityEngine;

public class HomePanelScript : MonoBehaviour
{
    public void Play_button_on_click()
    {
        this.PostEvent(EventID.OnOpenMenuLevelPanel);
    }

    public void Question_button_on_click()
    {
        this.PostEvent(EventID.OnOpenQuestionPanel);
    }
}