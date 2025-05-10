using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Image))]
public class CellView : MonoBehaviour
{
    private CellViewModel _viewModel;

    private Image _image;

    public int Row => _viewModel.RowView.Value;
    public int Column => _viewModel.ColumnView.Value;

    public void Init(CellViewModel viewModel)
    {
        _viewModel = viewModel;

        _image = GetComponent<Image>();
    }

    public void SetGamefieldPosition(int row, int column)
    {
        _viewModel.SetGamefieldPosition(row, column);
    }

    public void Break()
    {
        _image.enabled = false;
    }

    private void OnDestroy()
    {
        _viewModel.Dispose();
    }
}