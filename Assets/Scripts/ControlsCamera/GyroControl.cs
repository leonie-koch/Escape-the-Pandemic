using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroControl : MonoBehaviour
{
    private Gyroscope gyroscope;
    public GameObject cameraContainer;
    private Quaternion rotation;

    void Start()
    {
        GetComponentInParent<LookControl>().enabled = false;

        //cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        gyroscope = Input.gyro;
        gyroscope.enabled = true;

        cameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
        rotation = new Quaternion(0, 0, 1, 0);
    }


    // Update is called once per frame
    void Update()
    {
        cameraContainer.transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y*2, 0);
        this.transform.Rotate(-Input.gyro.rotationRateUnbiased.x, 0, 0);
    }
}
