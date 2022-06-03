using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Monster : MonoBehaviour
{
    public enum MonsterState
    {
        IDLE = 0,
        DAMAGE,
        DIE
    }
    public enum MonsterType
    {
        Normal_White = 0,
        Normal_Yellow,
        Normal_Pink,
        Normal_Bomb,
        Max
    }
    public enum MonsterParts
    {
        Body = 0,
        Wing_Left,
        Wing_Right,
        Eye_Left,
        Eye_Right
    }
    private Animator m_animator;
    public MonsterState m_state { get; set; }
    public int m_lineNum { get; set; }
    public bool m_isAlive { get; set; }
    private int m_life;
    public MonsterType m_type { get; set; }
    public float m_speed { get; set; }
    [SerializeField]
    private SpriteRenderer[] m_renderer;
    private Sprite m_normalEyeSpr;
    private void ChangeMonsterParts(Sprite[] parts)
    {
        for (int i = 0; i < m_renderer.Length; i++)
        {
            if (i == (int)MonsterParts.Body)
                m_renderer[i].sprite = parts[0];
            else if (i == (int)MonsterParts.Wing_Left || i == (int)MonsterParts.Wing_Right)
                m_renderer[i].sprite = parts[3];
            else if (i == (int)MonsterParts.Eye_Left || i == (int)MonsterParts.Eye_Right)
                m_renderer[i].sprite = parts[1];
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("wall_floor") )
        {
            MonsterManager.Instance.RemoveMonster(this);
        }

        if (collision.CompareTag("bullet")|| collision.CompareTag("Invincible"))
        {
            m_state = MonsterState.DAMAGE;
            m_animator.SetInteger("state", (int)m_state);
            if (collision.CompareTag("bullet") ) m_life -= PlayerManager.Instance.m_power;
            else m_life = 0;
            if (m_life <= 0)
            {
                if (m_type != MonsterType.Normal_Bomb)
                    SetDie();
                else
                    MonsterManager.Instance.DeleteMonsterLine(m_lineNum);
            }
        }
        if(collision.CompareTag("Player") && !GameManager.Instance.isInvincible)
        {
            PlayerManager.Instance.setDamaged();
            Debug.Log("Damaged!!");
        }
    }
    public int SetMonsterScore(MonsterType type)
    {
        int score = 0;
        switch (type)
        {
            case MonsterType.Normal_Yellow:
                score = 10;
                break;
              
            case MonsterType.Normal_Pink:
                score = 20;
                break;
            case MonsterType.Normal_White:
                score = 30;
                break;
            case MonsterType.Normal_Bomb:
                score = 5;
                break;
        }
        return score;
    }
    public void SetDieEffect()
    {
        ScoreManager.Instance.m_iScore += SetMonsterScore(m_type);
        ParticleManager.Instance.OnEffect(transform.position);
        ItemManager.Instance.CreateItem(transform.position);
        SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Mon_Die);
    }
    public void SetDie()
    {
        SetDieEffect();
        MonsterManager.Instance.RemoveMonster(this);
    }
    public void InitMonster(int life, MonsterType type)
    {
        m_life = life;
        m_type = type;
        m_isAlive = true;
        m_speed = MonsterManager.Instance.m_curSpeed;
        var parts = MonsterManager.Instance.GetMonsterParts(type);
        ChangeMonsterParts(parts);
    }
    public void SetIdleSprite()
    {
        //Debug.Log(m_normalEyeSpr.name);
        m_renderer[(int)MonsterParts.Eye_Right].sprite = m_renderer[(int)MonsterParts.Eye_Left].sprite = m_normalEyeSpr;
    }
    // Use this for initialization
    void Start()
    {
        m_state = MonsterState.IDLE;
        m_animator = GetComponentInChildren<Animator>();
        m_normalEyeSpr = m_renderer[3].sprite;
        m_speed = 2.0f;
    }
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += Vector3.down * m_speed * Time.deltaTime;

    }
}
