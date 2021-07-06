using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class comeBackLoadCard : MonoBehaviour
{
	public card cardList;
    public List<CardData> card_data;
    public static comeBackLoadCard _cardScript;
    public card_api cardApi;
    public int user_id;
    public card _collection;
    int load_success;
    [SerializeField]
    card_script[] _cards;

    [SerializeField]
    MoneyScript _money;

    [SerializeField]
    UnityEngine.UI.Button btnMiniGame;
    
    [SerializeField]
    string imgPath;
    int cardCollect;

    public coin_diamond_api coinApi;

    public GameObject collectAll;
    private void Awake()
    {
        _cardScript = this;
       
    }
    // Start is called before the first frame update
    void Start()
    {
        randomCardNumber();
    }

    // Update is called once per frame
    void Update()
    {
        if (load_success == 2)
        {
            load_success = -1;
            randomCard();
            insertCoin_Diamond(_money.money.coin, _money.money.diamond);


        }
    }

    void insertCoin_Diamond(int coin, int diamond)
    {
        coinApi.insert_coin_diamond(coin, diamond, user_id);
    }

    public void jsonToCard(string json)
    {
        if (json != "0")
        {
            cardList = JsonUtility.FromJson<card>(json);
            card_data = cardList.card_data;

        }
        load_success += 1;
    }

    public void jsonToCollection(string json)
    {
        if (json != "0")
        {
            _collection = JsonUtility.FromJson<card>(json);

        }
        load_success += 1;

    }


    public void randomCardNumber()
    {
        int random_number = Random.Range(0, 10);
        if(random_number % 2 == 0)
        {
            cardCollect = 1;
        }
        else
        {
            cardCollect = 2;
        }
    }
    public void randomCard()
    {
        foreach (CardData item in _collection.card_data)
        {
            card_data = card_data.FindAll(x => x.id != item.id);
        }

        for (int i = 0; i < cardCollect; i++)
        {
            CardData _card;
            if (card_data.Count == 0)
            {
                if (collectAll != null)
                {
                    collectAll.SetActive(true);
                }
                break;
            }
            else if (card_data.Count == 1)
            {
                int randomCard = Random.Range(0, card_data.Count);
                _card = card_data[randomCard];
                cardApi.insert_card(_card.id, user_id);
                
                int index = i;
                if (_card.minigame == 1)
                {
                    btnMiniGame.interactable = true;
                }
                network_setting.loadImage_RawImage(network_setting.hostname + "asset/" + _card.card_pathImage, (cb) =>
                {
                    _cards[index].img.texture = cb;
                    _cards[index].setAspect();
                    _cards[index].gameObject.SetActive(true);
                });

                break;
            }
            else
            {

                int randomCard = Random.Range(0, card_data.Count);
                _card = card_data[randomCard];
                cardApi.insert_card(_card.id, user_id);
                card_data.RemoveAt(randomCard);

                int index = i;
                if (_card.minigame == 1)
                {
                    btnMiniGame.interactable = true;
                }
                network_setting.loadImage_RawImage(network_setting.hostname + "asset/" + _card.card_pathImage, (cb) =>
                {
                    _cards[index].img.texture = cb;
                    _cards[index].setAspect();
                    _cards[index].gameObject.SetActive(true);
                });
            }

         


        }
    }
}
