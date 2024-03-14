using UnityEngine;

public class MenuLevelPanelScript : MonoBehaviour
{
    [SerializeField]
    private LevelPanelScript[] _levelScripts;
    

    public void Back_button_on_click()
    {
        this.PostEvent(EventID.OnOpenHomePanel);
    }
    
}