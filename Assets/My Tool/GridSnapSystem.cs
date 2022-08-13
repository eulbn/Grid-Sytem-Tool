using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;




public class GridSnapSystem : EditorWindow
{

    private List<Vector3> trackOfSelectedTransformsPosition;
    private float somevalue = 3.343f;
    enum GridOrientation
    {
        xy, yz, zx, xyz
    }

    static GridOrientation gridOrientation = new GridOrientation();

    Color gridColor;
    int gridSize = 100;
    float gridCellSize = 20;

    [MenuItem("Window/GridSnapSystem")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        GridSnapSystem window = (GridSnapSystem)GetWindow(typeof(GridSnapSystem));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        gridSize = EditorGUILayout.IntField("Grid Size", gridSize);
        gridCellSize = EditorGUILayout.FloatField("Grid Cell Size", gridCellSize);
        gridColor = EditorGUILayout.ColorField("Grid Color", gridColor);
        gridOrientation = (GridOrientation)EditorGUILayout.Popup("Orientation", (int)gridOrientation, System.Enum.GetNames(typeof(GridOrientation)));



        SceneView.RepaintAll();
    }


    void OnEnable()
    {
        trackOfSelectedTransformsPosition = new List<Vector3>();
        SceneView.duringSceneGui += this.OnSceneGUI;
        Selection.selectionChanged += this.ReassignTransfromsPosition;
    }

    void OnDisable()
    {
        trackOfSelectedTransformsPosition.Clear();
        trackOfSelectedTransformsPosition = null;
        SceneView.duringSceneGui -= this.OnSceneGUI;
        Selection.selectionChanged -= this.ReassignTransfromsPosition;
    }


    void OnSceneGUI(SceneView sceneView)
    {
        this.SetGrid(gridSize, gridCellSize, gridColor);

        foreach (var _transfrom in Selection.transforms)
        {
            if (this.hasSelectedPositionChanged())
            {
                _transfrom.position = this.SnaptoGrid(_transfrom.position, gridSize, gridCellSize, gridOrientation);
            }

            this.Highlights(gridCellSize, gridSize, _transfrom);
        }
    }
    
    void ReassignTransfromsPosition()
    {
        trackOfSelectedTransformsPosition.Clear();

        foreach (Transform _transform in Selection.transforms)
        {
            this.trackOfSelectedTransformsPosition.Add(_transform.position);
        }
    }

    bool hasSelectedPositionChanged()
    {
        Transform[] temSelectedTransfroms = Selection.transforms;
        for (int i = 0; i < temSelectedTransfroms.Length; i++)
        {
            if (i < this.trackOfSelectedTransformsPosition.Count)
            {
                if (!this.trackOfSelectedTransformsPosition[i].Equals(temSelectedTransfroms[i].position))
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        return false;
    }




    //-----------------------------------Grid Snaping-------------------------------------------
    private Vector3 SnaptoGrid(Vector3 _vector, int _gridSize, float _gridCellSize, GridOrientation _gridOrientation)
    {
        if(_gridSize % 2 == 0)
        {
             
            Vector3 roundedOffVector = new Vector3(
                Mathf.Round(_vector.x / _gridCellSize) * _gridCellSize,
                Mathf.Round(_vector.y / _gridCellSize) * _gridCellSize,
                Mathf.Round(_vector.z / _gridCellSize) * _gridCellSize
            );

            if (_gridOrientation == GridOrientation.xy)
                roundedOffVector.z = _vector.z;
            else if (_gridOrientation == GridOrientation.yz)
                roundedOffVector.x = _vector.x;
            else if (_gridOrientation == GridOrientation.zx)
                roundedOffVector.y = _vector.y;

            return roundedOffVector;

        }
        else
        {
            float temOffset = _gridCellSize / 2;
            Vector3 roundedOffVector = new Vector3(
                (Mathf.Round((_vector.x - temOffset) / _gridCellSize) * _gridCellSize) + temOffset,
                (Mathf.Round((_vector.y - temOffset) / _gridCellSize) * _gridCellSize) + temOffset,
                (Mathf.Round((_vector.z - temOffset) / _gridCellSize) * _gridCellSize) + temOffset
            );

            if (_gridOrientation == GridOrientation.xy)
                roundedOffVector.z = _vector.z;
            else if (_gridOrientation == GridOrientation.yz)
                roundedOffVector.x = _vector.x;
            else if (_gridOrientation == GridOrientation.zx)
                roundedOffVector.y = _vector.y;

            return roundedOffVector;
        }
        
    }
    //-------------------------------------------------------------------------------------------



    //------------------------------------Draw Grid----------------------------------------------
    void SetGrid(int gridSize, float cellSize, Color _color)
    {
        if(gridOrientation == GridOrientation.xyz)
        {
            this.DrawGrid3d(gridSize, cellSize, _color);
        }
        else
        {
            this.DrawGrid2d(gridSize, cellSize, 0, gridOrientation, _color);
        }

    }
    void DrawGrid3d(int gridSize, float cellSize, Color _color)
    {
        float actualSize = gridSize * cellSize;
        float thirdAxisOffset = (-actualSize / 2) + (cellSize / 2);

        for (int i = 0; i < gridSize; i++)
        {
            this.DrawGrid2d(gridSize, gridCellSize, thirdAxisOffset, GridOrientation.xy, _color);
            this.DrawGrid2d(gridSize, gridCellSize, thirdAxisOffset, GridOrientation.yz, _color);
            this.DrawGrid2d(gridSize, gridCellSize, thirdAxisOffset, GridOrientation.zx, _color);


            thirdAxisOffset += cellSize;
        }
    }
    void DrawGrid2d(int gridSize, float cellSize, float thirdAxisOffset, GridOrientation gridOrientation, Color _color)
    {
        Handles.color = _color;

        /*if (gridSize % 2 != 0)
            gridSize++;*/

        float actualSize = gridSize * cellSize;
        float temOffset = (-actualSize / 2) + (cellSize / 2);

        Vector3[] temLineOrigin = this.SetLineOrigin(gridOrientation, temOffset, thirdAxisOffset);

        Vector3 line1_Start = temLineOrigin[0];
        Vector3 line1_End = temLineOrigin[1];

        Vector3 line2_Start = temLineOrigin[2];
        Vector3 line2_End = temLineOrigin[3];

        for (int i = 0; i < gridSize; i++)
        {
            /*Handles.DrawAAPolyLine(80 / gridSize, line1_Start, line1_End);
            Handles.DrawAAPolyLine(80 / gridSize, line2_Start, line2_End);*/

            Handles.DrawPolyLine(line1_Start, line1_End);
            Handles.DrawPolyLine(line2_Start, line2_End);


            line1_Start = this.Line1Increment(line1_Start, gridOrientation, cellSize);
            line1_End = this.Line1Increment(line1_End, gridOrientation, cellSize);

            line2_Start = this.Line2Increment(line2_Start, gridOrientation, cellSize);
            line2_End = this.Line2Increment(line2_End, gridOrientation, cellSize);
        }
    }
    Vector3[] SetLineOrigin(GridOrientation _gridOrientation, float temOffset, float thirdAxisOffset)
    {
        if (_gridOrientation == GridOrientation.xy)
        {
            return (new Vector3[]{
                new Vector3(+temOffset, temOffset, thirdAxisOffset),
                new Vector3(-temOffset, temOffset, thirdAxisOffset),

                new Vector3(temOffset, +temOffset, thirdAxisOffset),
                new Vector3(temOffset, -temOffset, thirdAxisOffset),
            });
        }
        else if (_gridOrientation == GridOrientation.yz)
        {
            return (new Vector3[]{
                new Vector3(thirdAxisOffset, +temOffset, temOffset),
                new Vector3(thirdAxisOffset, -temOffset, temOffset),
                            
                new Vector3(thirdAxisOffset, temOffset, +temOffset),
                new Vector3(thirdAxisOffset, temOffset, -temOffset),
            });
        }
        else if (_gridOrientation == GridOrientation.zx)
        {
            return (new Vector3[]{
                new Vector3(+temOffset, thirdAxisOffset, +temOffset),
                new Vector3(+temOffset, thirdAxisOffset, -temOffset),
                                        
                new Vector3(+temOffset, thirdAxisOffset, temOffset),
                new Vector3(-temOffset, thirdAxisOffset, temOffset),
                
            });
        }
        else
        {
            return (new Vector3[]{
                new Vector3(temOffset, thirdAxisOffset, temOffset),
                new Vector3(-temOffset,thirdAxisOffset, temOffset),
                                       
                new Vector3(temOffset, thirdAxisOffset, temOffset),
                new Vector3(temOffset, thirdAxisOffset, -temOffset),
            });
        }
    }
    Vector3 Line1Increment(Vector3 _vector, GridOrientation _gridOrientation, float cellSize)
    {
        if (_gridOrientation == GridOrientation.xy)
        {
            _vector.y += cellSize;
            return _vector;
        }
        else if (_gridOrientation == GridOrientation.yz)
        {
            _vector.z += cellSize;
            return _vector;
        }
        else if (_gridOrientation == GridOrientation.zx)
        {
            _vector.x += cellSize;
            return _vector;
        }
        else
            return _vector;
    }
    Vector3 Line2Increment(Vector3 _vector, GridOrientation _gridOrientation, float cellSize)
    {
        if (_gridOrientation == GridOrientation.xy)
        {
            _vector.x += cellSize;
            return _vector;
        }
        else if (_gridOrientation == GridOrientation.yz)
        {
            _vector.y += cellSize;
            return _vector;
        }
        else if (_gridOrientation == GridOrientation.zx)
        {
            _vector.z += cellSize;
            return _vector;
        }
        else
            return _vector;
    }
    //-------------------------------------------------------------------------------------------






    void Highlights(float _cellSize, float _gridSize, Transform selectedTransform)
    {
        if (_gridSize % 2 != 0)
        {
            /*Debug.Log("Before" + _cellSize);
            Debug.Log(_cellSize * (0.75f));
            _cellSize = _cellSize * 0.75f;*/

            //_cellSize = _cellSize / 2;
        }

        Renderer selectedMeshRenderer = selectedTransform.GetComponent<MeshRenderer>();
        Bounds bounds;
        if (selectedMeshRenderer)
        {
            bounds = new Bounds(selectedMeshRenderer.bounds.center, selectedMeshRenderer.bounds.size);

            foreach (Renderer r in selectedTransform.GetComponentsInChildren<Renderer>())
            {
                Bounds tem_R_bounds = r.bounds;
                tem_R_bounds.size = new Vector3(tem_R_bounds.size.x, tem_R_bounds.size.y, tem_R_bounds.size.z);
                bounds.Encapsulate(tem_R_bounds);
            }


            /*Debug.Log("Mesh Center " + selectedMeshRenderer.bounds.center);
            Debug.Log("Transform Center " + selectedTransform.position);*/
            //bounds.center = selectedTransform.position;





            Bounds temBounds = bounds;

            temBounds.center = this.Recenter(temBounds.center, _cellSize / 2);
            temBounds.center = new Vector3(temBounds.center.x, 0, temBounds.center.z);

            /*temBounds.size = this.RoundOff(temBounds.size, temBounds.center, cellSize);
            temBounds.size = new Vector3(temBounds.size.x, 0, temBounds.size.z);*/



            if(_gridSize%2 != 0)
                temBounds.size = this.RoundOff(temBounds.size, temBounds.center, _cellSize, 1);
            else
                temBounds.size = this.RoundOff(temBounds.size, temBounds.center, _cellSize, 2);

            temBounds.size = new Vector3(temBounds.size.x , 0, temBounds.size.z);

            /*Vector3 temCenter = bounds.center;*/


            /* Debug.Log("bounds.center " + bounds.center);
             Debug.Log("temCenter " + temCenter);
             Debug.Log("cell size " + cellSize / 2);*/


            Color temColor = Color.green;
            temColor.a = 0.05f;
            this.DrawSolidCube(temBounds.center, temBounds.size, temColor);
            /*Gizmos.color = temColor;*/
            /*Handles.color = temColor;*/


            //Gizmos.DrawCube(temBounds.center, temBounds.size);

            /*Gizmos.color = new Color(1, 1, 1, 0.4f);
            Gizmos.DrawCube(bounds.center, new Vector3(bounds.size.x, 0, bounds.size.z));

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(bounds.center, 0.2f);


            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(temBounds.center, 0.2f);*/
        }

    }

    Vector3 RoundOff(Vector3 _vector, Vector3 _center, float roundOffTo, int multiple)
    {
        /*Debug.Log("Cell size" + roundOffTo);

        Debug.Log(_vector.z % roundOffTo);

        Debug.Log("Before " + _vector);

        Debug.Log(_center);

        _vector.x += roundOffTo - (_vector.x % roundOffTo);

        _vector.y += roundOffTo - (_vector.y % roundOffTo);*/

        if (_center.x % roundOffTo != 0)
        {
            _vector.x += (roundOffTo * multiple) - (_vector.x % (roundOffTo * multiple));
        }
        else
        {
            if (_vector.x % (roundOffTo * multiple) > roundOffTo)
                _vector.x += (roundOffTo - (_vector.x % roundOffTo)) + roundOffTo;
            else
                _vector.x += roundOffTo - (_vector.x % roundOffTo);
        }


        if (_center.y % roundOffTo != 0)
        {
            _vector.y += (roundOffTo * multiple) - (_vector.y % (roundOffTo * multiple));
        }
        else
        {
            if (_vector.y % (roundOffTo * multiple) > roundOffTo)
                _vector.y += (roundOffTo - (_vector.y % roundOffTo)) + roundOffTo;
            else
                _vector.y += roundOffTo - (_vector.y % roundOffTo);
        }


        if (_center.z % roundOffTo != 0)
        {
            _vector.z += (roundOffTo * multiple) - (_vector.z % (roundOffTo * multiple));
        }
        else
        {
            if (_vector.z % (roundOffTo * multiple) > roundOffTo)
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

    
    void DrawSolidCube(Vector3 _center, Vector3 _size, Color _color)
    {
        /*_size.x = Mathf.Abs(_size.x);
        _size.y = Mathf.Abs(_size.y);
        _size.z = Mathf.Abs(_size.z);*/

        if (_size.y != 0 || (_size.x != 0 && _size.z != 0))
        {
            Vector3[] verts = new Vector3[]
            {
                new Vector3(_center.x - _size.x/2, _center.y + _size.y/2 , _center.z - _size.z/2),
                new Vector3(_center.x - _size.x/2, _center.y + _size.y/2 , _center.z + _size.z/2),
                new Vector3(_center.x + _size.x/2, _center.y + _size.y/2 , _center.z + _size.z/2),
                new Vector3(_center.x + _size.x/2, _center.y + _size.y/2 , _center.z - _size.z/2),
            };

            Handles.DrawSolidRectangleWithOutline(verts, _color, _color);

            verts = new Vector3[]
            {
                new Vector3(_center.x - _size.x/2, _center.y - _size.y/2 , _center.z - _size.z/2),
                new Vector3(_center.x - _size.x/2, _center.y - _size.y/2 , _center.z + _size.z/2),
                new Vector3(_center.x + _size.x/2, _center.y - _size.y/2 , _center.z + _size.z/2),
                new Vector3(_center.x + _size.x/2, _center.y - _size.y/2 , _center.z - _size.z/2)
            };

            Handles.DrawSolidRectangleWithOutline(verts, _color, _color);
        }
        else
        {
            Vector3[] verts = new Vector3[]
            {
                new Vector3(_center.x - _size.x/2, _center.y, _center.z - _size.z/2),
                new Vector3(_center.x - _size.x/2, _center.y, _center.z + _size.z/2),
                new Vector3(_center.x + _size.x/2, _center.y, _center.z + _size.z/2),
                new Vector3(_center.x + _size.x/2, _center.y, _center.z - _size.z/2),
            };

            Handles.DrawSolidRectangleWithOutline(verts, _color, _color);

            return;
        }


        if (_size.x != 0 || (_size.y != 0 && _size.z != 0))
        {
            Vector3[] verts = new Vector3[]
            {
                new Vector3(_center.x + _size.x/2, _center.y - _size.y/2 , _center.z - _size.z/2),
                new Vector3(_center.x + _size.x/2, _center.y - _size.y/2 , _center.z + _size.z/2),
                new Vector3(_center.x + _size.x/2, _center.y + _size.y/2 , _center.z + _size.z/2),
                new Vector3(_center.x + _size.x/2, _center.y + _size.y/2 , _center.z - _size.z/2),
            };

            Handles.DrawSolidRectangleWithOutline(verts, _color, _color);

            verts = new Vector3[]
            {
                new Vector3(_center.x - _size.x/2, _center.y - _size.y/2 , _center.z - _size.z/2),
                new Vector3(_center.x - _size.x/2, _center.y - _size.y/2 , _center.z + _size.z/2),
                new Vector3(_center.x - _size.x/2, _center.y + _size.y/2 , _center.z + _size.z/2),
                new Vector3(_center.x - _size.x/2, _center.y + _size.y/2 , _center.z - _size.z/2)
            };

            Handles.DrawSolidRectangleWithOutline(verts, _color, _color);
        }
        else
        {
            Vector3[] verts = new Vector3[]
            {
                new Vector3(_center.x, _center.y - _size.y/2, _center.z - _size.z/2),
                new Vector3(_center.x, _center.y - _size.y/2, _center.z + _size.z/2),
                new Vector3(_center.x, _center.y + _size.y/2, _center.z + _size.z/2),
                new Vector3(_center.x, _center.y + _size.y/2, _center.z - _size.z/2),
            };

            Handles.DrawSolidRectangleWithOutline(verts, _color, _color);

            return;
        }



        if (_size.z != 0 || (_size.x != 0 && _size.y != 0))
        {
            Vector3[] verts = new Vector3[]
            {
                new Vector3(_center.x - _size.x/2, _center.y - _size.y/2 , _center.z + _size.z/2),
                new Vector3(_center.x + _size.x/2, _center.y - _size.y/2 , _center.z + _size.z/2),
                new Vector3(_center.x + _size.x/2, _center.y + _size.y/2 , _center.z + _size.z/2),
                new Vector3(_center.x - _size.x/2, _center.y + _size.y/2 , _center.z + _size.z/2),
            };

            Handles.DrawSolidRectangleWithOutline(verts, _color, _color);

            verts = new Vector3[]
            {
                new Vector3(_center.x - _size.x/2, _center.y - _size.y/2 , _center.z - _size.z/2),
                new Vector3(_center.x + _size.x/2, _center.y - _size.y/2 , _center.z - _size.z/2),
                new Vector3(_center.x + _size.x/2, _center.y + _size.y/2 , _center.z - _size.z/2),
                new Vector3(_center.x - _size.x/2, _center.y + _size.y/2 , _center.z - _size.z/2)
            };

            Handles.DrawSolidRectangleWithOutline(verts, _color, _color);
        }
        else
        {
            Vector3[] verts = new Vector3[]
            {
                new Vector3(_center.x, _center.y - _size.y/2, _center.z - _size.z/2),
                new Vector3(_center.x, _center.y - _size.y/2, _center.z + _size.z/2),
                new Vector3(_center.x, _center.y + _size.y/2, _center.z + _size.z/2),
                new Vector3(_center.x, _center.y + _size.y/2, _center.z - _size.z/2),
            };

            Handles.DrawSolidRectangleWithOutline(verts, _color, _color);

            return;
        }
    }
}
