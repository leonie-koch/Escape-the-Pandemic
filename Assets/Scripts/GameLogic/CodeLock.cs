using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeLock : MonoBehaviour
{
    CodeLockControlLightChanger controlLight;
    AudioSource audioSource;
    public AudioClip openDoor, accessDenied;

    int codeLength;
    int placeInCode;

    public string code = "";
    public string attemptedCode;

    public Transform doorL, doorR;
    public int hint;


    private void Start()
    {
        codeLength = code.Length;
        controlLight = GetComponent<CodeLockControlLightChanger>();
        audioSource = GetComponent<AudioSource>();
    }

    //the current value of the code
    public void SetValue(string value)
    {
        placeInCode++;


        if(placeInCode <= codeLength)
        {
            attemptedCode += value;
        }

        Invoke("resetCode", 2);
       
    }

   public void resetCode()
    {
        if (placeInCode == codeLength)
        {
            CheckCode();

            attemptedCode = "";
            placeInCode = 0;
        }
    }

    //chekcs if the code is right
    void CheckCode()
    {
        if(attemptedCode == code)
        {
            Open();
            if(audioSource != null)
            {
                audioSource.PlayOneShot(openDoor);
            }
            //the color of the Kontrollleuchte object is changing to green if the code is right
            controlLight.mesh.material.color = controlLight.rightCode;
            GameObject.Find("GameManager").GetComponent<GameManager>().gameStatus = hint;

        }
        else
        {
            audioSource.PlayOneShot(accessDenied);
            Debug.Log("Wrong Code");
        }
    }

    //opens the door
    void Open()
    {
        if(doorL != null)
        {
            doorL.Rotate(new Vector3(0, -90, 0), Space.World);
        }
        if(doorR != null)
        {
            doorR.Rotate(new Vector3(0, 90, 0), Space.World);
        } 
    }
}
