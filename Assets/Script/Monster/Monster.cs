using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Monster : MonoBehaviour {
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
    private int m_life;
    private MonsterType m_type;
    private float m_speed = 2.0f;
    [SerializeField]
    private SpriteRenderer[] m_renderer;

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
        if (collision.tag == "bullet")
        {
            m_state = MonsterState.DAMAGE;
            m_animator.SetInteger("state", (int)m_state);
            if (--m_life <= 0)
            {
                SetDie();
            }
        }
        if(collision.tag == "wall_bottom")
        {
            MonsterManager.Instance.RemoveMonster(this);
        }
    }
    public void SetDie()
    {
        gameObject.SetActive(false);
        ParticleManager.Instance.OnEffect(transform.position);
        ItemManager.Instance.CreateItem(transform.position);
        SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Mon_Die);
        MonsterManager.Instance.RemoveMonster(this);        
    }    
    public void InitMonster(int life, MonsterType type)
    {
        m_life = life;
        m_type = type;
        var parts = MonsterManager.Instance.GetMonsterParts(type);
        ChangeMonsterParts(parts);
    }
    // Use this for initialization
    void Start () {
        m_state = MonsterState.IDLE;
        m_animator = GetComponentInChildren<Animator>();
    }	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position += Vector3.down * m_speed * Time.deltaTime;

    }
}
