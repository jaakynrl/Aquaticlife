using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minigame3_chain : MonoBehaviour
{

    public float speedFade;
    public Color colorFinish;
    public Color colorStart;
    public bool downChain;
    public float speedDown;
    [SerializeField]
    public bool triggerItem;
    [SerializeField]
    public bool hitGarbage = false;
    [SerializeField]
    public bool fadeStart;
    [SerializeField]
    minigame3_mainScript _main;
    [SerializeField]
    float timeFade = 1f;
    private void Awake()    
    {
       // GetComponent<SpriteRenderer>().color = colorStart;
        _main = GameObject.Find("SCRIPT").GetComponent<minigame3_mainScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
            timeFade -= Time.deltaTime;
            if (timeFade <= 0)
            {
                GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, colorStart, Time.deltaTime * speedFade);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, colorFinish, Time.deltaTime * speedFade);
            }
            
            
       


        //if (!fadeStart)
        //{  
        //        GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, colorFinish, Time.deltaTime * speedFade);
        //}
        //else
        //{
        //    GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, colorStart, Time.deltaTime * speedFade);
        //}

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerItem && !hitGarbage)
        {
            if(collision.tag == "garbage")
            {
                
                if (collision.transform.position.y <= 3)
                {
                    if (!collision.GetComponent<minigame3_garbageMove>().hit && !collision.GetComponent<minigame3_garbageMove>().is_ground)
                    {
                        hitObject(collision);
                        if (_main != null)
                        {
                            _main.addScore();
                        }
                    }
                }


            }
            else if(collision.tag == "bone")
            {
                
                if (collision.transform.position.y <= 3)
                {
                    if (!collision.GetComponent<minigame3_garbageMove>().hit && !collision.GetComponent<minigame3_garbageMove>().is_ground)
                    {
                        hitObject(collision);
                        if (_main != null)
                        {
                            _main.minusScore();
                            _main.set_wrongItem();
                        }
                    }
                }
            }else if(collision.tag == "fish_g3")
            {
                
                if (!collision.GetComponentInParent<minigame3_fish>().hit)
                {
                    hitFish(collision);
                    if (_main != null)
                    {
                        _main.minusScore();
                    }
                }


            }
            Debug.Log(collision.tag);
        }
    }

    void hitObject(Collider2D collision) 
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        hitGarbage = true;
        collision.GetComponent<BoxCollider2D>().isTrigger = true;
        collision.GetComponent<Rigidbody2D>().Sleep();
        collision.GetComponent<minigame3_garbageMove>().hit = true;
        //delayFade();
    }

    void hitFish(Collider2D collision)
    {
        collision.GetComponentInParent<minigame3_fish>().fishHit();
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        hitGarbage = true;
        //delayFade();
    }

    void delayFade()
    {
        //yield return new WaitForSeconds(1f);
        transform.GetComponentInParent<RopeSlefMovementControl>().hitGarbage();
    }
}
