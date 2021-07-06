using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class card_collect_user : MonoBehaviour
{
    public RawImage img;
    public AspectRatioFitter arf;
    public Texture texture_img;
    public Texture imgPopup;
    public string url_popup;
    public void setAspect()
    {
        arf.aspectRatio = (float)img.texture.width / img.texture.height;
        texture_img = img.texture;
    }

}
