using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameplayPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TxtScore;
    [SerializeField] private Image m_ImgHpBar;

    private void OnEnable()
    {
        GameManager.Instance.onScoreChanged += onScoreChanged;
        SpawnManager.Instance.Player.OnHPChanged += OnHpChanged;
    }

    private void OnDisable()
    {
        GameManager.Instance.onScoreChanged -= onScoreChanged;
        SpawnManager.Instance.Player.OnHPChanged -= OnHpChanged;
    }

    private void OnHpChanged(int curHp, int maxHp)
    {
        m_ImgHpBar.fillAmount = curHp * 1f / maxHp;
    }

    public void BtnPause_Pressed()
    {
        GameManager.Instance.Pause();
    }
    
    private void onScoreChanged(int score)
    {
        m_TxtScore.text = "SCORE :" + score;
    }
}
