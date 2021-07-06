using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObj : MonoBehaviour
{
    public List<SpriteRenderer> customPos = new List<SpriteRenderer>();
    public GameObject lineCamera;

    private void Start()
    {
        customPos.Insert(0, null);
    }
    private void OnEnable()
    {
        SetCharacter(DataCore.instance.items);
    }

    public void SetCharacter(Dictionary<string,Item> items)
    {
        StartCoroutine(SetCustom(items));
    }

    IEnumerator SetCustom(Dictionary<string, Item> items)
    {
        var dc = DataCore.instance;
        yield return new WaitForSeconds(.01f);
        for (int i = 1; i <= 7; i++)
        {
            if (PlayerPrefs.HasKey("s" + i) && PlayerPrefs.GetString("s" + i) != "")
            {
                var itemId = PlayerPrefs.GetString("s" + i);
                customPos[i].sprite = dc.imgs["i" +itemId];
                customPos[i].color = new Color32(255, 255, 255, 255);
            }
            else
            {
                customPos[i].color = new Color32(0, 0, 0, 0);
            }
        }
        lineCamera.SetActive(PlayerPrefs.GetString("s6") != "");
    }
}
