using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanelScript : MonoBehaviour
{
    public int Level;

    [Header("References")]
    [SerializeField]
    private GameObject _lockObj;

    [SerializeField]
    private TextMeshProUGUI _levelText;

    [SerializeField]
    private GameObject[] _starsOn;
    
    private Button m_button;
    private bool m_isUnLock;

    private void Awake()
    {
        TryGetComponent(out m_button);
    }

    private void OnEnable()
    {
        if (m_button != null) 
        {
            m_button.onClick.AddListener(OnClickButton);
        }
    }

    private void OnDisable()
    {
        if (m_button != null) 
        {
            m_button.onClick.RemoveAllListeners();
        }
    }
    

    public void LoadData()
    {
        if (GameConfig.Instance)
        {
            UpdateInfo(GameConfig.Instance.GetLevelInfoData(Level));
        }
    }

    private void OnClickButton()
    {
        if(!m_isUnLock) return;
        this.PostEvent(EventID.OnPlayLevel,Level);
    }

    private void UpdateInfo(LevelInfo levelInfo)
    {
        if (levelInfo == null)
        {
            m_isUnLock = false;
            LoadStar(0);
            if (_levelText)
            {
                _levelText.gameObject.SetActive(false);
            }
            if (_lockObj)
            {
                _lockObj.SetActive(true);
            }
            return;
        }

        m_isUnLock = levelInfo.IsUnlock;
        LoadStar(levelInfo.StarUnlock);
        if (_levelText)
        {
            _levelText.text = levelInfo.Level.ToString();
            _levelText.gameObject.SetActive(levelInfo.IsUnlock);
        }

        if (_lockObj)
        {
            _lockObj.SetActive(!levelInfo.IsUnlock);
        }
    }

    private void LoadStar(int starCount)
    {
        starCount = Mathf.Min(starCount, 3);
        for (int i = 0; i < _starsOn.Length ; i++)
        {
            _starsOn[i].SetActive(starCount > i);
        }
    }
    
}