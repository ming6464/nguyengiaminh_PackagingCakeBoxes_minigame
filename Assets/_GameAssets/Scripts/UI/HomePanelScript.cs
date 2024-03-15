
public class HomePanelScript : PopupAnimScript
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