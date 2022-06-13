using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionMenu : MonoBehaviour
{
    [System.Serializable]
    public struct Option_Btn_List
    {
        public Button Save_Btn;
        public Button Cancel_Btn;
        public Button Reset_Btn;
    }


    [SerializeField]
    SoundManager m_soundManager;

    [SerializeField]
    AudioSource m_AudioSrc;
    [Header("SLIDER")]
    [SerializeField]
    Slider m_BGMSlider;
    [SerializeField]
    Slider m_SFXSlider;
    [Header("InputField")]
    [SerializeField]
    InputField m_BGM_Input;
    [SerializeField]
    InputField m_SFX_Input;
    [Header("Button List")]
    [SerializeField]
    Button m_closeBtn;
    [SerializeField]
    public Option_Btn_List m_Option_BtnList;
    [SerializeField]
    WarningPopup m_WarningPopup;

    float m_fBGM_Vol = -1.0f;
    bool isPopUp = false;

    private void Start()
    {
        m_Option_BtnList.Save_Btn.onClick.AddListener(SaveConfig);
        m_Option_BtnList.Reset_Btn.onClick.AddListener(ResetOption);
        m_BGMSlider.onValueChanged.AddListener(delegate { ApplyBGMNumber(); });
        //m_SFXSlider.onValueChanged.AddListener(delegate { ApplySettingNumber(); }); 
        m_BGM_Input.readOnly = true;
        m_SFX_Input.readOnly = true;
    }
    private void OnEnable()
    {
        m_AudioSrc = m_soundManager.gameObject.GetComponentInChildren<AudioSource>();

        SetSound_PlayerPref();
    }

    public void CloseMenu()
    {
        this.gameObject.SetActive(false);
    }

    void SaveConfig()
    {
        PlayerPrefs.SetFloat("BGM_Volume", m_BGMSlider.value);
        PlayerPrefs.Save();
        //Debug.Log("Save Success");
    }
    void SetSound_PlayerPref()
    {
        if (PlayerPrefs.HasKey("BGM_Volume"))
        {
            m_fBGM_Vol = PlayerPrefs.GetFloat("BGM_Volume");
            m_BGMSlider.value = m_fBGM_Vol;
            m_AudioSrc.volume = m_fBGM_Vol;
            m_BGM_Input.text = ((int)(m_fBGM_Vol * 100)).ToString();
        }
        else
            m_BGMSlider.value = m_AudioSrc.volume;
    }

    //void Update()
    //{
    //    //ApplySettingNumber();
    //}

    void ApplyBGMNumber()
    {
        m_AudioSrc.volume = m_BGMSlider.value;
        m_BGM_Input.text = ((int)(m_BGMSlider.value * 100)).ToString();
        //m_SFX_Input.text = ((int)(m_SFXSlider.value * 100)).ToString();
    }

    //void ApplySFXNumber()
    //{
    //    m_AudioSrc.volume = m_BGMSlider.value;
    //    //m_BGM_Input.text = ((int)(m_BGMSlider.value * 100)).ToString();
    //    m_SFX_Input.text = ((int)(m_SFXSlider.value * 100)).ToString();
    //}
    void ResetOption()
    {
        m_WarningPopup.PopupCall();
        
    }
}
