using UnityEngine;

public class Bootstraper : MonoBehaviour
{
    [SerializeField] private GameView _gameViewPrefab;
    [SerializeField] private GameUIView _gameUIViewPrefab;
    [SerializeField] private CellsConfigData _cellsConfigData;

    private GameView _gameView;
    private GameModel _gameModel;
    private GameUIView _gameUIView;
    private LevelLoader _levelLoader;
    private DataSaver _dataSaver;
    private SaveData _saveData;

    public void Boot()
    {
        var level = _levelLoader.LevelsData[_saveData.level];
        var cellsFactory = new CellsFactory(_cellsConfigData);

        _gameModel = new GameModel(_saveData.field == null ? level.field : _saveData.field);
        var viewModel = new GameViewModel(_gameModel, NextLevel);

        _gameView = Instantiate(_gameViewPrefab);
        _gameView.Init(viewModel, cellsFactory);

        var uiModel = new GameUIModel();
        var uiViewModel = new GameUIViewModel(uiModel, Restart, NextLevel);
        _gameUIView = Instantiate(_gameUIViewPrefab);
        _gameUIView.Init(uiViewModel);
    }

    public void Restart()
    {
        _saveData.field = null;

        Destroy(_gameView.gameObject);
        Destroy(_gameUIView.gameObject);

        Boot();
    }

    public void NextLevel()
    {
        _saveData.level = (_saveData.level + 1) % _levelLoader.LevelsData.Length;
        _saveData.field = null;

        _dataSaver.Save(_saveData);
        Restart();
    }

    #region MonoBehaviour
    private async void Awake()
    {
        _levelLoader = new LevelLoader();
        await _levelLoader.LoadLevelsJson();

        _dataSaver = new DataSaver();
        _saveData = _dataSaver.Load();

        Boot();
    }

    private void OnApplicationQuit()
    {
        _saveData.field = _gameModel.Field.Value;
        _dataSaver.Save(_saveData);
    }
    #endregion
}