using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum HeartStatus
{
    filled = 0,
    empty
};

public class Hearts : SingletonMonoBehaviour<Hearts>

{
    [System.Serializable]
    public class Heart
    {
        public GameObject heart_containter;
    }

    public Heart[] heart_list;
    public Sprite[] heart_sprites;
    // Start is called before the first frame update

    void Start()
    {
        InitializeHeart();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void InitializeHeart()
    {
        for (int i = 0; i < heart_list.Length; i++)
        {
            heart_list[i].heart_containter.GetComponent<SpriteRenderer>().sprite = heart_sprites[(int)HeartStatus.filled];
        }
    }

    public void ModifyHeart(int idx)
    {
        heart_list[idx].heart_containter.GetComponent<SpriteRenderer>()
            .sprite = heart_sprites[(int)HeartStatus.empty];    
    }
}
