using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class randomDiamonds : MonoBehaviour
{
    int randomNumber;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        randomNumber = Random.Range(1, 10);
        string strText = "+" + randomNumber.ToString();
        //randomNumber.ToString();
        //Debug.Log(randomNumber);
        text.text = strText;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
