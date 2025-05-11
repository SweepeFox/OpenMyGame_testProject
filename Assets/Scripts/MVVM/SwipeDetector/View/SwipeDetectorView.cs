using UnityEngine.EventSystems;
using UnityEngine;

public class SwipeDetectorView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private SwipeDetectorViewModel _viewModel;
    public ReactiveProperty<SwipeParams> SwipeParams => _viewModel.SwipeParamsView;

    public void Init(SwipeDetectorViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _viewModel.StartSwipe(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _viewModel.EndSwipe(eventData.position);
    }

    private void OnDestroy()
    {
        _viewModel.Dispose();
    }
}