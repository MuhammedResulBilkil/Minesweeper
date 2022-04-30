using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinesweeperGrid
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
            for (int y = 0; y < _gridArray.GetLength(1); y++)
            {
                //Debug.LogWarning("Z = " + z);
                
                //Debug.DrawRay(GetCenterWorldPosition(x, y), Vector3.back * 20f, Color.red, 5f);
                gridCenterPoses.Add(GetCenterWorldPosition(x, y));
                
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.green, 5f);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.green, 5f);
            }
        }
        
        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.green, 5f);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.green, 5f);
    }

    public Vector3 GetCenterWorldPosition(float x, float y)
    {
        Vector3 worldPos = new Vector3(x, y, 0.5f);
        worldPos *= cellSize;
        worldPos.x += cellSize / 2f;
        worldPos.y += cellSize / 2f;
        worldPos.x += gridStartTransform.position.x;
        worldPos.y += gridStartTransform.position.y;
        worldPos.z += gridStartTransform.position.z;
        
        return worldPos;
    }
    
    public Vector3 GetWorldPosition(float x, float y)
    {
        Vector3 worldPos = new Vector3(x, y, 0f);
        worldPos *= cellSize;
        worldPos.x += gridStartTransform.position.x;
        worldPos.y += gridStartTransform.position.y;
        worldPos.z += gridStartTransform.position.z;
        
        return worldPos;
    }
    
    public void DrawDebugLines()
    {
        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < _gridArray.GetLength(1); y++)
            {
                Debug.DrawRay(GetCenterWorldPosition(x, y), Vector3.back * 20f, Color.red, 20f);
                
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.green, 20f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.green, 20f);
            }
        }
        
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.green, 20f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.green, 20f);
    }
}
