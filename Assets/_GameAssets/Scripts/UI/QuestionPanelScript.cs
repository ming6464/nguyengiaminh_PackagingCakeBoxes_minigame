public class QuestionPanelScript : PopupAnimScript
{
    public void Back_button_on_click(){
        this.PostEvent(EventID.OnOpenHomePanel);
    }
}