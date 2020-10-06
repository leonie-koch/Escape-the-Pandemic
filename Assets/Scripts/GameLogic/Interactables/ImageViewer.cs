using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageViewer : Interactable
{

    [SerializeField] private Sprite pic;

    public override void Interact()
    {
        GameManager.ins.ivCanvas.Activate(pic);
    }
}
