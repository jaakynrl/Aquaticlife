using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public class CardData
{
    public int id;
    public string card_name;
    public string card_pathImage;
    public string card_thumbnail;
    public string card_detail;
    public string stage;
    public int minigame;
}

[Serializable]
public class card
{
    public List<CardData> card_data;
}

[Serializable]
public class CardCollection
{
    public int id;
    public int card_id;
    public int user_id;
    public string create_date;
}

[Serializable]
public class user_collection
{
    public List<CardCollection> card_collection;
}



public class card_api : MonoBehaviour
{
    [SerializeField]
    string url;
    [SerializeField]
    string stage_id;

    [SerializeField]
    string url_insert;

    [SerializeField]
    string url_collection;

    
    // Start is called before the first frame update
    [Obsolete]
    void Start()
    {
        load_card();
        load_collection();
    }

    // Update is called once per frame


    [Obsolete]
    void load_card()
    {
        ObservableWWW.Get(network_setting.hostname + url+"?stage_id="+ stage_id).Subscribe(
        x => comeBackLoadCard._cardScript.jsonToCard(x), // onSuccess
        ex => comeBackLoadCard._cardScript.jsonToCard("0")); // onError
    }

    [Obsolete]
    void load_collection()
    {
        ObservableWWW.Get(network_setting.hostname + url_collection + "?user_id=" + PlayerPrefs.GetInt("user_id")).Subscribe(
        x => comeBackLoadCard._cardScript.jsonToCollection(x), // onSuccess
        ex => comeBackLoadCard._cardScript.jsonToCollection("0")); // onError
    }

    [Obsolete]
    public void insert_card(int card_id, int user_id)
    {
        WWWForm form = new WWWForm();
        user_id = PlayerPrefs.GetInt("user_id");
        form.AddField("user_id", user_id);
        form.AddField("card_id", card_id);
        ObservableWWW.Post(network_setting.hostname + url_insert, form).Subscribe(
            x => Debug.Log(x),
            ex => Debug.Log(ex)
            );
    }

}
