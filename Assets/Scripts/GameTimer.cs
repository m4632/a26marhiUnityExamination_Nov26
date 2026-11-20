using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 20f;
    public TMP_Text timerText;

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            timerText.text = "Time: " +
                             Mathf.Ceil(timeRemaining).ToString();
        }
        else
        {
            timeRemaining = 0;
            SceneManager.LoadScene("menuscene");
        }
    }
}