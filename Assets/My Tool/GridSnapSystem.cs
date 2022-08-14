using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;



public class GridSnapSystem : EditorWindow
{
    enum GridOrientation
    {
        xy, yz, zx, xyz
    }

    static GridOrientation gridOrientation = new GridOrientation();
    static Color gridColor = Color.green;
    static int gridSize = 10;
    static float gridCellSize = 5;


    private List<Transform> trackOfSelectedTransforms;
    private List<Vector3> trackOfSelectedTransformsPosition;

    

    [MenuItem("Window/Grid Snap System")]
    static void Init()
    {
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
        this.trackOfSelectedTransforms = new List<Transform>();
        this.trackOfSelectedTransformsPosition = new List<Vector3>();
        SceneView.duringSceneGui += this.OnSceneGUI;
    }

    void OnDisable()
    {
        this.trackOfSelectedTransforms.Clear();
        this.trackOfSelectedTransformsPosition.Clear();
        this.trackOfSelectedTransforms = null;
        this.trackOfSelectedTransformsPosition = null;
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }


    void OnSceneGUI(SceneView sceneView)
    {
        this.SetGrid();

        foreach (var _transfrom in Selection.transforms)
        {
            if (this.hasSelectedPositionChanged() && this.RefernceCheck())
            {
                _transfrom.position = this.SnaptoGrid(_transfrom.position);
            }
            this.Highlights(_transfrom);
        }
    }

    private void OnSelectionChange()
    {
        this.trackOfSelectedTransforms.Clear();
        this.trackOfSelectedTransformsPosition.Clear();
        foreach (Transform _transform in Selection.transforms)
        {
            this.trackOfSelectedTransforms.Add(_transform);
            this.trackOfSelectedTransformsPosition.Add(_transform.position);
        }
    }


    bool hasSelectedPositionChanged()
    {
        List<Vector3> temSelectedTransfroms = new List<Vector3>();
        foreach (Transform _transform in Selection.transforms)
        {
            temSelectedTransfroms.Add(_transform.position);
        }

        if (temSelectedTransfroms.Count != this.trackOfSelectedTransformsPosition.Count)
        {
            return true;
        }

        for (int i = 0; i < temSelectedTransfroms.Count; i++)
        {
            if (!temSelectedTransfroms[i].Equals(this.trackOfSelectedTransformsPosition[i]))
            {
                return true;
            }
        }

        return false;
    }

    bool RefernceCheck()
    {
        List<Transform> temSelectedTransfroms = new List<Transform>();
        foreach (Transform _transform in Selection.transforms)
        {
            temSelectedTransfroms.Add(_transform);
        }

        if (temSelectedTransfroms.Count != this.trackOfSelectedTransforms.Count)
            return false;

        for (int i = 0; i < temSelectedTransfroms.Count; i++)
        {
            if (!temSelectedTransfroms[i].Equals(this.trackOfSelectedTransforms[i]))
                return false;
        }

        return true;
    }


    //-----------------------------------Grid Snaping-------------------------------------------
    private Vector3 SnaptoGrid(Vector3 _vector)
    {
        if(GridSnapSystem.gridSize% 2 == 0)
        {
             
            Vector3 roundedOffVector = new Vector3(
                Mathf.Round(_vector.x / gridCellSize) * gridCellSize,
                Mathf.Round(_vector.y / gridCellSize) * gridCellSize,
                Mathf.Round(_vector.z / gridCellSize) * gridCellSize
            );

            if (gridOrientation == GridOrientation.xy)
                roundedOffVector.z = _vector.z;
            else if (gridOrientation == GridOrientation.yz)
                roundedOffVector.x = _vector.x;
            else if (gridOrientation == GridOrientation.zx)
                roundedOffVector.y = _vector.y;

            return roundedOffVector;

        }
        else
        {
            float temOffset = gridCellSize / 2;
            Vector3 roundedOffVector = new Vector3(
                (Mathf.Round((_vector.x - temOffset) / gridCellSize) * gridCellSize) + temOffset,
                (Mathf.Round((_vector.y - temOffset) / gridCellSize) * gridCellSize) + temOffset,
                (Mathf.Round((_vector.z - temOffset) / gridCellSize) * gridCellSize) + temOffset
            );

            if (gridOrientation == GridOrientation.xy)
                roundedOffVector.z = _vector.z;
            else if (gridOrientation == GridOrientation.yz)
                roundedOffVector.x = _vector.x;
            else if (gridOrientation == GridOrientation.zx)
                roundedOffVector.y = _vector.y;

            return roundedOffVector;
        }
        
    }
    //-------------------------------------------------------------------------------------------




    #region Drawing Grid Here
    //------------------------------------Draw Grid----------------------------------------------
    void SetGrid()
    {
        if(gridOrientation == GridOrientation.xyz)
        {
            this.DrawGrid3d();
        }
        else
        {
            this.DrawGrid2d(0);
        }

    }
    void DrawGrid3d()
    {
        float actualSize = gridSize * gridCellSize;
        float thirdAxisOffset = (-actualSize / 2) + (gridCellSize / 2);

        for (int i = 0; i < gridSize; i++)
        {
            this.DrawGrid2d( thirdAxisOffset);
            this.DrawGrid2d( thirdAxisOffset);
            this.DrawGrid2d( thirdAxisOffset);


            thirdAxisOffset += gridCellSize;
        }
    }
    void DrawGrid2d(float thirdAxisOffset)
    {
        Handles.color = gridColor;

        /*if (gridSize % 2 != 0)
            gridSize++;*/

        float actualSize = gridSize * gridCellSize;
        float temOffset = (-actualSize / 2) + (gridCellSize/ 2);

        Vector3[] temLineOrigin = this.SetLineOrigin(temOffset, thirdAxisOffset);

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


            line1_Start = this.Line1Increment(line1_Start);
            line1_End = this.Line1Increment(line1_End);

            line2_Start = this.Line2Increment(line2_Start);
            line2_End = this.Line2Increment(line2_End);
        }
    }
    Vector3[] SetLineOrigin(float temOffset, float thirdAxisOffset)
    {
        if (gridOrientation == GridOrientation.xy)
        {
            return (new Vector3[]{
                new Vector3(+temOffset, temOffset, thirdAxisOffset),
                new Vector3(-temOffset, temOffset, thirdAxisOffset),

                new Vector3(temOffset, +temOffset, thirdAxisOffset),
                new Vector3(temOffset, -temOffset, thirdAxisOffset),
            });
        }
        else if (gridOrientation == GridOrientation.yz)
        {
            return (new Vector3[]{
                new Vector3(thirdAxisOffset, +temOffset, temOffset),
                new Vector3(thirdAxisOffset, -temOffset, temOffset),
                            
                new Vector3(thirdAxisOffset, temOffset, +temOffset),
                new Vector3(thirdAxisOffset, temOffset, -temOffset),
            });
        }
        else if (gridOrientation == GridOrientation.zx)
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
    Vector3 Line1Increment(Vector3 _vector)
    {
        if (gridOrientation == GridOrientation.xy)
        {
            _vector.y += gridCellSize;
            return _vector;
        }
        else if (gridOrientation == GridOrientation.yz)
        {
            _vector.z += gridCellSize;
            return _vector;
        }
        else if (gridOrientation == GridOrientation.zx)
        {
            _vector.x += gridCellSize;
            return _vector;
        }
        else
            return _vector;
    }
    Vector3 Line2Increment(Vector3 _vector)
    {
        if (gridOrientation == GridOrientation.xy)
        {
            _vector.x += gridCellSize;
            return _vector;
        }
        else if (gridOrientation == GridOrientation.yz)
        {
            _vector.y += gridCellSize;
            return _vector;
        }
        else if (gridOrientation == GridOrientation.zx)
        {
            _vector.z += gridCellSize;
            return _vector;
        }
        else
            return _vector;
    }
    //-------------------------------------------------------------------------------------------


    #endregion




    #region Drawing Heighlights for the selected object
    //-----------------------------------Drawing Highlights-------------------------------------------
    void Highlights(/*float _cellSize, float _gridSize,*/ Transform selectedTransform/*, GridOrientation _gridOrientation*/)
    {
        Bounds bounds;

        if (selectedTransform.GetComponent<Renderer>())
            bounds = new Bounds(selectedTransform.GetComponent<Renderer>().bounds.center, selectedTransform.GetComponent<Renderer>().bounds.size);
        else
            bounds = new Bounds(selectedTransform.position, selectedTransform.position);


        foreach (Renderer r in selectedTransform.GetComponentsInChildren<Renderer>())
        {
            Bounds tem_R_bounds = r.bounds;
            tem_R_bounds.size = new Vector3(tem_R_bounds.size.x, tem_R_bounds.size.y, tem_R_bounds.size.z);
            bounds.Encapsulate(tem_R_bounds);
        }

        Bounds temBounds = bounds;

        temBounds.center = this.Recenter(temBounds.center, gridCellSize / 2);

        if(gridOrientation == GridOrientation.xy)
            temBounds.center = new Vector3(temBounds.center.x, temBounds.center.y, 0);
        else if(gridOrientation == GridOrientation.yz)
            temBounds.center = new Vector3(0, temBounds.center.y, temBounds.center.z);
        else if (gridOrientation == GridOrientation.zx)
            temBounds.center = new Vector3(temBounds.center.x, 0, temBounds.center.z);
        else if (gridOrientation == GridOrientation.zx)
            temBounds.center = new Vector3(temBounds.center.x, temBounds.center.y, temBounds.center.z);


        if (gridSize%2 != 0)
            temBounds.size = this.RoundOff(temBounds.size, temBounds.center, gridCellSize, 1);
        else
            temBounds.size = this.RoundOff(temBounds.size, temBounds.center, gridCellSize, 2);


        if(gridOrientation == GridOrientation.xy)
            temBounds.size = new Vector3(temBounds.size.x , temBounds.size.y, 0);
        else if (gridOrientation == GridOrientation.yz)
            temBounds.size = new Vector3(0, temBounds.size.y, temBounds.size.z);
        else if (gridOrientation == GridOrientation.zx)
            temBounds.size = new Vector3(temBounds.size.x, 0, temBounds.size.z);
        else if (gridOrientation == GridOrientation.xyz)
            temBounds.size = new Vector3(temBounds.size.x, temBounds.size.y, temBounds.size.z);

        Color temColor = Color.green;
        temColor.a = 0.05f;
        this.DrawSolidCube(temBounds.center, temBounds.size, temColor);
        /*}*/

    }

    Vector3 RoundOff(Vector3 _vector, Vector3 _center, float roundOffTo, int multiple)
    {
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
    //-------------------------------------------------------------------------------------------
    #endregion
}
