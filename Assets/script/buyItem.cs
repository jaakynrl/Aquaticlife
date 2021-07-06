using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class buyItem : MonoBehaviour
{
    [SerializeField]
    string url_insert;
    [Obsolete]
    public void insert_card(int item_id, float price ,string type, int user_id = -1)
    {
        WWWForm form = new WWWForm();
        user_id = PlayerPrefs.GetInt("user_id");
        form.AddField("member_Id", user_id);
        form.AddField("item_id", item_id);
        form.AddField("type", type);
        form.AddField("price", price.ToString());
        ObservableWWW.Post(network_setting.hostname + url_insert, form).Subscribe(
            x => Debug.Log(x),
            ex => Debug.Log(ex)
            );
    }
}
