using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class garbage
{
    public string type_garbage;
    public List<Sprite> garbage_imge;
}
public class minigame1_mainScript : MonoBehaviour
{
    public List<garbage> _garbages;

    [SerializeField]
    int minRandomGarbage;
    [SerializeField]
    int maxRandomGarbage;
    [SerializeField]
    private int randomCreate;

    [SerializeField]
    float positionX_Max;
    [SerializeField]
    float positionX_Min;
    [SerializeField]
    float positionY_Max;
    [SerializeField]
    float positionY_Min;

    [SerializeField]
    GameObject prefabGarbage;

    [Header("WAVE")]
    [SerializeField]
    GameObject wave;
    [SerializeField]
    GameObject subwave;
    [SerializeField]
    SpriteRenderer waveSprite;

    [Header("SEA")]
    public Color[] colorStep;
    [SerializeField]
    int garbageSea_black;
    [SerializeField]
    public int garbageCount;
    [SerializeField]
    int currentColor;
    [SerializeField]
    float speedColor;

    [Header("GAME SETTING")]
    public float gameTime;
    public bool gameStart;
    public Image barGarbage;
    public int maxKeepGarbage;
    public TextMeshProUGUI text_time;
    [SerializeField]
    int keepGarbageCount;
    public TextMeshProUGUI textDiamond;
    public int diamond_value;
    public TextMeshProUGUI textCoin;
    public int coin_value;
    public GameObject bg_opaDiamond_coin;
    public GameObject bg_lose;
    public GameObject bg_win;
    [SerializeField]
    int diamond_multiply;
    [SerializeField]
    int coin_multiply;
    [SerializeField]
    int garbageForCalculate;
    public AudioSource soundCorrect;
    public AudioSource soundWrong;
    // Start is called before the first frame update
    [SerializeField]


    int count_animeItem;

    public void GameStart_btn()
    {
        create_garbageSystem();
        gameStart = true;
    }
    public void create_garbageSystem()
    {
        randomNumberCreate();
        create_garbage();
    }
    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            gameTime -= Time.deltaTime;
            text_time.text = FormatSeconds(gameTime);
            if(gameTime <= 0)
            {
                game_lose();
                gameStart = false;
            }
        }

        waveSprite.color = Color.Lerp(waveSprite.color, colorStep[currentColor], Time.deltaTime * speedColor);
        waveColorSet();

    }

    private void Start()
    {
        keepGarbageCount = maxKeepGarbage;
        text_time.text = FormatSeconds(gameTime);
        barGarbageCalculate();
    }

    public void keepGarbageDiscount()
    {
        keepGarbageCount -= 1;
        garbageForCalculate += 1;
        barGarbageCalculate();
    }

    void barGarbageCalculate()
    {
        if(keepGarbageCount <= 0)
        {
            barGarbage.fillAmount = 0;
            gameStart = false;
            game_win();
        }
        else
        {
            barGarbage.fillAmount = (float)keepGarbageCount / maxKeepGarbage;
        }
       
    }
    void waveColorSet()
    {
        Color colorMain = new Color(colorStep[currentColor].r,
            colorStep[currentColor].g,
            colorStep[currentColor].b,
            wave.GetComponent<SpriteRenderer>().color.a);
        wave.GetComponent<SpriteRenderer>().color = colorMain;

        Color colorSubMain = new Color(colorStep[currentColor].r,
            colorStep[currentColor].g,
            colorStep[currentColor].b,
            subwave.GetComponent<SpriteRenderer>().color.a);
        subwave.GetComponent<SpriteRenderer>().color = colorSubMain;

    }

    void randomNumberCreate()
    {
        randomCreate = Random.Range(minRandomGarbage, maxRandomGarbage);

    }

    void create_garbage()
    {

        for (int i = 0; i < randomCreate; i++)
        {
            float _positionX = Random.Range(positionX_Min, positionX_Max);
            float _positionY = Random.Range(positionY_Min, positionY_Max);
            int randomType = Random.Range(0, _garbages.Count);
            int random_img = Random.Range(0, _garbages[randomType].garbage_imge.Count);

            GameObject garbage_clone = Instantiate(prefabGarbage);
            /*if(count_animeItem > 0)
            {
                count_animeItem--;
            }
            else
            {
                garbage_clone.GetComponent<Animator>().enabled = false;
            }*/
            garbage_clone.transform.position = new Vector2(_positionX, _positionY);
            garbage_clone.GetComponent<SpriteRenderer>().sprite = _garbages[randomType].garbage_imge[random_img];
            garbage_clone.AddComponent<BoxCollider2D>();
            garbage_clone.GetComponent<BoxCollider2D>().isTrigger = true;
            garbage_clone.transform.localScale = Vector2.zero;
            garbage_clone.GetComponent<minigame1_garbage>().wave_position = wave;
            garbage_clone.GetComponent<minigame1_garbage>().garbageType = _garbages[randomType].type_garbage;
            garbage_clone.GetComponent<minigame1_garbage>()._mainScript = this;

        }
    }


    public void setCountGarbage()
    {
        garbageCount += 1;
        if(garbageCount > 0)
        {
            currentColor = Mathf.RoundToInt(garbageCount / garbageSea_black);
            if(currentColor == colorStep.Length)
            {
                currentColor = colorStep.Length - 1;
            }
        }
        
    }

    string FormatSeconds(float elapsed)
    {
        int d = (int)(elapsed * 100.0f);
        int minutes = d / (60 * 100);
        int seconds = (d % (60 * 100)) / 100;
        return System.String.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void game_lose()
    {
        bg_opaDiamond_coin.SetActive(true);
        bg_lose.SetActive(true);
        diamond_value = garbageForCalculate * diamond_multiply;
        coin_value = garbageForCalculate * coin_multiply;
        textDiamond.text = System.String.Format("{0:00}", diamond_value);
        textCoin.text = System.String.Format("{0:00}", coin_value);

        var dc = DataCore.instance;
        ServiceManager.instance.AddCurrency(coin_value, 0, () => {
            dc.account.coin += coin_value;
        });
        ServiceManager.instance.AddCurrency(diamond_value, 1, () => {
            dc.account.diamond += diamond_value;
        });
    }

    public void game_win()
    {
        bg_opaDiamond_coin.SetActive(true);
        bg_win.SetActive(true);
        diamond_value = garbageForCalculate * diamond_multiply;
        coin_value = garbageForCalculate * coin_multiply;
        textDiamond.text = System.String.Format("{0:00}", diamond_value);
        textCoin.text = System.String.Format("{0:00}", coin_value);
        
            var dc = DataCore.instance;
            ServiceManager.instance.AddCurrency(coin_value, 0, () => {
                dc.account.coin += coin_value;
            });
            ServiceManager.instance.AddCurrency(diamond_value, 1, () => {
                dc.account.diamond += diamond_value;
            });
    }

    public void show_hint_stopGame(GameObject itemShow)
    {
        itemShow.SetActive(true);
        Time.timeScale = 0;
    }

    public void hide_hint_playGame(GameObject itemShow)
    {
        Time.timeScale = 1;
        itemShow.SetActive(false);
        
    }

    public void playCorrectSound()
    {
        soundCorrect.Play();
    }

    public void playWrongSound()
    {
        soundWrong.Play();
    }

}
