using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


namespace GridSpace
{

    class Test: MonoBehaviour
    {
        private void Update()
        {
            if(Input.GetMouseButtonDown(1))
            {
                Debug.Log("KKKKKKKKKKKKKKKKKK");
                //ObjectUtilityHandler.CreateObjectUtilityHandle();
            }
        }
    }

    internal class ObjectUtilityHandler : MonoBehaviour
    {
        static GameObject handle;//the handle of this class like singleton
        static internal ObjectUtilityHandler instance { get; set; } 

        static internal void CreateHandle()
        {
            if(handle == null && Application.isPlaying)
            {

                if (GameObject.Find("ObjectUtilityHandler"))
                {
                    Debug.LogError("<color=red>ObjectUtilityHandle was destroyed by ObjectUtility. Please change the name of the gameobject 'ObjectUtilityHandle' in the Hierarchy.</color>");
                    Destroy(GameObject.Find("ObjectUtilityHandler"));
                }
                handle = new GameObject("ObjectUtilityHandler") /*{ name = "ObjectUtilityHandle", hideFlags = HideFlags.HideAndDontSave }*/;
                handle.AddComponent<ObjectUtilityHandler>();

                DontDestroyOnLoad(handle);
            }
        }

    }
    static public class ObjectUtility
    {
        static public void MoveTo(this GameObject _gameObject,  Vector3 moveTo)
        {
            
        }

    }
    


}
