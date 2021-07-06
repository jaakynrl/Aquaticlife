using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyScript : MonoBehaviour
{

    public Text coinText;
    public Text diamondText;

    public Money money;

    private void Start()
    {
        money = new Money();
        money.coin = Random.Range(1, 101);
        money.diamond = Random.Range(1, 11);


        coinText.text = money.coin.ToString();
        diamondText.text = money.diamond.ToString();

//        Firebase.instance.SetData(money);
    }

}
