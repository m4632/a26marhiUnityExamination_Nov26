using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CoinManager manager = FindFirstObjectByType<CoinManager>();

            manager.AddCoin();

            Destroy(gameObject);
        }
    }
}