using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hints : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject HintPanel;
    public TextMeshProUGUI hintText;
    private string[] hints = new string[6];
    // Start is called before the first frame update
    void Start()
    {
        hints[0] =  "look around for a chipcard to activate the keypad";
        hints[1] = "you need 3 numbers for the keypad and there are 3 pictures in the room...";
        hints[2] = "look for a key inside of the drawers";
        hints[3] = "The closet door is now open";
        hints[4] = "the sound from the radio seems to be a code";
        hints[5] = "In the notebook might be a text where you can count the number of used letters";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void displayHint()
    {
        Debug.Log("test");
        int status = gameManager.gameStatus;
        Debug.Log(hints[status]);
        HintPanel.SetActive(true);
        hintText.SetText(hints[status]);

    }
}
