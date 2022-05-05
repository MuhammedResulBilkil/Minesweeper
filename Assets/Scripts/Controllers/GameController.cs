using System;
using System.Collections.Generic;
using System.Globalization;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private Camera _camera;
    [SerializeField] private int _totalMinesCount;

    public bool isGameOver;
    public bool isFirstTimeClicked;
    public bool isFirstTimeNoNeighbour;
    
    public GameObject cellPrefab;
    public Transform cellsParent;
    public List<GameObject> platforms = new List<GameObject>();
    public List<MinesweeperGrid> grids = new List<MinesweeperGrid>();

    private float _cameraDistance = -10f;
    private int _totalEmptyCells = 0;
    private bool _isPlayerWin;
    private Cell[,] _nestedCells; 
    private List<Cell> _totalCells = new List<Cell>();

    private void Awake()
    {
        Instance = this;
        
        //DOTween.SetTweensCapacity(20000, 20);

        grids[0].gridCenterPoses.Clear();

        for (int i = 0; i < cellsParent.childCount; i++)
            Destroy(cellsParent.GetChild(i).gameObject);

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

        _totalEmptyCells = _totalCells.Count;

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
            _totalEmptyCells--;
        }
    }

    private void Start()
    {
        Vector3 averageCellPosition = Vector3.zero;
        
        for (int i = 0; i < _totalCells.Count; i++)
        {
            averageCellPosition += _totalCells[i].transform.position;
            _totalCells[i].SetGridXY(_nestedCells.GetLength(0), _nestedCells.GetLength(1));
            CountMines(_totalCells[i]);
        }

        averageCellPosition /= _totalCells.Count;
        averageCellPosition.z = _cameraDistance;
        _camera.transform.position = averageCellPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ClearMinesweeperEditMode();
            CreateNewMinesweeperEditMode();
        }

        if(isGameOver) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
            {
                if (hitInfo.transform.TryGetComponent(out Cell cell))
                {
                    if(cell.GetCellType() != CellType.Revealed)
                        StartCoroutine(cell.Reveal());
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
            {
                if (hitInfo.transform.TryGetComponent(out Cell cell))
                {
                    if(cell.GetCellType() != CellType.Revealed)
                        cell.ShowMark();
                }
            }
        }
    }

    public void PlayerLost()
    {
        isGameOver = true;
        _isPlayerWin = false;
        
        Debug.Log("Player Lost!!!");
        
        UIController.Instance.ShowGameLoseText();
        
        for (int i = 0; i < _totalCells.Count; i++)
            _totalCells[i].ShowCell();
    }

    private void PlayerWins()
    {
        isGameOver = true;
        _isPlayerWin = true;
        
        Debug.Log("Player Won!!!");
        
        UIController.Instance.ShowGameWinText();
        
        for (int i = 0; i < _totalCells.Count; i++)
            _totalCells[i].ShowCell();
    }

    public void ShowDebugLines()
    {
        for (int i = 0; i < grids.Count; i++)
            grids[i].DrawDebugLines();
    }

    public void CreateNewMinesweeperEditMode()
    {
        isGameOver = false;
        isFirstTimeClicked = false;
        isFirstTimeNoNeighbour = false;
        
        if (Application.isPlaying)
        {
            for (int i = 0; i < cellsParent.childCount; i++)
                Destroy(cellsParent.GetChild(i).gameObject);
        }
        else
        {
            while (cellsParent.childCount != 0)
                DestroyImmediate(cellsParent.GetChild(0).gameObject);
        }

        grids[0].gridCenterPoses.Clear();
        _nestedCells = new Cell[grids[0].width, grids[0].height];
        _totalCells.Clear();
        
        Awake();
        Start();
    }

    public void ClearMinesweeperEditMode()
    {
        isGameOver = false;
        isFirstTimeClicked = false;
        isFirstTimeNoNeighbour = false;

        if (Application.isPlaying)
        {
            for (int i = 0; i < cellsParent.childCount; i++)
                Destroy(cellsParent.GetChild(i).gameObject);
        }
        else
        {
            while (cellsParent.childCount != 0)
                DestroyImmediate(cellsParent.GetChild(0).gameObject);
        }

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

    public void CheckIsGameEnd()
    {
        if(_totalEmptyCells == 0)
            PlayerWins();
    }

    public void ChangeGridWidthHeight(string gridWidthHeight)
    {
        int gridValue = int.Parse(gridWidthHeight);
        grids[0].width = gridValue;
        grids[0].height = gridValue;
    }

    public void ChangeTotalMineCount(string totalMineCount)
    {
        int totalMine = int.Parse(totalMineCount);
        _totalMinesCount = totalMine;
    }

    public void ChangeCameraDistance(float cameraDistance)
    {
        Vector3 cameraPos = _camera.transform.position;
        _cameraDistance = -cameraDistance;
        cameraPos.z = _cameraDistance;
        _camera.transform.position = cameraPos;
    }

    public Cell GetCellByIndex(int gridX, int gridY)
    {
        return _nestedCells[gridX, gridY];
    }

    public void DecreaseTotalEmptyCellsCount()
    {
        _totalEmptyCells--;
    }

    public bool GetIsPlayerWin()
    {
        return _isPlayerWin;
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
