using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


// Thx to -> https://github.com/SebLague/Dreamlo-Highscores/blob/master/Episode%2002/Highscores.cs && https://www.youtube.com/watch?v=KZuqEyxYZCc
public class Highscores : MonoBehaviour 
{
		
	private const string privateCode = "onB4AfkQw0yMOWMcPJRHVQWZfh9d-ooUO5__Qi8PJPxQ";
	private const string publicCode = "5efe474e377eda0b6ca3a336";
	private const string webURL = "http://dreamlo.com/lb/";

    // ABSOLUTELY PRIVAT LINKS: http://dreamlo.com/lb/onB4AfkQw0yMOWMcPJRHVQWZfh9d-ooUO5__Qi8PJPxQ

	
	private const int FINAL_STATUS = 1337;
	private const string TEAM_BINDER = " & ";

	[SerializeField] private float secondsToWaitWhenErrorOccur = 5f;

	private HighscoresDisplay highscoreDisplay;
	private LinkedList<PlayerInformation> finalScores;
	private string teamName = "";



	private UIHandlerChatScene UI;
	InformationSwitch informationSwitch;
	string[] separatingStrings = { "_._" };
	private string channelName = "";
	private int status = 0;
	private int room = -1;
	private bool error = false;

	private string selfIdentification = "";
	private bool informationFound = false;
	PlayerInformation otherPlayer;

	private static Highscores instance = null;
	public static Highscores Instance
	{
        get { return instance; }
    }


	void Awake() {

		if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
	}


	private IEnumerator CheckForOtherPlayer(int awaitingStatus)
	{
		if(informationFound)
		{
			switch(awaitingStatus)
			{
				case 1:
				{
					CheckRightRoom();
					break;
				}

				case 2:
				{
					CheckIfOtherPlayerNoticed();
					break;
				}

				case 3:
				{
					StartCoroutine(CheckIfBothEnteredGame());
					break;
				}
			}

		}
		else
		{
			yield return new WaitForSeconds(0.05f);
			StartCoroutine(CheckForOtherPlayer(awaitingStatus));
		}
	}

	/* ------------------------------ BEFORE GAME CHECKS ------------------------------  */
	public void AddChannelName(string channelName)
	{
		this.channelName = channelName;
		selfIdentification = channelName + separatingStrings[0] + SystemInfo.deviceUniqueIdentifier;		
		StartCoroutine(RemoveSingleEntry(selfIdentification));
		AddNewHighscore(selfIdentification, status, room);
	}

	public void RoomChosen(int room, UIHandlerChatScene UI)
	{
		informationSwitch = GameObject.Find("Image").GetComponent<InformationSwitch>();
		this.UI = UI;
		this.room = room;
		status = 1;	// players started the game

		informationSwitch.waitingForPlayer();
		AddNewHighscore(selfIdentification, status, room);
		DownloadHighscores();
	 	StartCoroutine(CheckForOtherPlayer(1));
	}

	private void CheckRightRoom()
	{
		int statusCase = 1;

		if(otherPlayer.status == statusCase + 1)
		{
			CheckIfOtherPlayerNoticed();
		}
		else if(otherPlayer.status == statusCase)
		{
			int expectedRoom = 1 - room;
			if(otherPlayer.roomOrTime == expectedRoom)
			{
				informationFound = false;
				status = 2;
				AddNewHighscore(selfIdentification, status, room);
				DownloadHighscores();
				StartCoroutine(CheckForOtherPlayer(status));
			}
			else
			{
				// Debug.Log("Wrong room");
				// StartCoroutine(AboardRoomSelection());
				informationSwitch.wrongRoom();				
				otherPlayer = new PlayerInformation(otherPlayer.ID, 0, -1, null);
				informationFound = false;
				DownloadHighscores();
				StartCoroutine(CheckForOtherPlayer(statusCase));
			}
		}
		else
		{
			// Debug.Log("Waiting for other player");
			informationSwitch.waitingForPlayer();
			informationFound = false;
			DownloadHighscores();
			StartCoroutine(CheckForOtherPlayer(statusCase));
		}
	}

	private void CheckIfOtherPlayerNoticed()
	{
		int statusCase = 2;
		if(otherPlayer.status == statusCase || otherPlayer.status == statusCase + 1)
		{
			status = 3;
			AddNewHighscore(selfIdentification, status, room);
			CheckStatusOfOtherPlayerDuringGame();
			UI.loadRoomScene();
		}
		else
		{
			informationFound = false;
			DownloadHighscores();
			StartCoroutine(CheckForOtherPlayer(statusCase));
		}
	}

	public IEnumerator AboardRoomSelection()
	{
		otherPlayer = new PlayerInformation(otherPlayer.ID, 0, -1, null);
		StopAllCoroutines();
		// yield return new WaitForSeconds(0.03f);

		StartCoroutine(RemoveSingleEntry(selfIdentification));

		while(!informationFound)
		{
			// Debug.Log("waiting for server to confirm deletion");
			yield return new WaitForSeconds(0.05f);
		}

		status = 0;
		room = -1;
		AddNewHighscore(selfIdentification, status, room);
	}


	/* ------------------------------ IN-GAME CHECKS ------------------------------  */
	private void CheckStatusOfOtherPlayerDuringGame()
	{
		StartCoroutine(CheckForOtherPlayer(3));
	}

	private IEnumerator CheckIfBothEnteredGame()
	{
		int statusCase = 3;
		yield return new WaitForSeconds(5f);

		// string playerLink = channelName + separatingStrings[0] + otherPlayer.ID;
		// GetSingleEntry(playerLink);
		informationFound = false;
		DownloadHighscores();

		while(!informationFound)
		{
			yield return new WaitForSeconds(1f);
		}

		if(otherPlayer.status < statusCase)
		{
			SceneManager.LoadScene("ChooseRoomScene", LoadSceneMode.Single);
			yield return new WaitForSeconds(0.5f);
			informationSwitch = GameObject.Find("Image").GetComponent<InformationSwitch>();
			informationSwitch.otherPlayerIsNotInGame();
			StartCoroutine(AboardRoomSelection());
		}
		else if (otherPlayer.status == 4)
		{
			GameLost();
			SceneManager.LoadScene("LooseScene");
		}

		else if (otherPlayer.status > 4)
		{
			GameFinished();
			SceneManager.LoadScene("WinScene");
		}
		else
		{
			CheckStatusOfOtherPlayerDuringGame();
		}

		// if(room == 1) // just for testing, delete later
		// {
		// 	yield return new WaitForSeconds(1f);
		// 	User.finishTime = UnityEngine.Random.Range(200, 1800);
		// 	// User.finishTime = 802;
		// 	GameFinished();
		// 	// SceneManager.LoadScene("Highscore");
		// 	// SceneManager.LoadScene("LooseScene");
		// 	SceneManager.LoadScene("WinScene");
		// }
	}


	/* ------------------------------ AFTER GAME CHECKS ------------------------------  */

	public void GameLost()
	{
		StopAllCoroutines();
		status = 4;

		AddNewHighscore(selfIdentification, status, room);
	}

	public void GameFinished()
	{
		StopAllCoroutines();
		status = 5;

		AddNewHighscore(selfIdentification, status, room);
	}

	public void NameEntered()
	{
		StartCoroutine(RemoveSingleEntry(selfIdentification));

		status = 6;
		selfIdentification += separatingStrings[0] + User.name;		
		AddNewHighscore(selfIdentification, status, room);

		CheckIfOtherPlayerEnteredName();
	}

	private void CheckIfOtherPlayerEnteredName()
	{
		// Debug.Log("CheckIfOtherPlayerEnteredName");
		StartCoroutine(CheckForOtherPlayersName());
	}

	private IEnumerator CheckForOtherPlayersName()
	{		
		informationFound = false;
		DownloadHighscores();
		
		while(!informationFound)
		{
			yield return new WaitForSeconds(0.05f);
		}


		// Debug.Log("CheckForOtherPlayersName " + otherPlayer.status);

		if(otherPlayer.status < 6)
		{
			string message = "Waiting for other user entering the name";
			highscoreDisplay.SetMessageFromServer(message);
			CheckIfOtherPlayerEnteredName();
		}
		else
		{
			highscoreDisplay.DeleteMessageFromServer();
			status = 7;
			// Debug.Log("are you in here?");
			AddNewHighscore(selfIdentification, status, room);
			BothPlayersSavedTheOtherPlayersName(); // To this point, both have uploaded their names
		}
	}

	private void BothPlayersSavedTheOtherPlayersName()
	{
		StartCoroutine(SaveOtherPlayersName());
	}

	private IEnumerator SaveOtherPlayersName()
	{
		// Debug.Log("SaveOtherPlayersName");
				
		informationFound = false;
		DownloadHighscores();

		while(!informationFound)
		{
			yield return new WaitForSeconds(0.05f);
		}

		if(otherPlayer.status < 7)
		{			
			BothPlayersSavedTheOtherPlayersName();
		}
		else
		{
			UpdateHighscoreboard();
		}
	}

	private void UpdateHighscoreboard()
	{
		StartCoroutine(ClearOldEntriesAndUpdateFinalEntry());
	}

	private IEnumerator ClearOldEntriesAndUpdateFinalEntry()
	{
		// GameObject.Find("CountdownTimer");
		if(room == 1)
		{
			// informationFound = false;
			teamName = User.name + separatingStrings[0] + otherPlayer.name;
			AddNewHighscore(teamName, FINAL_STATUS, User.finishTime);			
		}
		else
		{
			teamName = otherPlayer.name + separatingStrings[0] + User.name;
		}

		informationFound = false;
		GetSingleEntry(teamName);

		while(!informationFound)
		{
			yield return new WaitForSeconds(0.05f);
		}

		informationFound = false;
		StartCoroutine(RemoveSingleEntry(selfIdentification));

		while(!informationFound)
		{
			yield return new WaitForSeconds(0.05f);
		}

		status = FINAL_STATUS;
		// StopAllCoroutines();
		// yield return new WaitForSeconds(3f);
		highscoreDisplay.SeeHighscoresAfterEnteringNames(GetNiceTeamString(teamName));
	}


	/* ------------------------------ UPLOAD METHODS ------------------------------  */

	public static void AddNewHighscore(string username, int status, int time)
	{
		if(int.TryParse(username, out int number))
		{
			if(number > 10)
			{
				instance.StartCoroutine(instance.UploadNewHighscore
					(
					username,
					status,
					time
					)
				);
			}
		}
		else
		{
			instance.StartCoroutine(instance.UploadNewHighscore
				(
				username,
				status,
				time
				)
			);
		}
	}

	IEnumerator UploadNewHighscore(string username, int status, int time)
	{
		informationFound = false;
		UnityWebRequest www = new UnityWebRequest(webURL + privateCode + "/add-pipe/" + UnityWebRequest.EscapeURL(username) +  "/" + status + "/" + time);
		// Debug.Log(www.url);

		yield return www.SendWebRequest();

		if (string.IsNullOrEmpty(www.error))
		{
			// print ("Upload Successful");
            error = false;
			informationFound = true;
			// DownloadHighscores();
		}
		else
		{
			error = true;
			print ("Error uploading: " + www.error);

			while(error)
			{
				yield return new WaitForSeconds(secondsToWaitWhenErrorOccur);
				AddNewHighscore(username, status, time);
			}
		}
	}


	/* ------------------------------ DOWNLOAD METHODS ------------------------------  */
	private void GetSingleEntry(string entry)
	{
		informationFound = false;		
		StartCoroutine(GetRequest(webURL +  publicCode  + "/pipe-get/" + UnityWebRequest.EscapeURL(entry)));
	}

	IEnumerator GetRequest(string url)
	{
		// Something not working? Try copying/pasting the url into your web browser and see if it works.
		// Debug.Log(url);

		using (UnityWebRequest www = UnityWebRequest.Get(url))
		{
			yield return www.SendWebRequest();

			if (string.IsNullOrEmpty(www.error))
			{
				yield return new WaitForSeconds(secondsToWaitWhenErrorOccur);
				GetSingleEntry(url);
			}
			else
			{
				string entry = www.downloadHandler.text;
				string[] entryInfo = entry.Split(new char[] {'|'});
				string[] otherPlayerID = entryInfo[0].Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

				// This block is just for debugging and can be deleted later

				if(otherPlayerID.Length > 1)
				{
					// Debug.Log(entry);
					otherPlayer = new PlayerInformation(
					otherPlayerID[1], 			// ID
					int.Parse(entryInfo[1]), 	// status
					int.Parse(entryInfo[2]),	// room
					null
					);	
					
					informationFound = true;
				}
				else if(entryInfo.Length > 1)
				{
					if(int.Parse(entryInfo[1]) == FINAL_STATUS)
					{
						informationFound = true;
					}
					else
					{						
						GetSingleEntry(url);
					}				
				}
				else
				{
					GetSingleEntry(url);
				}
			}
		}
	}

	public void DownloadHighscores()
	{
		informationFound = false;
		StartCoroutine(DownloadHighscoresFromDatabase());
	}


	// For this method, thx to -> https://forum.unity.com/threads/api-web-request-error-help-nullreferenceexception.549760/
	private IEnumerator DownloadHighscoresFromDatabase()
    {
		// Debug.Log("Downloading server information");
        UnityWebRequest request = new UnityWebRequest();

        request = UnityWebRequest.Get(webURL + publicCode + "/pipe/");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
			Debug.Log(request.error);
			error = true;
			yield return new WaitForSeconds(secondsToWaitWhenErrorOccur);
			DownloadHighscores();

			if(highscoreDisplay)
			{
				highscoreDisplay.SetError(true);
				highscoreDisplay.Error();
			}
        }
        else
        {
            // Debug.Log(request.downloadHandler.text); // Show results as text

			GetInformationOfOtherPlayer(request.downloadHandler.text);
			error = false;

			if(highscoreDisplay && (status == FINAL_STATUS || status < 4))
			{
				highscoreDisplay.SetError(false);
				highscoreDisplay.OnHighscoresDownloaded(finalScores);
			}
        }
    }

	private void GetInformationOfOtherPlayer(string textStream)
	{		
		finalScores = new LinkedList<PlayerInformation>();

		string[] ownSystem = selfIdentification.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
		string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < entries.Length; i++)
		{
			string[] entryInfo = entries[i].Split(new char[] {'|'});
			string[] channel = entryInfo[0].Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
			int status = int.Parse(entryInfo[1]);
			int roomOrTime = int.Parse(entryInfo[2]);
			string name = null;


			if(channel.Length > 1 && ownSystem.Length > 1)
			{
				if(channel[0] == channelName && channel[1] != ownSystem[1])
				{
					if(channel.Length == 3)
					{
						name = channel[2];
					}

					otherPlayer = new PlayerInformation(channel[1], status, roomOrTime, name);
					informationFound = true;

					// Debug.Log("Found other Player! || ID: " + otherPlayer.ID + " || Status: " + otherPlayer.status + " || Room: " + otherPlayer.room);
					// Debug.Log("Me: " + SystemInfo.deviceUniqueIdentifier + " - " + status + " - " + room);
				}
			}

			if(status == FINAL_STATUS)
			{
				SetPosition(GetNiceTeamString(channel[0], channel[1]), roomOrTime);
			}
		}

		// Debug.Log(!User.enteredHighscoreFromMenu);
		if(!informationFound && (status == 1 || status == 2 || status == 3 || status == 6 || status == 7))
		{
			DownloadHighscores();
		}
	}

	private string GetNiceTeamString(string name1, string name2)
	{
		return name1 + TEAM_BINDER + name2;
	}

	private string GetNiceTeamString(string teamCode)
	{
		string[] teamMembers = teamCode.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
		if(teamMembers.Length > 1) return teamMembers[0] + TEAM_BINDER + teamMembers[1];
		else return "";
	}

	private void SetPosition(string team, int time)
	{
		LinkedListNode<PlayerInformation> currentEntry = finalScores.First;
		while(currentEntry != null )
		{
			if(currentEntry.Value.roomOrTime > time)
			{
				PlayerInformation newEntry = new PlayerInformation(null, FINAL_STATUS, time, team);
				finalScores.AddBefore(currentEntry, newEntry);
				currentEntry = new LinkedListNode<PlayerInformation>(newEntry);				
				break;
			}
			else
			{
				currentEntry = currentEntry.Next;
			}
		}

		if(currentEntry == null)
		{
			finalScores.AddLast(new PlayerInformation(null, FINAL_STATUS, time, team));
		}
	}


	/* ------------------------------ DELETE METHODS ------------------------------  */

	private IEnumerator RemoveSingleEntry(string entry)
	{
		informationFound = false;
		UnityWebRequest www = new UnityWebRequest(webURL + privateCode + "/delete/" + UnityWebRequest.EscapeURL(entry));

		yield return www.SendWebRequest();

		if (string.IsNullOrEmpty(www.error))
		{
			// print ("Deletion Successful");
            error = false;
			informationFound = true;
		}
		else
		{
			error = true;
			print ("Error deleting: " + www.error);
			yield return new WaitForSeconds(secondsToWaitWhenErrorOccur);
			StartCoroutine(RemoveSingleEntry(entry));
		}
	}

	/* ------------------------------ GETTER METHODS ------------------------------  */

	public int GetStatus()
	{
		return status;
	}

	public int GetFinalStatus()
	{
		return FINAL_STATUS;
	}


	/* ------------------------------ SETTER METHODS ------------------------------  */
	public void SetHighscoresDisplay(HighscoresDisplay highscoreDisplay)
	{
		this.highscoreDisplay = highscoreDisplay;		
	}
	
	/* ------------------------------ METHOD FOR LEAVING EARLIER ------------------------------  */
	public void DontEnterHighscore()
	{
		StopAllCoroutines();
		StartCoroutine(RemoveSingleEntry(selfIdentification));
	}

	
} // END OF CLASS



public struct PlayerInformation
{
	public string ID;
	public string name;
	public int status;
	public int roomOrTime;
	

	public PlayerInformation(string _ID, int _status, int _roomOrTime, string _name)
	{
        ID = _ID;
		status = _status;
		roomOrTime = _roomOrTime;
		name = _name;
	}
} // END OF CLASS
