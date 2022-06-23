using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    BgScroll m_bgScroll;
    [SerializeField]
    GameObject[] inGame_UIlist;
    [SerializeField]
    Button restart_btn;
    [SerializeField]
    Button Home_btn;
    [SerializeField]
    MouseCursor m_MouseCursor;

    public enum GameState
    {
        Normal = 0,
        GameOver,
        Invincible

    }
    public GameState m_state;
    bool isSet;
    public bool isInvincible = false;
    float m_InvincibleMagPower = 5.0f;
    public bool isTimetoChangeMap;

    void ChangeUIActivation()
    {
        if(m_state == GameState.Normal)
        {
            inGame_UIlist[(int)GameState.Normal].SetActive(true);
            inGame_UIlist[(int)GameState.GameOver].SetActive(false); 
        }
        if(m_state == GameState.GameOver)
        {
            inGame_UIlist[(int)GameState.Normal].SetActive(false);
            inGame_UIlist[(int)GameState.GameOver].SetActive(true);
        }
    }

    private void InitializeBtnAction()
    {
        restart_btn.onClick.AddListener(SetRestartStatus);
        Home_btn.onClick.AddListener(SetHomeMenu);
    }

    public GameState GetGameState()
    {
        return m_state;
    }
    public float GetInvincibleSpeed()
    {
        if (m_state == GameState.Normal)
            return 1.0f;
        else
            return m_InvincibleMagPower ;
    }
    public void SetGameState(GameState state)
    {
        if (state == m_state)
            return;
        m_state = state;
        isSet = true;
    }
    private void SetInvincibleMode()
    {
        isInvincible = true;
        m_bgScroll.m_speed *= m_InvincibleMagPower;
        MonsterManager.Instance.SetMonsterSpeedInvincible(m_InvincibleMagPower);
        PlayerManager.Instance.SetInvincible();
        MonsterManager.Instance.ResetCreateMonster();
        isSet = false;
    }
    private void SetNormalMode()
    {
        isInvincible = false;
        m_bgScroll.m_speed /= m_InvincibleMagPower;
        MonsterManager.Instance.SetMonsterSpeedNormal(m_InvincibleMagPower);
        MonsterManager.Instance.ResetCreateMonster();
        isSet = false;
    }
    private void SetGameOver()
    {
        m_bgScroll.m_speed = 0;
        MonsterManager.Instance.StopMonsterLine();
        isSet = false;
        ScoreManager.Instance.SetGameOverScore();
        m_MouseCursor.ShowCursor();
    }

    protected override void OnStart()
    {
        base.OnStart();
        //float m_PlayerBGM_Vol = PlayerPrefs.GetFloat("BGM_Volume");
        //SoundManager.Instance.SetVolume(m_PlayerBGM_Vol);
        m_state = GameState.Normal;
        InitializeBtnAction();
        isSet = false;
        isTimetoChangeMap = true;
        ChangeUIActivation();
    }

    public void SetRestartStatus()
    {
        bool restartFlag = true;
        SetGameState(GameState.Normal);
        m_bgScroll.m_speed = 0.2f;
        ChangeUIActivation();
        PlayerManager.Instance.InitiatePlayer();
        ScoreManager.Instance.InitiateScore();
        MonsterManager.Instance.SetUpPool();
        Hearts.Instance.InitializeHeart();
        m_bgScroll.DoMapFadein(restartFlag);
        m_MouseCursor.HideCursor();
        isSet = false;
    }
    public void SetHomeMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    void Update()
    {
        if (isSet)
        {
            switch (m_state)
            {
                case GameState.Normal:
                    SetNormalMode();
                    break;
                case GameState.GameOver:
                    SetGameOver();
                    break;
                case GameState.Invincible:
                    SetInvincibleMode();
                    break;
            }
            ChangeUIActivation();
        }
    }
    void CheckDist()
    {
        int i_dist = (int)ScoreManager.Instance.m_dist;
        if (((i_dist >= 20 && i_dist < 21) && isTimetoChangeMap))
        {
            isTimetoChangeMap = false;
            m_bgScroll.DoMapFadeOut(BgScroll.StageType.Stage2);
            //Debug.Log("FadeOut Called!");
        }
        if (((i_dist >= 40 && i_dist < 41) && isTimetoChangeMap))
        {
            isTimetoChangeMap = false;
            m_bgScroll.DoMapFadeOut(BgScroll.StageType.Stage3);
            //Debug.Log("FadeOut2 Called!");
        }
    }
    void FixedUpdate()
    {
        CheckDist();
    }
}
