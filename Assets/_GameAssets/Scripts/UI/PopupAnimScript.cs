using System;
using UnityEngine;

public class PopupAnimScript : MonoBehaviour
{
    [Serializable]
    public struct ScalePopupInfo
    {
        public float TimeScale;
        public Vector3 ScaleStart;
        public Vector3 ScaleEnd;
        public bool BeginWithScaleStart;
    }
    
    [Header("Popup Setup")]
    [SerializeField]
    protected GameObject _popupObj;
    [SerializeField]
    protected CanvasGroup _popupCG;
    [SerializeField]
    protected CanvasGroup _backgroundCG;

    [Space(10)]
    [SerializeField]
    protected Transform _objScaleTf;

    [SerializeField]
    protected ScalePopupInfo[] _scalePopupInfos;

    protected int m_indexScalePopup;
    
    protected bool m_isOpenPopup;
    protected bool m_isClosePopup;

    protected ScalePopupInfo m_scaleInfoTarget;

    private Vector3 m_velocityScale;

    private void Update()
    {
        if (m_isOpenPopup)
        {
            if (!_objScaleTf)
            {
                EndOpenPopup();
                return;
            }
            
            if (m_indexScalePopup < 0|| Vector2.Distance(_objScaleTf.localScale,m_scaleInfoTarget.ScaleEnd) <= .07f)
            {
                m_indexScalePopup++;
                if (m_indexScalePopup >= _scalePopupInfos.Length)
                {
                    if (_scalePopupInfos.Length > 0)
                    {
                        _objScaleTf.localScale = m_scaleInfoTarget.ScaleEnd;
                    }
                    EndOpenPopup();
                    return;
                }
                m_velocityScale = Vector3.zero;
                m_scaleInfoTarget = _scalePopupInfos[m_indexScalePopup];
                if (m_scaleInfoTarget.BeginWithScaleStart)
                {
                    _objScaleTf.localScale = m_scaleInfoTarget.ScaleStart;
                }
            }

            _objScaleTf.localScale = Vector3.SmoothDamp(_objScaleTf.localScale, m_scaleInfoTarget.ScaleEnd,
                ref m_velocityScale, m_scaleInfoTarget.TimeScale);


            if (_backgroundCG)
            {
                _backgroundCG.alpha = Mathf.Lerp(_backgroundCG.alpha, 1, Time.deltaTime * 10f);
            }
           
        }
        else if (m_isClosePopup)
        {
            if (_popupCG)
            {
                _popupCG.alpha = Mathf.Lerp(_popupCG.alpha, 0, Time.deltaTime * 10f);
                if (_popupCG.alpha <= 0.07f)
                {
                    EndClosePopup();
                }
            }
            else
            {
                EndClosePopup();
            }
        }
    }

    public virtual void OpenPopup()
    {
        StartOpenPopup();
    }
    public virtual void ClosePopup()
    {
        StartClosePopup();
    }
    protected virtual void StartOpenPopup()
    {
        if (_popupCG)
        {
            _popupCG.alpha = 1f;
        }

        if (_popupObj)
        {
            _popupObj.SetActive(true);
        }

        if (_backgroundCG)
        {
            _backgroundCG.alpha = 0f;
        }

        m_indexScalePopup = -1;

        m_isOpenPopup = true;
        m_isClosePopup = false;
    }
    protected virtual void EndOpenPopup()
    {
        m_isOpenPopup = false;
        if (_backgroundCG)
        {
            _backgroundCG.alpha = 1f;
        }
        
    }
    protected virtual void StartClosePopup()
    {
        m_isClosePopup = true;
        m_isOpenPopup = false;
    }
    protected virtual void EndClosePopup()
    {
        m_isClosePopup = false;

        if (_popupCG)
        {
            _popupCG.alpha = 0f;
        }
        
        if (_popupObj)
        {
            _popupObj.SetActive(false);
        }
        if (_backgroundCG)
        {
            _backgroundCG.alpha = 0f;
        }
    }
    
}

