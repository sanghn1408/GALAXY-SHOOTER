using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HomePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TxtHighScore;

    private void OnEnable()
    {
        m_TxtHighScore.text = "HIGH SCORE: " + PlayerPrefs.GetInt("HighScore");
    }
    
    void Start()
    {
        //m_GameManager = FindObjectOfType<GameManager>();
    }

    public void BtnPlay_Pressed()
    {
        GameManager.Instance.Play();
    }
}
