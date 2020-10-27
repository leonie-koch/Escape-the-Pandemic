using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CorrectFlask : MonoBehaviour
{
    public CountdownTimer timer;
    private void OnMouseDown()
    {
        Debug.Log(GameObject.Find("Canvas"));
        User.finishTime = timer.GetFinishTime();

        SceneManager.LoadScene("WinScene");
        Highscores.Instance.gameObject.GetComponent<Highscores>().GameFinished();
        Debug.Log("yeah you won");
    }
}
