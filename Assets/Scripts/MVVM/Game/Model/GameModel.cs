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
}