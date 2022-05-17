using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    BgScroll m_bgScroll;
    public enum GameState
    {
        Normal = 0,
        Invincible
    }
    GameState m_state;
    bool isSet;
    float m_InvincibleMagPower = 5.0f;
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
        m_bgScroll.m_speed *= m_InvincibleMagPower;
        MonsterManager.Instance.SetMonsterSpeedInvincible(m_InvincibleMagPower);
        PlayerManager.Instance.SetInvincible();
        MonsterManager.Instance.ResetCreateMonster();
        isSet = false;
    }
    private void SetNormalMode()
    {
        m_bgScroll.m_speed /= m_InvincibleMagPower;
        MonsterManager.Instance.SetMonsterSpeedNormal(m_InvincibleMagPower);
        MonsterManager.Instance.ResetCreateMonster();
        isSet = false;
    }
    protected override void OnStart()
    {
        base.OnStart();
        m_state = GameState.Normal;
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
                case GameState.Invincible:
                    SetInvincibleMode();
                    break;
            }
        }
    }
}
