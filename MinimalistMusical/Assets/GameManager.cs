using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager ms_instance;
    private int m_highScore;
    private int m_yourScore;

    [SerializeField]
    private Text m_score;

    private IEnumerator points;

    [SerializeField]
    private Text m_top;

    [SerializeField]
    private Text m_bottom;
    bool ended = false;

    // Use this for initialization
    void Start () {
        m_yourScore = 0;
        ms_instance = this;
        points = GainPoints();
        StartCoroutine(points);
        m_highScore = PlayerPrefs.GetInt("HighScore",0);
	}


    private IEnumerator GainPoints()
    {
        while (!Input.GetMouseButton(0) && Input.touchCount ==  0)
        {
            yield return new WaitForEndOfFrame();
        }

        ZombieHive.ms_instance.Activate();

        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            m_yourScore++;
            m_score.text = "" + Mathf.Abs(m_yourScore - m_highScore);
        }
    }

    private IEnumerator WaitForClick()
    {
        m_score.text = "";
        m_top.text = "";
        m_bottom.text = "";


        yield return new WaitForSeconds(1.0f);
        m_top.text = "HIGH SCORE:" + m_highScore;
        yield return new WaitForSeconds(1.0f);
        m_bottom.text = "YOUR SCORE:" + m_yourScore;
        yield return new WaitForSeconds(1.0f);
        m_score.text = "Click to play again";

        while (!Input.GetMouseButton(0) && Input.touchCount == 0)
        {
            yield return new WaitForEndOfFrame();
        }

        if (m_yourScore > m_highScore)
        {
            PlayerPrefs.SetInt("HighScore", m_yourScore);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        if(ended == false)
        {
            ended = true;
            StopCoroutine(points);
            StartCoroutine(WaitForClick());
            
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
