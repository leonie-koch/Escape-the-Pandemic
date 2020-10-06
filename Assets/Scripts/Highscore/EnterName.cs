using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class EnterName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameInputField;
    [SerializeField] private GameObject nameInputObject;
    [SerializeField] private GameObject submitNameButton;
    private TouchScreenKeyboard touchScreenKeyboard;

  
    void Update()
    {
        if(TouchScreenKeyboard.visible == false && touchScreenKeyboard != null)
        {
            // Debug.Log(touchScreenKeyboard.status.GetType());
            if(touchScreenKeyboard.done);
            {
                User.name = touchScreenKeyboard.text.Trim();
                touchScreenKeyboard = null;
            }
        }

        if(Input.GetKeyDown(KeyCode.Return) && nameInputField.gameObject.activeSelf)
        {
            SubmitNameButton();
        }
    }

    public void OpenKeyboard()
    {        
        touchScreenKeyboard = TouchScreenKeyboard.Open ("", TouchScreenKeyboardType.Default, false, false, false, false, "", 10);
    }
    
    public void OnTouch()
    {
        OpenKeyboard();
    }

    public void ShowSubmitButton()
    {
        submitNameButton.SetActive(true);
    }

    public void SubmitNameButton()
    {
        submitNameButton.SetActive(false);
        Handheld.Vibrate();

        if(User.isOnDesktop)
        {
            User.name = nameInputField.text;
            nameInputObject.SetActive(false);
        }

        Highscores.Instance.gameObject.GetComponent<Highscores>().NameEntered();
    }
}
