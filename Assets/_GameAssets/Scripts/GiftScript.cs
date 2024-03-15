using System;
using UnityEngine;

public class GiftScript : MonoBehaviour
{
    private ObjectOnGrid m_objectGrid;

    private void Awake()
    {
        TryGetComponent(out m_objectGrid);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Cake"))
        {
            if (other.transform.TryGetComponent(out ObjectOnGrid movementGrid))
            {
                movementGrid.OnDead();
                if (m_objectGrid)
                {
                    m_objectGrid.IsChecked = true;
                }
            }
        }
    }
}