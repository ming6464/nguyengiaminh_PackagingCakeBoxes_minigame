using System;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [Header("Info swipe")]
    [SerializeField]
    private float _minDistance = .2f;

    [SerializeField]
    private float _maxTime = 1f;

    [SerializeField, Range(0,1f)]
    private float _directionThreshold = .85f;
    
    private InputManager m_inputManager;
    private Vector2 m_startPostion;
    private float m_startTime;
    private Vector2 m_endPostion;
    private float m_endTime;
    private void Awake()
    {
        if (InputManager.Instance) m_inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        if (m_inputManager)
        {
            m_inputManager.OnStartTouch += SwipeStart;
            m_inputManager.OnEndTouch += SwipeEnd;
        }
    }

    private void OnDisable()
    {
        if (m_inputManager)
        {
            m_inputManager.OnStartTouch -= SwipeStart;
            m_inputManager.OnEndTouch -= SwipeEnd;
        }
    }

    private void SwipeStart(Vector2 position, float time)
    {
        if(!GameManager.Instance || GameManager.Instance.IsFinishGame || !GameManager.Instance.FinishStep) return;
        m_startPostion = position;
        m_startTime = time;
    }
    
    private void SwipeEnd(Vector2 position, float time)
    {
        if(!GameManager.Instance || GameManager.Instance.IsFinishGame || !GameManager.Instance.FinishStep) return;
        m_endPostion = position;
        m_endTime = time;
        if(Vector2.Distance(m_endPostion,m_startPostion) < _minDistance ||  (m_endTime - m_startTime) > _maxTime) return;
        CheckSwipeDirection(m_endPostion - m_startPostion);
        m_endPostion = Vector2.zero;
        m_startPostion = Vector2.zero;
    }

    private void CheckSwipeDirection(Vector2 dir)
    {
        if (Vector2.Dot(Vector2.up, dir) >= _directionThreshold)
        {
            this.PostEvent(EventID.OnSwipe,SwipeKey.Up);
        }
        else if (Vector2.Dot(Vector2.down, dir) >= _directionThreshold)
        {
            this.PostEvent(EventID.OnSwipe,SwipeKey.Down);
        }
        else if (Vector2.Dot(Vector2.right, dir) >= _directionThreshold)
        {
            this.PostEvent(EventID.OnSwipe,SwipeKey.Right);
        }
        else if (Vector2.Dot(Vector2.left, dir) >= _directionThreshold)
        {
            this.PostEvent(EventID.OnSwipe,SwipeKey.Left);
        }
    }
}

[Serializable]
public enum SwipeKey
{
    Up,Down,Right,Left
}