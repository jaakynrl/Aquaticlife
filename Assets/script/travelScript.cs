using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class travelScript : MonoBehaviour
{
    public Text timeTravelText;
    public Text barTravelText;
    public Text deepTravelText;

    public float timeTravel;

    public int currentCountTravel;
    [Header("กำหนดค่่าเวลาที่ผ่านไป")]
    public float timeCalculate;
    [Header("กำหนดค่่า Bar")]
    public int valueBar;
    [Header("กำหนดค่่าความลึก")]
    public int valueDeep;

    [SerializeField]
    bool haveDeep = true;


    public float comeBackTime;
   

    // Update is called once per frame
    void Update()
    {
        timeTravel += Time.deltaTime;
        timeTravelText.text = (FormatSeconds(timeTravel));
        calculate_timeTravel(timeTravel);

        if (Input.GetKeyDown(KeyCode.A))
        {
            Time.timeScale = 10;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Time.timeScale = 1;
        }

        if(timeTravel >= comeBackTime)
        {
            nextScene();
        }
    }

    string FormatSeconds(float elapsed)
    {
        int d = (int)(elapsed * 100.0f);
        int minutes = d / (60 * 100);
        int seconds = (d % (60 * 100)) / 100;
        return String.Format("{0:00}:{1:00}", minutes, seconds);
    }



    void calculate_timeTravel(float time)
    {
        if(time > 0f)
        {
          
            int timeValue = Mathf.FloorToInt(time / timeCalculate);
            setDeep_bar(timeValue);
        }
    }


    void setDeep_bar(int timeCurrent)
    {
        barTravelText.text = String.Format("{0:00}" ,timeCurrent * valueBar);
        if (haveDeep)
        {
            deepTravelText.text = String.Format("{0:00}", timeCurrent * valueDeep);
        }
    }

    public string sceneName;
    // Start is called before the first frame update
    public void nextScene()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

}
