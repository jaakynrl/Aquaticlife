using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using Proyecto26;
using UnityEngine.Networking;
using UnityEngine.Events;

[System.Serializable]
public class Member
{
    public string id;
    public string uuid;
    public int coin;
    public int diamond;
    public int gas;
    public string[] achieve_stage;
    public string created_at;
    public string is_new;
}

[System.Serializable]
public class Item
{
    public string id;
    public string name;
    public int price;
    public int currency;
    public string image;
    public string image64;
    public string detail;
    public int diamond;
    //public Sprite iImg;
    public int category_id;
    public string create_by;
    public string created_at;
    public bool loadImg = false;
}

[System.Serializable]
public class Card
{
    public string id;
    public string name;
    public string detail;
    public string image;
    public string image_preview;
    public string image64;
    public string image_preview64;
    public int stage;
    public int minigame;
    public string created_at;
    //public Sprite cImg;
    //public Sprite cpImg;
    public bool loadImg = false;
}

[System.Serializable]
public class CharacterCustom
{
    public string id;
    public string item_id;
    public string position;
}
[System.Serializable]
public class Stage
{
    public string name;
    public string[] requestItemIds;
    public int unlock_price;
    public float range_time;
}
[System.Serializable]
class DataVersion
{
    public int version;
    public bool card_update;
    public bool item_update;
}

public class ServiceManager : MonoBehaviour
{
    public static ServiceManager instance { get; private set; }
    public bool isTest = false;
    public Button loginBtn;

    private string uuid;
    public string endPoint = "localhost:8080/cms-aquatic/"; // https://game-pirest.aowdtech.com/
    public bool isLogin = false;

    public int amountData = 0;
    public int currentData = 0;
    public float timeLoadImage = 0.5f;
    public DataCore dc;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(_GetDataVersion());
        if (!PlayerPrefs.HasKey("game_version"))
            PlayerPrefs.SetInt("game_version", 0);
        uuid = SystemInfo.deviceUniqueIdentifier;

        var lm = LobbyManager.instance;

        if (isLogin)
        {
            lm.Init();
        }
        lm.loadingObj.SetActive(true);
        lm.introObj.SetActive(true);

        //GetDataVersion();

        GetStage();
    }



    public void OnLogin()
    {
        StartCoroutine(Login());
    }

    public void CheckLogin()
    {
        var lm = LobbyManager.instance;
        if (lm && !isLogin)
        {
            //if (!isLogin && PlayerPrefs.HasKey("uuid"))
            //{
            //    StartCoroutine(Login());
            //}
            //else
            //{
            //    if (lm)
            //        lm.EndLoading();
            //}

            if (lm)
                lm.EndLoading();
        }
        
    }


    IEnumerator Login()
    {
        var lm = LobbyManager.instance;
        lm.loadingObj.SetActive(true);
        WWWForm form = new WWWForm();
        form.AddField("uuid", uuid);
        using (UnityWebRequest www = UnityWebRequest.Post(endPoint + "service-game/member-login.php", form))
        {
            yield return www.SendWebRequest();

            if (!www.isNetworkError || !www.isHttpError)
            {
                dc.account = JsonUtility.FromJson<Member>(www.downloadHandler.text);
                if (dc.account.is_new == "true")
                {
                    PlayerPrefs.DeleteAll();
                    lm.playTutorial = true;
                }
                PlayerPrefs.SetString("uuid", uuid);
                isLogin = true;
                GetInvenItem();
            }
            else
            {
                lm.EndLoading();
            }
        }
    }

    class DataInvenItem
    {
        public string[] invenItems;
    }

    public void GetInvenItem()
    {
        var lm = LobbyManager.instance;
        RestClient.Get<DataInvenItem>(endPoint + "service-game/inven-item.php?id=" + dc.account.id).Then((data) =>
        {
            var list = data.invenItems.ToList();
            if (list.Count > 0) { dc.invenItemsId = list; }
            GetInvenCard();
        }).Catch((err) =>
        {
            Debug.Log("Get data inven item fail");
        });
    }



    class DataCardCollection
    {
        public string[] invenCards;
    }

    public void GetInvenCard()
    {
        Debug.Log("Get card");
        var lm = LobbyManager.instance;
        RestClient.Get<DataCardCollection>(endPoint + "service-game/inven-card.php?id=" + dc.account.id).Then((data) =>
        {
            var list = data.invenCards.ToList();
            if (list.Count > 0)
            {
                dc.invenCardsId = list;
            }
            lm.EndLoading(() =>
            {
                AudioManager.instance.SetBGM(1);
                lm.Init();
            });
        }).Catch((err) =>
        {
            Debug.Log("Get data inven card fail");
        });
    }



    private DataVersion version;
    public void GetDataVersion()
    {
        StartCoroutine(_GetDataVersion());
    }
    IEnumerator _GetDataVersion()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(endPoint + "service-game/data-version.php"))
        {
            yield return www.SendWebRequest();

            Debug.Log(www.downloadHandler.text);
            if (!www.isNetworkError || !www.isHttpError)
            {
                var currenVersion = PlayerPrefs.GetInt("game_version");
                version = JsonUtility.FromJson<DataVersion>(www.downloadHandler.text);
                if (version.version < currenVersion)
                    currenVersion = version.version;
                dc.LoadData(currenVersion);
            }
            else
            {
                Debug.Log("fail");
            }
        }
    }
    public void CheckUpdate()
    {
        if (PlayerPrefs.GetInt("game_version") == version.version)
        {
            Debug.Log("Last update");
            if (dc.imgs.Values.Count <= 0)
            {
                StartCoroutine(_GetCard(dc.cardIds.ToArray()));
                StartCoroutine(_GetItem(dc.itemIds.ToArray()));
            }
            PlayerPrefs.SetInt("game_version", version.version);
            return;
        }

        Debug.Log("Load update ++++++");
        if (version.card_update)
        {
            StartCoroutine(_GetCard(dc.cardIds.ToArray()));
        }
        if(version.item_update)
        {
            StartCoroutine(_GetItem(dc.itemIds.ToArray()));
        }
        PlayerPrefs.SetInt("game_version", version.version);
    }

    class DataCard
    {
        public Card[] cards;
    }

    public void GetCard(string[] ids)
    {
        StartCoroutine(_GetCard(ids));
    }
    IEnumerator _GetCard(string[] ids)
    {
        WWWForm form = new WWWForm();
        if (ids.Length > 0)
        {
            form.AddField("ids", string.Join("','", ids));
        }
        using (UnityWebRequest www = UnityWebRequest.Post(endPoint + "service-game/card.php", form))
        {
            yield return www.SendWebRequest();
            if (!www.isNetworkError || !www.isHttpError)
            {
                Debug.Log("1");
                var list = JsonUtility.FromJson<DataCard>(www.downloadHandler.text);
                dc.listCard = list.cards.ToList();
                dc.SaveDataGame("data-card");
                dc.CreateSpriteCard(list.cards.ToList(), () => {
                    Debug.Log("Clear list card :" + dc.listCard.Count);
                    dc.listCard.Clear(); 
                });

                dc.listCard.ForEach(c => { dc.cardIds.Add(c.id); });
                dc.SaveDataGame("card-id");
                Debug.Log("3");
            }
            else
            {
                Debug.Log("Fail");
            }
        }
    }

    class DataItem
    {
        public Item[] items;
    }

    public void GetItem(string[] ids)
    {
        StartCoroutine(_GetItem(ids));
    }

    IEnumerator _GetItem(string[] ids)
    {
        WWWForm form = new WWWForm();
        if (ids.Length > 0)
        {
            form.AddField("ids", string.Join("','", ids));
        }
        using (UnityWebRequest www = UnityWebRequest.Post(endPoint + "service-game/item.php", form))
        {
            yield return www.SendWebRequest();
            if (!www.isNetworkError || !www.isHttpError)
            {
                var list = JsonUtility.FromJson<DataItem>(www.downloadHandler.text);
                dc.listItem = list.items.ToList();
                dc.SaveDataGame("data-item");
                dc.CreateSpriteItem(list.items.ToList(), () => {
                    Debug.Log("Clear list item :" + dc.itemIds.Count);
                    dc.listItem.Clear();
                });

                Debug.Log("i1");
                dc.listItem.ForEach(i => { dc.itemIds.Add(i.id); });
                dc.SaveDataGame("item-id");
                Debug.Log("i2");
            }
            else
            {
                Debug.Log("Fail");
            }
        }
    }
    class DataStage
    {
        public Stage[] stages;
    }

    public void GetStage()
    {
        RestClient.Get<DataStage>(endPoint + "service-game/stage.php").Then((data) =>
        {
            var lm = LobbyManager.instance;
            dc.stage = data.stages.ToList();
        }).Catch((err) =>
        {
            Debug.Log("Get data item fail");
        });
    }


    public void Puchase(int cash, int currency, string itemId, GameObject confirmPanel)
    {
        StartCoroutine(IEPuchase(cash, currency, itemId, confirmPanel));
    }

    IEnumerator IEPuchase(int cash, int currency, string itemId, GameObject confirmPanel)
    {
        var lm = LobbyManager.instance;
        lm.loadingObj.SetActive(true);

        WWWForm form = new WWWForm();
        form.AddField("currency", currency);
        form.AddField("cash", cash);
        form.AddField("item_id", itemId);
        form.AddField("member_id", dc.account.id);

        using (UnityWebRequest www = UnityWebRequest.Post(endPoint + "service-game/inven-item.php", form))
        {
            yield return www.SendWebRequest();

            if (!www.isNetworkError || !www.isHttpError)
            {
                var data = www.downloadHandler.text;
                string msg = "";
                if (data == "not enoght")
                {
                    msg = "เงินคุณไม่เพียงพอ";
                }
                else if (data == "success")
                {
                    DisCash(cash, currency);
                    dc.invenItemsId.Add(itemId);
                    msg = "ซื้อเรียบร้อย";
                }
                lm.EndLoading(() => {
                    confirmPanel.SetActive(false);
                    lm.SetConsole();
                    lm.SetAlert(msg);
                    lm.SetItemShop(lm.currentTypeShop);
                    lm.SetItemShopDetail(itemId, true);
                });
            }
            else
            {
                lm.EndLoading(() => {
                    lm.SetAlert("ผิดพลาด");
                });
            }
        }
    }

    public void PuchaseDiamond(int cash, int diamond, GameObject confirmPanel, Action method = null)
    {
        StartCoroutine(IEPuchaseDiamond(cash, diamond, confirmPanel, method));
    }

    IEnumerator IEPuchaseDiamond(int cash, int diamond, GameObject confirmPanel, Action method = null)
    {
        var lm = LobbyManager.instance;
        lm.loadingObj.SetActive(true);

        WWWForm form = new WWWForm();
        form.AddField("cash", cash);
        form.AddField("diamond", diamond);
        form.AddField("member_id", dc.account.id);

        using (UnityWebRequest www = UnityWebRequest.Post(endPoint + "service-game/diamond.php", form))
        {
            yield return www.SendWebRequest();

            if (!www.isNetworkError || !www.isHttpError)
            {
                var data = www.downloadHandler.text;
                string msg = "";
                if (data == "not enoght")
                {
                    msg = "เงินคุณไม่เพียงพอ";
                }
                else if (data == "success")
                {
                    dc.account.diamond += diamond;
                    DisCash(cash, 0);
                    msg = "ซื้อเรียบร้อย";
                }
                else
                {
                    msg = "มีด่านนี้แล้ว";
                }
                lm.EndLoading(() => {
                    if (confirmPanel)
                        confirmPanel.SetActive(false);
                    lm.SetConsole();
                    lm.SetAlert(msg);
                    method();
                    if (data == "not enoght")
                    {
                        lm.moneyPurchasePanel.SetActive(false);
                    }
                });
            }
            else
            {
                lm.EndLoading(() => {
                    lm.SetAlert("ผิดพลาด");
                });
            }
        }
    }

    public void PuchaseStage(int cash, string stage_id, GameObject confirmPanel, Action method = null)
    {
        StartCoroutine(IEPuchaseStage(cash, stage_id, confirmPanel, method));
    }


    IEnumerator IEPuchaseStage(int cash, string stage_id, GameObject confirmPanel, Action method = null)
    {
        var lm = LobbyManager.instance;
        lm.loadingObj.SetActive(true);

        WWWForm form = new WWWForm();
        form.AddField("cash", cash);
        form.AddField("stage_id", stage_id);
        form.AddField("member_id", dc.account.id);

        using (UnityWebRequest www = UnityWebRequest.Post(endPoint + "service-game/add-stage.php", form))
        {
            yield return www.SendWebRequest();

            if (!www.isNetworkError || !www.isHttpError)
            {
                var data = www.downloadHandler.text;
                string msg = "";
                if (data == "not enoght")
                {
                    msg = "เงินคุณไม่เพียงพอ";
                }
                else if (data == "success")
                {
                    msg = "ซื้อเรียบร้อย";
                }
                else
                {
                    msg = "มีด่านนี้แล้ว";
                }
                lm.EndLoading(() => {
                    if (confirmPanel)
                        confirmPanel.SetActive(false);
                    lm.SetConsole();
                    lm.SetAlert(msg);
                    if (data == "success")
                    {
                        method();
                    }
                    else if (data == "not enoght")
                    {
                        lm.moneyPurchasePanel.SetActive(false);
                    }
                });
            }
            else
            {
                lm.EndLoading(() => {
                    lm.SetAlert("ผิดพลาด");
                });
            }
        }
    }


    public void DisCash(int cash, int currency)
    {
        var lm = LobbyManager.instance;
        if (currency == 0)
        {
            dc.account.coin -= cash;
        }
        else if (currency == 1)
        {
            dc.account.diamond -= cash;
        }
    }


    public void AddCard(string cardId)
    {
        StartCoroutine(IEAddCard(cardId));
    }


    IEnumerator IEAddCard(string cardId)
    {
        var lm = LobbyManager.instance;
        lm.loadingObj.SetActive(true);

        WWWForm form = new WWWForm();
        form.AddField("member_id", dc.account.id);
        form.AddField("card_id", cardId);


        using (UnityWebRequest www = UnityWebRequest.Post(endPoint + "service-game/add-card.php", form))
        {
            yield return www.SendWebRequest();

            if (!www.isNetworkError || !www.isHttpError)
            {
                var data = www.downloadHandler.text;
                lm.EndLoading();
            }
            else
            {
                lm.EndLoading(() => {
                    lm.SetAlert("ผิดพลาด");
                });
            }
        }
    }

    public void AddCurrency(int cash, int currency, Action method)
    {
        StartCoroutine(IEAddCurrency(cash, currency, method));
    }


    IEnumerator IEAddCurrency(int cash, int currency, Action method)
    {
        var lm = LobbyManager.instance;
        if (lm)
        {
            lm.loadingObj.SetActive(true);
        }

        WWWForm form = new WWWForm();
        form.AddField("member_id", dc.account.id);
        form.AddField("currency", currency);
        form.AddField("cash", cash);


        using (UnityWebRequest www = UnityWebRequest.Post(endPoint + "service-game/add-currency.php", form))
        {
            yield return www.SendWebRequest();

            if (!www.isNetworkError || !www.isHttpError)
            {
                var data = www.downloadHandler.text;
                if (lm)
                    lm.EndLoading(() => { method(); });
            }
            else
            {
                lm.EndLoading(() => {
                    lm.SetAlert("ผิดพลาด");
                });
            }
        }
    }


    public void AddGas(int gas)
    {
        StartCoroutine(IEAddGas(gas));
    }

    IEnumerator IEAddGas(int gas)
    {
        var lm = LobbyManager.instance;
        lm.loadingObj.SetActive(true);

        WWWForm form = new WWWForm();
        form.AddField("member_id", dc.account.id);
        form.AddField("gas", gas);


        using (UnityWebRequest www = UnityWebRequest.Post(endPoint + "service-game/add-gas.php", form))
        {
            yield return www.SendWebRequest();

            if (!www.isNetworkError || !www.isHttpError)
            {
                var data = www.downloadHandler.text;
                if (lm)
                {
                    lm.EndLoading();
                }
            }
            else
            {
                if (lm)
                {
                    lm.EndLoading(() => {
                        lm.SetAlert("ผิดพลาด");
                    });
                }
            }
        }
    }
    public void AddStage(int stage_id)
    {
        StartCoroutine(IEAddStage(stage_id));
    }

    IEnumerator IEAddStage(int stage_id)
    {
        var lm = LobbyManager.instance;
        lm.loadingObj.SetActive(true);
        Debug.Log("Add stage");
        WWWForm form = new WWWForm();
        form.AddField("member_id", dc.account.id);
        form.AddField("stage_id", stage_id);


        using (UnityWebRequest www = UnityWebRequest.Post(endPoint + "service-game/add-stage.php", form))
        {
            yield return www.SendWebRequest();

            if (!www.isNetworkError || !www.isHttpError)
            {
                Debug.Log("Add stage ss");
                var data = www.downloadHandler.text;
                if (lm)
                {
                    lm.EndLoading();
                }
            }
            else
            {
                if (lm)
                {
                    lm.EndLoading(() => {
                        lm.SetAlert("ผิดพลาด");
                    });
                }
            }
        }
    }

}