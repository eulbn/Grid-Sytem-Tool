using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Visualization 
// Actual Logic
// GUI Tool

public class Test : MonoBehaviour
{
    public int gridSize = 10;
    public int gridCellSize = 1;

    Transform tran;
    
    private void OnDrawGizmos()
    {

        float temp = GetComponent<MeshRenderer>().bounds.size.x * transform.localScale.x;
        DrawGrid(gridSize, gridCellSize, Color.green);
        Highlights(gridCellSize, transform);

    }
    
    
   /* private void OnRenderObject()
    {
        foreach (var _transfrom in Selection.transforms)
        {
            _transfrom.position = this.SnaptoGrid(_transfrom.position, gridCellSize);
        }
    }*/
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, 0.5f);
        Gizmos.DrawCube(Vector3.zero, Vector3.one * 10);
        foreach (var _transfrom in Selection.transforms)
        {
            _transfrom.position = this.SnaptoGrid(_transfrom.position, gridCellSize);
        }
    }

    private Vector3 SnaptoGrid(Vector3 _vector, int gridSize)
    {
        return (new Vector3(
            Mathf.Round(_vector.x / gridSize) * gridSize,
            Mathf.Round(_vector.y / gridSize) * gridSize,
            Mathf.Round(_vector.z / gridSize) * gridSize
            ));
    }


    void DrawGrid(float size, float cellSize, Color color)
    {
        Gizmos.color = color;
        Handles.color = color;

        if (size % 2 != 0)
            size++;


        float actualSize = size * cellSize;
        float temOffset = (-actualSize / 2) + (cellSize/2);


        Vector3 offset_X_Start = new Vector3(temOffset,  0.0f, temOffset);
        Vector3 offset_X_End = new Vector3(-temOffset, 0.0f, temOffset);


        Vector3 offset_Z_Start = new Vector3(temOffset, 0.0f, temOffset);
        Vector3 offset_Z_End = new Vector3(temOffset, 0.0f, -temOffset);

        for (int i = 0; i < size; i++)
        {
            /*Gizmos.DrawLine(offset_X_Start, offset_X_End);
            Gizmos.DrawLine(offset_Z_Start, offset_Z_End);*/


            Handles.DrawAAPolyLine(offset_X_Start, offset_X_End);
            Handles.DrawAAPolyLine(offset_Z_Start, offset_Z_End);
            offset_X_Start.z += cellSize;
            offset_X_End.z += cellSize;

            offset_Z_Start.x += cellSize;
            offset_Z_End.x += cellSize;
        }
    }


    void Highlights(float cellSize, Transform selectedTransform)
    {
        Renderer selectedMeshRenderer = selectedTransform.GetComponent<MeshRenderer>();
        Bounds bounds;
        if(selectedMeshRenderer)
        {
            bounds = new Bounds(selectedMeshRenderer.bounds.center, selectedMeshRenderer.bounds.size);

            foreach (Renderer r in selectedTransform.GetComponentsInChildren<Renderer>())
            {
                Bounds tem_R_bounds = r.bounds;
                tem_R_bounds.size = new Vector3(tem_R_bounds.size.x , tem_R_bounds.size.y, tem_R_bounds.size.z);
                bounds.Encapsulate(tem_R_bounds);
            }

            
            /*Debug.Log("Mesh Center " + selectedMeshRenderer.bounds.center);
            Debug.Log("Transform Center " + selectedTransform.position);*/
            //bounds.center = selectedTransform.position;

            

            

            Bounds temBounds = bounds;

            temBounds.center = this.Recenter(temBounds.center, cellSize / 2);
            temBounds.center = new Vector3(temBounds.center.x, 0, temBounds.center.z);

            /*temBounds.size = this.RoundOff(temBounds.size, temBounds.center, cellSize);
            temBounds.size = new Vector3(temBounds.size.x, 0, temBounds.size.z);*/

            temBounds.size = this.RoundOff(temBounds.size, temBounds.center, cellSize);
            temBounds.size = new Vector3(temBounds.size.x, 0, temBounds.size.z);

            /*Vector3 temCenter = bounds.center;*/


            /* Debug.Log("bounds.center " + bounds.center);
             Debug.Log("temCenter " + temCenter);
             Debug.Log("cell size " + cellSize / 2);*/


            Color temColor = Color.green;
            temColor.a = 0.4f;
            Gizmos.color = temColor;
            Gizmos.DrawCube(temBounds.center, temBounds.size);

            /*Gizmos.color = new Color(1, 1, 1, 0.4f);
            Gizmos.DrawCube(bounds.center, new Vector3(bounds.size.x, 0, bounds.size.z));

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(bounds.center, 0.2f);


            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(temBounds.center, 0.2f);*/
        }
        
    }

    Vector3 RoundOff(Vector3 _vector, Vector3 _center, float roundOffTo)
    {
        /*Debug.Log("Cell size" + roundOffTo);

        Debug.Log(_vector.z % roundOffTo);

        Debug.Log("Before " + _vector);

        Debug.Log(_center);

        _vector.x += roundOffTo - (_vector.x % roundOffTo);

        _vector.y += roundOffTo - (_vector.y % roundOffTo);*/

        if (_center.x % roundOffTo != 0)
        {
            _vector.x += (roundOffTo * 2) - (_vector.x % (roundOffTo * 2));
        }
        else
        {
            if (_vector.x % (roundOffTo * 2) > roundOffTo)
                _vector.x += (roundOffTo - (_vector.x % roundOffTo)) + roundOffTo;
            else
                _vector.x += roundOffTo - (_vector.x % roundOffTo);
        }


        if (_center.y % roundOffTo != 0)
        {
            _vector.y += (roundOffTo * 2) - (_vector.y % (roundOffTo * 2));
        }
        else
        {
            if (_vector.y % (roundOffTo * 2) > roundOffTo)
                _vector.y += (roundOffTo - (_vector.y % roundOffTo)) + roundOffTo;
            else        
                _vector.y += roundOffTo - (_vector.y % roundOffTo);
        }


        if (_center.z % roundOffTo != 0)
        {
            _vector.z += (roundOffTo * 2) - (_vector.z % (roundOffTo * 2));
        }
        else
        {
            if(_vector.z % ( roundOffTo * 2) > roundOffTo)
                _vector.z += (roundOffTo - (_vector.z % roundOffTo)) + roundOffTo;
            else
                _vector.z += roundOffTo - (_vector.z % roundOffTo);
        }


        return _vector;
    }

    Vector3 Recenter(Vector3 _vector, float roundOffTo)
    {
        
        if (Mathf.Abs(_vector.x) % roundOffTo >= roundOffTo / 2)
        {
            if (_vector.x <= 0)
                _vector.x += -roundOffTo - (_vector.x % roundOffTo);
            else
                _vector.x += roundOffTo - (_vector.x % roundOffTo);
        }
        else
            _vector.x -= _vector.x % roundOffTo;


        if (Mathf.Abs(_vector.y) % roundOffTo > roundOffTo / 2)
        {
            if (_vector.y < 0)
                _vector.y += -roundOffTo - (_vector.y % roundOffTo);
            else
                _vector.y += roundOffTo - (_vector.y % roundOffTo);
        }
        else            
            _vector.y -= _vector.y % roundOffTo;


        if (Mathf.Abs(_vector.z) % roundOffTo > roundOffTo / 2)
        {
            if (_vector.z < 0)
                _vector.z += -roundOffTo - (_vector.z % roundOffTo);
            else
                _vector.z += roundOffTo - (_vector.z % roundOffTo);
        }
        else
            _vector.z -= _vector.z % roundOffTo;


        return _vector;
    }

    Vector3 AnotherRoundOff(Vector3 _vector, float roundOffTo)
    {
        _vector.x += roundOffTo - (_vector.x % roundOffTo);
        _vector.x -= _vector.x % roundOffTo;

        _vector.y += roundOffTo - (_vector.y % roundOffTo);
        _vector.y -= _vector.y % roundOffTo;

        _vector.z += roundOffTo - (_vector.z % roundOffTo);
        _vector.z -= _vector.z % roundOffTo;

        return _vector;
    }

}
