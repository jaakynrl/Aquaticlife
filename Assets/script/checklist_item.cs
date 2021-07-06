using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class itemStage
{
    public bool itemCheck;
    public string itemName;
}

[System.Serializable]
public class stage_game
{
    public List<itemStage> items;
    public string stage_name;
}

public class checklist_item : MonoBehaviour
{
    public GameObject itemCamera;
    public GameObject itemCostume;
    public Button btnStart;

    public int checkCostume;
    public int checkCamera;


    public stage_game stage_game;
    [SerializeField]
    //int camaeraId;

    public stage_game test_stage_game;
    // Start is called before the first frame update
    void Start()
    {

        if (dreeRoom.itemStages != null)
        {
            test_stage_game = dreeRoom.itemStages;
            if (dreeRoom.itemStages.stage_name == stage_game.stage_name)
            {
                int count_item = 0;
                itemCostume.SetActive(true);
                checkCostume = 1;
                foreach (itemStage item in dreeRoom.itemStages.items)
                {
                    Debug.Log("xxx");
                    if (count_item == 0)
                    {
                        if (item.itemCheck)
                        {
                            checkCamera = 1;
                            itemCamera.SetActive(true);
                        }
                    }
                    else
                    {
                        if (!item.itemCheck)
                        {
                            checkCostume = 0;
                            itemCostume.SetActive(false);
                        }

                    }

                    count_item++;
                }
            }
            else
            {
                dreeRoom.itemStages = stage_game;
            }
        }
        else
        {
            dreeRoom.itemStages = stage_game;
        }


        if (checkCostume == 1 && checkCamera == 1)
        {
            btnStart.interactable = true;
        }
        else
        {
            btnStart.interactable = false;
        }
    }

}
