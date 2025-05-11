using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Image))]
public class CellView : MonoBehaviour
{
    [SerializeField] private CellViewInteractionZone _interactionZone;

    private const string BREAK_ANIMATION_NAME = "Break";

    private CellViewModel _viewModel;

    private Image _image;
    private Animator _animator;
    public RectTransform Rect { get; private set; }

    public int Row => _viewModel.RowView.Value;
    public int Column => _viewModel.ColumnView.Value;

    public void Init(CellViewModel viewModel)
    {
        _viewModel = viewModel;

        _image = GetComponent<Image>();
        _animator = GetComponent<Animator>();
        Rect = GetComponent<RectTransform>();

        _interactionZone?.Init(this);
    }

    public void SetGamefieldPosition(int row, int column)
    {
        _viewModel.SetGamefieldPosition(row, column);
    }

    public void Break()
    {
        _image.enabled = false;

        if (_interactionZone != null)
        {
            Destroy(_interactionZone.gameObject);
            _interactionZone = null;
        }
    }

    public void PlayDeathAnimation(float duration)
    {
        _animator.speed = 1f / duration;
        _animator.PlayInFixedTime(BREAK_ANIMATION_NAME);
    }

    private void OnDestroy()
    {
        _viewModel.Dispose();
    }
}