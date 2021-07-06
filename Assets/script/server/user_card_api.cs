using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class user_card_api : MonoBehaviour
{
    public card _collection;
    [SerializeField]
    string url_collection;

    public GameObject card_create;
    public GameObject wrap_card;

    public GameObject popup_card;
    public RawImage card_img;
    [Obsolete]
	void Start()
	{
        load_collection();
	}

    [Obsolete]
    void load_collection()
    {
        ObservableWWW.Get(network_setting.hostname + url_collection + "?user_id=" + PlayerPrefs.GetInt("user_id")).Subscribe(
        x => jsonToCollection(x), // onSuccess
        ex => jsonToCollection("0")); // onError
    }

    public void jsonToCollection(string json)
    {
        if (json != "0")
        {
            _collection = JsonUtility.FromJson<card>(json);
            if(_collection.card_data.Count > 0)
            {

                for (int i = 0; i < _collection.card_data.Count; i++)
                {
                    GameObject card_clone = Instantiate(card_create, wrap_card.transform);
                    card_clone.GetComponent<card_collect_user>().url_popup = network_setting.hostname + "asset/" + _collection.card_data[i].card_pathImage;
                    network_setting.loadImage_RawImage(network_setting.hostname + "asset/" + _collection.card_data[i].card_thumbnail, (cb) =>
                    {
                        card_clone.GetComponent<card_collect_user>().img.texture = cb;
                        card_clone.GetComponent<card_collect_user>().setAspect();
                        int index_img = i;

                        network_setting.loadImage_RawImage(card_clone.GetComponent<card_collect_user>().url_popup, (cb_popup) =>
                        {
                            card_clone.GetComponent<card_collect_user>().imgPopup = cb_popup;
                            card_clone.GetComponent<Button>().onClick.AddListener(() =>
                            {
                                popup_card.SetActive(true);
                                card_img.texture = card_clone.GetComponent<card_collect_user>().imgPopup;
                                card_img.GetComponent<AspectRatioFitter>().aspectRatio = card_clone.GetComponent<card_collect_user>().imgPopup.width / (float)card_clone.GetComponent<card_collect_user>().imgPopup.height;
                            });
                        });

                    });
                }
            }

        }

    }

}
