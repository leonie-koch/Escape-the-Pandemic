using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InformationSwitch : MonoBehaviour
{
    [SerializeField] private GameObject chooseOptions;
    [SerializeField] private GameObject waitingForOtherPlayerScreen;
    private Highscores highscores;

    void Start()
    {
        highscores = Highscores.Instance.gameObject.GetComponent<Highscores>();
    }
  
    public void waitingForPlayer()
    {
        string message = "Waiting for the other player...";       
        waitingForOtherPlayerScreen.SetActive(true);
        waitingForOtherPlayerScreen.GetComponentInChildren<TextMeshProUGUI>().text = message;
        waitingForOtherPlayerScreen.GetComponentInChildren<TextMeshProUGUI>().fontSize = 200;

        chooseOptions.SetActive(false);
    }

    public void wrongRoom()
    {
        string message = "You both chose the same room, but you need to choose different ones. Please go back and try again.";
        waitingForOtherPlayerScreen.SetActive(true);
        chooseOptions.SetActive(false);
        waitingForOtherPlayerScreen.GetComponentInChildren<TextMeshProUGUI>().text = message;
        waitingForOtherPlayerScreen.GetComponentInChildren<TextMeshProUGUI>().fontSize = 125;
    }

    public void otherPlayerIsNotInGame()
    {
        string message = "The other player didn't join the game, please try again.";
        waitingForOtherPlayerScreen.GetComponentInChildren<TextMeshProUGUI>().text = message;
        waitingForOtherPlayerScreen.GetComponentInChildren<TextMeshProUGUI>().fontSize = 150;
        waitingForOtherPlayerScreen.SetActive(true);
        chooseOptions.SetActive(false);
    }

    public void goBackToRoomSelection()
    {
        waitingForOtherPlayerScreen.SetActive(false);
        chooseOptions.SetActive(true);        
        StartCoroutine(highscores.AboardRoomSelection());
    }
}
