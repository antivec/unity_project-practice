using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WarningPopup : MonoBehaviour
{
    //[SerializeField]
    Button Proceed_btn;
    //[SerializeField]
    Button Cancel_btn;

    GameObject popup_obj;
    //Canvas popup_canvas;

    public void PopupCall( )
    {
        popup_obj = Instantiate(Resources.Load("Prefab/Warning_Canvas", typeof(GameObject)) as GameObject);
        popup_obj.gameObject.transform.SetParent(transform, false);
        popup_obj.gameObject.SetActive(true);
 

        this.Proceed_btn = GameObject.Find("Warning_Proceed_Btn").GetComponent<Button>();
        this.Cancel_btn = GameObject.Find("Warning_Cancel_Btn").GetComponent<Button>();

        Cancel_btn.onClick.AddListener(CancelAction);
        Proceed_btn.onClick.AddListener(ConfirmAction);

    }
    // Start is called before the first frame update

    void CancelAction()
    {
        popup_obj.gameObject.SetActive(false);
        Destroy(popup_obj.gameObject);
    }
    void ConfirmAction()
    {

        PlayerPrefs.DeleteKey("Player_Coin");
        PlayerPrefs.DeleteKey("Player_Score");
        PlayerPrefs.DeleteKey("Player_Final_Score");
        PlayerPrefs.DeleteKey("Player_Distance");
        Debug.Log("Reset Complete!");

       
        popup_obj.gameObject.SetActive(false);
        Destroy(popup_obj.gameObject);


    }
}
