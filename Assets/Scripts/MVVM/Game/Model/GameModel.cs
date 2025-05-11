using System.Collections.Generic;

public class GameModel
{
    public readonly ReactiveProperty<List<MoveCellParams>> MoveCells = new ();
    public readonly ReactiveProperty<List<DestroyCellParams>> DestroyCells = new ();

    public readonly ReactiveProperty<int[,]> Field = new ();

    public GameModel(int[,] field)
    {
        Field.Value = field;
    }

    public bool IsMoving(int row, int column)
    {
        return MoveCells.Value != null && MoveCells.Value.Exists(x => x.rowIndex1 == row && x.columnIndex1 == column || x.rowIndex2 == row && x.columnIndex2 == column);
    }

    public bool IsDestroying(int row, int column)
    {
        return DestroyCells.Value != null && DestroyCells.Value.Exists(x => x.row == row && x.column == column);
    }
}