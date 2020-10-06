using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : Interactable
{

    public Item myItem;
    public Inventory inventory;
    

    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
    public override void Interact()
    {
        GameManager.ins.itemHeld = myItem;
        GameManager.ins.invDisp.UpdateDisplay();
        gameObject.SetActive(false);
        Debug.Log("item");
        inventory.AddItem(myItem);
    }

}
