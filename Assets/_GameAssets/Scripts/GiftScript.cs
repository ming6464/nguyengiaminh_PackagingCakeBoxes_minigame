using UnityEngine;

public class GiftScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cake") && transform.position.y < other.transform.position.y)
        {
            if (other.transform.TryGetComponent(out ObjectOnGrid movementGrid))
            {
                movementGrid.OnDead();
            }
        }
    }
}