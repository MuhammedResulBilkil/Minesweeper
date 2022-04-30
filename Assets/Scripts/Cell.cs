using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CellType
{
    Empty,
    Mine
}

[Serializable]
public class Cell : MonoBehaviour
{
    public int gridIndexX;
    public int gridIndexY;
    
    [SerializeField] private Material _cellMaterial;
    [SerializeField] CellType _cellType;
    [SerializeField] private bool _revealed;

    private MeshRenderer _meshRenderer;
    private Material _copiedCellMaterial;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _copiedCellMaterial = _meshRenderer.material;

        _cellType = Random.value > 0.5f ? CellType.Empty : CellType.Mine;
    }

    public void Reveal()
    {
        _revealed = true;

        _copiedCellMaterial.color = _cellType == CellType.Empty ? Color.green : Color.red;
    }
}
