using UnityEngine;

public class GiftScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cake"))
        {
            other.gameObject.SetActive(false);
        }
    }
}