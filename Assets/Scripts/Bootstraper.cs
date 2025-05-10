using UnityEngine;

public class Bootstraper : MonoBehaviour
{
    [SerializeField] private CellsConfigData _cellsConfigData;
    [SerializeField] private GameView _viewPrefab;

    private LevelLoader _levelLoader;
    private GameViewModel _viewModel;

    public async void Boot()
    {
        _levelLoader = new LevelLoader();
        await _levelLoader.LoadLevelsJson();

        var currentLevel = _levelLoader.LevelsData[1];
        var cellsFactory = new CellsFactory(_cellsConfigData);

        var model = new GameModel(currentLevel.field);
        _viewModel = new GameViewModel(model, currentLevel.rows, currentLevel.columns);

        var view = Instantiate(_viewPrefab);
        view.Init(_viewModel, cellsFactory);
    }

    private void Awake()
    {
        Boot();
    }

    private void OnDestroy()
    {
        _viewModel.Dispose();
    }
}