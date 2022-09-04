using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace GridSpace
{
    
    public enum ObjectType
    {
        ObjectTag, ObjectLayer
    }

    class ObjectsList
    {
        public ObjectType _objectType;
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

    public class GridExtened : Editor
    {
        
        static List<ObjectsList> _objectList = new List<ObjectsList>();
        static public void AssignObjects(string name, ObjectType objectType)
        {

            int temLayered = LayerMask.NameToLayer(name);
            if (temLayered == -1 && objectType == ObjectType.ObjectLayer)
            {
                return;
            }

            if(!TagExists(name) && objectType == ObjectType.ObjectTag)
            {
                return;
            }
           
            ObjectsList temObjectList;

            bool exists = false;
            foreach (ObjectsList item in _objectList)
            {
                if (item.objectName == name)
                {
                    exists = true;
                    temObjectList = item;
                    temObjectList.ClearGridObject();
                    break;
                }

            }

            if (exists == false)
            {
                Vector2Int temGridSize = GridSnapSystem.GetGridSize();
                temObjectList = new ObjectsList(temGridSize.x, temGridSize.y);
                temObjectList._objectType = objectType;
                temObjectList.objectName = name;
            }



            foreach (GameObject _gameObject in GameObject.FindGameObjectsWithTag(name))
            {
                if(objectType == ObjectType.ObjectLayer)
                {
                }
            }
            
        }

        static bool TagExists(string aTag)
        {
            if (GameObject.FindGameObjectsWithTag(aTag).Length == 0)
                return false;

            return true;
        }
    }
}


