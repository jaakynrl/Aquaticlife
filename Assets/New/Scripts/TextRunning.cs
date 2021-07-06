using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextRunning : MonoBehaviour
{
    public Text msgText;
    private string msg;
    private bool isSkip = false;
    public float speedText = 0.1f;
    Action method;
    public void SetText(string txt, Action method)
    {
        StopAllCoroutines();
        msg = txt;
        isSkip = false;
        this.method = method;
        StartCoroutine(RunText());
    }

    IEnumerator RunText()
    {
        int count = msg.Length;
        for (int i = 1; i <= count; i++)
        {
            msgText.text = msg.Substring(0, i);
            yield return new WaitForSeconds(speedText);
        }
        isSkip = true;
    }

    public void SkipText()
    {
        Debug.Log("Skip");
        StopAllCoroutines();
        msgText.text = msg;
    }

    public void Next()
    {
        if (isSkip)
        {
            Debug.Log("Next");
            method();
        }
        else
        {
            SkipText();
            isSkip = true;
        }
    }
}
