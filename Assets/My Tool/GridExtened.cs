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
<<<<<<< Updated upstream
        public ObjectKind objectType;
=======
        public ObjectKind objectKind;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        
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
=======

        static List<ObjectsList> objectList = new List<ObjectsList>();
        static public void AssignObjects(string objectTypeName, ObjectKind objectKind)
        {

            if (LayerMask.NameToLayer(objectTypeName) == -1 && objectKind == ObjectKind.ObjectLayer) { return; }// check if the layer exists
            if (!TagExists(objectTypeName) && objectKind == ObjectKind.ObjectTag) { return; }//check if the tag exisits

            ObjectsList temObjectList = objectList.Find(item => item.objectName == objectTypeName);
            if (temObjectList != null)//if already exists, clear the grid with that layer/tag etc..
            {
                temObjectList.ClearGridObject();
            }
            else//if already does not exist, create a new object
            {
                Vector2Int temGridSize = GridSnapSystem.GetGridSize();

                temObjectList = new ObjectsList(temGridSize.x, temGridSize.y);
                temObjectList.objectKind = objectKind;
                temObjectList.objectName = objectTypeName;
                objectList.Add(temObjectList);
            }
            

            if (objectKind == ObjectKind.ObjectTag)
            {
                foreach (GameObject _gameObject in GameObject.FindGameObjectsWithTag(objectTypeName))
>>>>>>> Stashed changes
                {
                    Vector2Int objectIndexes = GridSnapSystem.ObjectIndex(_gameObject.transform.position);
                    temObjectList.gridObjects[objectIndexes.x, objectIndexes.y] = _gameObject;

                    Debug.Log("Object Name:" + _gameObject.name + " Index:" + objectIndexes);
                }
            }
<<<<<<< Updated upstream

            Debug.Log("_____" + objectList.Count);
=======
            else if (objectKind == ObjectKind.ObjectLayer)
            {

                foreach (GameObject _gameObject in FindObjectsOfType<GameObject>())
                {
                    if (_gameObject.layer == LayerMask.NameToLayer(objectTypeName))
                    {
                        Vector2Int objectIndexes = GridSnapSystem.ObjectIndex(_gameObject.transform.position);
                        temObjectList.gridObjects[objectIndexes.x, objectIndexes.y] = _gameObject;
                    }
                }
            }
>>>>>>> Stashed changes
        }

        static bool TagExists(string aTag)
        {
            if (GameObject.FindGameObjectsWithTag(aTag).Length == 0)
                return false;

            return true;
        }
<<<<<<< Updated upstream
=======


        static public List<GameObject> GetObjects(string objectTypeName, ObjectKind objectKind)
        {
            if (LayerMask.NameToLayer(objectTypeName) == -1 && objectKind == ObjectKind.ObjectLayer) { return null; }// check if the layer exists
            if (!TagExists(objectTypeName) && objectKind == ObjectKind.ObjectTag) {return null; }//check if the tag exisits
            
            ObjectsList temObjectsList = objectList.Find(item => item.objectName == objectTypeName && item.objectKind == objectKind);

            if (temObjectsList == null)
                return null;

            List<GameObject> temGridObjects = new List<GameObject>();

            for (int i = 0; i < temObjectsList.gridObjects.GetLength(0); i++)
                for (int j = 0; j < temObjectsList.gridObjects.GetLength(1); j++)
                    if (temObjectsList.gridObjects[i, j] != null)
                        temGridObjects.Add(temObjectsList.gridObjects[i, j]);

            return temGridObjects;

        }
>>>>>>> Stashed changes
    }
}


