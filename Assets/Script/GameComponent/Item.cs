using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public enum ItemType
    {
        Coin = 0,
        Gem_Red,
        Gem_Green,
        Gem_Blue,
        
    }
    Rigidbody2D m_rigidbody;
    float m_power = 1.3f;
   
	// Use this for initialization
	void Awake () {
        m_rigidbody = GetComponent<Rigidbody2D>();        
    }
    public void SetItem()
    {
        m_rigidbody.isKinematic = false;
        m_rigidbody.AddForce(new Vector2((Random.Range(0, 2) % 2) == 0 ? -0.1f : 0.1f, 1f) * m_power, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            ItemManager.Instance.RemoveItem(this);
            SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Get_Coin);
        }
        if (collision.tag == "wall_bottom")
        {
            ItemManager.Instance.RemoveItem(this);
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
