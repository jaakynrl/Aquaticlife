using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class network_setting : MonoBehaviour
{
#if UNITY_EDITOR
    public static string hostname = "http://localhost:85/thesis/";
#else
     public static string hostname = "http://thesis.thesisict2020.site/thesis/";
#endif



    [System.Obsolete]
    public static void loadImage_RawImage(string imagePath,RawImage img)
    {
        ObservableWWW.GetAndGetBytes(imagePath).Subscribe(
            x =>
            {
                Texture2D itemImage = new Texture2D(0, 0);
                itemImage.LoadImage(x);
                img.texture = itemImage;
                img.GetComponent<AspectRatioFitter>().aspectRatio = (float)itemImage.width / itemImage.height;
            },
            err =>
            {
                img.texture = null;
            });
    }


    public static void loadImage_RawImage(string imagePath, System.Action<Texture> cb)
    {
        ObservableWWW.GetAndGetBytes(imagePath).Subscribe(
            x =>
            {
                Texture2D itemImage = new Texture2D(0, 0);
                itemImage.LoadImage(x);
               
                cb(itemImage);
            },
            err =>
            {
                cb(null);
            });
    }

}
