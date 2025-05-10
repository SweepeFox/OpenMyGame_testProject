using System.Collections.Generic;
using UnityEngine;

public class MovingCellsViewModel
{
    private readonly GameModel _model;
    private readonly int _rows;
    private readonly int _columns;

    public MovingCellsViewModel(GameModel model, int rows, int columns)
    {
        _model = model;
        _rows = rows;
        _columns = columns;
    }

    public List<MoveCellParams> MoveCell(CellView cell, SwipeDirection direction)
    {
        // note: if cell is empty (broken) or already moving, return null
        var isCell1Destroying = _model.DestroyCells.Value != null && _model.DestroyCells.Value.Exists(x => x.row == cell.Row && x.column == cell.Column);
        if (isCell1Destroying)
        {
            return null;
        }

        if (IsMovingCell(cell.Row, cell.Column))
        {
            return null;
        }

        int row1 = cell.Row;
        int column1 = cell.Column;

        switch (direction)
        {
            case SwipeDirection.RIGHT:
                column1++;
                break;
            case SwipeDirection.LEFT:
                column1--;
                break;
            case SwipeDirection.UP:
                if (row1 > 0 && _model.Field.Value[row1 - 1, column1] != 0)
                {
                    row1--;
                }
                break;
            case SwipeDirection.DOWN:
                row1++;
                break;
        }

        var isCell2Destroying = _model.DestroyCells.Value != null && _model.DestroyCells.Value.Exists(x => x.row == row1 && x.column == column1);    
        if (isCell2Destroying)
        {
            return null;
        }

        var isBorder = row1 < 0 || row1 >= _rows || column1 < 0 || column1 >= _columns;
        if (isBorder)
        {
            return null;
        }

        if (row1 == cell.Row && column1 == cell.Column)
        {
            return null;
        }

        if (IsMovingCell(row1, column1))
        {
            return null;
        }

        var fieldCopy = _model.Field.Value.Clone() as int[,];
        var temp = fieldCopy[row1, column1];

        fieldCopy[row1, column1] = fieldCopy[cell.Row, cell.Column];
        fieldCopy[cell.Row, cell.Column] = temp;

        _model.Field.Value = fieldCopy;

        return new List<MoveCellParams>()
        {
            new MoveCellParams()
            {
                rowIndex1 = cell.Row,
                columnIndex1 = cell.Column,
                rowIndex2 = row1,
                columnIndex2 = column1
            }
        };
    }

    private bool IsMovingCell(int row, int column)
    {
        if (_model.MoveCells.Value == null)
        {
            return false;
        }

        return _model.MoveCells.Value.Exists(x => 
            x.rowIndex1 == row && x.columnIndex1 == column ||
            x.rowIndex2 == row && x.columnIndex2 == column
        );
    }
}