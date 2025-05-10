using UnityEngine;

public class GamefieldViewModel
{
    public readonly ReactiveProperty<Vector2[,]> CellsPositionsView = new ();

    private readonly GamefieldModel _model;

    public GamefieldViewModel(GamefieldModel model)
    {
        _model = model;

        _model.CellsPositions.OnChanged += OnModelCellsPositionsChanged;
    }

    public void SetCellsPositions(Vector2[,] cellsPositions)
    {
        _model.CellsPositions.Value = cellsPositions;
    }

    private void OnModelCellsPositionsChanged(Vector2[,] value)
    {
        CellsPositionsView.Value = value;
    }

    public void Dispose()
    {
        _model.CellsPositions.OnChanged -= OnModelCellsPositionsChanged;
    }
}