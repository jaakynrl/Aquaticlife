using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minigame3_garbageMove : MonoBehaviour
{
    public float speedRote;
    public bool markTarget;
    public float speedMove;
    public bool inWater;
    [SerializeField]
    float randomYPositionMax;
    [SerializeField]
    float randomYPositionMin;
    public float randomY;

    [SerializeField]
    float randomFloat_Max;
    [SerializeField]
    float randomFloat_min;
    [SerializeField]
    float speedLerp;

    public bool is_ground;
    public bool inScene;
    public bool hit;

    public minigame3_mainScript _main;

    [SerializeField]
    bool noGround;
    // Start is called before the first frame update
    void Start()
    {
        randomY = Random.Range(randomYPositionMin, randomYPositionMax);
    }

    public bool checkItem()
    {
        if(inScene && !is_ground && !markTarget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < 3)
        {
            inScene = true;
        }
        transform.eulerAngles = (Vector3.forward *(Time.deltaTime* speedRote));
        /* if(transform.position.y <= randomY)
         {
             transform.position = Vector2.Lerp(
                 transform.position,
                 new Vector2(transform.position.x, transform.position.y + Random.Range(randomFloat_min, randomFloat_Max)),
                 speedLerp * Time.deltaTime) ;
         }
         else
         {
             transform.Translate(Vector2.down * speedMove);
         }
         */

        if (hit)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.deltaTime * speedLerp);
            if(transform.localScale.x < 0.1f)
            {
                Destroy(this.gameObject);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            is_ground = true;
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().angularVelocity = 0;
            if (!noGround)
            {
                _main.garbageInGround += 1;
            }
            //speedLerp = speedLerp / 10f;
        }
        Debug.Log(collision.gameObject.name);
    }
}
