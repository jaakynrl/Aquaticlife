using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

[Serializable]
public class _item_shop
{
    public string id;
    public string item_name;
    public string price;
    public string price_type;
    public string image_path;
    public string thumb_image_path;
    public int category_id;
    public int member_Id;
}
[Serializable]
public class item_shop_data
{
    public List<_item_shop> data;
}


public class itemShope_api : MonoBehaviour
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

        ObservableWWW.Post(network_setting.hostname+url, form).Subscribe(
        x => shop_script.static_shop.jsonToItem(x), // onSuccess
        ex => shop_script.static_shop.jsonToItem("")); // onError
    }


   
}
