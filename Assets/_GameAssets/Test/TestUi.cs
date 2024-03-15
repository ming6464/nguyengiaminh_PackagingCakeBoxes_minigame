using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUi : MonoBehaviour
{
    public PopupAnimScript PopupTest;

    public void Back_button_on_click()
    {
        PopupTest.ClosePopup();
    }

    public void Open_Popup_button_on_click()
    {
        PopupTest.OpenPopup();
    }
    
}
