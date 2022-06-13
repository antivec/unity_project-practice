using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    [System.Serializable]
    public struct ButtonList
    {
        public Button GamePlayBtn;
        public Button OptionBtn;
        public Button ShopBtn;
        public Button StatusBtn;
    }


    enum UIType
    {
        Entrance = 0, // 아무 키나 클릭하세요
        Lobby,// 메인 로비 UI
        Option, //옵션 UT
        Shop, // 상점 UI
        Status // 최고기록, 스테이터스 UI
        
    }
    enum CharaType
    {
        Chara_00 = 0,
        Chara_01,
        Chara_02,
        Chara_03,
        Chara_04
    }

    [Header("UI")]
    [SerializeField]
    GameObject[] UI_Object;
    [SerializeField]
    Sprite[] SD_Chara_list;

    [Header("Buttons")]
    public ButtonList m_ButtonList;
    public bool isEntranceProceed = false;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBGM(SoundManager.BGM_CLIP.BGM_01);
        UI_Object[(int)UIType.Entrance].SetActive(true);
        UI_Object[(int)UIType.Option].SetActive(false);

        m_ButtonList.GamePlayBtn.onClick.AddListener(PlayGame);
        m_ButtonList.OptionBtn.onClick.AddListener(OpenOptionMenu);
    }

    void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
    void OpenOptionMenu()
    {
        UI_Object[(int)UIType.Option].SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isEntranceProceed)
        {
            isEntranceProceed = true;
            UI_Object[(int)UIType.Entrance].SetActive(false);
            UI_Object[(int)UIType.Lobby].SetActive(true);
        }
    }
}
