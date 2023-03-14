using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : SingletonMonoBehaviour<LoginManager>
{
    [Header("Button")]
    [SerializeField]
    Button LoginBtn;
    [SerializeField]
    Button RegisterBtn;

    [Header("InputField")]
    [SerializeField]
    TMP_InputField IDField;
    [SerializeField]
    TMP_InputField PWField;

    GameMenuManager menuMng;
    public DB_Connection m_connection;
    //string m_PathName = string.Format(@"Assets/StreamingAssets/PlayerInfo.db");
    string m_PathName = Path.Combine(Application.streamingAssetsPath , "PlayerInfo.db");
    protected override void OnStart()
    {
        base.OnStart();
        
        menuMng = FindObjectOfType<GameMenuManager>();
        LoginBtn.onClick.AddListener(LoginBtnClicked);
        RegisterBtn.onClick.AddListener(RegisterClicked);

        m_connection = new DB_Connection(m_PathName);
    }

    public void LoginBtnClicked()
    {
        string Input_ID = IDField.text;
        string Input_PW = PWField.text;
        int LoginFlag;
        if (Input_ID.Length < 4 || Input_PW.Length < 4)
        {

        }
        else
        {
            LoginFlag = m_connection.LoginAttempt(Input_ID, Input_PW);
        }
     
    }

    public void RegisterClicked()
    {
        string Input_ID = IDField.text;
        string Input_PW = PWField.text;
        m_connection.RegisterID(Input_ID,Input_PW);
    }
}
