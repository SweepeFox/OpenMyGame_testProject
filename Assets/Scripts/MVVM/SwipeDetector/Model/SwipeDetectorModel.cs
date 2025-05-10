using UnityEngine;

public class SwipeDetectorModel
{
    public readonly ReactiveProperty<float> DetectSwipeMinDistance = new ();
    public readonly ReactiveProperty<SwipeParams> SwipeParams = new ();

    public SwipeDetectorModel(float detectSwipeMinDistance)
    {
        DetectSwipeMinDistance.Value = detectSwipeMinDistance;
    }
}