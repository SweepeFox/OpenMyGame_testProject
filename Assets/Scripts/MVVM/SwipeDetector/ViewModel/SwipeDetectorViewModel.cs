using UnityEngine;

public class SwipeDetectorViewModel
{
    private readonly SwipeDetectorModel _model;

    public readonly ReactiveProperty<SwipeParams> SwipeParamsView = new ();

    private Vector2 _startPosition;
    private Vector2 _previousPosition;
    private bool _isSwiping;

    public SwipeDetectorViewModel(SwipeDetectorModel model)
    {
        _model = model;

        _model.SwipeParams.OnChanged += OnModelSwipeParamsChanged;
    }

    private void OnModelSwipeParamsChanged(SwipeParams value)
    {
        SwipeParamsView.Value = value;
    }

    public void StartSwipe(Vector2 position)
    {
        _startPosition = position;
        _previousPosition = _startPosition;
        _isSwiping = true;
    }

    public void EndSwipe()
    {
        _isSwiping = false;
    }

    public void MoveSwipe(Vector2 position)
    {
        if (!_isSwiping)
        {
            return;
        }

        var currentPosition = position;
        var deltaPosition = currentPosition - _previousPosition;
        _previousPosition = currentPosition;

        var distanceFromStart = Vector2.Distance(_startPosition, position);
        if (distanceFromStart > _model.DetectSwipeMinDistance.Value)
        {
            _model.SwipeParams.Value = new SwipeParams
            {
                startPosition = position - deltaPosition,
                direction = GetSwipeDirection(deltaPosition)
            };

            _isSwiping = false;
        }
    }

    private SwipeDirection GetSwipeDirection(Vector2 deltaPosition)
    {
        if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
        {
            return deltaPosition.x > 0 ? SwipeDirection.RIGHT : SwipeDirection.LEFT;
        }
        else
        {
            return deltaPosition.y > 0 ? SwipeDirection.UP : SwipeDirection.DOWN;
        }
    }

    public void Dispose()
    {
        _model.SwipeParams.OnChanged -= OnModelSwipeParamsChanged;
    }
}
