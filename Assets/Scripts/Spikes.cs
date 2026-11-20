using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    public float speed = 3f;
    public float distance = 5f;

    private Vector3 startPosition;
    private bool movingRight = true;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float movement = speed * Time.deltaTime;

        if (movingRight)
        {
            transform.Translate(Vector2.right * movement);

            if (transform.position.x >= startPosition.x + distance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * movement);

            if (transform.position.x <= startPosition.x)
            {
                movingRight = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}