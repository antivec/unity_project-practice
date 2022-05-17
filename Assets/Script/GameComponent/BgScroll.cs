using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScroll : MonoBehaviour
{

    public float m_speed { get; set; }
    Material m_material;
    // Use this for initialization
    void Start()
    {
        var renderer = GetComponent<Renderer>();
        m_material = renderer.material;
        m_speed = 0.2f;
    }

    // Update is called once per frame    
    void Update()
    {
        if (m_material != null)
        {
            m_material.mainTextureOffset = new Vector2(0, m_material.mainTextureOffset.y + m_speed * Time.deltaTime);
        }
    }
}
