using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeLockControlLightChanger : MonoBehaviour
{
    public Transform cameraRig;
    public Node numPad;
    public Transform cameraPosition;
    public List<GameObject> buttons;

    [HideInInspector]
    public MeshRenderer mesh;
    [HideInInspector]
    public Color currentColor;
    public Color active, rightCode;
    public GameObject controlLight;

    // Start is called before the first frame update
    void Start()
    {
        mesh = controlLight.GetComponent<MeshRenderer>();
        currentColor = mesh.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        //if the camera is infront of the codelock the color of the controllight is changing into its active color (blue)
        if (GameManager.ins.currentNode == numPad && cameraRig.transform.position == cameraPosition.transform.position)
        {

            foreach (GameObject number in buttons)
            {
                var col = number.GetComponent<Collider>();
                col.enabled = true;
            }
            if (mesh.material.color != rightCode)
            {
                mesh.material.color = active;
            }
        }
        else
        {
            foreach (GameObject number in buttons)
            {
                var col = number.GetComponent<Collider>();
                col.enabled = false;
            }
            if (mesh.material.color != rightCode)
            {
                mesh.material.color = currentColor;
            }

        }
    }
}
