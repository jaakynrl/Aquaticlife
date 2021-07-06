using System.Collections;
using System.Collections.Generic;
using SimpleInputNamespace;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class miniGame2_controller : MonoBehaviour
{
	public GameObject boad;

    [SerializeField]
    bool moveL;
    [SerializeField]
    bool moveR;

    [SerializeField]
    float speed;

    [SerializeField]
    float position_move;

    [Header("ROPE SETTING")]
    public SteeringWheel _wheel;
    public GameObject rope;
    public float scaleRope;
    public float ropeRange;
    public float ropeScaleMax;
    public float ropeScaleMin;
    public float wheelMax;
    public float wheelMin;


    [Header("FISH CATCH SETTING")]
    public GameObject fishCatch;
    public GameObject point;

    public float Value;
    public mniGame2_fishCatch _fishCatch;
    [SerializeField]
    float timeDel;

    [Header("FISH CREATE")]
    [SerializeField]
    float positioCreateFish_max;
    [SerializeField]
    float positioCreateFish_min;
    [SerializeField]
    GameObject[] fishPrefabs;
    [SerializeField]
    GameObject fishClone;
    [SerializeField]
    float positioCreateFish_x;
    [SerializeField]
    float createFish_time;
    [SerializeField]
    float createFish_rate;
    [SerializeField]
    float scaleFishMin;
    [SerializeField]
    float scaleFishMax;

    [Header("BIG FISH CREATE")]
    [SerializeField]
    public GameObject[] bigFish;
    [SerializeField]
    float timeBigCreate_max;
    [SerializeField]
    float timeBigCreate_min;
    [SerializeField]
    float positioCreateBigFish_x;
    [SerializeField]
    float positioCreateBigFish_max;
    [SerializeField]
    float positioCreateBigFish_min;
    [SerializeField]
    int currentBigFish;


    [SerializeField]
    float timeCount;
    [SerializeField]
    float timeCreate;

    [Header("GAME SETTING")]
    [SerializeField]
    GameObject[] Lifes;
    [SerializeField]
    Color disableColor;
    [SerializeField]
    int currentLife;
    [SerializeField]
    TextMeshProUGUI time_text;
    [SerializeField]
    float timeGame_Count;
    [SerializeField]
    int scoreFish;
    [SerializeField]
    int scoreBigFish;
    [SerializeField]
    TextMeshProUGUI text_score;
    [SerializeField]
    int currentScore;
    [SerializeField]
    bool GameEnd = true;
    [SerializeField]
    GameObject lifePrefab;
    [SerializeField]
    GameObject wrapLife;

    [Header("GAME UI SETTING")]
    [SerializeField]
     GameObject popupOpacity;
    [SerializeField]
    GameObject winUI;
    [SerializeField]
    GameObject loseUI;
    [SerializeField]
    int rateDiamond_loseMax;
    [SerializeField]
    int rateDiamond_loseMin;
    [SerializeField]
    int rateCoin_loseMax;
    [SerializeField]
    int rateCoin_loseMin;
    [SerializeField]
    TextMeshProUGUI TextDiamond_lose;
    [SerializeField]
    TextMeshProUGUI TextCoin_lose;

    [SerializeField]
    int rateDiamond_winMax;
    [SerializeField]
    int rateDiamond_winMin;
    [SerializeField]
    int rateCoin_winMax;
    [SerializeField]
    int rateCoin_winMin;
    [SerializeField]
    TextMeshProUGUI TextDiamond_win;
    [SerializeField]
    TextMeshProUGUI TextCoin_win;

    [SerializeField]
    GameObject hintObj;


    private void Start()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        if(Value > _wheel.Value)
        {
            Debug.Log("Up");
            Value = _wheel.Value;
            _fishCatch.isUp = true;
        }
        else if(Value < _wheel.Value)
        {
            Debug.Log("Down");
            Value = _wheel.Value;
            _fishCatch.isUp = false;
        }
        else
        {
            _fishCatch.isUp = false;
        }


        fishCatch.transform.position = point.transform.position;
        if (moveR)
        {
            if (boad.transform.position.x < 8.5f)
            {
                boad.transform.position = Vector2.Lerp(
                    boad.transform.position,
                    new Vector2(
                        boad.transform.position.x + position_move,
                         boad.transform.position.y
                    ), Time.deltaTime * speed);
            }
        }

        if (moveL)
        {
            if (boad.transform.position.x > -2.5f)
            {
                boad.transform.position = Vector2.Lerp(
                    boad.transform.position,
                    new Vector2(
                        boad.transform.position.x - position_move,
                         boad.transform.position.y
                    ), Time.deltaTime * speed);
            }
        }

        rope.transform.localScale = new Vector2(1, Mathf.Clamp( ropeRange + ((_wheel.Value / 360) * scaleRope), ropeScaleMin, ropeScaleMax));

        if(_wheel.Value <= _wheel.minWheel)
        {
            StartCoroutine(_fishCatch.clearFish(timeDel));
        }
    }

    public void StartGame()
    {
        loseUI.SetActive(false);
        winUI.SetActive(false);
        GameEnd = false;
        Time.timeScale = 1;
        _fishCatch._controller = this;
        ropeRange = rope.transform.localScale.y;
        _wheel.maxWheel = wheelMax / scaleRope;
        _wheel.minWheel = wheelMin / scaleRope;
        Value = _wheel.Value;

        timeCreate = Random.Range(timeBigCreate_min, timeBigCreate_max);
        InvokeRepeating("create_fnish", createFish_time, createFish_rate);
    }

    public void PauseGame()
    {
        hintObj.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        hintObj.SetActive(false);
        Time.timeScale = 1;
    }


    private void FixedUpdate()
    {
       
        if (!GameEnd) {
            timeGame_Count -= Time.fixedDeltaTime;
            time_text.text = FormatSeconds(timeGame_Count);

            if(timeGame_Count < 0)
            {
                GameEnd = true;
                if (currentLife >= 0)
                {
                    gameWin();
                }
                else
                {
                    gameLose();
                }
                time_text.text = FormatSeconds(0);
            }

            timeCount += Time.fixedDeltaTime;
            if (timeCount >= timeCreate)
            {
                create_bigFish();
                timeCount = 0;
            }

        }

   
    }
    public void create_fnish()
    {
        int randomTypeFish = Random.Range(0, fishPrefabs.Length);
        fishClone = fishPrefabs[randomTypeFish];
        GameObject clone_fish = Instantiate(fishClone);
        float randomY = Random.Range(positioCreateFish_min, positioCreateFish_max);
        float randomScale = Random.Range(scaleFishMin, scaleFishMax);
        int randomNumber = Random.Range(0, 100);
        if(randomNumber % 2 == 0)
        {
            clone_fish.transform.position = new Vector2(positioCreateFish_x, randomY);
            clone_fish.transform.localScale = new Vector2(randomScale, randomScale);
        }
        else
        {
            clone_fish.transform.position = new Vector2(-positioCreateFish_x, randomY);
            clone_fish.transform.localScale = new Vector2(-randomScale, randomScale);
            clone_fish.GetComponent<miniGame2_fishMove>().isLeft = true;    
        }
        
    }


    public void create_bigFish()
    {

        GameObject clone_BigFish = Instantiate(bigFish[currentBigFish]);
        clone_BigFish.GetComponent<miniGame2_fishMove>()._controller = this;
        float randomY = Random.Range(positioCreateBigFish_min, positioCreateBigFish_max);
        int randomNumber = Random.Range(0, 100);
        if (randomNumber % 2 == 0)
        {
            clone_BigFish.transform.position = new Vector2(positioCreateBigFish_x, randomY);
            clone_BigFish.transform.localScale = new Vector2(-clone_BigFish.transform.localScale.x, clone_BigFish.transform.localScale.y);
        }
        else
        {
            clone_BigFish.transform.position = new Vector2(-positioCreateBigFish_x, randomY);
           
            clone_BigFish.GetComponent<miniGame2_fishMove>().isLeft = true;
        }


        currentBigFish++;
        if(currentBigFish == bigFish.Length)
        {
            currentBigFish = 0;
        }

        timeCreate = Random.Range(timeBigCreate_min, timeBigCreate_max);
    }

    public void moveLeft()
    {
        moveL = true;
        
    }

    public void moveRight()
	{
        moveR = true;
        

    }

    public void stopMoveLeft()
    {
        moveL = false;
       
    }

    public void stopMoveRight()
    {
        moveR = false;

    }

    public void lifeDisable(Sprite fishImage)
    {

        currentScore -= scoreBigFish;
        if(currentScore < 0)
        {
            currentScore = 0;
        }
        text_score.text = "SCORE: "+currentScore.ToString();
        GameObject iconBlackFish = Instantiate(lifePrefab, wrapLife.transform, false);
        iconBlackFish.GetComponent<miniGame2_icon>()._iconFish.sprite = fishImage;


        currentLife--;
        if(currentLife < 0)
        {
            GameEnd = true;
            gameLose();
        }
        
    }


    public void resetCatch()
    {
        _fishCatch.clearSmallFish();
        _fishCatch.catchBigFish = false;
    }

    public void addScore()
    {
        currentScore += scoreFish;
        text_score.text = "SCORE: " + currentScore.ToString();
    }

    public void gameLose()
    {
        int diamondRandom = Random.Range(rateDiamond_loseMin, rateDiamond_loseMax);
        int coinRandom = Random.Range(rateCoin_loseMin, rateCoin_loseMax);
        TextCoin_lose.text = coinRandom.ToString();
        TextDiamond_lose.text = diamondRandom.ToString();
        popupOpacity.SetActive(true);
        loseUI.SetActive(true);

        var dc = DataCore.instance;
        ServiceManager.instance.AddCurrency(coinRandom, 0, () => {
            dc.account.coin += coinRandom;
        });
        ServiceManager.instance.AddCurrency(diamondRandom, 1, () => {
            dc.account.diamond += diamondRandom;
        });
    }

    public void gameWin()
    {
        int diamondRandom = Random.Range(rateDiamond_winMin, rateDiamond_winMax);
        int coinRandom = Random.Range(rateCoin_winMin, rateCoin_winMax);
        TextCoin_win.text = coinRandom.ToString();
        TextDiamond_win.text = diamondRandom.ToString();
        popupOpacity.SetActive(true);
        winUI.SetActive(true);

        var dc = DataCore.instance;
        ServiceManager.instance.AddCurrency(coinRandom, 0, () => {
            dc.account.coin += coinRandom;
        });
        ServiceManager.instance.AddCurrency(diamondRandom, 1, () => {
            dc.account.diamond += diamondRandom;
        });
    }

    string FormatSeconds(float elapsed)
    {
        int d = (int)(elapsed * 100.0f);
        int minutes = d / (60 * 100);
        int seconds = (d % (60 * 100)) / 100;
        return System.String.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
