using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using TMPro;

[Serializable]
public class preview_item
{
    public TextMeshProUGUI text_name;
    public TextMeshProUGUI text_price;
    public RawImage imagePreview;
    public int item_id;
    public float price_item;
    public item_shop item;
    public Image price_typeImg;
    public Sprite coinType;
    public Sprite diamondType;
}

public class shop_script : MonoBehaviour
{
    public List<GameObject> boxTypeShop;
    public item_shop _item;
    public item_shop_data _itemShop;
    public static shop_script static_shop;
    public string urlImagePath;
    public preview_item _preview;
    public GameObject groupPreview;
    public GameObject typeCurrent;
    public item_shop currentItem;
    public buyItem _buyItem;

    public RawImage popupBuyImage;
    public GameObject popupBuy;
    public gameSystem _system;

    public Button buyItem_btn;

    private void Awake()
    {
        static_shop = this;
    }
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        
    }

    void shopItemCreate()
    {
        foreach (_item_shop item in _itemShop.data)
        {
            GameObject itemClone = Instantiate(_item.gameObject,
                boxTypeShop[item.category_id-1].GetComponent<item_shop_type>().wrapBox.transform,
                false);
            network_setting.loadImage_RawImage(network_setting.hostname + urlImagePath + item.thumb_image_path,
                itemClone.GetComponent<item_shop>().image);
            float item_plus = float.Parse(item.price)+0f;
            itemClone.GetComponent<item_shop>().price = item_plus;
            itemClone.GetComponent<item_shop>().text_price.text = item_plus.ToString();//
            itemClone.GetComponent<item_shop>().item_name = item.item_name;
            itemClone.GetComponent<item_shop>().type_item = item.category_id;
            itemClone.GetComponent<item_shop>().typePrice = item.price_type;
            itemClone.GetComponent<item_shop>().item_id = int.Parse(item.id);
            item_shop _itemShopClone = itemClone.GetComponent<item_shop>();
            if(item.member_Id == PlayerPrefs.GetInt("user_id"))
            {
                itemClone.GetComponent<item_shop>().isBuy.SetActive(true);
                itemClone.GetComponent<Button>().interactable = false;
            }
            itemClone.GetComponent<Button>().onClick.AddListener(() => preview_show(_itemShopClone));
        }
    }


    public void jsonToItem(string json)
    {
        if (json != "0")
        {
            _itemShop = JsonUtility.FromJson<item_shop_data>(json);
            shopItemCreate();
        }
    }


    public void select_shopType(GameObject shopType)
    {
        typeCurrent.SetActive(false);
        typeCurrent = shopType;
        typeCurrent.SetActive(true);
    }

    public void preview_show(item_shop _itemShop)
    {
        groupPreview.SetActive(true);
        currentItem = _itemShop;
        _preview.price_item = _itemShop.price;
        _preview.item_id = _itemShop.item_id;
        _preview.text_name.text = _itemShop.item_name;
        _preview.text_price.text = _itemShop.text_price.text;
        _preview.imagePreview.texture = _itemShop.image.texture;
        _preview.imagePreview.GetComponent<AspectRatioFitter>().aspectRatio = (float)_itemShop.image.texture.width / _itemShop.image.texture.height;
        if (currentItem.typePrice == "c")
        {
            _preview.price_typeImg.sprite = _preview.coinType;
        }
        else if (currentItem.typePrice == "d")
        {
            _preview.price_typeImg.sprite = _preview.diamondType;
        }
    }


    public void showPopup_buy()
    {
        popupBuy.SetActive(true);
        buyItem_btn.interactable = false;
        popupBuyImage.texture = _preview.imagePreview.texture;
        popupBuyImage.GetComponent<AspectRatioFitter>().aspectRatio = (float)popupBuyImage.texture.width / popupBuyImage.texture.height;
        Debug.Log(currentItem.typePrice);
        if(currentItem.typePrice == "c")
        {
            
            if (_system.coin - _preview.price_item >= 0)
            {
                buyItem_btn.interactable = true;
            }
        }
        else if(currentItem.typePrice == "d")
        {

           
            if (_system.diamond - _preview.price_item >= 0)
            {
                buyItem_btn.interactable = true;
            }
        }


        
    }
    public void buy_item()
    {
        _buyItem.insert_card(_preview.item_id, _preview.price_item, currentItem.typePrice);
        popupBuy.SetActive(false);

        if (currentItem.typePrice == "c")
        {
            _system.coin = _system.coin - _preview.price_item;
            _system.coinText.text = _system.coin.ToString();
           
        }
        else if (currentItem.typePrice == "d")
        {
            _system.diamond = _system.diamond - _preview.price_item;
            _system.diamondText.text = _system.diamond.ToString();
        }


        currentItem.isBuy.SetActive(true);
        groupPreview.SetActive(false);
        currentItem.GetComponent<Button>().interactable = false;
    }

}
