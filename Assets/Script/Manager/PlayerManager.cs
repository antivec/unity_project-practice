﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerManager : SingletonMonoBehaviour<PlayerManager> {
     
    [SerializeField]
    GameObject m_projectile;
    [SerializeField]
    GameObject m_firePos;
    [SerializeField]
    GameObject m_bulletPool;
    [SerializeField]
    GameObject m_invincibleEffect;
    [SerializeField]
    GameObject m_magnetEffect;
    [SerializeField]
    Text m_distLabel;
    [SerializeField]
    Text m_coinLabel;
    [SerializeField]
    Text m_scoreLabel;

    List<Projectile> m_projectileList;
    GameObjectPool<GameObject> m_projectile_Pool;
    Animation m_animation;    

    float m_speed = 5f;
    float m_invincibleDuration = 3f;

    public int m_power { get; set; }
    public int m_coin { get; set; }
    public float m_dist { get; set; }
    bool m_col_left, m_col_right;
    bool m_clickOn;

    public int m_iScore;
    int m_playerLife;
    public void InitiateGame()
    {
        int num = 0;
        this.gameObject.SetActive(true);
        m_projectile_Pool = new GameObjectPool<GameObject>(10, () =>
        {
            ++num;
            GameObject obj = Instantiate(m_projectile) as GameObject;
            obj.name = "bullet" + num;
            obj.transform.parent = m_bulletPool.transform;
            return obj;
        });
        SoundManager.Instance.PlayBGM(SoundManager.BGM_CLIP.BGM_01);
        if(m_projectileList == null)
            m_projectileList = new List<Projectile>();
        m_invincibleEffect.SetActive(false);
        m_magnetEffect.SetActive(false);
        m_animation = GetComponent<Animation>();
        Invoke("OnShoot", 3f);
        m_playerLife = 3;
        m_power = 1;
        m_dist = 0;
        if (PlayerPrefs.GetInt("Player_Coin") > 0)
        {
            m_coin = PlayerPrefs.GetInt("Player_Coin");
        }
        else
            m_coin = 0;
        m_iScore = 0;

        // m_totalCoinLabel.text = string.Format("{0:n0}", PlayerDataManager.Instance.GetCoin());
    }
    protected override void OnStart() {
        InitiateGame();
    }
    void OnShoot()
    {
        // Get object from pool.
        GameObject obj = m_projectile_Pool.pop();
        obj.SetActive(true);
        //obj.transform.parent = m_firePos.transform;
        obj.transform.position = m_firePos.transform.position;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        Projectile item = obj.GetComponent<Projectile>();           
        m_projectileList.Add(item);
        Invoke("OnShoot", 0.25f);
    }
    public void RemoveProjectile(Projectile projectile)
    {
        m_projectileList.Remove(projectile);
        projectile.gameObject.SetActive(false);
        projectile.gameObject.transform.parent = m_bulletPool.transform;
        m_projectile_Pool.push(projectile.gameObject);
    }
    public void SetMagnet()
    {
        m_magnetEffect.SetActive(true);
        if (IsInvoking("ReleaseMagnet"))
            CancelInvoke("ReleaseMagnet");
        Invoke("ReleaseMagnet", 8.0f);
    }
    public void ReleaseMagnet()
    {
        m_magnetEffect.SetActive(false);
    }
    public void SetInvincible()
    {
        m_invincibleEffect.SetActive(true);
        m_animation.CrossFade("Invincible");
        Invoke("SetIdle", m_invincibleDuration);
        CancelInvoke("OnShoot");
    }
    public void SetIdle()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Normal);
        m_invincibleEffect.SetActive(false);
        m_animation.CrossFade("fly");
        OnShoot();
    }
    public void setDamaged()
    {
        if(m_playerLife > 1) 
        {
            this.gameObject.tag = "Damaged_Invincible";
            m_playerLife -= 1;
            Hearts.Instance.ModifyHeart(m_playerLife);
            m_animation.CrossFade("damaged_blink");

            Invoke("setRecovered", 2.0f);
        }
        else
        {
            m_playerLife = 0;
            Hearts.Instance.ModifyHeart(m_playerLife);
            CancelInvoke("OnShoot");
            GameManager.Instance.SetGameState(GameManager.GameState.GameOver);
            this.gameObject.SetActive(false);
        }
    }

    public void setRecovered()
    {
        m_animation.CrossFade("fly");
        this.gameObject.tag = "Player";
    }
    Vector3 startPos;
    Vector3 targetPos;
    Vector3 result;
    void MovePlayer()
    {
        float dir = Input.GetAxis("Horizontal");       
        if (Input.GetMouseButtonDown(0))
        {
            m_clickOn = true;
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);            
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_clickOn = false;
            startPos = Vector3.zero;
        }        
        if (m_clickOn)
        {
            Vector3 mousePos = Input.mousePosition;                
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //float reverse = 1.0f;
            dir = (targetPos.x - startPos.x) * 1.2f;
            if (dir < 0 && m_col_left || dir > 0 && m_col_right) { dir = 0; }
            transform.Translate(new Vector3(dir, 0, 0));
            startPos = targetPos;
        }
    }    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("wall_left")) {
            m_col_left = true;            
        }
        if (collision.CompareTag("wall_right")) {
            m_col_right = true;            
        }
    }   
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("wall_left")) {
            m_col_left = false;
            
        }
        if (collision.CompareTag("wall_right")) {
            m_col_right = false;            
        }
    }    
    private void ProcessScore()
    {
        m_dist += Time.deltaTime * GameManager.Instance.GetInvincibleSpeed();
        m_distLabel.text = string.Format("{0:f1} M" ,m_dist);
        m_coinLabel.text = string.Format("{0:d}", m_coin);
        m_scoreLabel.text = string.Format("{0:d}", m_iScore);
    }
    // Update is called once per frame
    void Update () {
        MovePlayer();
        ProcessScore();
    }
    private void FixedUpdate()
    {
        
    }
}
