using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : Singleton<LoadSceneManager>
{
    [SerializeField]
    private GameObject _uiPanel;
    
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private Image _slider;

    private string m_nameSceneLoad;
    private bool m_startLoadNewScene;
    private bool m_endLoadNewScene;
    private bool m_isFinishLoad;
    private float m_velocitySlider;
    
    public override void Awake()
    {
        base.Awake();
        if (_canvasGroup)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = true;
        }
        if (_uiPanel)
        {
            _uiPanel.SetActive(false);
        }
        
        m_startLoadNewScene = false;
        m_endLoadNewScene = false;
    }

    public void LoadScene(string name)
    {
        m_nameSceneLoad = name;
        if (_canvasGroup)
        {
            _canvasGroup.alpha = 0f;
        }

        if (_slider)
        {
            _slider.fillAmount = 0f;
        }

        if (_uiPanel)
        {
            _uiPanel.SetActive(true);
        }
        m_startLoadNewScene = true;
        m_endLoadNewScene = false;
        m_isFinishLoad = false;
        m_velocitySlider = 0f;
    }

    private void Update()
    {
        if (_canvasGroup)
        {
            if (m_startLoadNewScene)
            {
                _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 1, 10 * Time.deltaTime);
                if (_canvasGroup.alpha > 0.9f)
                {
                    _canvasGroup.alpha = 1f;
                    m_startLoadNewScene = false;
                    StartCoroutine(OnLoadScene());
                }
            }else if (m_endLoadNewScene)
            {
                _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 0, 10 * Time.deltaTime);
                if (_canvasGroup.alpha < 0.1f)
                {
                    this.PostEvent(EventID.OnFinishLoadScene,m_nameSceneLoad);
                    m_endLoadNewScene = false;
                    if (_uiPanel)
                    {
                        _uiPanel.SetActive(false);
                    }
                }
            }else if (_slider)
            {
                if (!m_isFinishLoad)
                {
                    _slider.fillAmount = Mathf.SmoothDamp(_slider.fillAmount, 0.5f, ref m_velocitySlider,0.7f);
                }
                else
                {
                    _slider.fillAmount = Mathf.SmoothDamp(_slider.fillAmount, 1f, ref m_velocitySlider,0.3f);
                    if (_slider.fillAmount > 0.9f)
                    {
                        _slider.fillAmount = 1f;
                        m_endLoadNewScene = true;
                    }
                }
                
            }
        }
        
    }

    private IEnumerator OnLoadScene()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(m_nameSceneLoad);
        while (!load.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        m_isFinishLoad = true;
    }
}