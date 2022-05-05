using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    
    [SerializeField] private TextMeshProUGUI _gameWinText;
    [SerializeField] private TextMeshProUGUI _gameLoseText;
    [SerializeField] private String _winText;
    [SerializeField] private String _loseText;

    private int _successDOTweenID;
    private int _errorDOTweenID;

    private void Awake()
    {
        Instance = this;
        
        _successDOTweenID = Random.Range(0, 100000);
        _errorDOTweenID = Random.Range(0, 100000);
    }

    public void ShowGameWinText()
    {
        if (DOTween.IsTweening(_successDOTweenID))
            DOTween.Kill(_successDOTweenID);

        _gameWinText.text = _winText;
        _gameWinText.alpha = 1f;
        DOTween.To(() => _gameWinText.alpha, x => _gameWinText.alpha = x, 0f, 2f)
            .SetId(_successDOTweenID).SetEase(Ease.InOutSine).OnComplete(() => _gameWinText.text = "")
            .OnKill(() =>
            {
                _gameWinText.text = "";
                _gameWinText.alpha = 0;
            });
    }

    public void ShowGameLoseText()
    {
        if (DOTween.IsTweening(_errorDOTweenID))
            DOTween.Kill(_errorDOTweenID);

        _gameLoseText.text = _loseText;
        _gameLoseText.alpha = 1f;
        DOTween.To(() => _gameLoseText.alpha, x => _gameLoseText.alpha = x, 0f, 2f)
            .SetId(_errorDOTweenID).SetEase(Ease.InOutSine).OnComplete(() => _gameLoseText.text = "")
            .OnKill(() =>
            {
                _gameLoseText.text = "";
                _gameLoseText.alpha = 0;
            });
    }
}