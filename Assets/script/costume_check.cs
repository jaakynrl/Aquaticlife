using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class costume_item
{
    public string type;
    public GameObject[] Item;
}



public class costume_check : MonoBehaviour
{
    public bool[] costumeSelected;
    public bool cameraSelected;
    public bool checkAll;
    // Start is called before the first frame update
    public costume_item[] _costume_item;
    public void checkUpdate()
    {
        PlayerPrefs.SetInt("costumeCheck", 1);
        PlayerPrefs.SetInt("cameraCheck", 1);
        for (int i = 0; i < costumeSelected.Length; i++)
        {
            if (costumeSelected[i] == false)
            {
                PlayerPrefs.SetInt("costumeCheck", 0);
                break;
            }
        }


        if (cameraSelected == false)
        {
            PlayerPrefs.SetInt("cameraCheck", 0);

        }
        nextScene();
    }

    public string sceneName;
    // Start is called before the first frame update
    public void nextScene()
    {
        sceneName = PlayerPrefs.GetString("currentScene");
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
