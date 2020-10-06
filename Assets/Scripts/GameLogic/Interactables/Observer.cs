using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : Interactable
{
    public override void Interact()
    {
        GameObject item = Instantiate(gameObject);
        item.transform.SetParent(GameManager.ins.obsCamera.rig);
        item.transform.localPosition = Vector3.zero;
        item.transform.GetChild(0).localPosition = Vector3.zero;
        GameManager.ins.obsCamera.model = item.transform;
        GameManager.ins.obsCamera.gameObject.SetActive(true);
    }
}
