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
        if (_model.IsDestroying(cell.Row, cell.Column))
        {
            return null;
        }

        if (_model.IsMoving(cell.Row, cell.Column))
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
    
        if (_model.IsDestroying(row1, column1))
        {
            return null;
        }
        
        if (_model.IsMoving(row1, column1))
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
}