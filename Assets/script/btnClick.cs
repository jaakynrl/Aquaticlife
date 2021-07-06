using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnClick : MonoBehaviour
{
    public AudioSource myFX;
    public AudioClip clickFX;
    public Button btnBox;
    public scene_manage _scene;
    public bool nextSceneBox;

    private void Start()
    {
        if (nextSceneBox)
        {
            _scene = GetComponent<scene_manage>();
            btnBox = GetComponent<Button>();
            btnBox.onClick.RemoveAllListeners();
            btnBox.onClick.AddListener(() =>
            {
                _scene.nextScene();
                Clicksound();
            });
            myFX = GetComponent<AudioSource>();
        }
    }
    public void Clicksound()
    {
        myFX.PlayOneShot(clickFX);
    }
}
