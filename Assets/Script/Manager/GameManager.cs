using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    //[System.Serializable]
    //public class game_ui
    //{
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
    public enum GameState
    {
        Normal = 0,
        GameOver,
        Invincible

    }
    GameState m_state;
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
    }
    protected override void OnStart()
    {
        base.OnStart();
        m_state = GameState.Normal;
        ChangeUIActivation();
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

    }
}
