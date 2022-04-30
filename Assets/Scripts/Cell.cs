using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CellType
{
    Empty,
    Mine
}

public class Cell : MonoBehaviour
{
    public int gridIndexX;
    public int gridIndexY;
    
    [SerializeField] private CellType _cellType;
    [SerializeField] private TextMeshProUGUI _cellText;
    [SerializeField] private int _neighbourCount;

    private MeshRenderer _meshRenderer;
    private Material _copiedCellMaterial;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _copiedCellMaterial = _meshRenderer.material;

        _cellType = Random.value > 0.5f ? CellType.Empty : CellType.Mine;
        _copiedCellMaterial.color = _cellType == CellType.Empty ? Color.green : Color.red;
    }

    public void ShowNeighbours(int neighbourCount)
    {
        _neighbourCount = neighbourCount;
        
        _cellText.text = _neighbourCount.ToString();
    }

    public CellType GetCellType()
    {
        return _cellType;
    }
}
