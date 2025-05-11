using UnityEngine;

public class CellViewInteractionZone : MonoBehaviour
{
    public CellView CellView { get; private set; }

    public void Init(CellView cellView)
    {
        CellView = cellView;
    }
}