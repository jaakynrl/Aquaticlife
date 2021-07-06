using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scene_manage : MonoBehaviour
{
	public string sceneName;
    // Start is called before the first frame update
    public void nextScene()
	{
        if(sceneName == "dressroom")
        {
            PlayerPrefs.SetString("currentScene", SceneManager.GetActiveScene().name);
        }
        SceneManager.LoadScene(sceneName,LoadSceneMode.Single);//single โหลดหน้าใหม่
	}

    public void loadSceneAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }


}
