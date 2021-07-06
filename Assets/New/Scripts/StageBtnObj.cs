using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageBtnObj : MonoBehaviour
{
    public Button stageBtn;
    public GameObject imgBG;
    public GameObject lockObj;
    public Button unlockBtn;
    public Text priceUnlockText;

    public void Set(bool islock,int unlockPrice, Action unlockFun)
    {
        imgBG.SetActive(islock);
        lockObj.SetActive(islock);
        unlockBtn.gameObject.SetActive(islock);
        stageBtn.interactable = !islock;
        if (islock)
        {
            unlockBtn.onClick.AddListener(() => { unlockFun(); });
            priceUnlockText.text = unlockPrice + " THB";
        }
    }
}
