using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager ins;
    public IVCanvas ivCanvas;
    public ObsCamera obsCamera;
    public InventoryDisplay invDisp;
    public Node startingNode;
    public int gameStatus=0;
    
   public Node currentNode;    
    [HideInInspector] public CameraRig camRig;
    public Item itemHeld;

    private ControlManager controller;

    //very bad singleton
    void Awake()
    {
        ins = this;
        ivCanvas.gameObject.SetActive(false);
        obsCamera.gameObject.SetActive(false);
    }

    private void Start()
    {
        startingNode.Arrive();
        controller = GameObject.Find("ControlManager").GetComponent<ControlManager>();
    }

    private void Update()
    {
        Prop prop = currentNode.GetComponent<Prop>();
        if (controller.UseAction2() && prop)
        {
            if (ivCanvas.gameObject.activeInHierarchy)
            {
                ivCanvas.Close();
                return;
            }

            if (obsCamera.gameObject.activeInHierarchy)
            {
                obsCamera.Close();
                return;
            }

            prop.loc.Arrive();
        }
    }
  
}
