using UnityEngine;

public class MenuLevelPanelScript : PopupAnimScript
{
    [SerializeField]
    private LevelPanelScript[] _levelScripts;
    

    public void Back_button_on_click()
    {
        this.PostEvent(EventID.OnOpenHomePanel);
    }

    public void LoadData()
    {
        foreach (LevelPanelScript levelScript in _levelScripts)
        {
            levelScript.LoadData();
        }
    }
    
}