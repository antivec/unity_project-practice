using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager> {
     
    [SerializeField]
    GameObject m_projectile;
    [SerializeField]
    GameObject m_firePos;
    [SerializeField]
    GameObject m_bulletPool;
    List<Projectile> m_projectileList;
    GameObjectPool<GameObject> m_projectile_Pool;

    float m_speed = 2.5f;
    bool m_col_left, m_col_right;
    bool m_clickOn;
    // Use this for initialization
    protected override void OnStart() {
        //base.OnStart();
        int num = 0;
        m_projectile_Pool = new GameObjectPool<GameObject>(10, () =>
        {
            ++num;
            GameObject obj = Instantiate(m_projectile) as GameObject;            
            obj.name = "bullet" + num;            
            obj.transform.parent = m_bulletPool.transform;            
            return obj;
        });
        SoundManager.Instance.PlayBGM(SoundManager.BGM_CLIP.BGM_01);
        m_projectileList = new List<Projectile>();
        Invoke("OnShoot", 3f);
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
        Invoke("OnShoot", 0.1f);
    }
    public void RemoveProjectile(Projectile projectile)
    {
        m_projectileList.Remove(projectile);
        projectile.gameObject.SetActive(false);
        projectile.gameObject.transform.parent = m_bulletPool.transform;
        m_projectile_Pool.push(projectile.gameObject);
    }
    void MovePlayer()
    {
        float dir = Input.GetAxis("Horizontal");
        if (Input.GetMouseButton(0))
            m_clickOn = true;
        if (Input.GetMouseButtonUp(0))
            m_clickOn = false;

        if (m_clickOn)
        {
            Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dir = mouseWorldPoint.x;
        }
        if (dir < 0 && m_col_left || dir > 0 && m_col_right) dir = 0;
        transform.Translate(dir * m_speed * Time.deltaTime, 0, 0);
    }    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "wall_left") {
            m_col_left = true;
        }
        if (collision.tag == "wall_right") {
            m_col_right = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "wall_left") {
            m_col_left = false;
        }
        if (collision.tag == "wall_right") {
            m_col_right = false;
        }
    }    
    // Update is called once per frame
    void Update () {
        MovePlayer();                
    }
}
