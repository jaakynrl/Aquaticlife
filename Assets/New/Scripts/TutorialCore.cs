using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TutorialStep
{
    public int stepAt;
    public GameObject obj;
    public bool hasBtnNext;
}
public class TutorialCore : MonoBehaviour
{
    public GameObject tutorialObj;
    public List<string> message = new List<string>();
    public List<TutorialStep> step = new List<TutorialStep>();

    private Dictionary<int, TutorialStep> dicStep = new Dictionary<int, TutorialStep>();
    public TextRunning tr;

    public int indexMsg = 0;

    private void Awake()
    {
        step.ForEach(s =>
        {
            dicStep.Add(s.stepAt, s);
        });
    }
    private void OnEnable()
    {
        LobbyManager.instance.playTutorial = true;
        indexMsg = 0;
        OnNextStep();
    }

    public void OnNextStep()
    {
        if(indexMsg < message.Count)
        {
            tutorialObj.SetActive(true);
        }

        if (dicStep.ContainsKey(indexMsg))
        {
            dicStep[indexMsg].obj.SetActive(false);
        }
        tr.SetText(message[indexMsg], () => {
            if (dicStep.ContainsKey(indexMsg) && !dicStep[indexMsg].hasBtnNext && indexMsg < 6)
            {
                Debug.Log(indexMsg + ": Set next");
                OnNextStep();
            }
            else if(indexMsg >= 6)
            {
                if (indexMsg >= message.Count - 1)
                {
                    step[step.Count - 1].obj.SetActive(false);
                    LobbyManager.instance.playTutorial = false;
                    gameObject.SetActive(false);
                    return;
                }
                Debug.Log(indexMsg + ": Set none");
                tutorialObj.SetActive(false);
            }else if (!dicStep.ContainsKey(indexMsg)){
                OnNextStep();
            }
        });
        indexMsg++;
        if (dicStep.ContainsKey(indexMsg))
        {
            dicStep[indexMsg].obj.SetActive(true);
        }
    }
}
