using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class groupItem
{
	public string typeName;
	public int id;
    public Button type_btn;
    public List<dressItem> _dressItem;
    public GameObject itemHuman;
    public bool accType;
    public List<AccItem> _AccItem;


}
[System.Serializable]
public class AccItem
{
    public string accName;
    public GameObject accItemHuman;
    public Sprite _itemSprite;
    public Texture2D itemTexture;

}

[System.Serializable]
public class dressItem
{
	public string dressName;
	public int dress_item;
    public Texture dressImage;
    public Sprite _dreeSprite;
    public string accKey;
    public Texture dressItemHuman;
    public Sprite _dressItemHuman;
    public string type_item;
}

[System.Serializable]
public class labelGameStage
{
    public GameObject label;
    public string stageName;
}

public class dreeRoom : MonoBehaviour
{
    public static stage_game itemStages;
    public List<groupItem> _groupItem;
    public GameObject dressType_box;
    public GameObject dressItem_box;
    public GameObject parentGroup;

    public GameObject currentType_select;
    public stage_game _itemStages;
    [SerializeField]
    bool create_first;

    public static dreeRoom _room;
    public List<labelGameStage> _labelStage;

    private void Awake()
    {
        _room = this;
    }

    public item_shop_data _item;

    public GameObject checkBox1;
    public GameObject checkBox2;


    // Start is called before the first frame update
    void Start()
    {
        _itemStages = itemStages;
        if(itemStages != null)
        {
            if(itemStages.stage_name != "")
            {
                int indexStage = _labelStage.FindIndex(x => x.stageName == itemStages.stage_name);
                _labelStage[indexStage].label.SetActive(true);

                foreach (itemStage item in itemStages.items)
                {
                    item.itemCheck = false;
                }
            }
        }
    }

    public void dressMainScript()
    {
        foreach (groupItem _group in _groupItem)
        {
            GameObject _groupCreate = Instantiate(dressType_box, parentGroup.transform, false);
            _groupCreate.name = _group.typeName;

            _group.type_btn.onClick.AddListener(() =>
            {
                if (currentType_select != null)
                {
                    currentType_select.SetActive(false);

                }

                currentType_select = _groupCreate;
                currentType_select.SetActive(true);
            });



            if (!create_first)
            {
                _groupCreate.SetActive(false);
            }
            else
            {
                currentType_select = _groupCreate;
            }


            create_first = false;
            foreach (dressItem _dressItem in _group._dressItem)
            {
                GameObject _itemCreate = Instantiate(dressItem_box, _groupCreate.transform, false);
                _itemCreate.name = _dressItem.dressName;
                if(_dressItem.type_item == "2")
                {
                    _itemCreate.GetComponent<dressItem_box>().itemName = "เสื้อ";
                }

                if (_dressItem.type_item == "3")
                {
                    _itemCreate.GetComponent<dressItem_box>().itemName = "กางเกง";
                }

                if (_dressItem.dressImage != null)
                {
                    _itemCreate.GetComponent<dressItem_box>().imgShow.texture = _dressItem.dressImage;
                    _itemCreate.GetComponent<dressItem_box>().imgShow.GetComponent<AspectRatioFitter>().aspectRatio = (float)_dressItem.dressImage.width / _dressItem.dressImage.height;
                }

                _itemCreate.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (_itemCreate.GetComponent<dressItem_box>().itemName != "")
                    {
                        if (itemStages != null)
                        {
                            if (itemStages.items.Count > 0)
                            {

                                int findItem = itemStages.items.FindIndex(x => x.itemName == _itemCreate.GetComponent<dressItem_box>().itemName);
                                if (findItem != -1)
                                {
                                    itemStages.items[findItem].itemCheck = true;
                                }

                                if (itemStages.items.Count == 3)
                                {
                                    if (itemStages.items[1].itemCheck && itemStages.items[2].itemCheck)
                                    {
                                        checkBox2.SetActive(true);
                                    }
                                    else
                                    {
                                        checkBox2.SetActive(false);
                                    }
                                }

                            }
                        }

                    }
                    else
                    {

                        if (itemStages != null)
                        {
                            if (itemStages.items.Count > 0)
                            {
                                Debug.Log(_itemCreate.GetComponent<dressItem_box>().name);
                                if (_itemCreate.GetComponent<dressItem_box>().name.Contains("กล้อง"))
                                {

                                    int findItem = itemStages.items.FindIndex(x => x.itemName == _itemCreate.GetComponent<dressItem_box>().name);
                                    if (findItem != -1)
                                    {
                                        itemStages.items[0].itemCheck = true;
                                        checkBox1.SetActive(true);
                                    }
                                    else
                                    {
                                        itemStages.items[0].itemCheck = false;
                                        checkBox1.SetActive(false);
                                    }
                                }
                                else
                                {
                                    int findItem = itemStages.items.FindIndex(x => x.itemName == _itemCreate.GetComponent<dressItem_box>().name);
                                    if (findItem != -1)
                                    {
                                        itemStages.items[1].itemCheck = true;
                                        checkBox2.SetActive(true);
                                    }
                                    else
                                    {
                                        itemStages.items[1].itemCheck = false;
                                        checkBox2.SetActive(false);
                                    }
                                }
                            }
                        }
                    }

                        if (_group.accType)
                    {
                        int findIndex = _group._AccItem.FindIndex(x => x.accName == _dressItem.accKey);
                        if (findIndex != -1)
                        {
                            _group._AccItem[findIndex].accItemHuman.SetActive(true);
                            if (_group._AccItem[findIndex].accName == "กล้อง" || _group._AccItem[findIndex].accName == "เรือดำน้ำ")
                            {
                                _group._AccItem[findIndex].accItemHuman.transform.GetChild(0).GetComponent<RawImage>().texture = _dressItem.dressItemHuman;
                                _group._AccItem[findIndex].accItemHuman.transform.GetChild(0).GetComponent<AspectRatioFitter>().aspectRatio = (float)_dressItem.dressItemHuman.width / _dressItem.dressItemHuman.height;

                            }
                            else
                            {
                                _group._AccItem[findIndex].accItemHuman.GetComponent<Image>().sprite = _dressItem._dressItemHuman;
                            }

                            if (costume_human._human != null)
                            {
                                int findIndexCoustume = costume_human._human._costume.FindIndex(z => z.typeItem == _group.typeName);
                                int findAccIndex = costume_human._human._costume[findIndexCoustume]._accItem.FindIndex(zz => zz.accName == _dressItem.accKey);
                                costume_human._human._costume[findIndexCoustume]._accItem[findAccIndex]._itemSprite = _dressItem._dressItemHuman; 
                            }
                        }
                    }
                    else
                    {
                        _group.itemHuman.SetActive(true);
                        _group.itemHuman.transform.GetChild(0).GetComponent<RawImage>().texture = _dressItem.dressItemHuman;
                        _group.itemHuman.transform.GetChild(0).GetComponent<AspectRatioFitter>().aspectRatio = (float)_dressItem.dressItemHuman.width / _dressItem.dressItemHuman.height;
                        if (costume_human._human != null)
                        {
                            int findIndexCoustume = costume_human._human._costume.FindIndex(z => z.typeItem == _group.typeName);
                         
                            costume_human._human._costume[findIndexCoustume]._itemSprite = _dressItem._dressItemHuman;
                        }

                    }

                    _itemStages = itemStages;

                });
            }
        }

    }

    public void jsonToItem(string json)
    {
        if (json != "0")
        {
            _item = JsonUtility.FromJson<item_shop_data>(json);

            if(_item != null)
            {
                foreach (_item_shop item in _item.data)
                {
                    Debug.Log(item.category_id);
                    int cat_id = item.category_id;

                    dressItem item_create = new dressItem();
                    item_create.dressName = item.item_name;
                    item_create.type_item = item.category_id.ToString();
                    Debug.Log(item.thumb_image_path);
                    //item_create.
                    network_setting.loadImage_RawImage(network_setting.hostname + "asset/item_img/" + item.thumb_image_path, (cb) =>
                    {
                        item_create.dressItemHuman = cb;
                        item_create.dressImage = cb;

                    });

                    if(cat_id == 5)
                    {
                        if (item_create.dressName.Contains("กล้อง"))
                        {
                            item_create.accKey = "กล้อง";
                        }

                        if (item_create.dressName.Contains("เรือดำน้ำ"))
                        {
                            item_create.accKey = "เรือดำน้ำ";
                        }

                    }
                    _groupItem[cat_id - 1]._dressItem.Add(item_create);
                }


                Invoke("dressMainScript", 5f);
               // dressMainScript();
            }
        }
    }

}
