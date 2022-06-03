using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBGM(SoundManager.BGM_CLIP.BGM_01);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SceneManager.LoadScene("Game");
            SoundManager.Instance.Stop();
        }
    }
}
