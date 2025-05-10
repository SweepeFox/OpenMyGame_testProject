using UnityEngine;

public class Bootstraper : MonoBehaviour
{
    [SerializeField] private GameView _gameViewPrefab;
    [SerializeField] private GameUIView _gameUIViewPrefab;
    [SerializeField] private CellsConfigData _cellsConfigData;

    private GameView _gameView;
    private GameUIView _gameUIView;
    private LevelLoader _levelLoader;

    private int _currentLevelIndex = 0;

    public void Boot()
    {
        var level = _levelLoader.LevelsData[_currentLevelIndex];
        var cellsFactory = new CellsFactory(_cellsConfigData);

        var model = new GameModel(level.field);
        var viewModel = new GameViewModel(model);

        _gameView = Instantiate(_gameViewPrefab);
        _gameView.Init(viewModel, cellsFactory);

        var uiModel = new GameUIModel();
        var uiViewModel = new GameUIViewModel(uiModel, Restart, NextLevel);
        _gameUIView = Instantiate(_gameUIViewPrefab);
        _gameUIView.Init(uiViewModel);
    }

    public void Restart()
    {
        Destroy(_gameView.gameObject);
        Destroy(_gameUIView.gameObject);

        Boot();
    }

    public void NextLevel()
    {
        _currentLevelIndex = (_currentLevelIndex + 1) % _levelLoader.LevelsData.Length;
        Restart();
    }

    #region MonoBehaviour
    private async void Awake()
    {
        _levelLoader = new LevelLoader();
        await _levelLoader.LoadLevelsJson();

        Boot();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Restart();
        }
        else if (Input.GetKeyUp(KeyCode.N))
        {
            NextLevel();
        }
    }
    #endregion
}