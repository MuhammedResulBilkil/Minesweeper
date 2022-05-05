using System;
using System.Collections;
using DG.Tweening;
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
    [SerializeField] private GameObject _mineSpriteGameObject;
    [SerializeField] private int _neighbourCount;

    private MeshRenderer _meshRenderer;
    private Material _copiedCellMaterial;
    private WaitForSeconds _waitForSecondsMoveZTween = new WaitForSeconds(0.05f);
    private int _gridX;
    private int _gridY;
    private int _randomMoveZTweenID;
    private bool _isMoveZTweenStarted;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _copiedCellMaterial = new Material(_meshRenderer.sharedMaterial);
        _meshRenderer.sharedMaterial = _copiedCellMaterial;

        _randomMoveZTweenID = Random.Range(0, 10000);

        //_cellType = Random.value > 0.5f ? CellType.Empty : CellType.Mine;
        //_copiedCellMaterial.color = _cellType == CellType.Empty ? Color.green : Color.red;
    }

    private void Start()
    {
        //_copiedCellMaterial.color = _cellType == CellType.Empty ? Color.green : Color.red;
    }

    public void ShowNeighbours()
    {
        if (_neighbourCount > 0)
            _cellText.text = _neighbourCount.ToString();
    }

    public void ShowCell()
    {
        switch (_cellType)
        {
            case CellType.Empty:
                _copiedCellMaterial.color = Color.green;
                if (_neighbourCount != 0)
                    ShowNeighbours();
                break;
            case CellType.Mine:
                _copiedCellMaterial.color = Color.red;
                _mineSpriteGameObject.SetActive(true);
                break;
            case CellType.Revealed:
                if (_neighbourCount == 0)
                    _copiedCellMaterial.color = Color.green;
                break;
            default:
                Debug.LogError("Could not find Cell Type!");
                break;
        }
    }

    public IEnumerator Reveal()
    {
        if (_cellType == CellType.Mine)
        {
            _copiedCellMaterial.color = Color.red;

            if (!GameController.Instance.isGameOver)
                GameController.Instance.GameOver();

            yield break;
        }

        if (_cellType == CellType.Empty && _neighbourCount != 0)
        {
            
            if (!GameController.Instance.isFirstTimeClicked)
                GameController.Instance.isFirstTimeClicked = true;
                
            _cellType = CellType.Revealed;

            yield return _waitForSecondsMoveZTween;

            if (!_isMoveZTweenStarted)
            {
                _isMoveZTweenStarted = true;

                if(!DOTween.IsTweening(_randomMoveZTweenID))
                    transform.DOLocalMoveZ(-1f, 0.1f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo).SetId(_randomMoveZTweenID);
                
                ShowNeighbours();
                _copiedCellMaterial.color = Color.green;
            }
        }

        _cellType = CellType.Revealed;

        if (_neighbourCount == 0)
        {
            if (!GameController.Instance.isFirstTimeClicked || !GameController.Instance.isFirstTimeNoNeighbour)
            {
                GameController.Instance.isFirstTimeClicked = true;
                GameController.Instance.isFirstTimeNoNeighbour = true;
                
                if(!DOTween.IsTweening(_randomMoveZTweenID))
                    transform.DOLocalMoveZ(-1f, 0.1f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo).SetId(_randomMoveZTweenID);
                
                _copiedCellMaterial.color = Color.green;
            }
            //_copiedCellMaterial.color = Color.green;
            
            //Flood Fill time!!!
            StartCoroutine(FloodFill());
        }
    }

    private IEnumerator FloodFill()
    {
        for (int xOff = -1; xOff <= 1; xOff++)
        {
            for (int yOff = -1; yOff <= 1; yOff++)
            {
                int i = gridIndexX + xOff;
                int j = gridIndexY + yOff;
                
                yield return _waitForSecondsMoveZTween;
                
                if (i > -1 && i < _gridX && j > -1 && j < _gridY)
                {
                    Cell neighbour = GameController.Instance.GetCellByIndex(i, j);
                    
                    if (neighbour.GetCellType() != CellType.Mine && neighbour.GetCellType() != CellType.Revealed)
                    {
                        neighbour.transform.DOLocalMoveZ(-1f, 0.1f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo)
                            .SetId(neighbour.GetRandomMoveZTweenID());
                        neighbour.SetMaterialColor(Color.green);
                        /*if (!neighbour.GetIsMoveZTweenStarted())
                        {
                            neighbour.SetIsMoveZTweenStarted(true);

                            
                        }*/

                        StartCoroutine(neighbour.Reveal());
                    }
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

    public void SetMaterialColor(Color color)
    {
        _copiedCellMaterial.color = color;
    }

    public bool GetIsMoveZTweenStarted()
    {
        return _isMoveZTweenStarted;
    }

    public void SetIsMoveZTweenStarted(bool isMoveZTweenStarted)
    {
        _isMoveZTweenStarted = isMoveZTweenStarted;
    }

    public int GetRandomMoveZTweenID()
    {
        return _randomMoveZTweenID;
    }
}