using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextBehaviour : MonoBehaviour
{
    private float timer = 0.0f;
    private float startTime;
    
    public TextMeshProUGUI m_text;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI highscoreText;
    private float highscore;
    public static bool countdownFinished = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Component[] cmps = GetComponents<Component>();

        if (countdownText != null) {
            StartCoroutine(CountdownCoroutine());
            countdownText.SetText("Go!");
        }

        highscore = PlayerPrefs.GetFloat("Highscore", 0.0f);
        if (highscoreText != null) {
            int minutes = Mathf.FloorToInt(highscore / 60);
            int seconds = Mathf.FloorToInt(highscore % 60);
            highscoreText.SetText("Highscore: \n{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer = Time.time - startTime;
        //Debug.Log("time thus far: " + timer);

        if (countdownFinished)
        {
            timer = Time.time - startTime;

            if (m_text != null) {
                int minutes = Mathf.FloorToInt(timer/60);
                int seconds = Mathf.FloorToInt(timer % 60);
                string timeLabel = 
                string.Format("<color=black>Time: <color=#8B0000>{0:00}:<color=#8B0000>{1:00}", minutes, seconds);
                m_text.SetText(timeLabel);

                if (timer >= highscore) {
                    highscore = timer;
                    PlayerPrefs.SetFloat("Highscore", highscore);
                    highscoreText.SetText("Highscore: \n{0:00}:{1:00}", minutes, seconds);
                }

                if (timer >= 2.0f) {
                    countdownText.SetText("");
                }
            }
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        float countdownTimer = 4f;
        while (countdownTimer > 1f)
        {
            int seconds = Mathf.FloorToInt(countdownTimer);
            countdownText.SetText(seconds.ToString());
            countdownTimer -= Time.deltaTime;
            yield return null;
        }

        countdownText.SetText("Go!");
        countdownFinished = true;
        startTime = Time.time;
    }

}
