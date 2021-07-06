using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEditor;

public class User {
    public string ID;
    public string username;
    public string password;
}

public class Money
{
    public int coin;
    public int diamond;
}

public class Firebase : MonoBehaviour
{
    public static Firebase instance { get; private set; }

    public string endPoint = "https://thesis-aquatic.firebaseio.com/";
    public string playerID = "player2";
    public Money currentMoney;

    [Header("Money")]
    public int coinInput;
    public int diamondInput;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))  // ส่งไปสร้าง
        {
            Money money = new Money();
            money.coin = coinInput;
            money.diamond = diamondInput;

            RestClient.Post(endPoint + playerID + ".json", money);
        }
        if (Input.GetKeyDown(KeyCode.D)) // ดึงข้อมูล playerID จาก firebase
        {
            RestClient.Get<Money>(endPoint + playerID + ".json").Then(resMoney => {
                currentMoney = resMoney;
            });
        }
        if (Input.GetKeyDown(KeyCode.F)) // ลบ playerID ใน firebase
        {
            RestClient.Delete(endPoint + playerID + ".json", (err, res) => {
                if(res.StatusCode.ToString() == "200")
                {
                    Debug.Log("Delete success.");
                }
            });
        }
        if (Input.GetKeyDown(KeyCode.G))  // แก้ หรือสร้าง
        {
            Money money = new Money();
            money.coin = coinInput;
            money.diamond = diamondInput;
            SetData(money);
        }
    }

    public void SetData(Money money)
    {
        RestClient.Put(endPoint + playerID + ".json", money);
    }

    public void GetData()
    {
        var gameSys = FindObjectOfType<gameSystem>();
        RestClient.Get<Money>(endPoint + playerID + ".json").Then(res => {
            if (gameSys)
            {
                gameSys.SetMoney(res);
            }
        });

    }

   
}
