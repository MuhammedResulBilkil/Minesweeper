using System;
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

    private Camera _camera;
    private Cell[,] _nestedCells; 
    private List<Cell> _totalCells = new List<Cell>();

    private void Awake()
    {
        Instance = this;
        
        _camera = Camera.main;

        for (int i = 0; i < platforms.Count; i++)
        {
            grids[i].CreateGrid();
            _nestedCells = new Cell[grids[i].width, grids[i].width];
        }
        
        for (int i = 0; i < grids.Count; i++)
        {
            for (int j = 0; j < grids[i].gridCenterPoses.Count; j++)
            {
                GameObject go = Instantiate(cellPrefab, grids[i].gridCenterPoses[j], Quaternion.identity, cellsParent);
                _totalCells.Add(go.GetComponent<Cell>());
            }
        }

        int k = 0;
        
        for (int i = 0; i < _nestedCells.GetLength(0); i++)
        {
            for (int j = 0; j < _nestedCells.GetLength(1); j++)
            {
                _nestedCells[i, j] = _totalCells[k];
                _nestedCells[i, j].gridIndexX = i;
                _nestedCells[i, j].gridIndexY = j;
                
                k++;
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < _totalCells.Count; i++)
            CountMines(_totalCells[i]);
    }

    /*private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
            {
                if (hitInfo.transform.TryGetComponent(out Cell cell))
                {
                    cell.Reveal();
                }
            }
        }
    }*/

    public void ShowDebugLines()
    {
        for (int i = 0; i < grids.Count; i++)
            grids[i].DrawDebugLines();
    }

    private void CountMines(Cell cell)
    {
        if(cell.GetCellType() == CellType.Mine) return;

        int totalNeighbour = 0;

        for (int xOff = -1; xOff <=1; xOff++)
        {
            for (int yOff = -1; yOff <=1; yOff++)
            {
                int i = cell.gridIndexX + xOff;
                int j = cell.gridIndexY + yOff;

                if (i > -1 && i < _nestedCells.GetLength(0) && j > -1 && j < _nestedCells.GetLength(1))
                {
                    Cell neighbourCell = _nestedCells[i, j];
                
                    if(neighbourCell.GetCellType() == CellType.Mine)
                        totalNeighbour++;
                }
            }
        }

        cell.ShowNeighbours(totalNeighbour);
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
