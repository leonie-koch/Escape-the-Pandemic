using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorseCode : MonoBehaviour
{
    public AudioSource morseCode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown()
    {
        morseCode.Play();
        GameObject.Find("GameManager").GetComponent<GameManager>().gameStatus = 4;
    }
}
