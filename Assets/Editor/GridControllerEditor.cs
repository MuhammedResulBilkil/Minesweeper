using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(GridController))]
public class GridControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        GridController myTarget = (GridController)target;

        if (GUILayout.Button("Draw Debug Lines"))
        {
            myTarget.ShowDebugLines();
        }
    }
}