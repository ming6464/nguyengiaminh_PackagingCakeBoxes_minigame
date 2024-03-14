using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        this.RegisterListener(EventID.OnPlayLevel,OnPlayLevel);
    }

    private void OnPlayLevel(object obj)
    {
        if(obj == null && GameConfig.Instance) return;
        SceneManager.LoadScene(GameConfig.Instance.NameLevelScene + (int)obj);
    }
}
