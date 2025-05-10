using UnityEngine.UI;
using UnityEngine;

public class GamefieldView : MonoBehaviour
{
    private const float DEFAULT_GAMEFIELD_SIZE = 720;
    private const float DEFAULT_BLOCK_SIZE = 170;
    private const float DEFAULT_BLOCK_SPACING = 60;

    private GridLayoutGroup _gridLayoutGroup;
    private GamefieldViewModel _viewModel;

    public Vector2[,] CellsPositions => _viewModel.CellsPositionsView.Value;

    public void Init(int rows, int columns, GamefieldViewModel viewModel)
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        _viewModel = viewModel;

        var cellSize = DEFAULT_GAMEFIELD_SIZE / columns;
        var spacing = cellSize * DEFAULT_BLOCK_SPACING / DEFAULT_BLOCK_SIZE;

        var totalWidth = cellSize * columns + spacing * (columns - 1);
        var scaleFactor = DEFAULT_GAMEFIELD_SIZE / totalWidth;

        var desiredCellSize = cellSize / scaleFactor;
        var desiredSpacing = spacing / scaleFactor;

        _gridLayoutGroup.cellSize = new Vector2(Mathf.Ceil(desiredCellSize), Mathf.Ceil(desiredCellSize));
        _gridLayoutGroup.spacing = new Vector2(Mathf.Ceil(-desiredSpacing), Mathf.Ceil(-desiredSpacing));

        _viewModel.SetCellsPositions(new Vector2[rows, columns]);
    }

    public void DisableLayoutGroup()
    {
        _gridLayoutGroup.enabled = false;
    }
}