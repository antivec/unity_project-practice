using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : SingletonMonoBehaviour<ParticleManager> {
    GameObjectPool<ParticleSystem> m_particlePool;
    [SerializeField]
    GameObject m_effectObject;
    List<ParticleSystem> m_listParticle;
    // Use this for initialization
    protected override void OnStart()
    {
        base.OnStart();
        m_particlePool = new GameObjectPool<ParticleSystem>(10, () =>
        {
            GameObject obj = Instantiate(m_effectObject) as GameObject;
            obj.transform.parent = transform;
            obj.SetActive(false);
            ParticleSystem particle = obj.GetComponent<ParticleSystem>();            
            return particle;
        });
        m_listParticle = new List<ParticleSystem>();
    }      
	
	public void OnEffect(Vector3 pos)
    {
        var effect = m_particlePool.pop();
        effect.transform.position = pos;
        effect.gameObject.SetActive(true);
        effect.transform.parent = null;
        effect.Play();
        //Invoke("OffEffect", 1f);
        StartCoroutine("OffEffect", effect);
    }

    public IEnumerator OffEffect(ParticleSystem effect)
    {
        yield return new WaitForSeconds(1f);
        m_listParticle.Remove(effect);
        effect.transform.parent = transform;
        effect.gameObject.SetActive(false);
        m_particlePool.push(effect);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
