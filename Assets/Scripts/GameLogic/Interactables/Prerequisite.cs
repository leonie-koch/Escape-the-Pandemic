using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prerequisite : MonoBehaviour
{

    //if true, check for item instead
    public bool requireItem;

    //watch this switcher
    public Switcher watchSwitcher;

    //if requirementItem is true, we`ll check this collector
    public Collector checkCollector;

    //if true, then block access altogether
    public bool nodeAccess;

    public Inventory inventory;

    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    //check if prerequisite is met
    public bool Complete
    {
        get{
            if (!requireItem)
            {
                return watchSwitcher.state;
            }
            else
            {
                if (GameManager.ins.itemHeld.itemName == "KeyCard")
                    inventory.UseItem(GameManager.ins.itemHeld);
                if (GameManager.ins.itemHeld.itemName == "Key")
                {
                    inventory.UseItem(GameManager.ins.itemHeld);
                    this.GetComponent<ClosetManager>().openDoor();
                }

                return GameManager.ins.itemHeld.itemName == checkCollector.myItem.itemName;
            }

        }
    }

}
