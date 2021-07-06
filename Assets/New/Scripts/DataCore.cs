using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class DataGame
{
    public int dataVersion = 0;
    public List<Item> listItem = new List<Item>();
    public List<Card> listCard = new List<Card>();
}
public class DataCore : MonoBehaviour
{
    public static DataCore instance { get; private set; }
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


    [Header("Data")]
    public Member account;
    public List<string> invenCardsId = new List<string>();
    public List<string> invenItemsId = new List<string>();

    public List<string> cardIds = new List<string>();
    public List<string> itemIds = new List<string>();
    public List<Item> listItem = new List<Item>();
    public List<Card> listCard = new List<Card>();
    public Dictionary<string, Item> items = new Dictionary<string, Item>();
    public Dictionary<string, Card> cards = new Dictionary<string, Card>();
    public List<Stage> stage = new List<Stage>();

    public Dictionary<string, Sprite> imgs = new Dictionary<string, Sprite>();
    //public DataGame data;

    public string pathData;

    public void LoadData(int version)
    {
        Debug.Log(GetPath("tagetfile"));
        for (int i = 1; i <= version + 1; i++)
        {
            _LoadData("card-id", i);
            _LoadData("item-id", i);
            _LoadData("data-card", i);
            _LoadData("data-item", i);
        }
        ServiceManager.instance.CheckUpdate();
    }
    public string GetPath(string fileName)
    {
        return Application.persistentDataPath + "/"+ fileName + ".bytes";
    }

    public void SaveDataGame(string fileName)
    {
        var version = PlayerPrefs.GetInt("game_version");
        PlayerPrefs.SetInt("game_version", version);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(GetPath(fileName +"-v" + version), FileMode.OpenOrCreate);
        if (fileName == "data-card")
        {
            bf.Serialize(file, listCard);
        }
        else if (fileName == "data-item")
        {
            bf.Serialize(file, listItem);
        }
        else if (fileName == "card-id")
        {
            bf.Serialize(file, cardIds);
        }
        else if (fileName == "item-id")
        {
            bf.Serialize(file, itemIds);
        }
        Debug.Log(fileName + ":save");
        file.Close();
    }

    public IEnumerable _LoadData(string fileName, int version)
    {
        string path = GetPath(fileName + "-v" + version);
        if (File.Exists(path))
        {
            FileStream readerFileStream = new FileStream(path, FileMode.Open, System.IO.FileAccess.Read);
            BinaryFormatter formatter = new BinaryFormatter();
            if (fileName == "data-card")
            {
                var list = formatter.Deserialize(readerFileStream) as List<Card>;
                CreateSpriteCard(list, () => { Debug.Log("Load card ver" + version + " success."); });
            }
            else if (fileName == "data-item")
            {
                var list = formatter.Deserialize(readerFileStream) as List<Item>;
                CreateSpriteItem(list, () => { Debug.Log("Load item ver" + version + " success."); });
            }
            else if (fileName == "card-id")
            {
                var list = formatter.Deserialize(readerFileStream) as List<string>;
                list.ForEach(l => { cardIds.Add(l); });
            }
            else if (fileName == "item-id")
            {
                var list = formatter.Deserialize(readerFileStream) as List<string>;
                list.ForEach(l => { itemIds.Add(l); });
            }
            readerFileStream.Close();
        }
        else
        {
            Debug.Log("No data" + version);
        }
        return null;
    }


    public void CreateSprite() {
        SaveDataGame("card-id");
        SaveDataGame("item-id");
        SaveDataGame("data-card");
        SaveDataGame("data-item");

    }
    public void CreateSpriteCard(List<Card> list, Action method)
    {
        StartCoroutine(_CreateSpriteCard(list, method));
    }
    [Obsolete]
    IEnumerator _CreateSpriteCard(List<Card> list, Action method)
    {
        Debug.Log("Create c:" + list.Count + ": ver:" + PlayerPrefs.GetInt("game_version"));
        var lm = LobbyManager.instance;
        var sm = ServiceManager.instance;
        if(list.Count > 0)
        {
            sm.amountData += list.Count;
        }
        for (int i = 0; i < list.Count; i++)
        {
            var v = list[i];
            //byte[] imageBytes = Convert.FromBase64String(v.image64);
            //Texture2D tex = new Texture2D(2, 2);
            //tex.LoadImage(imageBytes);
            //var sp = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            //v.image64 = "";
            //if(!imgs.ContainsKey("c" + v.id))
            //{
            //    imgs.Add("c" + v.id, sp);
            //}
            //yield return new WaitForSeconds(sm.timeLoadImage);

            var imageBytes = Convert.FromBase64String(v.image_preview64);
            var tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            var sp = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            v.image_preview64 = "";
            if (!imgs.ContainsKey("cp" + v.id))
            {
                imgs.Add("cp" + v.id, sp);
            }
            yield return new WaitForSeconds(sm.timeLoadImage);

            v.loadImg = true;
            if (!cards.ContainsKey(v.id))
            {
                cards.Add(v.id, v);
            }
            Debug.Log(v.name + ": succ");
            sm.currentData++;
            lm.SetLoadData(sm.currentData, sm.amountData);
        }
        method();
    }

    public void CreateSpriteItem(List<Item> list, Action method)
    {
        StartCoroutine(_CreateSpriteItem(list, method));
    }
    [Obsolete]
    IEnumerator _CreateSpriteItem(List<Item> list, Action method)
    {
        Debug.Log("Create i:" + list.Count + ": ver:" + PlayerPrefs.GetInt("game_version"));
        var lm = LobbyManager.instance;
        var sm = ServiceManager.instance;
        if (list.Count > 0)
        {
            sm.amountData += list.Count;
        }
        for (int i = 0; i < list.Count; i++)
        {
            var v = list[i];
            byte[] imageBytes = Convert.FromBase64String(v.image64);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            var sp = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            v.image64 = "";
            if (!imgs.ContainsKey("i" + v.id))
            {
                imgs.Add("i" + v.id, sp);
            }
            yield return new WaitForSeconds(sm.timeLoadImage);

            v.loadImg = true;
            if (!items.ContainsKey(v.id))
            {
                items.Add(v.id, v);
            }
            sm.currentData++;
            lm.SetLoadData(sm.currentData, sm.amountData);
        }
        method();
    }

}