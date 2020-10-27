using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;


// Thx to -> https://github.com/SebLague/Dreamlo-Highscores/blob/master/Episode%2002/DisplayHighscores.cs // https://www.youtube.com/watch?v=9jejKPPKomg
public class HighscoresDisplay : MonoBehaviour
{
	
    [SerializeField] private GameObject leaderboard;
	[SerializeField] private GameObject enteringName;
	[SerializeField] private GameObject closeHighscoreButton;
	[SerializeField] private GameObject goBackButton;

    private TextMeshProUGUI[] highscoreFields;
	private Highscores highscoresManager;
	private float refreshHighscoreInSeconds = 120;
	private string team = null;
	private bool error = false;
	private bool teamFound = false;
	
	

	void Start() 
	{
		highscoresManager = Highscores.Instance.gameObject.GetComponent<Highscores>();
        highscoresManager.SetHighscoresDisplay(this);

		highscoreFields = new TextMeshProUGUI[leaderboard.transform.childCount];

		if(highscoresManager.GetStatus() < 5)
		{
			goBackButton.SetActive(true);
		}
		
		if(highscoresManager.GetStatus() > 4)
		{
			leaderboard.SetActive(false);
			enteringName.SetActive(true);
		}
		else
		{
			leaderboard.SetActive(true);
			enteringName.SetActive(false);
			StartCoroutine(RefreshHighscores());
		}
	}

	private void SetupHighscoreFields()
	{
		for (int i = 0; i < highscoreFields.Length; i++) 
		{			
            highscoreFields[i] = leaderboard.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
			highscoreFields[i].gameObject.SetActive(true);
			highscoreFields[i].text = "";
			highscoreFields[i].alignment = TMPro.TextAlignmentOptions.Right;
			highscoreFields[i].color = Color.white;
		}
		string message = "Fetching...";
		SetMiddleTextField(message);
	}
	
	public void OnHighscoresDownloaded(LinkedList<PlayerInformation> finalScores)
	{
		ResetMiddleTextField();		
		LinkedListNode<PlayerInformation> currentEntry = finalScores.First;
		teamFound = false;

		for (int i = 0; i < highscoreFields.Length; i ++) 
		{
			if(currentEntry != null)
			{
				highscoreFields[i].text = i + 1 + ". " + currentEntry.Value.name + " - " + CountdownTimer.GetTimeAsString(currentEntry.Value.roomOrTime);

				if(!string.IsNullOrEmpty(team))
				{
					if(currentEntry.Value.name == team)
					{
						teamFound = true;
						highscoreFields[i].color = Color.red;						
					}

					if(i == highscoreFields.Length - 1 && !teamFound)
					{
						Debug.Log(team);

						for(int j = i + 1; j < finalScores.Count; j++)
						{
							currentEntry = currentEntry.Next;

							if(currentEntry.Value.name == team)
							{
								Debug.Log(j + 1 + ". " + currentEntry.Value.name);
								highscoreFields[i].text = j + 1 + ". " + currentEntry.Value.name + " - " + CountdownTimer.GetTimeAsString(currentEntry.Value.roomOrTime);
								highscoreFields[i].color = Color.red;
							}
						}
					}
				}
				
				currentEntry = currentEntry.Next;
			}
			else
			{
				highscoreFields[i].gameObject.SetActive(false);
			}
		}

		if(!highscoreFields[0].gameObject.activeSelf)
		{
			string message = "Please be patient, sometimes it takes a few moments, until the data arrived";
			SetMiddleTextField(message);
		}
	}
	
	IEnumerator RefreshHighscores() 
	{
		while (true)
		{			
			if(error)
			{
				refreshHighscoreInSeconds = 5;
			}
			else
			{
				refreshHighscoreInSeconds = 15;
			}

			SetupHighscoreFields();
			highscoresManager.DownloadHighscores();
			yield return new WaitForSeconds(refreshHighscoreInSeconds);
		}
	}

    public void Error()
    {
        string message = "We're sorry, but the server is currently not available.";

        for(int i = 0; i < highscoreFields.Length; i++)
        {
            highscoreFields[i].gameObject.SetActive(false);
        }

        SetMiddleTextField(message);
    }

	private void SetMiddleTextField(string message)
	{
		int middleTextField = highscoreFields.Length / 2;
        highscoreFields[middleTextField].gameObject.SetActive(true);
        highscoreFields[middleTextField].text = message;
		highscoreFields[middleTextField].alignment = TMPro.TextAlignmentOptions.Center;
		highscoreFields[middleTextField].color = Color.white;		
	}

	private void ResetMiddleTextField()
	{
		int middleTextField = highscoreFields.Length / 2;
		highscoreFields[middleTextField].alignment = TMPro.TextAlignmentOptions.Right;
	}

	public void SetError(bool status)
	{
		error = status;
	}

	public void SeeHighscoresAfterEnteringNames(string team)
	{
		this.team = team;
		enteringName.SetActive(false);
		leaderboard.SetActive(true);
		closeHighscoreButton.SetActive(true);
		StartCoroutine(RefreshHighscores());
	}

	public void SetMessageFromServer(string message)
	{
		leaderboard.SetActive(true);
		SetupHighscoreFields();
		SetMiddleTextField(message);
	}

	public void DeleteMessageFromServer()
	{
		leaderboard.SetActive(true);
		SetupHighscoreFields();
	}

	public void GoBack()
	{
		if(highscoresManager.GetStatus() == 3)
		{
			// go back in game
		}
		else if(highscoresManager.GetStatus() < 3 || highscoresManager.GetStatus() == highscoresManager.GetFinalStatus())
		{
			SceneManager.LoadScene("StartScene");
		}
	}
}