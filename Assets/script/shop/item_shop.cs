using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class item_shop : MonoBehaviour
{
    public int item_id;
    public int type_item;
   
    public string item_name;
    public float price;
    public string typePrice
    {
        get
        {
            return TypePrice;
        }
        set
        {
            TypePrice = value;
            if (value == "c")
            {
                img_type.sprite = coinImg;
            }else if(value == "d")
            {
                img_type.sprite = diamondImg;

            }
        }
    }

    [SerializeField]
    private string TypePrice;
    public GameObject isBuy;
    public Sprite diamondImg;
    public Sprite coinImg;
    public Image img_type;

    private Texture2D TextureImage;
    public Texture2D textureImage
    {
        get
        {
            return TextureImage;
        }

        set
        {
            TextureImage = value;

        }
    }
    public RawImage image;
    public TextMeshProUGUI text_price;
}
