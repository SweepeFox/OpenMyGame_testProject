using UnityEngine;

public class Bootstraper : MonoBehaviour
{
    [SerializeField] private CellsConfigData _cellsConfigData;
    [SerializeField] private GameView _viewPrefab;

    private GameViewModel _viewModel;

    public void Boot()
    {
        // TODO: load by LevelLoader from json
        var level1Params = new LevelParams()
        {
            rows = 2,
            columns = 7,
            field = new int[2, 7] {
                {0, 2, 0, 1, 0, 0, 0},
                {0, 2, 0, 2, 1, 1, 0}
            }
        };

        var level2Params = new LevelParams()
        {
            rows = 5,
            columns = 6,
            field = new int[5, 6] {
                {0, 2, 2, 0, 0, 0},
                {0, 2, 1, 2, 2, 0},
                {0, 1, 2, 1, 1, 0},
                {0, 2, 1, 2, 2, 0},
                {0, 2, 1, 2, 2, 0}
            }
        };

        var level3Params = new LevelParams()
        {
            rows = 6,
            columns = 6,
            field = new int[6, 6] {
                {0, 0, 2, 0, 0, 0},
                {0, 2, 1, 0, 0, 0},
                {0, 2, 2, 0, 2, 0},
                {0, 1, 2, 1, 1, 0},
                {0, 2, 1, 2, 2, 0},
                {0, 2, 1, 2, 2, 0}
            }
        };

        var model = new GameModel(level3Params.field);
        _viewModel = new GameViewModel(model, level3Params.rows, level3Params.columns);

        var cellsFactory = new CellsFactory(_cellsConfigData);

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

// TODO: move to LevelLoader
[System.Serializable]
public class LevelParams
{
    public int rows;
    public int columns;
    public int[,] field;
}