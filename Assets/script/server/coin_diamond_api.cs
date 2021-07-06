using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class coin_diamond_api : MonoBehaviour
{
    [SerializeField]
    string url_insert;
    [SerializeField]
    string url_get;
    // Start is called before the first frame update

    [Obsolete]
    public void insert_coin_diamond(int coin,int diamond, int user_id = -1)
    {
        WWWForm form = new WWWForm();
        user_id = PlayerPrefs.GetInt("user_id");
        form.AddField("user_id", user_id);
        form.AddField("coin_value", coin);
        form.AddField("diamond_value", diamond);
        ObservableWWW.Post(network_setting.hostname + url_insert, form).Subscribe(
            x => Debug.Log(x),
            ex => Debug.Log(ex)
            );
    }


    [Obsolete]
    public void get_coin_diamond(int user_id,Action<Money> cb)
    {
        Money _coin_diamond = new Money();
        WWWForm form = new WWWForm();
        user_id = PlayerPrefs.GetInt("user_id");
        form.AddField("user_id", user_id);
        ObservableWWW.Post(network_setting.hostname + url_get, form).Subscribe(
            x =>
            {
                _coin_diamond = JsonUtility.FromJson<Money>(x);
                cb(_coin_diamond);
            },
            ex =>
            {
                cb(null);
            }
            );
    }
}
