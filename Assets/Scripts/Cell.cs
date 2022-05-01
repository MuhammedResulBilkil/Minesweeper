using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CellType
{
    Empty,
    Mine,
    Revealed
}

[ExecuteInEditMode]
public class Cell : MonoBehaviour
{
    public int gridIndexX;
    public int gridIndexY;
    
    [SerializeField] private CellType _cellType;
    [SerializeField] private TextMeshProUGUI _cellText;
    [SerializeField] private int _neighbourCount;

    private MeshRenderer _meshRenderer;
    private Material _copiedCellMaterial;
    private int _gridX;
    private int _gridY;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _copiedCellMaterial = new Material(_meshRenderer.sharedMaterial);
        _meshRenderer.sharedMaterial = _copiedCellMaterial;

        //_cellType = Random.value > 0.5f ? CellType.Empty : CellType.Mine;
        //_copiedCellMaterial.color = _cellType == CellType.Empty ? Color.green : Color.red;
    }
    
    private void Start()
    {
        //_copiedCellMaterial.color = _cellType == CellType.Empty ? Color.green : Color.red;
    }

    public void ShowNeighbours()
    {
        if(_neighbourCount > 0)
            _cellText.text = _neighbourCount.ToString();
    }

    public void ShowCell()
    {
        switch (_cellType)
        {
            case CellType.Empty:
                _copiedCellMaterial.color = Color.green;
                if(_neighbourCount != 0)
                    ShowNeighbours();
                break;
            case CellType.Mine:
                _copiedCellMaterial.color = Color.red;
                break;
            case CellType.Revealed:
                if(_neighbourCount == 0)
                    _copiedCellMaterial.color = Color.green;
                break;
            default:
                Debug.LogError("Could not find Cell Type!");
                break;
        }
    }

    public void Reveal()
    {
        if (_cellType == CellType.Mine)
        {
            _copiedCellMaterial.color = Color.red;
            
            if(!GridController.Instance.isGameOver)
                GridController.Instance.GameOver();
            
            return;
        }
        
        if (_cellType == CellType.Empty && _neighbourCount != 0)
        {
            ShowNeighbours();
            _copiedCellMaterial.color = Color.green;
        }
        
        _cellType = CellType.Revealed;

        if (_neighbourCount == 0)
        {
            _copiedCellMaterial.color = Color.green;
            
            //Flood Fill time!!!
            FloodFill();
        }
    }

    private void FloodFill()
    {
        for (int xOff = -1; xOff <=1; xOff++)
        {
            for (int yOff = -1; yOff <=1; yOff++)
            {
                int i = gridIndexX + xOff;
                int j = gridIndexY + yOff;

                if (i > -1 && i < _gridX && j > -1 && j < _gridY)
                {
                    Cell neighbour = GridController.Instance.GetCellByIndex(i, j);
                    
                    if(neighbour.GetCellType() != CellType.Mine && neighbour.GetCellType() != CellType.Revealed)
                        neighbour.Reveal();
                }
            }
        }
    }

    public CellType GetCellType()
    {
        return _cellType;
    }

    public void SetCellType(CellType cellType)
    {
        _cellType = cellType;
    }

    public void SetNeighbourCount(int neighbourCount)
    {
        _neighbourCount = neighbourCount;
    }

    public void SetGridXY(int gridX, int gridY)
    {
        _gridX = gridX;
        _gridY = gridY;
    }
}
