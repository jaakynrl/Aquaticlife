using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public AudioSource speaker;
    public AudioSource speakerBGM;

    public AudioClip buttonClickClip;
    public AudioClip winClip;

    public List<AudioClip> listAudioClip = new List<AudioClip>();
    public void SetBGM(int index)
    {
        speakerBGM.clip = listAudioClip[index];
        speakerBGM.Play();
    }

    public void Init()
    {
        var btns = FindObjectsOfType<Button>();
        foreach (var btn in btns)
        {
            btn.onClick.AddListener(() => { SetAudio(buttonClickClip); });
        }
    }

    public void PlayClick()
    {
        speaker.PlayOneShot(buttonClickClip);
    }

    public void SetAudio(AudioClip clip)
    {
        speaker.PlayOneShot(clip);
    }
}
