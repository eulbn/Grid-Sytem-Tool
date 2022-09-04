using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace GridSpace
{

    public class GridSnapSystem : EditorWindow
    {
        enum GridOrientation
        {
            xy, yz, zx
        }

        static GridOrientation gridOrientation = new GridOrientation();
        static Color gridColor = Color.green;
        static Vector3Int gridSize = new Vector3Int(10, 10, 10);
        static Vector3 gridCellSize = Vector3.one;


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
            gridSize = EditorGUILayout.Vector3IntField("Grid Size", gridSize);
            gridCellSize = EditorGUILayout.Vector3Field("Grid Cell Size", gridCellSize);
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

        #region Logic
        //-----------------------------------Grid Snaping-------------------------------------------
        private Vector3 SnaptoGrid(Vector3 _vector)
        {
            Vector3 roundedOffVector = Vector3.zero;


            float temOffsetX = gridCellSize.x / 2;
            if (GridSnapSystem.gridSize.x % 2 == 0)
            {
                roundedOffVector.x = Mathf.Round(_vector.x / gridCellSize.x) * gridCellSize.x;
            }
            else
            {
                roundedOffVector.x = (Mathf.Round((_vector.x - temOffsetX) / gridCellSize.x) * gridCellSize.x) + temOffsetX;
            }


            float temOffsetY = gridCellSize.y / 2;
            if (GridSnapSystem.gridSize.y % 2 == 0)
            {
                roundedOffVector.y = Mathf.Round(_vector.y / gridCellSize.y) * gridCellSize.y;
            }
            else
            { 
                roundedOffVector.y = (Mathf.Round((_vector.y - temOffsetY) / gridCellSize.y) * gridCellSize.y) + temOffsetY;
            }


            float temOffsetZ = gridCellSize.z / 2;
            if (GridSnapSystem.gridSize.z % 2 == 0)
            {
                roundedOffVector.z = Mathf.Round(_vector.z / gridCellSize.z) * gridCellSize.z;
            }
            else
            {
                roundedOffVector.z = (Mathf.Round((_vector.z - temOffsetZ) / gridCellSize.z) * gridCellSize.z) + temOffsetZ;
            }


            if (gridOrientation == GridOrientation.xy)
                roundedOffVector.z = _vector.z;
            else if (gridOrientation == GridOrientation.yz)
                roundedOffVector.x = _vector.x;
            else if (gridOrientation == GridOrientation.zx)
                roundedOffVector.y = _vector.y;


            return roundedOffVector;

        }
        //-------------------------------------------------------------------------------------------
        #endregion



        #region Drawing Grid
        //------------------------------------Draw Grid----------------------------------------------
        void SetGrid()
        {
            this.DrawGrid2d(0);


            /*if (gridOrientation == GridOrientation.xyz)
            {
                this.DrawGrid3d();
            }
            else
            {
                this.DrawGrid2d(0);
            }*/

        }
        /*void DrawGrid3d()
        {
            float actualSize = gridSize * gridCellSize;
            float thirdAxisOffset = (-actualSize / 2) + (gridCellSize / 2);

            for (int i = 0; i < gridSize; i++)
            {
                this.DrawGrid2d(thirdAxisOffset);
                this.DrawGrid2d(thirdAxisOffset);
                this.DrawGrid2d(thirdAxisOffset);


                thirdAxisOffset += gridCellSize;
            }
        }*/
        void DrawGrid2d(float thirdAxisOffset)
        {
            Handles.color = gridColor;
            float actualSizeA = 0, actualSizeB = 0;
            int gridSizeA = 0, gridSizeB = 0;
            if(gridOrientation == GridOrientation.xy)
            {
                actualSizeA = gridSize.x * gridCellSize.x;
                actualSizeB = gridSize.y * gridCellSize.y;

                gridSizeA = gridSize.x;
                gridSizeB = gridSize.y;
            }
            else if (gridOrientation == GridOrientation.yz)
            {
                actualSizeA = gridSize.y * gridCellSize.y;
                actualSizeB = gridSize.z * gridCellSize.z;

                gridSizeA = gridSize.y;
                gridSizeB = gridSize.z;
            }
            else if(gridOrientation == GridOrientation.zx)
            {
                actualSizeA = gridSize.z * gridCellSize.z;
                actualSizeB = gridSize.x * gridCellSize.x;

                gridSizeA = gridSize.z;
                gridSizeB = gridSize.x;
            }
            /*float temOffsetA = (-actualSizeA / 2) + (gridCellSize / 2);

            float temOffsetB = (-actualSizeB / 2) + (gridCellSize / 2);*/

            Vector3[] temLineOrigin = this.SetLineOrigin(actualSizeA, actualSizeB, thirdAxisOffset);

            Vector3 line1_Start = temLineOrigin[0];
            Vector3 line1_End = temLineOrigin[1];

            Vector3 line2_Start = temLineOrigin[2];
            Vector3 line2_End = temLineOrigin[3];

            for (int i = 0; i < gridSizeA || i < gridSizeB; i++)
            {
                /*Handles.DrawAAPolyLine(80 / gridSize, line1_Start, line1_End);
                Handles.DrawAAPolyLine(80 / gridSize, line2_Start, line2_End);*/

                
                if (i < gridSizeB)
                    Handles.DrawPolyLine(line1_Start, line1_End);

                if (i < gridSizeA)
                    Handles.DrawPolyLine(line2_Start, line2_End);

                line1_Start = this.Line1Increment(line1_Start);
                line1_End = this.Line1Increment(line1_End);

                line2_Start = this.Line2Increment(line2_Start);
                line2_End = this.Line2Increment(line2_End);

            }
        }
        Vector3[] SetLineOrigin(float actualSizeA, float actualSizeB, float thirdAxisOffset)
        {
            if (gridOrientation == GridOrientation.xy)
            {
                float temOffsetA = (-actualSizeA / 2) + (gridCellSize.x / 2);
                float temOffsetB = (-actualSizeB / 2) + (gridCellSize.y / 2);

                return (new Vector3[]{
                new Vector3(+temOffsetA, temOffsetB, thirdAxisOffset),
                new Vector3(-temOffsetA, temOffsetB, thirdAxisOffset),

                new Vector3(temOffsetA, +temOffsetB, thirdAxisOffset),
                new Vector3(temOffsetA, -temOffsetB, thirdAxisOffset),
                });
            }
            else if (gridOrientation == GridOrientation.yz)
            {
                float temOffsetA = (-actualSizeA / 2) + (gridCellSize.y / 2);
                float temOffsetB = (-actualSizeB / 2) + (gridCellSize.z / 2);

                return (new Vector3[]{
                new Vector3(thirdAxisOffset, +temOffsetA, temOffsetB),
                new Vector3(thirdAxisOffset, -temOffsetA, temOffsetB),

                new Vector3(thirdAxisOffset, temOffsetA, +temOffsetB),
                new Vector3(thirdAxisOffset, temOffsetA, -temOffsetB),
                });
            }
            else
            {
                float temOffsetA = (-actualSizeA / 2) + (gridCellSize.z / 2);
                float temOffsetB = (-actualSizeB / 2) + (gridCellSize.x / 2);

                return (new Vector3[]{
                new Vector3(+temOffsetB, thirdAxisOffset, +temOffsetA),
                new Vector3(+temOffsetB, thirdAxisOffset, -temOffsetA),

                new Vector3(+temOffsetB, thirdAxisOffset, +temOffsetA),
                new Vector3(-temOffsetB, thirdAxisOffset, +temOffsetA),
                });
            }
            /*else
            {
                return (new Vector3[]{
                new Vector3(temOffset, thirdAxisOffset, temOffset),
                new Vector3(-temOffset,thirdAxisOffset, temOffset),

                new Vector3(temOffset, thirdAxisOffset, temOffset),
                new Vector3(temOffset, thirdAxisOffset, -temOffset),
                });
            }*/
        }
        Vector3 Line1Increment(Vector3 _vector)
        {
            if (gridOrientation == GridOrientation.xy)
            {
                _vector.y += gridCellSize.y;
                return _vector;
            }
            else if (gridOrientation == GridOrientation.yz)
            {
                _vector.z += gridCellSize.z;
                return _vector;
            }
            else if (gridOrientation == GridOrientation.zx)
            {
                _vector.x += gridCellSize.x;
                return _vector;
            }
            else
                return _vector;
        }
        Vector3 Line2Increment(Vector3 _vector)
        {
            if (gridOrientation == GridOrientation.xy)
            {
                _vector.x += gridCellSize.x;
                return _vector;
            }
            else if (gridOrientation == GridOrientation.yz)
            {
                _vector.y += gridCellSize.y;
                return _vector;
            }
            else if (gridOrientation == GridOrientation.zx)
            {
                _vector.z += gridCellSize.z;
                return _vector;
            }
            else
                return _vector;
        }
        //-------------------------------------------------------------------------------------------


        #endregion




        #region Drawing Heighlights for the selected object
        //-----------------------------------Drawing Highlights-------------------------------------------
        void Highlights(Transform selectedTransform)
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

            if (gridOrientation == GridOrientation.xy)
                temBounds.center = new Vector3(temBounds.center.x, temBounds.center.y, 0);
            else if (gridOrientation == GridOrientation.yz)
                temBounds.center = new Vector3(0, temBounds.center.y, temBounds.center.z);
            else if (gridOrientation == GridOrientation.zx)
                temBounds.center = new Vector3(temBounds.center.x, 0, temBounds.center.z);
            else if (gridOrientation == GridOrientation.zx)
                temBounds.center = new Vector3(temBounds.center.x, temBounds.center.y, temBounds.center.z);


            temBounds.size = this.RoundOff(temBounds.size, temBounds.center, gridCellSize);


            if (gridOrientation == GridOrientation.xy)
                temBounds.size = new Vector3(temBounds.size.x, temBounds.size.y, 0);
            else if (gridOrientation == GridOrientation.yz)
                temBounds.size = new Vector3(0, temBounds.size.y, temBounds.size.z);
            else if (gridOrientation == GridOrientation.zx)
                temBounds.size = new Vector3(temBounds.size.x, 0, temBounds.size.z);

            Color temColor = Color.green;
            temColor.a = 0.05f;
            this.DrawSolidCube(temBounds.center, temBounds.size, temColor);

        }

        Vector3 RoundOff(Vector3 _vector, Vector3 _center, Vector3 roundOffTo)
        {
            int multiple;
            {
                if (gridSize.x % 2 != 0)
                {
                    multiple = 1;
                }
                else
                {
                    multiple = 2;
                }

                if (_center.x % roundOffTo.x != 0)
                {
                    _vector.x += (roundOffTo.x * multiple) - (_vector.x % (roundOffTo.x * multiple));
                }
                else
                {
                    if (_vector.x % (roundOffTo.x * multiple) >= roundOffTo.x)
                        _vector.x += (roundOffTo.x - (_vector.x % roundOffTo.x)) + roundOffTo.x;
                    else
                        _vector.x += roundOffTo.x - (_vector.x % roundOffTo.x);
                }
            }

            {
                if (gridSize.y % 2 != 0)
                {
                    multiple = 1;
                }
                else
                {
                    multiple = 2;
                }

                if (_center.y % roundOffTo.y != 0)
                {
                    _vector.y += (roundOffTo.y * multiple) - (_vector.y % (roundOffTo.y * multiple));
                }
                else
                {
                    if (_vector.y % (roundOffTo.y * multiple) >= roundOffTo.y)
                        _vector.y += (roundOffTo.y - (_vector.y % roundOffTo.y)) + roundOffTo.y;
                    else
                        _vector.y += roundOffTo.y - (_vector.y % roundOffTo.y);
                }
            }

            {
                if (gridSize.z % 2 != 0)
                {
                    multiple = 1;
                }
                else
                {
                    multiple = 2;
                }

                if (_center.z % roundOffTo.z != 0)
                {
                    _vector.z += (roundOffTo.z * multiple) - (_vector.z % (roundOffTo.z * multiple));
                }
                else
                {
                    if (_vector.z % (roundOffTo.z * multiple) >= roundOffTo.z)
                        _vector.z += (roundOffTo.z - (_vector.z % roundOffTo.z)) + roundOffTo.z;
                    else
                        _vector.z += roundOffTo.z - (_vector.z % roundOffTo.z);
                }
            }

            return _vector;
        }

        Vector3 Recenter(Vector3 _vector, Vector3 roundOffTo)
        {

            if (Mathf.Abs(_vector.x) % roundOffTo.x >= roundOffTo.x / 2)
            {
                if (_vector.x <= 0)
                    _vector.x += -roundOffTo.x - (_vector.x % roundOffTo.x);
                else
                    _vector.x += roundOffTo.x - (_vector.x % roundOffTo.x);
            }
            else
                _vector.x -= _vector.x % roundOffTo.x;


            if (Mathf.Abs(_vector.y) % roundOffTo.y > roundOffTo.y / 2)
            {
                if (_vector.y < 0)
                    _vector.y += -roundOffTo.y - (_vector.y % roundOffTo.y);
                else
                    _vector.y += roundOffTo.y - (_vector.y % roundOffTo.y);
            }
            else
                _vector.y -= _vector.y % roundOffTo.y;


            if (Mathf.Abs(_vector.z) % roundOffTo.z > roundOffTo.z / 2)
            {
                if (_vector.z < 0)
                    _vector.z += -roundOffTo.z - (_vector.z % roundOffTo.z);
                else
                    _vector.z += roundOffTo.z - (_vector.z % roundOffTo.z);
            }
            else
                _vector.z -= _vector.z % roundOffTo.z;


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
            _size.x = Mathf.Abs(_size.x);
            _size.y = Mathf.Abs(_size.y);
            _size.z = Mathf.Abs(_size.z);

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








        #region Access

        static public Vector2Int GetGridSize()
        {
            if(gridOrientation == GridOrientation.xy)
            {
                return new Vector2Int(gridSize.x, gridSize.y);
            }
            else if (gridOrientation == GridOrientation.yz)
            {
                return new Vector2Int(gridSize.z, gridSize.y);
            }
            else
            {
                return new Vector2Int(gridSize.z, gridSize.x);
            }
        }

        #endregion



    }
}