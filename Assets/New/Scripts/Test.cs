using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Test : MonoBehaviour
{
    public string endPoint = "https://game-pirest.aowdtech.com/"; // https://game-pirest.aowdtech.com/
    public Image img;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var dc = DataCore.instance;
            img.sprite = dc.imgs["c90"];
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            var dc = DataCore.instance;
            img.sprite = dc.imgs["i127"];
        }
    }

    IEnumerator Fire()
    {
        WWWForm form = new WWWForm();
        string[] words = { "one", "two", "three" };

        form.AddField("ids", string.Join("','", words));
        using (UnityWebRequest www = UnityWebRequest.Post(endPoint + "service-game/data-version.php", form))
        {
            yield return www.SendWebRequest();

            if (!www.isNetworkError || !www.isHttpError)
            {
                var version = JsonUtility.FromJson<DataVersion>(www.downloadHandler.text);
                Debug.Log("card: "  +version.card_update);
                Debug.Log("item: "  +version.item_update);
            }
            else
            {
                Debug.Log("fail");
            }
        }
    }
}
