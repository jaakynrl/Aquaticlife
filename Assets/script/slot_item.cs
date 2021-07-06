using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slot_item : MonoBehaviour
{
    public Sprite coin;
    public Sprite diamond;
    public Image slot;

    public float rangeMin;
    public float rangeMax;
    public float randomTime;
    [SerializeField]
    bool isCoin;

    public string result;

    public bool stop;
    public int randomImage;
  
    // Start is called before the first frame update
    void Start()
    {
        slot = GetComponent<Image>();
        randomTime = Random.Range(rangeMin, rangeMax);
      
    }

    // Update is called once per frame
    void Update()
    {
        if (slot_script._slot.startSpin)
        {

            if (randomTime > 0)
            {
                randomImage = Random.Range(0, 10);

                if (randomImage % 2 == 0)
                {
                    slot.sprite = diamond;
                    result = "d";
                    isCoin = false;
                }
                else
                {
                    result = "c";
                    slot.sprite = coin;
                    isCoin = true;
                }



                randomTime -= Time.deltaTime;
            }
            else
            {
                if (!stop)
                {
                    Debug.Log("xx");
                    slot_script._slot.finish_count = slot_script._slot.finish_count + 1;
                    stop = true;
                }
            }
        }
    }
}
