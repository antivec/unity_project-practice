using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAniControll : MonoBehaviour {
    Animator m_animator;
    Monster m_monster;
	// Use this for initialization
	void Start () {
        m_animator = GetComponent<Animator>();
        m_monster = transform.parent.GetComponent<Monster>();
    }
    public void SetStateIdle()
    {
        m_monster.m_state = Monster.MonsterState.IDLE;
        m_animator.SetInteger("state", (int)m_monster.m_state);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
