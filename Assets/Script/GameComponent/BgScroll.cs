using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScroll : MonoBehaviour {

    private float m_speed = 0.2f;
    Material m_material;
	// Use this for initialization
	void Start () {
        var renderer = GetComponent<Renderer>();
        m_material = renderer.material;
        renderer.sortingLayerName = "BackGround";    
    }

    // Update is called once per frame    
	void Update () {
        if (m_material != null)
        {
            m_material.mainTextureOffset = new Vector2(0, m_material.mainTextureOffset.y + m_speed * Time.deltaTime);
        }
    }
}
