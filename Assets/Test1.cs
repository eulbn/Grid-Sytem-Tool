using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSpace;

public class Test1 : MonoBehaviour
{
    public string some = "";
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Debug.Log(LayerMask.NameToLayer(some));


        }
    }
}
