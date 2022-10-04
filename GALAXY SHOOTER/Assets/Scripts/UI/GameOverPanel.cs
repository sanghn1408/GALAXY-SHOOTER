using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Txtresul;
    [SerializeField] private TextMeshProUGUI m_TxtHighScore;

    //private GameManager m_GaneManager;
    // Start is called before the first frame update
    void Start()
    {
        //m_GaneManager = FindObjectOfType<GameManager>();
    }

    public void BtnHome_Pressed()
    {
        GameManager.Instance.Home();
    }
    public void DisplayHighScore(int score)
    {
        m_TxtHighScore.text = "HIGHSCORE : " + score;
    }
    public void DisplayResult(bool iswin)
    {
        if (iswin)
            m_Txtresul.text = "YOU WIN";
        else
            m_Txtresul.text = "YOU LOSE";
    }
}
