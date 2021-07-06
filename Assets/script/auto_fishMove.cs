using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class auto_fishMove : MonoBehaviour
{
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
    
    private void OnEnable()
    {
        InvokeRepeating("create_fnish", createFish_time, createFish_rate);
    }


    private void OnDisable()
    {
        CancelInvoke();
    }

    public void create_fnish()
    {
        int randomTypeFish = Random.Range(0, fishPrefabs.Length);
        fishClone = fishPrefabs[randomTypeFish];
        GameObject clone_fish = Instantiate(fishClone,transform);
        float randomY = Random.Range(positioCreateFish_min, positioCreateFish_max);
        float randomScale = Random.Range(scaleFishMin, scaleFishMax);
        int randomNumber = Random.Range(0, 100);
        if (randomNumber % 2 == 0)
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
}
