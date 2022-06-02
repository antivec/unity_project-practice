using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    //[System.Serializable]
    //public class game_ui
    //{s
    //    public GameObject[] UIObject;
    //    Text dist;
    //    Text score;
    //    Text coin;
    //    public game_ui()
    //    {
    //        dist = UIObject.
    //    }
    //};
    [SerializeField]
    BgScroll m_bgScroll;
    [SerializeField]
    GameObject[] inGame_UIlist;
    [SerializeField]
    Text m_GameOverDist;
    [SerializeField]
    Text m_GameOverCoin;
    [SerializeField]
    Text m_GameOverScore;
    [SerializeField]
    Text m_GameOverFinal;
    [SerializeField]
    Button restart_btn;
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
        SetGameOverScore();
        m_MouseCursor.ShowCursor();
    }
    private void SetGameOverScore()
    {
        float m_fdist = PlayerManager.Instance.m_dist;
        int m_iScore = PlayerManager.Instance.m_iScore;
        int m_iCoin = PlayerManager.Instance.m_coin;
        int m_final = m_iScore + (int)m_fdist;

        m_GameOverCoin.text = string.Format("{0:d}",
            m_iCoin);
        m_GameOverDist.text = string.Format("{0:f1} M",
           m_fdist);
        m_GameOverScore.text = string.Format("{0:d}",
            m_iScore);
        m_GameOverFinal.text = string.Format("{0:d}",
            m_final);

        PlayerPrefs.SetInt("Player_Coin", m_iCoin);
        PlayerPrefs.SetInt("Player_Score", m_iScore);
        PlayerPrefs.SetInt("Player_Final_Score", m_final);
        PlayerPrefs.SetFloat("Player_Distance", m_fdist);
        PlayerPrefs.Save();
    }
    protected override void OnStart()
    {
        base.OnStart();
        m_state = GameState.Normal;
        ChangeUIActivation();
        restart_btn.onClick.AddListener(SetRestartStatus);
        isSet = false;

    }

    public void SetRestartStatus()
    {
        SetGameState(GameState.Normal);
        m_bgScroll.m_speed = 0.2f;
        ChangeUIActivation();
        PlayerManager.Instance.InitiateGame();
        MonsterManager.Instance.SetUpPool();
        Hearts.Instance.InitializeHeart();
        //ItemManager.Instance.setItemPool();
        m_bgScroll.DoMapFadein();
        m_MouseCursor.HideCursor();
        isSet = false;
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
        if(PlayerManager.Instance.m_dist >= 10.0f && 
            PlayerManager.Instance.m_dist < 10.1f)
        {
            Debug.Log("Map FadeOut!!");
            m_bgScroll.DoMapFadeOut();
        }
    }
}
