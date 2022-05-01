using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridController : MonoBehaviour
{
    public static GridController Instance { get; private set; }

    [SerializeField] private int _totalMinesCount;
    
    public bool isGameOver;
    
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

        // Pick Total Mines Spots
        for (int n = 0; n < _totalMinesCount; n++)
        {
            int i = Mathf.FloorToInt(Random.Range(0f, _nestedCells.GetLength(0)));
            int j = Mathf.FloorToInt(Random.Range(0f, _nestedCells.GetLength(1)));
            
            if (_nestedCells[i, j].GetCellType() == CellType.Mine)
            {
                n--;
                continue;
            }
            
            _nestedCells[i, j].SetCellType(CellType.Mine);
        }
    }

    private void Start()
    {
        for (int i = 0; i < _totalCells.Count; i++)
        {
            _totalCells[i].SetGridXY(_nestedCells.GetLength(0), _nestedCells.GetLength(1));
            CountMines(_totalCells[i]);
        }
    }

    private void Update()
    {
        if(isGameOver) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
            {
                if (hitInfo.transform.TryGetComponent(out Cell cell))
                {
                    if(cell.GetCellType() != CellType.Revealed)
                        cell.Reveal();
                }
            }
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!!!");
        
        for (int i = 0; i < _totalCells.Count; i++)
            _totalCells[i].ShowCell();
        
        isGameOver = true;
    }

    public void ShowDebugLines()
    {
        for (int i = 0; i < grids.Count; i++)
            grids[i].DrawDebugLines();
    }

    public void CreateNewMinesweeperEditMode()
    {
        isGameOver = false;
        
        for (int i = 0; i < _totalCells.Count; i++)
            DestroyImmediate(_totalCells[i].gameObject);

        grids[0].gridCenterPoses.Clear();
        _nestedCells = new Cell[grids[0].width, grids[0].height];
        _totalCells.Clear();
        
        Awake();
        Start();
    }

    public void ClearMinesweeperEditMode()
    {
        isGameOver = false;
        
        for (int i = 0; i < _totalCells.Count; i++)
            DestroyImmediate(_totalCells[i].gameObject);

        grids[0].gridCenterPoses.Clear();
        _nestedCells = new Cell[grids[0].width, grids[0].height];
        _totalCells.Clear();
    }

    private void CountMines(Cell cell)
    {
        if (cell.GetCellType() == CellType.Mine)
        {
            cell.SetNeighbourCount(-1);
            return;
        }

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

        cell.SetNeighbourCount(totalNeighbour);
        //cell.ShowNeighbours(totalNeighbour);
    }

    public Cell GetCellByIndex(int gridX, int gridY)
    {
        return _nestedCells[gridX, gridY];
    }

    #region ShuffleList

    //================================================================//
    //===================Fisher_Yates_CardDeck_Shuffle====================//
    //================================================================//
 
    /// With the Fisher-Yates shuffle, first implemented on computers by Durstenfeld in 1964, 
    ///   we randomly sort elements. This is an accurate, effective shuffling method for all array types.
 
    public static void ShuffleList<T> (ref List<T> aList) {
 
        System.Random randomValue = new System.Random ();
 
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
