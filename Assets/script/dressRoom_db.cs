using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class dressRoom_db : MonoBehaviour
{
    public string url;

    // Start is called before the first frame update
    [Obsolete]
    void Start()
    {
        load_itemShop();
    }

    [Obsolete]
    void load_itemShop()
    {
        WWWForm form = new WWWForm();
        int user_id = PlayerPrefs.GetInt("user_id");
        form.AddField("member_Id", user_id);

        ObservableWWW.Post(network_setting.hostname + url, form).Subscribe(
        x => dreeRoom._room.jsonToItem(x), // onSuccess
        ex => dreeRoom._room.jsonToItem("")); // onError
    }
}
