using UnityEngine;
using UnityEditor;


public class Some : Editor
{
    private bool showLevel = false;

    public override void OnInspectorGUI()
    {
        
        /*DrawDefaultInspector();*/

        SomeOther myTarget = target as SomeOther;

        myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);

        if(showLevel)
            EditorGUILayout.LabelField("Level", myTarget.Level.ToString());

        if (GUILayout.RepeatButton("Show Level"))
            showLevel = !showLevel;
        //EditorGUILayout.HelpBox("Select to show more", MessageType.Info);
    }

    /*private void OnSceneGUI()
    {
        foreach (var _transfrom in Selection.transforms)
        {
            _transfrom.position = this.SnaptoGrid(_transfrom.position, 4);
        }
    }

    private Vector3 SnaptoGrid(Vector3 _vector, int gridSize)
    {
        return (new Vector3(
            Mathf.Round(_vector.x / gridSize) * gridSize,
            Mathf.Round(_vector.y / gridSize) * gridSize,
            Mathf.Round(_vector.z / gridSize) * gridSize
            ));
    }*/

    


}