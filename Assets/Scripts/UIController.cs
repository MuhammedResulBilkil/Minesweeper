using System;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    
    [SerializeField] private TextMeshProUGUI _gameWinText;
    [SerializeField] private TextMeshProUGUI _gameLoseText;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowGameWinText()
    {
        _gameWinText.gameObject.SetActive(true);
    }

    public void ShowGameLoseText()
    {
        _gameLoseText.gameObject.SetActive(true);
    }
}