using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    private Inventory inventory;
    public GameObject UIInventory;

    // Start is called before the first frame update
    void Start()
    {
       //inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        // UIInventory = GameObject.Find("Inventory");

    }

    // Update is called once per frame
    void Update()
    {

    }
    //public void useItem(GameObject item)
    //{
    //    if(item!=null)
    //    inventory.UseItem(item);
    //}
    public void showInventory(bool show)
    {
        UIInventory.SetActive(show);

    }
}
