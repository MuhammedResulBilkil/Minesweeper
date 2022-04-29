using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Grid
{
    public int width;
    public int height;
    public float cellSize;
    public Transform gridStartTransform;
    public List<Vector3> gridCenterPoses = new List<Vector3>();

    private int[,] _gridArray;

    public void CreateGrid()
    {
        _gridArray = new int[width, height];
        
        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            //Debug.LogWarning("X = " + x);
            for (int z = 0; z < _gridArray.GetLength(1); z++)
            {
                //Debug.LogWarning("Z = " + z);
                
                //Debug.DrawRay(GetCenterWorldPosition(x, z), Vector3.up * 20f, Color.red, 20f);
                gridCenterPoses.Add(GetCenterWorldPosition(x, z));
                
                //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.green, 20f);
                //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.green, 20f);
            }
        }
        
        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.green, 20f);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.green, 20f);
    }

    public Vector3 GetCenterWorldPosition(float x, float z)
    {
        Vector3 worldPos = new Vector3(x, 0f, z);
        worldPos *= cellSize;
        worldPos.x += cellSize / 2f;
        worldPos.z += cellSize / 2f;
        worldPos.x += gridStartTransform.position.x;
        worldPos.y += gridStartTransform.position.y;
        worldPos.z += gridStartTransform.position.z;
        
        return worldPos;
    }
    
    public Vector3 GetWorldPosition(float x, float z)
    {
        Vector3 worldPos = new Vector3(x, 0f, z);
        worldPos *= cellSize;
        worldPos.x += gridStartTransform.position.x;
        worldPos.y += gridStartTransform.position.y;
        worldPos.z += gridStartTransform.position.z;
        
        return worldPos;
    }
}
