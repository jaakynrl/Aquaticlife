using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManage : MonoBehaviour
{
    public member_api _api;
    public GameObject loading;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("user_id"))
        {
            Debug.Log(PlayerPrefs.GetInt("user_id"));
            loading.SetActive(false);

        }
        else
        { 
            _api.insert_member();
            loading.SetActive(false);
        }
    }

}
