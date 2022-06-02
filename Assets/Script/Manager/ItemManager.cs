using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonMonoBehaviour<ItemManager>
{

    [SerializeField]
    GameObject m_itemObject;
    [SerializeField]
    Sprite[] m_itemSprite;
    GameObjectPool<Item> m_itemPool;
    List<Item> m_itemList;
    int[] m_itemProbability = new int[] { 60, 6, 2, 1, 12, 19 };
    public void setItemPool()
    {
        SelectItem();
    }
    protected override void OnStart()
    {
        base.OnStart();
        m_itemPool = new GameObjectPool<Item>(10, () =>
        {
            GameObject obj = Instantiate(m_itemObject) as GameObject;
            obj.SetActive(false);
            obj.transform.parent = transform;
            Item item = obj.GetComponent<Item>();
            return item;
        });
        m_itemList = new List<Item>();
        SelectItem();
    }
    public Sprite GetItemSprite(Item.ItemType type)
    {
        return m_itemSprite[(int)type];
    }
    public Item.ItemType SelectItem()
    {
        var result = Util.GetPriority(m_itemProbability);
        return (Item.ItemType)result;
    }
    public void CreateItem(Vector3 pos)
    {
        Item item = m_itemPool.pop();
        item.transform.position = pos;
        item.gameObject.SetActive(true);
        item.transform.parent = null;
        item.SetItem(SelectItem());
        m_itemList.Add(item);
    }
    public void RemoveItem(Item item)
    {
        m_itemList.Remove(item);
        item.transform.parent = transform;
        item.gameObject.SetActive(false);
        m_itemPool.push(item);
    }
}
