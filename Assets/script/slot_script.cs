using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class slot_script : MonoBehaviour
{

    public List<slot_item> slot_row1;
    public List<slot_item> slot_row2;
    public List<slot_item> slot_row3;

    public List<slot_item> slot_column1;
    public List<slot_item> slot_column2;
    public List<slot_item> slot_column3;


    public static slot_script _slot;
    public int finish_count;
    public bool stop;

    public GameObject btnspin;
    [SerializeField]
    int count_d;
    [SerializeField]
    int count_c;
    // public GameObject 
    // Start is called before the first frame update
    [SerializeField]
    GameObject popup_result;
    [SerializeField]
    TextMeshProUGUI text_coin;
    [SerializeField]
    TextMeshProUGUI text_diamond;
    public bool startSpin;
    public gameSystem _system;
    public coin_diamond_api _api;

    private void Awake()
    {
        _slot = this;
    }

    private void Update()
    {
        if (!stop)
        {
            if (finish_count == 9)
            {
                check_allSlot();
                stop = true;
            }
        }
    }


    public void check_allSlot()
    {
        checkSlot(slot_row1);
        checkSlot(slot_row2);
        checkSlot(slot_row3);

        checkSlot(slot_column1);
        checkSlot(slot_column2);
        checkSlot(slot_column3);

        text_coin.text = count_c.ToString();
        text_diamond.text = count_d.ToString();
        _system.coin += count_c;
        _system.diamond += count_d;
        _system.coinText.text = _system.coin.ToString();
        _system.diamondText.text = _system.diamond.ToString();
        _api.insert_coin_diamond(count_c, count_d);
        Invoke("show_result", 1f);

    }


    public void show_result()
    {
        popup_result.SetActive(true);

    }

    public void checkSlot(List<slot_item> slot_item)
    {

        int d_count = slot_item.FindAll(x => x.result == "d").Count;
        int c_count = slot_item.FindAll(x => x.result == "c").Count;

        if(d_count == 3)
        {
            count_d += Random.Range(100, 200);
           
        }

        if(c_count == 3)
        {
            count_c += Random.Range(500, 1000);

        }
    }

    public void start_btn()
    {
        startSpin = true;
        btnspin.SetActive(false);
    }
}
