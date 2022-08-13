using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class SomeOther : MonoBehaviour
{
    [ContextMenuItem("Randomize", "ResetName")]
    public int experience = 50;
    public bool showLevel = false;

    public int Level
    {
        get { return experience / 750; }
    }

    public void ResetName()
    {
        
        experience = Random.Range(0, 10000);
    }

    
}
