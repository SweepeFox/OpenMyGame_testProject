using UnityEngine;
using System;

public class GameUIViewModel
{
    private readonly GameUIModel _model;
    private readonly Action _onRestart;
    private readonly Action _onNextLevel;

    public GameUIViewModel(GameUIModel model, Action onRestart, Action onNextLevel)
    {
        _model = model;
        _onRestart = onRestart;
        _onNextLevel = onNextLevel;
    }

    public void Restart() => _onRestart?.Invoke();
    public void NextLevel() => _onNextLevel?.Invoke();
}