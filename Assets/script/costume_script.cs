using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class costume_script : MonoBehaviour
{
    public GameObject checkmark;
    public GameObject costume;
    public bool _checked;
    public costume_check _costumeCheck;
    private void Start()
    {
        _costumeCheck = GameObject.Find("costumeCheck").GetComponent<costume_check>();
    }
    // Start is called before the first frame update
    public void selectCostume(int numberItem)
    {
        if(_checked == false)
        {
            checkmark.SetActive(true);
            if(costume != null)
            {
                costume.SetActive(true);
            }
            _costumeCheck.costumeSelected[numberItem] = true;
            _checked = true;
        }else
        {
            checkmark.SetActive(false);
            if (costume != null)
            {
                costume.SetActive(false);
            }
            _costumeCheck.costumeSelected[numberItem] = false;
            _checked = false;
        }
    }

    public void selectCamera()
    {
        if (_checked == false)
        {
            checkmark.SetActive(true);
            if (costume != null)
            {
                costume.SetActive(true);
            }
            _costumeCheck.cameraSelected = true;
            _checked = true;
        }
        else
        {
            checkmark.SetActive(false);
            if (costume != null)
            {
                costume.SetActive(false);
            }
            _costumeCheck.cameraSelected = false;
            _checked = false;
        }
    }
}
