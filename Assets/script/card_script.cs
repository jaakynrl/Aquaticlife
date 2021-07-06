using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class card_script : MonoBehaviour
{
    public RawImage img;
    public AspectRatioFitter arf;
    public Texture texture_img;
    public GameObject popup;
    

    public void setAspect()
    {
        arf.aspectRatio = (float)img.texture.width / img.texture.height;
        texture_img = img.texture;
    }

    public void preview_card(GameObject card)
    {
        popup.SetActive(true);
        card.GetComponent<RawImage>().texture = texture_img;
        card.GetComponent<AspectRatioFitter>().aspectRatio = (float)img.texture.width / img.texture.height;
    }
}
