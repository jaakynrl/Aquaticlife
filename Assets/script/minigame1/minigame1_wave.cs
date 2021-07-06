using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minigame1_wave : MonoBehaviour
{
    public float speed;
    public minigame1_mainScript _main;
    [SerializeField]
    float maxPostionWave;
    [SerializeField]
    float minPositionWave;
    [SerializeField]
    public bool Is_waveUp;
    [SerializeField]
    float limitPosition;
    [SerializeField]
    float alphaWave;
    [SerializeField]
    GameObject subWave;
    [SerializeField]
    float alphaSubwave;
    [SerializeField]
    Color subWaveColor;
    [SerializeField]
    Color waveColor;
    [SerializeField]
    float startAlpha;

    [SerializeField]
    float speed_fadeAplha;
    [SerializeField]
    float limitColor;
    [SerializeField]
    bool createGarbage;
    public bool startWaveUP;

    private void Start()
    {
      
        waveColor = GetComponent<SpriteRenderer>().color;
        alphaWave = waveColor.a;

        subWaveColor = subWave.GetComponent<SpriteRenderer>().color;
        alphaSubwave = subWaveColor.a;
        startAlpha = alphaSubwave;
    }



    private void FixedUpdate()
    {
        if (_main.gameStart)
        {
            if (!Is_waveUp)
            {
                waveDown();
            }
            else
            {
                waveUp();
            }
        }
       
    }


    void waveUp()
    {
        startWaveUP = false; 
        transform.position = new Vector2(
           transform.position.x,
           Mathf.Lerp(transform.position.y, maxPostionWave, Time.fixedDeltaTime * speed)
           );
        if((maxPostionWave-5) - transform.position.y < limitPosition)
        {
            if (!createGarbage)
            {
               
                createGarbage = true;
                StartCoroutine(delayCreateBin());
            }
            fadeUpColor();
        }

        if (maxPostionWave - transform.position.y  < limitPosition)
        {
            Is_waveUp = false;

        }
    }

    void waveDown()
    {
        transform.position = new Vector2(
           transform.position.x,
           Mathf.Lerp(transform.position.y, minPositionWave, Time.fixedDeltaTime * speed)
           );
        if(transform.position.y - (minPositionWave+2) < limitPosition)
        {
            fadeColor();
        }

        if(transform.position.y - minPositionWave < limitPosition)
        {
            startWaveUP = true;
            createGarbage = false;
            Is_waveUp = true;
        }
    }

    void fadeColor()
    {
        waveColor.a = Mathf.Lerp(waveColor.a, limitColor, Time.fixedDeltaTime * speed_fadeAplha);
        subWaveColor.a =  Mathf.Lerp(subWaveColor.a, startAlpha * waveColor.a, Time.fixedDeltaTime * speed_fadeAplha);
        GetComponent<SpriteRenderer>().color = waveColor;
        subWave.GetComponent<SpriteRenderer>().color = subWaveColor;
    }

    void fadeUpColor()
    {
        waveColor.a = Mathf.Lerp(waveColor.a, 1, Time.fixedDeltaTime * speed_fadeAplha);
        subWaveColor.a = Mathf.Lerp(subWaveColor.a, startAlpha * waveColor.a, Time.fixedDeltaTime * speed_fadeAplha);
        GetComponent<SpriteRenderer>().color = waveColor;
        subWave.GetComponent<SpriteRenderer>().color = subWaveColor;
    }

    IEnumerator delayCreateBin()
    {
        yield return new WaitForSeconds(2f);
        _main.create_garbageSystem();
    }
}
