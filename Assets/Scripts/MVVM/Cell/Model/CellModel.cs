public class CellModel
{
    public ReactiveProperty<int> Row = new ();
    public ReactiveProperty<int> Column = new ();

    public CellModel(int row, int column)
    {
        Row.Value = row;
        Column.Value = column;
    }
}