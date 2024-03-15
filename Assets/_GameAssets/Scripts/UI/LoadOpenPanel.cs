using UnityEngine;
using UnityEngine.UI;

public class LoadOpenPanel : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _uiLoadCG;

    [SerializeField]
    private Image _slideLoad;

    [SerializeField]
    private float _timeLoadIn;

    [SerializeField]
    private float _timeLoadOut;

    private float m_velocityLoad;

    private int m_loadState;
    private void OnEnable()
    {
        if (_uiLoadCG)
        {
            _uiLoadCG.alpha = 1;
        }

        if (_slideLoad)
        {
            _slideLoad.fillAmount = 0;
        }

        m_velocityLoad = 0f;
        m_loadState = 0;
    }

    private void Update()
    {
        if (m_loadState == 0)
        {
            if (_slideLoad)
            {
                _slideLoad.fillAmount = Mathf.SmoothDamp(_slideLoad.fillAmount, 1f, ref m_velocityLoad, _timeLoadIn);
                if (_slideLoad.fillAmount >= 0.9f)
                {
                    _slideLoad.fillAmount = 1f;
                    m_loadState++;
                    m_velocityLoad = 0f;
                }
            }else
            {
                m_loadState++;
            }
        }
        else if(m_loadState == 1)
        {
            if (_uiLoadCG)
            {
                _uiLoadCG.alpha = Mathf.SmoothDamp(_uiLoadCG.alpha, 0f, ref m_velocityLoad, _timeLoadOut);
                if (_uiLoadCG.alpha <= .1f)
                {
                    _uiLoadCG.alpha = 0f;
                    m_loadState++;
                }
            } else
            {
                m_loadState++;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
