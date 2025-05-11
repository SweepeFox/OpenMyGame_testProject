using System.Collections.Generic;
using UnityEngine;

public class CombinatingCellsViewModel
{
    private readonly List<int[,]> specialShapesMasks = new List<int[,]>()
    {
        new int[,]
        {
            {1, 1, 1},
            {0, 1, 0}
        },
        new int[,]
        {
            {0, 0, 0, 1},
            {1, 1, 1, 1},
            {0, 1, 0, 0}
        }
    };
    private List<int[,]> _specialShapes = new List<int[,]>();

    private readonly GameModel _model;
    private readonly int _rows;
    private readonly int _columns;

    public CombinatingCellsViewModel(GameModel model, int rows, int columns)
    {
        _model = model;
        _rows = rows;
        _columns = columns;

        specialShapesMasks.ForEach(mask => _specialShapes.Add(ConvertMaskToOffsets(mask)));
    }

    public List<DestroyCellParams> CheckCombinations()
    {
        var fieldCopy = _model.Field.Value.Clone() as int[,];
        var destroyCells = new List<DestroyCellParams>();

        CheckCombinations(fieldCopy, destroyCells, true);
        CheckCombinations(fieldCopy, destroyCells, false);
        CheckSpecialPatterns(fieldCopy, destroyCells);

        for (int i = 0; i < destroyCells.Count; i++)
        {
            fieldCopy[destroyCells[i].row, destroyCells[i].column] = 0;
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
                    destroyCells.Add(new DestroyCellParams { row = i, column = j });
                    destroyCells.Add(new DestroyCellParams { row = i + (isHorizontal ? 0 : 1), column = j + (isHorizontal ? 1 : 0) });
                    destroyCells.Add(new DestroyCellParams { row = i + (isHorizontal ? 0 : 2), column = j + (isHorizontal ? 2 : 0) });
                }
            }
        }
    }

    private void CheckSpecialPatterns(int[,] field, List<DestroyCellParams> destroyCells)
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                int cellValue = field[i, j];
                if (cellValue == 0) continue;

                foreach (var shape in _specialShapes)
                {
                    var positions = new List<(int, int)> { (i, j) };
                    bool isMatch = true;

                    for (int k = 0; k < shape.GetLength(0); k++)
                    {
                        int rowOffset = shape[k, 0];
                        int colOffset = shape[k, 1];
                        int ni = i + rowOffset;
                        int nj = j + colOffset;
                        if (ni >= 0 && ni < _rows && nj >= 0 && nj < _columns && field[ni, nj] == cellValue)
                        {
                            positions.Add((ni, nj));
                        }
                        else
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch)
                    {
                        foreach (var (row, col) in positions)
                        {
                            if (!destroyCells.Exists(c => c.row == row && c.column == col))
                                destroyCells.Add(new DestroyCellParams { row = row, column = col });
                        }
                    }
                }
            }
        }
    }

    private int[,] ConvertMaskToOffsets(int[,] mask)
    {
        int rows = mask.GetLength(0);
        int cols = mask.GetLength(1);

        int centerRow = rows / 2;
        int centerCol = cols / 2;

        var offsets = new List<(int, int)>();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (mask[i, j] == 1)
                {
                    int rowOffset = i - centerRow;
                    int colOffset = j - centerCol;
                    offsets.Add((rowOffset, colOffset));
                }
            }
        }

        var result = new int[offsets.Count, 2];
        for (int i = 0; i < offsets.Count; i++)
        {
            result[i, 0] = offsets[i].Item1;
            result[i, 1] = offsets[i].Item2;
        }

        return result;
    }
}