using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mniGame2_fishCatch : MonoBehaviour
{
    public GameObject keepFish;
    public bool isUp;
    public List<GameObject> fish;
    bool startClear;
    [SerializeField]
    bool delAll;
    public bool catchBigFish;
    public miniGame2_controller _controller;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (!catchBigFish)
        {
            if (collision.tag == "fish_small" && isUp)
            {
                collision.transform.SetParent(keepFish.transform);
                collision.transform.localPosition = new Vector2(0, -1);
                collision.GetComponent<miniGame2_fishMove>().stopMove = true;
                collision.GetComponent<BoxCollider2D>().isTrigger = false;
                collision.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
                fish.Add(collision.gameObject);
            }

            if (collision.tag == "fish_big" && isUp)
            {
                collision.transform.SetParent(keepFish.transform);
                collision.transform.localPosition = new Vector2(0, 0);
                collision.GetComponent<miniGame2_fishMove>().stopMove = true;
                collision.GetComponent<miniGame2_fishMove>().fade = true;
                _controller.lifeDisable(collision.GetComponent<miniGame2_fishMove>().ImageSprite);
                catchBigFish = true;
            }
        }
    }


    public IEnumerator clearFish(float timeDelete)
    {
        if (!catchBigFish)
        {
            if (fish.Count > 0 && !startClear)
            {
                startClear = true;
                if (delAll)
                {
                    yield return new WaitForSeconds(timeDelete);
                    foreach (GameObject item in fish)
                    {
                        _controller.addScore();
                        Destroy(item);
                    }
                }
                else
                {
                    foreach (GameObject item in fish)
                    {
                        yield return new WaitForSeconds(timeDelete);
                        _controller.addScore();
                        Destroy(item);
                    }
                }


                startClear = false;
                fish.Clear();
            }
        }
    }

    public void clearSmallFish()
    {
        if (fish.Count > 0 )
        {
            foreach (GameObject item in fish)
            {

                Destroy(item);
            }
            fish.Clear();
        }
    }
}
