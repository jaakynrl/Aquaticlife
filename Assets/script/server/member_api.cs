using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class member_api : MonoBehaviour
{
    [SerializeField]
    string url_insert;
    [Obsolete]
    public void insert_member()
    {
        WWWForm form = new WWWForm();
        form.AddField("uuid", Guid.NewGuid().ToString());
        ObservableWWW.Post(network_setting.hostname + url_insert, form).Subscribe(
            x =>
            {

                if(x != "-1")
                {
                    PlayerPrefs.SetInt("user_id", int.Parse(x));
                }
            },
            ex => Debug.Log(ex)
            );
    }

}
