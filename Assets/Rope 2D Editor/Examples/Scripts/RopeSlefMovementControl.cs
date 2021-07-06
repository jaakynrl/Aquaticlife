using UnityEngine;
using System.Collections;

public class RopeSlefMovementControl : MonoBehaviour {
    public Rope rope;
    [SerializeField]
	Transform firstSegment;

    [SerializeField]
    Color colorstart;
    [SerializeField]
    Color colorFinish;
    [SerializeField]
    float speedFade;
    [SerializeField]
    float speedFadeOut;
    [SerializeField]
    float speedDown;
    // Use this for initialization
    bool isDown;
    [SerializeField]
    public float start_position;
    GameObject lastRope;
   
    public void setRope()
    {
        if (!rope)
        {
            rope = GetComponent<Rope>();
            if (rope)
            {

                
                foreach (Transform item in transform)
                {
                    if(firstSegment == null)
                    {
                        firstSegment = item;
                        firstSegment.GetComponent<Rigidbody2D>().isKinematic = true;
                    }
                    item.gameObject.AddComponent<minigame3_chain>();
                    item.GetComponent<minigame3_chain>().colorStart = colorstart;
                    item.GetComponent<minigame3_chain>().colorFinish = colorFinish;
                    item.GetComponent<minigame3_chain>().speedFade = speedFade;
                    item.GetComponent<minigame3_chain>().speedDown = speedDown;
                    lastRope = item.gameObject;
                }

                lastRope.GetComponent<minigame3_chain>().triggerItem = true;
                
            }
        }
    }
    public void hitGarbage()
    {
        foreach (Transform item in transform)
        {
            item.GetComponent<minigame3_chain>().fadeStart = true;

        }
    }

    float elapsedTime = 0;
    bool hit;
    public Vector3 mouse_position;
    // Update is called once per frame
    void Update () {

        if (firstSegment != null)
        {

            if (firstSegment.GetComponent<SpriteRenderer>().color.a < 0.1f)
            {
                Destroy(this.gameObject);
            }
        }
   
    }
}
