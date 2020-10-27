using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory: MonoBehaviour
{

    public List<Item> inventory;
    public List<GameObject> UIInventory;
    private Sprite defaultSprite;
    
    public void Awake()
    {
        inventory = new List<Item>();
        defaultSprite = UIInventory[0].GetComponent<Image>().sprite;
    }
 
    public void AddItem(Item item)
    {
        inventory.Add(item);
        SetUIInventory();
        if(item.itemName== "KeyCard")
            GameObject.Find("GameManager").GetComponent<GameManager>().gameStatus = 1;
        if (item.itemName == "Key")
            GameObject.Find("GameManager").GetComponent<GameManager>().gameStatus = 3;

    }
    private void SetUIInventory()
    {
       // foreach(GameObject item in UIInventory)
       for(int i =0; i<UIInventory.Count; i++)
        {
            if (i >= inventory.Count)
            {

               UIInventory[i].transform.GetChild(0).GetComponent<Image>().enabled=false;
            }
            else
            {
                Image image = UIInventory[i].transform.GetChild(0).GetComponent<Image>();
                image.enabled=true;
                image.sprite = inventory[i].inventoryPic;
                //inventory[i].transform.parent = UIInventory[i].transform;
            }
        
        }
    }
    public void UseItem(Item item)
    {
        inventory.Remove(item);
        SetUIInventory();

        // item = item.transform.GetChild(1).gameObject;
        //item.transform.parent = null;

        // inventory.Remove(item);
        //item.SetActive(true);
        //item.GetComponent<Image>().sprite = defaultSprite;

    }
    //    public void UseItem(GameObject item)
    //    {
    //        //inventory.FindIndex(element => element ==item);
    //        //inventory.Remove(item);
    //        //item.GetComponent<Image>().sprite = null;
    //.
    //    }
}
