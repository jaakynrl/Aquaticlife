using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniGame2_fishMove : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    public bool stopMove;
    public bool isLeft;

    public bool fade;
    public Color _color;
    public float speedFade;

    public bool hitObject;
    public miniGame2_controller _controller;
    public Sprite ImageSprite;
    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > 12 || transform.position.x < -12)
        {
            Destroy(gameObject);
        }


        if (!stopMove)
        {
            if (isLeft)
            {
                transform.Translate(Vector2.left * -speed);
            }
            else
            {
                transform.Translate(Vector2.left * speed);
            }
          
        }


        if (fade)
        {
            float alphaValue = Mathf.Lerp(_color.a, 0,Time.deltaTime* speedFade);
            _color.a = alphaValue;
            GetComponent<SpriteRenderer>().color = _color;
            if(alphaValue < 0.09)
            {
                _controller.resetCatch();
                Destroy(gameObject);
            }
        }
            
    }

    public void fishHitObj()
    {
        if (!hitObject)
        {
            speed = speed * -2;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            hitObject = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "_fish")
        {
            fishHitObj();
        }
    }
    
}
