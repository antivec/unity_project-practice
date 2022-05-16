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
    MonsterSprite[] m_sprits;
    [SerializeField]
    int m_count = 5;
    Vector3 m_startPos = new Vector3(-1.36f, 4.5f);
    float m_gap_x = 0.68f;
    GameObjectPool<Monster> m_monsterPool;
    List<Monster> m_monsterList;
    // Use this for initialization
    protected override void OnStart()
    {
        base.OnStart();
        m_monsterPool = new GameObjectPool<Monster>(10, ()=> 
        {
            GameObject obj = Instantiate(m_monsterObject) as GameObject;
            obj.SetActive(false);
            obj.transform.parent = transform;
            Monster mon = obj.GetComponent<Monster>();
            return mon;
        });
        m_monsterList = new List<Monster>();
        Invoke("CreateMonsters", 2.0f);
    }
	public void CreateMonsters()
    {
        for(int i = 0; i < m_count; i++ )
        {
            Monster mon = m_monsterPool.pop();
            mon.gameObject.SetActive(true);
            mon.gameObject.transform.parent = null;
            mon.gameObject.transform.position = new Vector3(m_startPos.x + (i * m_gap_x), m_startPos.y);
            mon.InitMonster(1, (Monster.MonsterType)UnityEngine.Random.Range(0, (int)Monster.MonsterType.Max));
            Debug.Log("Monster[" + i + "].position : " +mon.gameObject.transform.position);            
            m_monsterList.Add(mon);            
        }
        Debug.Log(m_monsterList.Count);
        Invoke("CreateMonsters", 5.0f);
    }
    public void RemoveMonster(Monster mon)
    {
        m_monsterList.Remove(mon);
        mon.gameObject.SetActive(false);
        mon.gameObject.transform.position = new Vector3(m_startPos.x, m_startPos.y);
        mon.gameObject.transform.parent = transform;
        m_monsterPool.push(mon);
    }
    public Sprite[] GetMonsterParts(Monster.MonsterType type)
    {
        return m_sprits[(int)type].m_spriteArray;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
