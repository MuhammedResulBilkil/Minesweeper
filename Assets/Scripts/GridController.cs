using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GridController : MonoBehaviour
{
    public static GridController Instance { get; private set; }

    public GameObject cellPrefab;
    public Transform cellsParent;
    public List<GameObject> platforms = new List<GameObject>();
    public List<MinesweeperGrid> grids = new List<MinesweeperGrid>();

    private List<Transform> _cells = new List<Transform>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < platforms.Count; i++)
            grids[i].CreateGrid();

        for (int i = 0; i < grids[0].gridCenterPoses.Count; i++)
        {
           GameObject go = Instantiate(cellPrefab, grids[0].gridCenterPoses[i], Quaternion.identity, cellsParent);
           _cells.Add(go.transform);
        }
           
    }
    
    public void ShowDebugLines()
    {
        for (int i = 0; i < grids.Count; i++)
            grids[i].DrawDebugLines();
    }

    #region ShuffleList

    //================================================================//
    //===================Fisher_Yates_CardDeck_Shuffle====================//
    //================================================================//
 
    /// With the Fisher-Yates shuffle, first implemented on computers by Durstenfeld in 1964, 
    ///   we randomly sort elements. This is an accurate, effective shuffling method for all array types.
 
    public static void ShuffleList<T> (ref List<T> aList) {
 
        Random randomValue = new Random ();
 
        var myGO = default(T);
 
        int n = aList.Count;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            // ... It is equivalent to Math.random() in Java.
            int r = i + (int)(randomValue.NextDouble() * (n - i));
            myGO = aList[r];
            aList[r] = aList[i];
            aList[i] = myGO;
        }
 
        //return aList;
    }

    #endregion
}
