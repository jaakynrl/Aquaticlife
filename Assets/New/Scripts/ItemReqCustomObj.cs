using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ItemReqCustomObj : MonoBehaviour
{
    public string id;
    public int catType;
    public Image img;
    public GameObject check;

    public void Set(string id,int catType, Sprite sp)
    {
        this.id = id;
        this.catType = catType;
        img.sprite = sp;
    }
    public void Check(bool check)
    {
        this.check.SetActive(check);
    }
}
