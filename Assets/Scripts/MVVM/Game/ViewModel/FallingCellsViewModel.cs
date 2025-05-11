using System.Collections.Generic;
using UnityEngine;

public class FallingCellsViewModel
{
    private readonly GameModel _model;
    private readonly int _rows;
    private readonly int _columns;

    public FallingCellsViewModel(GameModel model, int rows, int columns)
    {
        _model = model;
        _rows = rows;
        _columns = columns;
    }

    public List<MoveCellParams> CheckFallingCells()
    {
        var fieldCopy = _model.Field.Value.Clone() as int[,];
        var moveCells = new List<MoveCellParams>();

        for (int row = _rows - 1; row >= 0; row--)
        {
            for (int column = 0; column < _columns; column++)
            {
                if (fieldCopy[row, column] != 0)
                {
                    continue;
                }

                if (row - 1 < 0 || fieldCopy[row - 1, column] == 0)
                {
                    continue;
                }

                if (_model.IsDestroying(row, column) || _model.IsDestroying(row - 1, column))
                {
                    continue;
                }

                moveCells.Add(new MoveCellParams()
                {
                    rowIndex1 = row,
                    columnIndex1 = column,
                    rowIndex2 = row - 1,
                    columnIndex2 = column
                });

                var topCell = fieldCopy[row - 1, column];
                fieldCopy[row - 1, column] = 0;
                fieldCopy[row, column] = topCell;
            }
        }

        _model.Field.Value = fieldCopy;
        return moveCells;
    }
}