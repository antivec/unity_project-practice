using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop_Control : MonoBehaviour
{
    [System.Serializable]
    public struct Shop_Item_Info
    {
        public string itemName;
        public string itemDescription;
        public int itemRank;
        public Image itemIcon;
    }

    [SerializeField]
     Shop_Item_Info[] merchandise_list;

    private void Start()
    {
    }

}
