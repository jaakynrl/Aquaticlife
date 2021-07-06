using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameSystem : MonoBehaviour
{
    public Text coinText;
    public Text diamondText;
    public coin_diamond_api coin_diamondAPI;
    public int user_id;

    public float coin;
    public float diamond;
    void Start()
    {
     
        coin_diamondAPI.get_coin_diamond(user_id,cb=>
        {
            Debug.Log(cb.coin);
            if(cb != null)
            {
                SetMoney(cb);
            }
            else
            {
                Money m = new Money();
                m.coin = 0;
                m.diamond = 0;
                SetMoney(m);
            }
        });
    
    }

    public void SetMoney(Money money)
    {
        if (money != null)
        {
            coinText.text = money.coin.ToString();
            coin = money.coin;
            diamondText.text = money.diamond.ToString();
            diamond = money.diamond;
        }
    }
}
