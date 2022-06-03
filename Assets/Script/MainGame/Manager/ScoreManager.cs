using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    [Header ("GameProgressing")]
    [SerializeField]
    Text m_distLabel;
    [SerializeField]
    Text m_coinLabel;
    [SerializeField]
    Text m_scoreLabel;

    [Header ("Result_Labels")]
    [SerializeField]
    Text m_GameOverDist;
    [SerializeField]
    Text m_GameOverCoin;
    [SerializeField]
    Text m_GameOverScore;
    [SerializeField]
    Text m_GameOverFinal;

    public float m_dist { get; set; }
    public int m_coin { get; set; }
    public int m_iScore = 0;
// Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Player_Coin") > 0)
        {
            m_coin = PlayerPrefs.GetInt("Player_Coin");
        }
        else
            m_coin = 0;
    }

    public void InitiateScore()
    {
        if(m_dist > 0)
        {
            m_dist = 0;
        }
        if(m_iScore > 0)
        {
            m_iScore = 0;
        }
    }
    public void SetGameOverScore()
    {
        int m_final = m_iScore + (int)m_dist;

        m_GameOverCoin.text = string.Format("{0:d}",
            m_coin);
        m_GameOverDist.text = string.Format("{0:f1} M",
           m_dist);
        m_GameOverScore.text = string.Format("{0:d}",
            m_iScore);
        m_GameOverFinal.text = string.Format("{0:d}",
            m_final);

        PlayerPrefs.SetInt("Player_Coin", m_coin);
        PlayerPrefs.SetInt("Player_Score", m_iScore);
        PlayerPrefs.SetInt("Player_Final_Score", m_final);
        PlayerPrefs.SetFloat("Player_Distance", m_dist);
        PlayerPrefs.Save();
    }
    public void ProcessScore()
    {
        m_dist += Time.deltaTime * GameManager.Instance.GetInvincibleSpeed();
        m_distLabel.text = string.Format("{0:f1} M", m_dist);
        m_coinLabel.text = string.Format("{0:d}", m_coin);
        m_scoreLabel.text = string.Format("{0:d}", m_iScore);
    }
}
