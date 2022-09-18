using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSpace;

public class Test1 : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            //Debug.Log(GridSnapSystem.ObjectIndex(transform.position));

            GridExtened.AssignObjects("SomeTag", ObjectKind.ObjectTag);
        }
    }
}
