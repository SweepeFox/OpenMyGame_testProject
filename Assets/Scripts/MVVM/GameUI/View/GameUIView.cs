using UnityEngine;

public class GameUIView : MonoBehaviour
{
    private GameUIViewModel _viewModel;

    public void Init(GameUIViewModel viewModel) => _viewModel = viewModel;
    public void Restart() => _viewModel.Restart();
    public void NextLevel() => _viewModel.NextLevel();
}