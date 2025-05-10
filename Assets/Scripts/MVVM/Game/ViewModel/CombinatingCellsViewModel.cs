using System.Collections.Generic;
using UnityEngine;

public class CombinatingCellsViewModel
{
    private readonly GameModel _model;
    private readonly int _rows;
    private readonly int _columns;

    public CombinatingCellsViewModel(GameModel model, int rows, int columns)
    {
        _model = model;
        _rows = rows;
        _columns = columns;
    }

    public List<DestroyCellParams> CheckCombinations()
    {
        var fieldCopy = _model.Field.Value.Clone() as int[,];
        var destroyCells = new List<DestroyCellParams>();

        CheckCombinations(fieldCopy, destroyCells, true);
        CheckCombinations(fieldCopy, destroyCells, false);

        for (int i = 0; i < destroyCells.Count; i++)
        {
            fieldCopy[destroyCells[i].rowIndex, destroyCells[i].columnIndex] = 0;
        }

        _model.Field.Value = fieldCopy;
        
        return destroyCells;
    }

    private void CheckCombinations(int[,] field, List<DestroyCellParams> destroyCells, bool isHorizontal)
    {
        int rows = isHorizontal ? field.GetLength(0) : field.GetLength(0) - 2;
        int columns = isHorizontal ? field.GetLength(1) - 2 : field.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (field[i, j] == field[i + (isHorizontal ? 0 : 1), j + (isHorizontal ? 1 : 0)] &&
                    field[i, j] == field[i + (isHorizontal ? 0 : 2), j + (isHorizontal ? 2 : 0)] &&
                    field[i, j] != 0)
                {
                    destroyCells.Add(new DestroyCellParams { rowIndex = i, columnIndex = j });
                    destroyCells.Add(new DestroyCellParams { rowIndex = i + (isHorizontal ? 0 : 1), columnIndex = j + (isHorizontal ? 1 : 0) });
                    destroyCells.Add(new DestroyCellParams { rowIndex = i + (isHorizontal ? 0 : 2), columnIndex = j + (isHorizontal ? 2 : 0) });
                }
            }
        }
    }
}