using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeDisplay : MonoBehaviour
{

    public CodeLock codelock;
    TextMeshProUGUI text;
    string attemptedCode;

    private void Start()
    {
        this.text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        text.text = codelock.GetComponent<CodeLock>().attemptedCode;
    }
}
