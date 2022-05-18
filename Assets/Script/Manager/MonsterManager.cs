using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class MonsterSprite
{
    //[SerializeField]
    public Sprite[] m_spriteArray;
    /*public Sprite this[int index]
    {
        get { return m_spriteArray[index];}
        set { m_spriteArray[index] = value; }
    }*/

}
public class MonsterManager : SingletonMonoBehaviour<MonsterManager>
{
    [SerializeField]
    GameObject m_monsterObject;
    [SerializeField]
    MonsterSprite[] m_sprites;
    [SerializeField]
    int m_count = 5;
    Vector3 m_startPos = new Vector3(-1.36f, 4.5f);
    float m_gap_x = 0.68f;
    GameObjectPool<Monster> m_monsterPool;
    List<Monster> m_monsterList;
    public float m_curSpeed { get; set; }
    float m_respawnInterval = 8.0f;
    // Use this for initialization
    protected override void OnStart()
    {
        base.OnStart();
        int number = 0;
        m_monsterPool = new GameObjectPool<Monster>(10, () =>
        {
            number++;
            GameObject obj = Instantiate(m_monsterObject) as GameObject;
            obj.name = obj.name + number;
            obj.SetActive(false);
            obj.transform.parent = transform;
            Monster mon = obj.GetComponent<Monster>();
            return mon;
        });
        m_monsterList = new List<Monster>();
        m_curSpeed = 2.0f;
        Invoke("CreateMonsters", 2.0f);
    }
    public void CreateMonsters()
    {
        bool isBomb = false;
        bool isSelect = false;
        Monster.MonsterType type;
        for (int i = 0; i < m_count; i++)
        {
            Monster mon = m_monsterPool.pop();
            do
            {
                type = (Monster.MonsterType)UnityEngine.Random.Range(0, (int)Monster.MonsterType.Max);
                if (type == Monster.MonsterType.Normal_Bomb && !isBomb)
                {
                    isBomb = true;
                }
                else if (type == Monster.MonsterType.Normal_Bomb && isBomb)
                {
                    isSelect = true;
                }
                else
                    isSelect = false;
            } while (isSelect);
            mon.m_lineNum = i / m_count + 1;
            mon.InitMonster(3, type);
            mon.gameObject.SetActive(true);
            mon.gameObject.transform.parent = null;
            mon.gameObject.transform.position = new Vector3(m_startPos.x + (i * m_gap_x), m_startPos.y);
            //Debug.Log("Monster[" + i + "].position : " +mon.gameObject.transform.position);            
            m_monsterList.Add(mon);
        }
        Debug.Log(m_monsterList.Count);
        Invoke("CreateMonsters", m_respawnInterval / m_curSpeed);
    }
    public void ResetCreateMonster()
    {
        CancelInvoke("CreateMonsters");
        Invoke("CreateMonsters", m_respawnInterval / m_curSpeed);
    }
    public void RemoveMonster(Monster mon)
    {
        m_monsterList.Remove(mon);
        ResetMonster(mon);
    }
    public void ResetMonster(Monster mon)
    {
        mon.gameObject.transform.position = new Vector3(m_startPos.x, m_startPos.y);
        mon.gameObject.transform.parent = transform;
        mon.gameObject.SetActive(false);
        m_monsterPool.push(mon);
    }
    public void DeleteMonsterLine(int lineNum)
    {
        Debug.Log("DeleteMonsterLine: " + m_monsterList.Count + "LineNum : " + lineNum);
        for (int i = 0; i < m_monsterList.Count; i++)
        {
            if (m_monsterList[i].gameObject.activeSelf && m_monsterList[i].m_lineNum == lineNum)
            {
                m_monsterList[i].m_isAlive = false;
                m_monsterList[i].SetDieEffect();
                ResetMonster(m_monsterList[i]);
            }
        }
        m_monsterList.RemoveAll(element => element.m_isAlive == false);
    }
    public void StopMonsterLine()
    {
        Debug.Log("Stop called");
        CancelInvoke("CreateMonsters");
    }
    public void SetMonsterSpeedInvincible(float magpower)
    {
        m_curSpeed *= magpower;
        for (int i = 0; i < m_monsterList.Count; i++)
        {
            if (m_monsterList[i].gameObject.activeSelf)
            {
                m_monsterList[i].m_speed = m_curSpeed;
            }
        }
    }
    public void SetMonsterSpeedNormal(float magpower)
    {
        m_curSpeed /= magpower;
        for (int i = 0; i < m_monsterList.Count; i++)
        {
            if (m_monsterList[i].gameObject.activeSelf)
            {
                m_monsterList[i].m_speed = m_curSpeed;
            }
        }
    }
    public Sprite[] GetMonsterParts(Monster.MonsterType type)
    {
        return m_sprites[(int)type].m_spriteArray;
    }
    // Update is called once per frame

}
