using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BgScroll : MonoBehaviour
{
    public enum StageType
    {
        Stage1 = 0,
        Stage2,
        Stage3,
        Stage4,
        Stage5
    };

    public float m_speed { get; set; }
    Material m_material;
    public Texture[] m_MapTexture;
    public bool isMapChange = false;
    // Use this for initialization
    void Start()
    {
        var renderer = GetComponent<Renderer>();
        m_material = renderer.material ;
        //float progression = Mathf.PingPong(Time.deltaTime,Color.gray);
        StartCoroutine("MapFadeIn");
         m_speed = 0.2f;
    }
    public void ChangeMapTexture(StageType stageType)
    {
        m_material.mainTexture = m_MapTexture[(int)stageType];
       
    }
    public void DoMapFadein()
    {
        StartCoroutine("MapFadeIn");
    }

    public void DoMapFadeOut(StageType _stageType)
    {
        StageType stageType = _stageType;
        StartCoroutine("MapFadeOut",stageType);
    }

   public IEnumerator MapFadeIn()
   {    
        //Debug.Log("FadeIn called!");
          Color lerp_Color;
          float m_lerpTime = 0.0f;
          var yield_Fixed = new WaitForFixedUpdate();
        if(m_material.GetColor("_TintColor") == Color.gray)
        {
            m_material.SetColor("_TintColor", Color.black);
        }
          while (m_lerpTime < 1.0f)
           {
                m_lerpTime += (Time.deltaTime * m_speed);
                lerp_Color = Color.Lerp(Color.black, Color.gray, m_lerpTime);
                m_material.SetColor("_TintColor", lerp_Color);

                yield return yield_Fixed;
           }
           //Debug.Log("End of Coroutine ");

        yield return null;
        if (GameManager.Instance != null)
        {
            if (!GameManager.Instance.isTimetoChangeMap)
            {
                GameManager.Instance.isTimetoChangeMap = true;
            }
        }
   }

    IEnumerator MapFadeOut(StageType stageType)
    {
        Color lerp_Color;
        float m_lerpTime = 0.0f;
        Color Current_BGColor = m_material.GetColor("_TintColor");
        var yield_Fixed = new WaitForFixedUpdate();
        while (m_lerpTime < 1.0f)
        {
            m_lerpTime += (Time.deltaTime * m_speed);
            lerp_Color = Color.Lerp(Current_BGColor, Color.black, m_lerpTime);
            m_material.SetColor("_TintColor", lerp_Color);

            yield return yield_Fixed;
        }

        //Debug.Log("End of Coroutine (FADEOUT) ");
        ChangeMapTexture(stageType);
        yield return StartCoroutine("MapFadeIn");
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
