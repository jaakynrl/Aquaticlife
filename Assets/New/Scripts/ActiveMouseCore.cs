using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMouseCore : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public GameObject activePrefab;
    private Transform canvas;
    private Vector3 pos;
    Transform canvasTran;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if(pos == Input.mousePosition)
                Clicked();
        }
    }
    public void Clicked()
    {
        if (!canvasTran)
        {
            canvasTran = FindObjectOfType<Canvas>().transform;
        }
        var ac = Instantiate(activePrefab, canvasTran);
        ac.GetComponent<RectTransform>().position = Input.mousePosition;
    }
}
