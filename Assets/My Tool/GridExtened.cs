using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace GridSpace
{
    
    public enum ObjectKind
    {
        ObjectTag, ObjectLayer
    }

    class ObjectsList
    {
        public ObjectKind objectType;
        public string objectName;
        public GameObject[,] gridObjects;
        public ObjectsList(int sizeA, int SizeB)
        {
            gridObjects = new GameObject[sizeA, SizeB];
        }

        public void ClearGridObject()
        {
            for (int i = 0; i < gridObjects.GetLength(0); i++)
            {
                for (int j = 0; j < gridObjects.GetLength(1); j++)
                {
                    gridObjects[i, j] = null;
                }
            }
        }
    }

    public class GridExtened : MonoBehaviour
    {
        
        static List<ObjectsList> objectList = new List<ObjectsList>();
        static public void AssignObjects(string name, ObjectKind objectType)
        {

            if (LayerMask.NameToLayer(name) == -1 && objectType == ObjectKind.ObjectLayer) { return; }// check if the layer exists
            if (!TagExists(name) && objectType == ObjectKind.ObjectTag) { return; }//check if the tag exisits

            ObjectsList temObjectList = objectList.Find(item => item.objectName == name);
            {
                if (temObjectList != null)//if already does not exist, create a new object
                {
                    temObjectList.ClearGridObject();
                }
                else
                {
                    Vector2Int temGridSize = GridSnapSystem.GetGridSize();

                    temObjectList = new ObjectsList(temGridSize.x, temGridSize.y);
                    temObjectList.objectType = objectType;
                    temObjectList.objectName = name;
                    objectList.Add(temObjectList);
                }
            }

            if (objectType == ObjectKind.ObjectTag)
            {
                foreach (GameObject _gameObject in GameObject.FindGameObjectsWithTag(name))
                {
                    Vector2Int objectIndexes = GridSnapSystem.ObjectIndex(_gameObject.transform.position);
                    temObjectList.gridObjects[objectIndexes.x, objectIndexes.y] = _gameObject;

                    Debug.Log("Object Name:" + _gameObject.name + " Index:" + objectIndexes);
                }
            }

            Debug.Log("_____" + objectList.Count);
        }

        static bool TagExists(string aTag)
        {
            if (GameObject.FindGameObjectsWithTag(aTag).Length == 0)
                return false;

            return true;
        }
    }
}


