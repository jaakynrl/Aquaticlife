using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minigame3_fish : MonoBehaviour
{
    public minigame3_mainScript _mainScript;
    public GameObject targetItem;

    [SerializeField]
    float timeEat;
    [SerializeField]
    float[] timeEatRandom;
    [SerializeField]
    float speed;
    [SerializeField]
    Vector3 resultVector;
    [SerializeField]
    bool setWay;

    [SerializeField]
    float scaleX;
    [SerializeField]
    GameObject fishImg;

    [SerializeField]
    float speedMove;
    [SerializeField]
    float randomUpDown;
    [SerializeField]
    bool isUp;
    [SerializeField]
    string eatName;
    [SerializeField]
    bool isDead;
    [SerializeField]
    float speedFade;
    [SerializeField]
    float time_toDead;
    [SerializeField]
    float speedLerp;

    public bool hit;
    // Start is called before the first frame update
    void Start()
    {
        randomTime_toEat();
           scaleX = fishImg.transform.localScale.x;
        _mainScript = GameObject.Find("SCRIPT").GetComponent<minigame3_mainScript>();
    }

    void randomTime_toEat()
    {
        timeEat = Random.Range(timeEatRandom[0], timeEatRandom[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if(_mainScript != null)
        {
            if(_mainScript.itemInScene.Count > 0)
            {
                if (!isDead) {
                    if (timeEat <= 0)
                    {
                        setTargetItem();
                    }
                    else
                    {
                        moveFish();
                        timeEat -= Time.deltaTime;
                    }
                }
                else
                {
                    if (hit)
                    {
                        fishDead();
                    }
                    else
                    {
                        time_toDead -= Time.deltaTime;
                        if (time_toDead <= 0)
                        {
                            fishDead();
                        }
                    }
                }

            }
            else
            {

            }
        }
    }


    void moveFish()
    {
        if (!hit)
        {
            randomUpDown -= Time.deltaTime;
            if (randomUpDown <= 0)
            {
                isUp = !isUp;
                randomUpDown = Random.Range(.5f, 1);
            }

            if (isUp)
                transform.Translate(Vector2.down * speedMove * Time.deltaTime);
            else
                transform.Translate(-Vector2.down * speedMove * Time.deltaTime);
        }
        
    }

    void move_toTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetItem.transform.position, Time.deltaTime * speed);
        if (!setWay)
        {
            resultVector = transform.InverseTransformPoint(targetItem.transform.position);
            
                if (resultVector.x > 0)
                {
                    fishImg.transform.localScale = new Vector2(
                        -scaleX,
                        1
                        );
                }
                else
                {
                    fishImg.transform.localScale = new Vector2(
                        scaleX,
                        1
                        );
                }
            setWay = true;
        }
    }

    void setTargetItem()
    {
        if(targetItem == null)
        {
            moveFish();
            setWay = false;
            targetItem = randomItem();
        }
        else
        {
            move_toTarget();
            if (targetItem.GetComponent<minigame3_garbageMove>().is_ground)
            {
                targetItem = null;
            }
            
        }
    }

    GameObject randomItem()
    {
        GameObject itemRandom;
        List<GameObject> items = _mainScript.itemInScene.FindAll(x => x != null
        && x.GetComponent<minigame3_garbageMove>().checkItem());
        int randomNumber = Random.Range(0, items.Count);
        try
        {
            itemRandom = items[randomNumber];
            itemRandom.GetComponent<minigame3_garbageMove>().markTarget = true;
            itemRandom.name = eatName;
            return itemRandom;
        }
        catch
        {
            return null;
        }
   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead)
        {
            if (collision.tag == "garbage" && collision.name == eatName)
            {
                if (!collision.GetComponent<minigame3_garbageMove>().hit)
                {
                    hitObject(collision);
                    isDead = true;
                    
                }


            }

            if (collision.tag == "bone" && collision.name == eatName)
            {
                if (!collision.GetComponent<minigame3_garbageMove>().hit)
                {
                    hitObject(collision);
                    randomTime_toEat();
                }
            }
        }
    }



    void fishDead()
    {
        if (hit)
        {
            fishImg.transform.localScale = Vector2.Lerp(fishImg.transform.localScale, Vector2.zero, Time.deltaTime * speedLerp);
            if (fishImg.transform.localScale.x < 0.1f)
            {
                Destroy(this.gameObject);
            }

        }
        else
        {


            transform.Translate(-Vector2.down * 0.1f * Time.deltaTime);
            fishImg.transform.localScale = new Vector2(
                            scaleX,
                            -1
                            );

            fishImg.GetComponent<SpriteRenderer>().color = Color.Lerp(
                fishImg.GetComponent<SpriteRenderer>().color,
                Color.clear,
                Time.deltaTime * speedFade);

            if (fishImg.GetComponent<SpriteRenderer>().color.a < 0.1f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void hitObject(Collider2D collision)
    {
        collision.GetComponent<BoxCollider2D>().isTrigger = true;
        collision.GetComponent<Rigidbody2D>().Sleep();
        collision.GetComponent<minigame3_garbageMove>().hit = true;
    }

    public void fishHit()
    {
        hit = true;
           isDead = true;
    }
}
