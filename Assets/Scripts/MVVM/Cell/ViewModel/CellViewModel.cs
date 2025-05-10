public class CellViewModel
{
    public ReactiveProperty<int> RowView = new ();
    public ReactiveProperty<int> ColumnView = new ();

    private readonly CellModel _model;

    public CellViewModel(CellModel model)
    {
        _model = model;

        RowView.Value = _model.Row.Value;
        ColumnView.Value = _model.Column.Value;

        _model.Row.OnChanged += OnModelRowChanged;
        _model.Column.OnChanged += OnModelColumnChanged;
    }

    private void OnModelRowChanged(int value)
    {
        RowView.Value = value;
    }

    private void OnModelColumnChanged(int value)
    {
        ColumnView.Value = value;
    }

    public void SetGamefieldPosition(int row, int column)
    {
        _model.Row.Value = row;
        _model.Column.Value = column;
    }

    public void Dispose()
    {
        _model.Row.OnChanged -= OnModelRowChanged;
        _model.Column.OnChanged -= OnModelColumnChanged;
    }
}