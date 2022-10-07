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

        static Vector3Int gridSize = new Vector3Int(10, 10, 10);
        static Vector3 gridCellSize = Vector3.one;
        static Vector3 gridCenter = Vector3.zero;
        static GridOrientation gridOrientation = new GridOrientation();
        static Color gridColor = Color.green;
        static ObjectKind objectKind = new ObjectKind();
        static int objectKindLayer = 0;
<<<<<<< Updated upstream
        static string objectKindTag = "Untagged";

=======
        static int objectKindLayerCopy = 0;
        static string objectKindTag = "Untagged";
        static string objectKindTagCopy = "Untagged";
        
        private List<GameObject> gridObjects = null;
>>>>>>> Stashed changes

        private List<Transform> trackOfSelectedTransforms;
        private List<Vector3> trackOfSelectedTransformsPosition;
        private const float precisionLimit = 0.1f;
        private Vector2 scrollPosition = Vector2.zero;

        private List<int> someList = Enumerable.Range(0, 1000).ToList();
        private const string resourcesPath = "GridSnapSystem/";
        private GUIStyle assignedButtonStyle;
        private Texture2D normalButtonTexture;
        private Texture2D hoverButtonTexture;
        private Texture2D activeButtonTexture;
        private bool hasAssignedTexture = false;


        private GUIStyle assignedDeleteButtonStyle;
        private Texture2D normalDeleteButtonTexture;
        private Texture2D hoverDeleteButtonTexture;
        private Texture2D activeDeleteButtonTexture;
        private bool hasAssignedDeleteTexture = false;

        [MenuItem("Window/Grid Snap System")]
        static void Init()
        {
            GridSnapSystem window = (GridSnapSystem)GetWindow(typeof(GridSnapSystem));

            window.Show();
        }

        private void OnGUI()
        {
<<<<<<< Updated upstream
            GUILayout.Label("Base Settings", EditorStyles.largeLabel);
            gridSize = EditorGUILayout.Vector3IntField("Grid Size", gridSize);
            gridCellSize = EditorGUILayout.Vector3Field("Grid Cell Size", gridCellSize);
            gridCenter = EditorGUILayout.Vector3Field("Grid Center", gridCenter);
            gridColor = EditorGUILayout.ColorField("Grid Color", gridColor);
            gridOrientation = (GridOrientation)EditorGUILayout.Popup("Orientation", (int)gridOrientation, System.Enum.GetNames(typeof(GridOrientation)));
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            
            objectKind = (ObjectKind)EditorGUILayout.Popup("Object Kind", (int)objectKind, System.Enum.GetNames(typeof(ObjectKind)));
            Rect objectKindRect = EditorGUILayout.GetControlRect();

            if(objectKind == ObjectKind.ObjectTag)
            {
                objectKindTag = EditorGUI.TagField(objectKindRect, "Object Tag", objectKindTag);
            }
            else if (objectKind == ObjectKind.ObjectLayer)
            {
                objectKindLayer = EditorGUI.LayerField(objectKindRect, "Object Layer", objectKindLayer);
            }
            
            Rect assignButtonRect = new Rect(EditorGUILayout.GetControlRect().width/2 - 100 + 1, EditorGUILayout.GetControlRect().y - 1, 200, 24);
            if(GUI.Button(assignButtonRect, "Assign Objects"))
            {

            }

          

            EditorGUILayout.BeginHorizontal(GUILayout.Height(182));

            Rect rectPosition = EditorGUILayout.GetControlRect();
            Rect rectBox = new Rect(rectPosition.x, rectPosition.y + 18, rectPosition.width, 182);

            EditorGUI.DrawRect(rectBox, Color.black);

            Rect viewRect = new Rect(rectBox.x, rectBox.y, rectBox.width, someList.Count * rectPosition.height);
            scrollPosition = GUI.BeginScrollView(rectBox, scrollPosition, viewRect, false, true, GUIStyle.none, GUI.skin.verticalScrollbar);

            rectPosition.height += 2;
            int viewCount = 18;
            int firstIndex = (int)(scrollPosition.y / rectPosition.height);
            Rect assignedButtonPosition = new Rect(rectBox.x, rectBox.y + (firstIndex * rectPosition.height + 2), rectBox.width * 0.65f, 18);
            Rect assignedButtonDeletePosition = new Rect(rectBox.x + (float)(rectBox.width * 0.65), rectBox.y + (firstIndex * rectPosition.height + 2), rectBox.width * 0.35f, 18);

            this.SetAssignedButtonTexture();
            this.SetAssignedDeleteButtonTexture();

            for (int i = firstIndex; i < Mathf.Min(someList.Count, firstIndex + viewCount); i++)
            {
                GUI.Button(assignedButtonPosition, "  >   " + someList[i].ToString(), assignedButtonStyle);
                GUI.Button(assignedButtonDeletePosition, "Delete", assignedDeleteButtonStyle);
                assignedButtonPosition.y += rectPosition.height;
                assignedButtonDeletePosition.y += rectPosition.height;
            }
            GUI.EndScrollView();

            EditorGUILayout.EndHorizontal();
            
=======

            //-------------------------Grid Variables--------------------------------
            GUILayout.Label("Base Settings", EditorStyles.largeLabel);
            
            gridSize = EditorGUILayout.Vector3IntField("Grid Size", gridSize);
            gridCellSize = EditorGUILayout.Vector3Field("Grid Cell Size", gridCellSize);
            gridCenter = EditorGUILayout.Vector3Field("Grid Center", gridCenter);
            gridColor = EditorGUILayout.ColorField("Grid Color", gridColor);
            gridOrientation = (GridOrientation)EditorGUILayout.Popup("Orientation", (int)gridOrientation, System.Enum.GetNames(typeof(GridOrientation)));
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            //-----------------------------------------------------------------------


            //-------------------------Object assign variables-----------------------
            objectKind = (ObjectKind)EditorGUILayout.Popup("Object Kind", (int)objectKind, System.Enum.GetNames(typeof(ObjectKind)));
            if(objectKind == ObjectKind.ObjectTag)
            {
                objectKindTag = EditorGUI.TagField(EditorGUILayout.GetControlRect(), "Object Tag", objectKindTag);
            }
            else if (objectKind == ObjectKind.ObjectLayer)
            {
                objectKindLayer = EditorGUI.LayerField(EditorGUILayout.GetControlRect(), "Object Layer", objectKindLayer);
            }

            
            Rect assignButtonRect = new Rect(EditorGUILayout.GetControlRect().width/2 - 100 + 1, EditorGUILayout.GetControlRect().y - 1, 200, 24);
            if(GUI.Button(assignButtonRect, "Assign Objects"))//assign button
            {
                if (objectKind == ObjectKind.ObjectTag)
                {
                    GridExtened.AssignObjects(objectKindTag, objectKind);
                    objectKindTagCopy = "";
                }
                else if(objectKind == ObjectKind.ObjectLayer)
                {
                    GridExtened.AssignObjects(LayerMask.LayerToName(objectKindLayer), objectKind);
                    objectKindLayerCopy = -1;
                }
            }
            //-----------------------------------------------------------------------



            //-------------------------Scroll View for Assigned objects-----------------------
            EditorGUILayout.BeginHorizontal(GUILayout.Height(144));
            #region Some Defined Values for Scroll View
            Rect rectPosition = EditorGUILayout.GetControlRect();
            Rect rectBox = new Rect(rectPosition.x, rectPosition.y + 18, rectPosition.width, 144);

            EditorGUI.DrawRect(rectBox, new Color(0.1764706f, 0.1764706f, 0.1764706f, 0.7176471f));

            Rect viewRect = new Rect(rectBox.x, rectBox.y, rectBox.width, (gridObjects != null? gridObjects.Count : 0) * rectPosition.height);
            scrollPosition = GUI.BeginScrollView(rectBox, scrollPosition, viewRect, false, (gridObjects != null ? true : false), GUIStyle.none, GUI.skin.verticalScrollbar);

            rectPosition.height += 2;
            int viewCount = 18;
            int firstIndex = (int)(scrollPosition.y / rectPosition.height);
            Rect assignedButtonPosition = new Rect(rectBox.x, rectBox.y + (firstIndex * rectPosition.height + 2), rectBox.width * 0.65f, 18);
            Rect assignedButtonDeletePosition = new Rect(rectBox.x + (float)(rectBox.width * 0.65), rectBox.y + (firstIndex * rectPosition.height + 2), rectBox.width * 0.35f, 18);

            this.SetAssignedButtonTexture();
            this.SetAssignedDeleteButtonTexture();
            #endregion

            if (objectKindTag != objectKindTagCopy || objectKindLayer != objectKindLayerCopy)
            {
                gridObjects = null;
                if (objectKind == ObjectKind.ObjectTag)
                {
                    gridObjects = GridExtened.GetObjects(objectKindTag, objectKind);
                    objectKindTagCopy = objectKindTag;
                }
                else if (objectKind == ObjectKind.ObjectLayer)
                {
                    gridObjects = GridExtened.GetObjects(LayerMask.LayerToName(objectKindLayer), objectKind);
                    objectKindLayer = objectKindLayerCopy;
                }
                    
            }
            if (gridObjects == null)
            {
                GUI.Label(new Rect(rectBox.x + (rectBox.width / 2) - 85, rectBox.y + (rectBox.height / 2) - 50, 170, 90), "You can assign objects in \nthe grid based on tag/layer" +
                    "\n\n            Click On The \n        'Assign Objects'\nYour objects will show here");
            }
            else
            {
                int counter = 0;
                for (int i = firstIndex; i < Mathf.Min(gridObjects != null ? gridObjects.Count : 0, firstIndex + viewCount); i++)
                {

                    GUI.Button(assignedButtonPosition, "  >   " + gridObjects[counter].name, assignedButtonStyle);
                    GUI.Button(assignedButtonDeletePosition, "Delete", assignedDeleteButtonStyle);


                    assignedButtonPosition.y += rectPosition.height;
                    assignedButtonDeletePosition.y += rectPosition.height;

                    counter++;
                }
                
            }
            GUI.EndScrollView();
            EditorGUILayout.EndHorizontal();
            //-----------------------------------------------------------------------

>>>>>>> Stashed changes

            //Repaint();
            SceneView.RepaintAll();
        }

       

        void SetAssignedButtonTexture()
        {
            if (hasAssignedTexture)
                return;

            assignedButtonStyle = new GUIStyle(/*GUI.skin.button*/);

            normalButtonTexture = new Texture2D(1, 1);
            normalButtonTexture.SetPixel(0, 0, new Color(0.2196079f, 0.2196079f, 0.2196079f, 1));
            normalButtonTexture.Apply();

            hoverButtonTexture = new Texture2D(1, 1);
            hoverButtonTexture.SetPixel(0, 0, new Color(0.3019608f, 0.3019608f, 0.3019608f, 0.7f));
            hoverButtonTexture.Apply();

            activeButtonTexture = new Texture2D(1, 1);
            activeButtonTexture.SetPixel(0, 0, new Color(0.2666666f, 0.6640455f, 0.8980392f, 0.4f));
            activeButtonTexture.Apply();

            assignedButtonStyle.alignment = TextAnchor.MiddleLeft;
            assignedButtonStyle.border = GUI.skin.button.border;

            assignedButtonStyle.normal.textColor = Color.white;
            assignedButtonStyle.normal.background = normalButtonTexture;

            assignedButtonStyle.hover.background = hoverButtonTexture;
            assignedButtonStyle.hover.textColor = Color.white;

            assignedButtonStyle.active.background = activeButtonTexture;
            assignedButtonStyle.active.textColor = Color.white;

            hasAssignedTexture = true;
        }

        void SetAssignedDeleteButtonTexture()
        {
            if (hasAssignedDeleteTexture)
                return;

            assignedDeleteButtonStyle = new GUIStyle(/*GUI.skin.button*/);

            normalDeleteButtonTexture = new Texture2D(1, 1);
            normalDeleteButtonTexture.SetPixel(0, 0, new Color(0.196079f, 0.196079f, 0.196079f, 1));
            normalDeleteButtonTexture.Apply();

            hoverDeleteButtonTexture = new Texture2D(1, 1);
            hoverDeleteButtonTexture.SetPixel(0, 0, new Color(0.3019608f, 0.3019608f, 0.3019608f, 0.7f));
            hoverDeleteButtonTexture.Apply();

            activeDeleteButtonTexture = new Texture2D(1, 1);
            activeDeleteButtonTexture.SetPixel(0, 0, new Color(0.2666666f, 0.6640455f, 0.8980392f, 0.4f));
            activeDeleteButtonTexture.Apply();

            assignedButtonStyle.border = GUI.skin.button.border;
            assignedDeleteButtonStyle.alignment = TextAnchor.MiddleCenter;

            assignedDeleteButtonStyle.normal.textColor = Color.white;
            assignedDeleteButtonStyle.normal.background = normalDeleteButtonTexture;

            assignedDeleteButtonStyle.hover.background = hoverDeleteButtonTexture;
            assignedDeleteButtonStyle.hover.textColor = Color.white;

            assignedDeleteButtonStyle.active.background = activeDeleteButtonTexture;
            assignedDeleteButtonStyle.active.textColor = Color.white;

            hasAssignedDeleteTexture = true;
        }

        void OnEnable()
        {
<<<<<<< Updated upstream

=======
            gridObjects = null;
>>>>>>> Stashed changes
            this.trackOfSelectedTransforms = new List<Transform>();
            this.trackOfSelectedTransformsPosition = new List<Vector3>();

            SceneView.duringSceneGui += this.OnSceneGUI;
        }
        void OnDisable()
        {
<<<<<<< Updated upstream

=======
            gridObjects = null;
>>>>>>> Stashed changes
            this.trackOfSelectedTransforms.Clear();
            this.trackOfSelectedTransformsPosition.Clear();
            this.trackOfSelectedTransforms = null;
            this.trackOfSelectedTransformsPosition = null;

            this.assignedButtonStyle = null;
            this.normalButtonTexture = null;
            this.activeButtonTexture = null;
            this.hasAssignedTexture = false;

            this.assignedDeleteButtonStyle = null;
            this.normalDeleteButtonTexture = null;
            this.activeDeleteButtonTexture = null;
            this.hasAssignedDeleteTexture = false;

            SceneView.duringSceneGui -= this.OnSceneGUI;
        }

        void OnSceneGUI(SceneView sceneView)
        {
            this.SetGrid();

            foreach (var _transfrom in Selection.transforms)
            {
                if (IsInsideGrid(_transfrom.position))
                {


                    if (this.hasSelectedPositionChanged() && this.RefernceCheck())
                    {
                        _transfrom.position = this.SnaptoGrid(_transfrom.position);
                    }
                    this.Highlights(_transfrom);
                }
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
        private Vector3 SnaptoGrid(Vector3 _position)
        {
            Vector3 roundedOffVector = Vector3.zero;


            float temOffsetX = gridCellSize.x / 2;
            if ((gridSize.x + 1) % 2 == 0)
            {
                roundedOffVector.x = (Mathf.Round((_position.x - gridCenter.x) / gridCellSize.x) * gridCellSize.x) + gridCenter.x;
            }
            else
            {
                roundedOffVector.x = (Mathf.Round((_position.x - temOffsetX - gridCenter.x) / gridCellSize.x) * gridCellSize.x) + temOffsetX + gridCenter.x;
            }


            float temOffsetY = gridCellSize.y / 2;
            if ((gridSize.y + 1) % 2 == 0)
            {
                roundedOffVector.y = (Mathf.Round((_position.y - gridCenter.y) / gridCellSize.y) * gridCellSize.y) + gridCenter.y;
            }
            else
            {
                roundedOffVector.y = (Mathf.Round((_position.y - temOffsetY - gridCenter.y) / gridCellSize.y) * gridCellSize.y) + temOffsetY + gridCenter.y;
            }


            float temOffsetZ = gridCellSize.z / 2;
            if ((gridSize.z + 1) % 2 == 0)
            {
                roundedOffVector.z = (Mathf.Round((_position.z - gridCenter.z) / gridCellSize.z) * gridCellSize.z) + gridCenter.z;
            }
            else
            {
                roundedOffVector.z = (Mathf.Round((_position.z - temOffsetZ - gridCenter.z) / gridCellSize.z) * gridCellSize.z) + temOffsetZ + gridCenter.z;
            }


            if (gridOrientation == GridOrientation.xy)
                roundedOffVector.z = _position.z;
            else if (gridOrientation == GridOrientation.yz)
                roundedOffVector.x = _position.x;
            else if (gridOrientation == GridOrientation.zx)
                roundedOffVector.y = _position.y;


            return roundedOffVector;

        }
        //-------------------------------------------------------------------------------------------
        #endregion



        #region Drawing Grid
        //------------------------------------Draw Grid----------------------------------------------
        void SetGrid()
        {
            this.DrawGrid2d();


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
        void DrawGrid2d( )
        {
            Handles.color = gridColor;
            float actualSizeA = 0, actualSizeB = 0;
            int gridSizeA = 0, gridSizeB = 0;
            if (gridOrientation == GridOrientation.xy)
            {
                actualSizeA = (gridSize.x + 1) * gridCellSize.x;
                actualSizeB = (gridSize.y + 1) * gridCellSize.y;

                gridSizeA = (gridSize.x + 1);
                gridSizeB = (gridSize.y + 1);
            }
            else if (gridOrientation == GridOrientation.yz)
            {
                actualSizeA = (gridSize.y + 1) * gridCellSize.y;
                actualSizeB = (gridSize.z + 1) * gridCellSize.z;

                gridSizeA = (gridSize.y + 1);
                gridSizeB = (gridSize.z + 1);
            }
            else if (gridOrientation == GridOrientation.zx)
            {
                actualSizeA = (gridSize.z + 1) * gridCellSize.z;
                actualSizeB = (gridSize.x + 1) * gridCellSize.x;

                gridSizeA = (gridSize.z + 1);
                gridSizeB = (gridSize.x + 1);
            }
            /*float temOffsetA = (-actualSizeA / 2) + (gridCellSize / 2);
            float temOffsetB = (-actualSizeB / 2) + (gridCellSize / 2);*/

            Vector3[] temLineOrigin = this.SetLineOrigin(actualSizeA, actualSizeB);

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
        Vector3[] SetLineOrigin(float actualSizeA, float actualSizeB)
        {
            if (gridOrientation == GridOrientation.xy)
            {
                float temOffsetA = (-actualSizeA / 2) + (gridCellSize.x / 2);
                float temOffsetB = (-actualSizeB / 2) + (gridCellSize.y / 2);

                return (new Vector3[]{
                new Vector3((+temOffsetA + gridCenter.x),  (temOffsetB + gridCenter.y),  (gridCenter.z)),
                new Vector3((-temOffsetA + gridCenter.x),  (temOffsetB + gridCenter.y),  (gridCenter.z)),

                new Vector3((temOffsetA + gridCenter.x),  (+temOffsetB + gridCenter.y),  (gridCenter.z)),
                new Vector3((temOffsetA + gridCenter.x),  (-temOffsetB + gridCenter.y),  (gridCenter.z)),
                });
            }
            else if (gridOrientation == GridOrientation.yz)
            {
                float temOffsetA = (-actualSizeA / 2) + (gridCellSize.y / 2);
                float temOffsetB = (-actualSizeB / 2) + (gridCellSize.z / 2);

                return (new Vector3[]{
                new Vector3((gridCenter.x),  (+temOffsetA + gridCenter.y),  (temOffsetB + gridCenter.z)),
                new Vector3((gridCenter.x),  (-temOffsetA + gridCenter.y),  (temOffsetB + gridCenter.z)),

                new Vector3((gridCenter.x),  (temOffsetA + gridCenter.y),  (+temOffsetB + gridCenter.z)),
                new Vector3((gridCenter.x),  (temOffsetA + gridCenter.y),  (-temOffsetB + gridCenter.z)),
                });
            }
            else
            {
                float temOffsetA = (-actualSizeA / 2) + (gridCellSize.z / 2);
                float temOffsetB = (-actualSizeB / 2) + (gridCellSize.x / 2);

                return (new Vector3[]{
                new Vector3((+temOffsetB + gridCenter.x),  (gridCenter.y),  (+temOffsetA + gridCenter.z)),
                new Vector3((+temOffsetB + gridCenter.x),  (gridCenter.y),  (-temOffsetA + gridCenter.z)),

                new Vector3((+temOffsetB + gridCenter.x),  (gridCenter.y),  (+temOffsetA + gridCenter.z)),
                new Vector3((-temOffsetB + gridCenter.x),  (gridCenter.y),  (+temOffsetA + gridCenter.z)),
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

            Vector3 averagePostion = Vector3.zero;
            int numberOfChilds = 0;
            foreach (Renderer r in selectedTransform.GetComponentsInChildren<Renderer>())
            {
                Bounds tem_R_bounds = r.bounds;
                tem_R_bounds.size = new Vector3(tem_R_bounds.size.x, tem_R_bounds.size.y, tem_R_bounds.size.z);
                bounds.Encapsulate(tem_R_bounds);

                averagePostion += r.transform.position;
                numberOfChilds++;
            }

            Bounds temBounds = bounds;
            temBounds.center = this.SnaptoGrid(temBounds.center);

            if (gridOrientation == GridOrientation.xy)
                temBounds.center = new Vector3(temBounds.center.x, temBounds.center.y, 0);
            else if (gridOrientation == GridOrientation.yz)
                temBounds.center = new Vector3(0, temBounds.center.y, temBounds.center.z);
            else if (gridOrientation == GridOrientation.zx)
                temBounds.center = new Vector3(temBounds.center.x, 0, temBounds.center.z);



            Vector3[] temData = this.RoundOffTo(temBounds.size, temBounds.center, gridCellSize);

            temBounds.size = temData[0];
            temBounds.center = temData[1];

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

        Vector3 RoundOff(Vector3 _size, Vector3 _center, Vector3 roundOffTo)
        {
            //_center += gridCenter;
            //roundOffTo += gridCenter;
            int multiple;
            {
                if ((gridSize.x + 1) % 2 != 0)
                {
                    multiple = 1;
                }
                else
                {
                    multiple = 2;
                }

                if (_center.x % roundOffTo.x != 0)
                {
                    _size.x += (roundOffTo.x * multiple) - (_size.x % (roundOffTo.x * multiple));
                }
                else
                {
                    if (_size.x % (roundOffTo.x * multiple) >= roundOffTo.x)
                        _size.x += (roundOffTo.x - (_size.x % roundOffTo.x)) + roundOffTo.x;
                    else
                        _size.x += roundOffTo.x - (_size.x % roundOffTo.x);
                }
            }

            {
                if ((gridSize.y + 1) % 2 != 0)
                {
                    multiple = 1;
                }
                else
                {
                    multiple = 2;
                }

                if (_center.y % roundOffTo.y != 0)
                {
                    _size.y += (roundOffTo.y * multiple) - (_size.y % (roundOffTo.y * multiple));
                }
                else
                {
                    if (_size.y % (roundOffTo.y * multiple) >= roundOffTo.y)
                        _size.y += (roundOffTo.y - (_size.y % roundOffTo.y)) + roundOffTo.y;
                    else
                        _size.y += roundOffTo.y - (_size.y % roundOffTo.y);
                }
            }

            {
                if ((gridSize.z + 1) % 2 != 0)
                {
                    multiple = 1;
                }
                else
                {
                    multiple = 2;
                }

                if (_center.z % roundOffTo.z != 0)
                {
                    _size.z += (roundOffTo.z * multiple) - (_size.z % (roundOffTo.z * multiple));
                }
                else
                {
                    if (_size.z % (roundOffTo.z * multiple) >= roundOffTo.z)
                        _size.z += (roundOffTo.z - (_size.z % roundOffTo.z)) + roundOffTo.z;
                    else
                        _size.z += roundOffTo.z - (_size.z % roundOffTo.z);
                }
            }

            return _size;
        }

        Vector3 Recenter(Vector3 _center, Vector3 roundOffTo)
        {
            Debug.Log(gridCenter.x % gridCellSize.x);

            //roundOffTo += new Vector3(gridCenter.x % gridCellSize.x, gridCenter.y % gridCellSize.y, gridCenter.z % gridCellSize.z);

            if (Mathf.Abs(_center.x) % roundOffTo.x >= roundOffTo.x / 2)
            {
                if (_center.x <= 0)
                {
                    //_center.x += gridCenter.x % gridCellSize.x;
                    _center.x += -roundOffTo.x - (_center.x % roundOffTo.x);
                }
                else
                {
                    //_center.x += gridCenter.x % gridCellSize.x;
                    _center.x += roundOffTo.x - (_center.x % roundOffTo.x);
                }
            }
            else
            {
                //_center.x += gridCenter.x % gridCellSize.x;
                _center.x -= _center.x % roundOffTo.x;
            }


            if (Mathf.Abs(_center.y) % roundOffTo.y > roundOffTo.y / 2)
            {
                if (_center.y < 0)
                    _center.y += -roundOffTo.y - (_center.y % roundOffTo.y);
                else
                    _center.y += roundOffTo.y - (_center.y % roundOffTo.y);
            }
            else
                _center.y -= _center.y % roundOffTo.y;


            if (Mathf.Abs(_center.z) % roundOffTo.z > roundOffTo.z / 2)
            {
                if (_center.z < 0)
                    _center.z += -roundOffTo.z - (_center.z % roundOffTo.z);
                else
                    _center.z += roundOffTo.z - (_center.z % roundOffTo.z);
            }
            else
                _center.z -= _center.z % roundOffTo.z;

            /*Vector3 offset = new Vector3(gridCenter.x % gridCellSize.x, 
                                         gridCenter.y % gridCellSize.y,
                                         gridCenter.z % gridCellSize.z);*/

            /*Debug.Log(offset);
            Debug.Log( 5.78 % 1);*/
            return _center;
        }

        Vector3 AnotherRoundOff(Vector3 _size, Vector3 roundOffTo)
        {
            _size.x += roundOffTo.x - (_size.x % roundOffTo.x);
            _size.x -= _size.x % roundOffTo.x;

            _size.y += roundOffTo.y - (_size.y % roundOffTo.y);
            _size.y -= _size.y % roundOffTo.y;

            _size.z += roundOffTo.z - (_size.z % roundOffTo.z);
            _size.z -= _size.z % roundOffTo.z;

            return _size;
        }

        Vector3[] RoundOffTo(Vector3 _size, Vector3 _center, Vector3 roundOffTo)
        {
            _size.x = Mathf.Ceil(_size.x / roundOffTo.x) * roundOffTo.x;
            if (_size.x % (gridCellSize.x * 2) == 0) { _size.x += gridCellSize.x; }

            if (_center.x + (_size.x / 2) > gridCenter.x + ((gridSize.x + 1) * gridCellSize.x) / 2 ||
                _center.x - (_size.x / 2) < gridCenter.x - ((gridSize.x + 1) * gridCellSize.x) / 2)//checking if Highlighed area is out of the grid
            {
                //Debug.Log(_size.x);
                float trimX = Mathf.Abs(_center.x - gridCenter.x) + (_size.x / 2) - ((gridSize.x + 1) * gridCellSize.x) / 2;// Triming out grid Highlighed area
                trimX += gridCellSize.x / 2;
                if (_center.x - gridCenter.x < 0)
                {
                    _center.x += trimX / 2;
                }
                else if (_center.x - gridCenter.x > 0)
                {
                    _center.x -= trimX / 2;
                }
                _size.x -= trimX;


            }

            _size.y = Mathf.Ceil(_size.y / roundOffTo.y) * roundOffTo.y;
            if (_size.y % (gridCellSize.y * 2) == 0) { _size.y += gridCellSize.y; }

            if (_center.y + (_size.y / 2) > gridCenter.y + ((gridSize.y + 1) * gridCellSize.y) / 2 ||
                _center.y - (_size.y / 2) < gridCenter.y - ((gridSize.y + 1) * gridCellSize.y) / 2)//checking if Highlighed area is out of the grid
            {
                float trimY = Mathf.Abs(_center.y - gridCenter.y) + (_size.y / 2) - ((gridSize.y + 1) * gridCellSize.y) / 2;// Triming out grid Highlighed area
                trimY += gridCellSize.y / 2;
                if (_center.y - gridCenter.y < 0)
                {
                    _center.y += trimY / 2;
                }
                else if (_center.y - gridCenter.y > 0)
                {
                    _center.y -= trimY / 2;
                }
                _size.y -= trimY;
            }
<<<<<<< Updated upstream

            _size.z = Mathf.Ceil(_size.z / roundOffTo.z) * roundOffTo.z;
            if (_size.z % (gridCellSize.z * 2) == 0) { _size.z += gridCellSize.z; }

            if (_center.z + (_size.z / 2) > gridCenter.z + ((gridSize.z + 1) * gridCellSize.z) / 2 ||
                _center.y - (_size.z / 2) < gridCenter.z - ((gridSize.z + 1) * gridCellSize.z) / 2)//checking if Highlighed area is out of the grid
            {
                float trimZ = Mathf.Abs(_center.z - gridCenter.z) + (_size.z / 2) - ((gridSize.z + 1) * gridCellSize.z) / 2;// Triming out grid Highlighed area
                trimZ += gridCellSize.z / 2;
                if (_center.z - gridCenter.z < 0)
                {
                    _center.z += trimZ / 2;
                }
                else if (_center.z - gridCenter.z > 0)
                {
                    _center.z -= trimZ / 2;
                }
                _size.z -= trimZ;
            }

            return new Vector3[] { _size, _center };
        }





=======

            _size.z = Mathf.Ceil(_size.z / roundOffTo.z) * roundOffTo.z;
            if (_size.z % (gridCellSize.z * 2) == 0) { _size.z += gridCellSize.z; }

            if (_center.z + (_size.z / 2) > gridCenter.z + ((gridSize.z + 1) * gridCellSize.z) / 2 ||
                _center.y - (_size.z / 2) < gridCenter.z - ((gridSize.z + 1) * gridCellSize.z) / 2)//checking if Highlighed area is out of the grid
            {
                float trimZ = Mathf.Abs(_center.z - gridCenter.z) + (_size.z / 2) - ((gridSize.z + 1) * gridCellSize.z) / 2;// Triming out grid Highlighed area
                trimZ += gridCellSize.z / 2;
                if (_center.z - gridCenter.z < 0)
                {
                    _center.z += trimZ / 2;
                }
                else if (_center.z - gridCenter.z > 0)
                {
                    _center.z -= trimZ / 2;
                }
                _size.z -= trimZ;
            }

            return new Vector3[] { _size, _center };
        }





>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        }
        //-------------------------------------------------------------------------------------------
        #endregion








        #region Access

        static public Vector2Int GetGridSize()
        {
            if (gridOrientation == GridOrientation.xy)
            {
                return new Vector2Int((gridSize.x + 1), (gridSize.y + 1));
            }
            else if (gridOrientation == GridOrientation.yz)
            {
                return new Vector2Int((gridSize.z + 1), (gridSize.y + 1));
            }
            else
            {
                return new Vector2Int((gridSize.z + 1), (gridSize.x + 1));
            }
        }
=======
        }
        //-------------------------------------------------------------------------------------------
        #endregion








        #region Access

        static public Vector2Int GetGridSize()
        {
            if (gridOrientation == GridOrientation.xy)
            {
                return new Vector2Int((gridSize.x + 1), (gridSize.y + 1));
            }
            else if (gridOrientation == GridOrientation.yz)
            {
                return new Vector2Int((gridSize.z + 1), (gridSize.y + 1));
            }
            else
            {
                return new Vector2Int((gridSize.z + 1), (gridSize.x + 1));
            }
        }
>>>>>>> Stashed changes


        static private bool IsInsideGrid(Vector3 _position)
        {

            if (_position.x <= (gridCenter.x - ((gridSize.x + 1) * gridCellSize.x) / 2) || _position.x >= (gridCenter.x + ((gridSize.x + 1) * gridCellSize.x) / 2))
                return false;

            if (_position.y <= (gridCenter.y - ((gridSize.y + 1) * gridCellSize.y) / 2) || _position.y >= (gridCenter.y + ((gridSize.y + 1) * gridCellSize.y) / 2))
                return false;

            if (_position.z <= (gridCenter.z - ((gridSize.z + 1) * gridCellSize.z) / 2) || _position.z >= (gridCenter.z + ((gridSize.z + 1) * gridCellSize.z) / 2))
                return false;

            return true;

        }

        static public Vector2Int ObjectIndex(Vector3 _position)
        {


            if (IsInsideGrid(_position) && IsSnaped(_position))
            {
                Vector3 _indexes = Vector3.zero;

                //_indexes.x = Mathf.Abs((((float)gridSize.x + 1)/2) - gridCenter.x) + (_position.x / gridCellSize.x)  - 1;

                _indexes.x = ((( (gridSize.x + 1) * gridCellSize.x / 2) + (_position.x - gridCenter.x)) / gridCellSize.x) - 1;

                _indexes.y = ((( (gridSize.y + 1) * gridCellSize.y / 2) + (_position.y - gridCenter.y)) / gridCellSize.y) - 1;

                _indexes.z = ((( (gridSize.z + 1) * gridCellSize.z / 2) + (_position.z - gridCenter.z)) / gridCellSize.z) - 1;


                if (gridOrientation == GridOrientation.xy)
                    return new Vector2Int((int)_indexes.x, (int)_indexes.y);

                else if (gridOrientation == GridOrientation.yz)
                    return new Vector2Int((int)_indexes.y, (int)_indexes.z);

                else if (gridOrientation == GridOrientation.zx)
                    return new Vector2Int((int)_indexes.z, (int)_indexes.x);
            }

            return new Vector2Int(-1, -1);
            

        }


        static private bool IsSnaped(Vector3 _position)
        {

            float temOffsetX = gridCellSize.x / 2;
            if (gridOrientation != GridOrientation.yz)
            {
                if ((gridSize.x + 1) % 2 == 0)
                {
                    if (Mathf.Abs(_position.x - ((Mathf.Round((_position.x - gridCenter.x) / gridCellSize.x) * gridCellSize.x) + gridCenter.x)) > precisionLimit)
                        return false;
                }
                else
                {
                    if (Mathf.Abs(_position.x - ((Mathf.Round((_position.x - temOffsetX - gridCenter.x) / gridCellSize.x) * gridCellSize.x) + temOffsetX + gridCenter.x)) > precisionLimit)
                        return false;
                }
            }

            if (gridOrientation != GridOrientation.zx)
            {
                float temOffsetY = gridCellSize.y / 2;
                if ((gridSize.y + 1) % 2 == 0)
                {
                    if (Mathf.Abs(_position.y - ((Mathf.Round((_position.y - gridCenter.y) / gridCellSize.y) * gridCellSize.y) + gridCenter.y)) > precisionLimit)
                        return false;
                }
                else
                {
                    if (Mathf.Abs(_position.y - ((Mathf.Round((_position.y - temOffsetY - gridCenter.y) / gridCellSize.y) * gridCellSize.y) + temOffsetY + gridCenter.y)) > precisionLimit)
                        return false;
                }
            }

            if (gridOrientation != GridOrientation.xy)
            {
                float temOffsetZ = gridCellSize.z / 2;
                if ((gridSize.z + 1) % 2 == 0)
                {
                    if (Mathf.Abs(_position.z - ((Mathf.Round((_position.z - gridCenter.z) / gridCellSize.z) * gridCellSize.z) + gridCenter.z)) > precisionLimit)
                        return false;
                }
                else
                {
                    if (Mathf.Abs(_position.z - ((Mathf.Round((_position.z - temOffsetZ - gridCenter.z) / gridCellSize.z) * gridCellSize.z) + temOffsetZ + gridCenter.z)) > precisionLimit)
                        return false;
                }
            }

            return true;
        }


        #endregion



    }
}