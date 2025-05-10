using System.Collections.Generic;
using UnityEngine;

public class GameViewModel
{
    public ReactiveProperty<int[,]> FieldView = new ();
    public ReactiveProperty<List<MoveCellParams>> MoveCellsView = new ();
    public ReactiveProperty<List<DestroyCellParams>> DestroyCellsView = new ();

    private readonly MovingCellsViewModel _movingCellsViewModel;
    private readonly FallingCellsViewModel _fallingCellsViewModel;
    private readonly CombinatingCellsViewModel _combinatingCellsViewModel;

    public int Rows { get; private set; }
    public int Columns { get; private set; }

    private GameModel _model;

    public GameViewModel(GameModel model, int rows, int columns)
    {
        _model = model;

        Rows = rows;
        Columns = columns;
        FieldView.Value = model.Field.Value;

        _movingCellsViewModel = new MovingCellsViewModel(model, rows, columns);
        _fallingCellsViewModel = new FallingCellsViewModel(model, rows, columns);
        _combinatingCellsViewModel = new CombinatingCellsViewModel(model, rows, columns);

        _model.Field.OnChanged += OnModelFieldChanged;
        _model.MoveCells.OnChanged += OnModelMoveCellsChanged;
        _model.DestroyCells.OnChanged += OnModelDestroyCellsChanged;
    }

    private void OnModelFieldChanged(int[,] value)
    {
        FieldView.Value = value;
    }

    private void OnModelMoveCellsChanged(List<MoveCellParams> value)
    {
        MoveCellsView.Value = value;
    }

    private void OnModelDestroyCellsChanged(List<DestroyCellParams> value)
    {
        DestroyCellsView.Value = value;
    }

    public bool CheckFallingCells()
    {
        var fallingCells = _fallingCellsViewModel.CheckFallingCells();
        if (fallingCells.Count > 0)
        {
            _model.MoveCells.Value = fallingCells;
            return true;
        }

        return false;
    }

    public void CheckCombinations()
    {
        var combinatingCells = _combinatingCellsViewModel.CheckCombinations();
        if (combinatingCells.Count > 0)
        {
             _model.DestroyCells.Value = combinatingCells;
        }
    }

    public void MoveCell(CellView cell, SwipeDirection direction)
    {
        var movingCells = _movingCellsViewModel.MoveCell(cell, direction);
        if (movingCells != null)
        {
             _model.MoveCells.Value = movingCells;
        }
    }

    public void Dispose()
    {
        _model.Field.OnChanged -= OnModelFieldChanged;
        _model.MoveCells.OnChanged -= OnModelMoveCellsChanged;
        _model.DestroyCells.OnChanged -= OnModelDestroyCellsChanged;
    }
}