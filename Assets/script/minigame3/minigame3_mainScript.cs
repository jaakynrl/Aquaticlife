using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class minigame3_mainScript : MonoBehaviour
{
    public List<GameObject> itemInScene;

    [Header("Garbage")]

    public Sprite[] garbages;
    public GameObject garbagePrefab;
    public float garbage_positionYCreate;
    public float garbage_maxPositionX;
    public float garbage_minPositionX;
    [SerializeField]
    float rateCreate_garbage;

    [Header("Fish")]
    public GameObject[] _fish;
    [SerializeField]
    float fish_positionX_MAX;
    [SerializeField]
    float fish_positionX_MIN;
    [SerializeField]
    float fish_positionY_MAX;
    [SerializeField]
    float fish_positionY_MIN;
    [SerializeField]
    float rateCreate_fish;
    [SerializeField]
    float positionCreate_1;
    [SerializeField]
    float positionCreate_2;


    [Header("BoneFood")]
    public Sprite[] bones;
    public GameObject bonePrefab;
    public float bone_positionYCreate;
    public float bone_maxPositionX;
    public float bone_minPositionX;
    [SerializeField]
    float rateCreate_bone;

    [Header("Rope")]
    public GameObject Rope;

    [Header("Game setting")]
    [SerializeField]
    float timeGame;
    [SerializeField]
    TextMeshProUGUI textTime;
    [SerializeField]
    int ScorePlus;
    [SerializeField]
    int ScoreMinus;
    [SerializeField]
    TextMeshProUGUI textScore;
    [SerializeField]
    int currentScore;
    [SerializeField]
    bool gameEnd;
    [SerializeField]
    GameObject popupWin;
    [SerializeField]
    GameObject popupLose;
    [SerializeField]
    TextMeshProUGUI diamondText;
    [SerializeField]
    int diamond_item;
    [SerializeField]
    TextMeshProUGUI coinText;
    [SerializeField]
    int coin_item;
    [SerializeField]
    GameObject opacityBg;
    [SerializeField]
    int scoreLose;
    [SerializeField]
    int score_toCoin;
    [SerializeField]
    int score_toDiamond;
    [SerializeField]
    GameObject wrapBoxScore;

    [SerializeField]
    Transform startPosition;

    [SerializeField]
    public int garbageInGround;
    [SerializeField]
    int maxGarbage_ground;

    [Header("SOUND GAME")]
    
    [SerializeField]
    AudioSource wrongSound;
    [SerializeField]
    AudioSource correctSound;

    [SerializeField]
    int wrongItem;

    [SerializeField]
    GameObject hintObj;

    private void Start()
    {
        Time.timeScale = 0;
    }
    public void StartGame()
    {
        gameEnd = false;
        Time.timeScale = 1;
        InvokeRepeating("create_garbage", 1, rateCreate_garbage);
        InvokeRepeating("create_bone", 2, rateCreate_bone);
        InvokeRepeating("create_fish", 5, rateCreate_fish);
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

    public void playCorrectSound()
    {
       
        correctSound.Play();
    }

    public void playWrongSound()
    {
      
        wrongSound.Play();
    }

    void create_bone()
    {
        int randomImage_bone = Random.Range(0, bones.Length);
        float randomX = Random.Range(bone_minPositionX, bone_maxPositionX);
        GameObject cloneBone = Instantiate(bonePrefab);
        cloneBone.transform.position = new Vector3(randomX, bone_positionYCreate, 0);
        cloneBone.GetComponent<SpriteRenderer>().sprite = bones[randomImage_bone];
        cloneBone.GetComponent<minigame3_garbageMove>()._main = this;
        int missingIndex = itemInScene.FindIndex(x => x == null);
        if (missingIndex != -1)
        {
            itemInScene[missingIndex] = cloneBone;
        }
        else
        {
            itemInScene.Add(cloneBone);
        }
        
    }

    void create_garbage()
    {
        int randomImage_garbage = Random.Range(0, garbages.Length);
        float randomX = Random.Range(garbage_minPositionX, garbage_maxPositionX);
        GameObject cloneGarbage = Instantiate(garbagePrefab);
        cloneGarbage.transform.position = new Vector3(randomX, garbage_positionYCreate, 0);
        cloneGarbage.GetComponent<SpriteRenderer>().sprite = garbages[randomImage_garbage];
        cloneGarbage.GetComponent<minigame3_garbageMove>()._main = this;
        int missingIndex = itemInScene.FindIndex(x => x == null);
        if (missingIndex != -1)
        {
            itemInScene[missingIndex] = cloneGarbage;
        }
        else
        {
            itemInScene.Add(cloneGarbage);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!gameEnd)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 camera_mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (camera_mouse.y <= 3)
                {
                    GameObject ropeClone = Instantiate(Rope);
                    ropeClone.transform.position = Vector3.zero;
                    ropeClone.GetComponent<ropeEdit>().startPosition = startPosition;
                    ropeClone.GetComponent<ropeEdit>().create_rope(camera_mouse);
                    ropeClone.GetComponent<RopeSlefMovementControl>().setRope();
                }
                //ropeClone.GetComponent<RopeSlefMovementControl>().mouse_position = camera_mouse;
                //ropeClone.GetComponent<RopeSlefMovementControl>().start_position = ropeClone.GetComponent<RopeSlefMovementControl>().mouse_position.y;

            }
            timeGame_manage();

            if(garbageInGround >= maxGarbage_ground)
            {
                gameEnd = true;
                Invoke("popupEndGame", 1f);
            }
        }
    }

    void create_fish()
    {
        int randomFish = Random.Range(0, 100);
        int numberFish = 0;
        float randomPoistion_x = Random.Range(fish_positionX_MIN, fish_positionX_MAX);
        float randomPoistion_y = Random.Range(fish_positionY_MIN, fish_positionY_MAX);
        if (randomFish% 3 == 0)
        {
            numberFish = 0;
        }
        else if(randomFish % 3 == 1)
        {
            numberFish = 1;
        }
        else
        {
            numberFish = 2;
        }

       
        GameObject clonefish = Instantiate(_fish[numberFish]);
        int randomPositioCreate = Random.Range(0, 5);
        if (randomPositioCreate % 2 == 0)
        {
            clonefish.transform.position = new Vector2(positionCreate_1, randomPoistion_y);

        }
        else
        {
            clonefish.transform.position = new Vector2(positionCreate_2, randomPoistion_y);
        }
        

    }

    public void addScore()
    {
        if (!gameEnd)
        {
            playCorrectSound();
            currentScore += ScorePlus;
            textScore.text = "SCORE : " + System.String.Format("{0:00}", currentScore);
        }

    }

    public void minusScore()
    {
        if (!gameEnd)
        {
            playWrongSound();
            currentScore -= ScoreMinus;
            if (currentScore < 0)
            {
                currentScore = 0;
            }

            textScore.text = "SCORE : " + System.String.Format("{0:00}", currentScore);
        }
    }


     void timeGame_manage()
    {

        if (timeGame > 0)
        {
            timeGame -= Time.deltaTime;
            textTime.text = "TIME : " + FormatSeconds(timeGame);
        }
        else
        {
            gameEnd = true;
            textTime.text = "TIME : " + FormatSeconds(0);
            Invoke("popupEndGame", 1f);
        }
    }


    void popupEndGame()
    {
        opacityBg.SetActive(true);
        if (currentScore <= scoreLose)
        {
            popupLose.SetActive(true);
        }
        else {
            popupWin.SetActive(true);
        }

        wrapBoxScore.SetActive(true);
        if(currentScore > 0)
        {
            int diamondRandom = Mathf.RoundToInt(currentScore / score_toDiamond);
            int coinRandom = Mathf.RoundToInt(currentScore / score_toCoin);
            diamondText.text = System.String.Format("{0:00}", diamondRandom);
            coinText.text = System.String.Format("{0:00}", coinRandom);

            var dc = DataCore.instance;
            ServiceManager.instance.AddCurrency(coinRandom, 0, () => {
                dc.account.coin += coinRandom;
            });
            ServiceManager.instance.AddCurrency(diamondRandom, 1, () => {
                dc.account.diamond += diamondRandom;
            });
        }
        else
        {
            diamondText.text = System.String.Format("{0:00}", 0);
            coinText.text = System.String.Format("{0:00}", 0);

        }

    }

    string FormatSeconds(float elapsed)
    {
        int d = (int)(elapsed * 100.0f);
        int minutes = d / (60 * 100);
        int seconds = (d % (60 * 100)) / 100;
        return System.String.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void set_wrongItem()
    {
        wrongItem++;
    }
}
