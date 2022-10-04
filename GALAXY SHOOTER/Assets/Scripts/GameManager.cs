using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public  class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Home,
        GamePlay,
        Pause,
        GameOver
    }

    private static GameManager m_Instance;
    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<GameManager>();
            return m_Instance;
        }
    }

    public Action<int> onScoreChanged;

    [SerializeField] private HomePanel m_HomePanel;
    [SerializeField] private GameplayPanel m_GameplayPanel;
    [SerializeField] private PausePanel m_PausePanel;
    [SerializeField] private GameOverPanel m_GameOverPanel;
    [SerializeField] private WaveData[] m_Waves;

    //private AudioManager m_AudioManager;
    public GameState m_GameState;
    public bool m_Win;
    private int m_Score;
    private int m_CurWaveIndex;
    //private SpawnManager m_SpawnManager;


    private void Awake()
    {
        if(m_Instance == null)
        {
            m_Instance = this;
        }
        else if(m_Instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //m_AudioManager = FindObjectOfType<AudioManager>();
        //m_SpawnManager = FindObjectOfType<SpawnManager>();
        m_HomePanel.gameObject.SetActive(false);
        m_GameplayPanel.gameObject.SetActive(false);
        m_PausePanel.gameObject.SetActive(false);
        m_GameOverPanel.gameObject.SetActive(false);
        SetState(GameState.Home);

    }


    private void SetState(GameState state)
    {
        m_GameState = state;
        m_HomePanel.gameObject.SetActive(m_GameState == GameState.Home);
        m_GameplayPanel.gameObject.SetActive(m_GameState == GameState.GamePlay);
        m_PausePanel.gameObject.SetActive(m_GameState == GameState.Pause);
        m_GameOverPanel.gameObject.SetActive(m_GameState == GameState.GameOver);

        if (m_GameState == GameState.Pause)
        {
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;
        if(m_GameState == GameState.Home)
        {
            AudioManager.Instance.PlayHomeMusic();
        }
        else
        {
            AudioManager.Instance.PlayBattleMusic();
        }
    }
    public bool IsActive()
    {
        return m_GameState == GameState.GamePlay;
    }
    public void Play()
    {
        m_CurWaveIndex = 0;
        WaveData wave = m_Waves[m_CurWaveIndex];
        SpawnManager.Instance.StartBattle(wave,true);
        SetState(GameState.GamePlay);
        m_Score = 0;
        if(onScoreChanged != null)
        {
            onScoreChanged(m_Score);
        }
    }
    public void Pause()
    {
        SetState(GameState.Pause);
    }
    public void Home()
    {
        SetState(GameState.Home);
        SpawnManager.Instance.Clear();
    }
    public void Continue()
    {
        SetState(GameState.GamePlay);
    }
    //win game
    public void Gameover(bool win)
    {
        int curHighScore = PlayerPrefs.GetInt("HighScore");
        if (curHighScore < m_Score)
        {
            PlayerPrefs.SetInt("HighScore", m_Score);
            curHighScore = m_Score;
        }

        m_Win = win;
        SetState(GameState.GameOver);
        m_GameOverPanel.DisplayResult(m_Win);
        m_GameOverPanel.DisplayHighScore(curHighScore);
    }
    public void AddScore(int value)
    {
        m_Score += value;
        if (onScoreChanged != null)
        {
            onScoreChanged(m_Score);
        }
        if (SpawnManager.Instance.Isclear())
        {
            m_CurWaveIndex++;
            if(m_CurWaveIndex >= m_Waves.Length)
            {
                Gameover(true);
            }
            else
            {
                WaveData wave = m_Waves[m_CurWaveIndex];
                SpawnManager.Instance.StartBattle(wave, false);
            }
        }
    }       
}
