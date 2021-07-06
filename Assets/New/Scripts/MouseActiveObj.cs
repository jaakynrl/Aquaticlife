using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseActiveObj : MonoBehaviour
{
    private void Start()
    {
        Invoke("Des", 2);
    }
    public void Des()
    {
        Destroy(gameObject);
    }
}
