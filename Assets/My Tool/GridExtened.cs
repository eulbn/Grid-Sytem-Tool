using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSpace
{
    
    public enum ObjectType
    {
        ObjectTag, ObjectLayer
    }

    class ObjectsList
    {
        ObjectType _objectType;
        GameObject[,] gridObjects;

        ObjectsList(int sizeX, int SizeY)
        {
            gridObjects = new GameObject[sizeX, SizeY];
        }
    }


    public class GridExtened : MonoBehaviour
    {
        List<ObjectsList> _objectList = new List<ObjectsList>();
        void AssignObjects(string name, ObjectType objectType)
        {
            foreach (GameObject _gameObject in GameObject.FindGameObjectsWithTag(name))
            {

            }
        }
    }
}


