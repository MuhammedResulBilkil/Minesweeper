using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(GridController))]
public class GridControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridController myTarget = (GridController)target;

        if (GUILayout.Button("Draw Debug Lines", GUILayout.Height(100)))
        {
            myTarget.ShowDebugLines();
        }
        
        if (GUILayout.Button("Create New Minesweeper", GUILayout.Height(100)))
        {
            myTarget.CreateNewMinesweeperEditMode();
        }
        
        if (GUILayout.Button("Clear Minesweeper", GUILayout.Height(100)))
        {
            myTarget.ClearMinesweeperEditMode();
        }
    }
}