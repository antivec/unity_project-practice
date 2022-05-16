using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    float m_speed = 7f;    
	// Use this for initialization
	void Start () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "wall_top" || collision.tag == "monster")
            PlayerManager.Instance.RemoveProjectile(this);        
    }
    // Update is called once per frame
    void Update () {
        transform.position += Vector3.up * m_speed * Time.deltaTime;
	}
}
