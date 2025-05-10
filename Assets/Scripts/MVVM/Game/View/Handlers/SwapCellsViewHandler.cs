using System.Collections.Generic;
using UnityEngine;

public class SwapCellsViewHandler
{
    private readonly List<MoveCellsViewParams> _cellsForSwap = new List<MoveCellsViewParams>();
        private readonly List<MoveCellsViewParams> _cellsForSwapQeueu = new List<MoveCellsViewParams>();

    public HandlerStatus Handle(GameViewModel viewModel)
    {
        if (_cellsForSwapQeueu.Count == 0 && _cellsForSwap.Count == 0)
        {
            return HandlerStatus.NONE;
        }

        for (int i = _cellsForSwap.Count - 1; i >= 0; i--)
        {
            var data = _cellsForSwap[i];
            var swapTime = Mathf.Clamp01((Time.time - data.StartTime) / data.Duration);

            data.Cell1.transform.position = Vector3.Lerp(data.Cell1StartPosition, data.Cell2StartPosition, swapTime);
            data.Cell2.transform.position = Vector3.Lerp(data.Cell2StartPosition, data.Cell1StartPosition, swapTime);

            if (swapTime >= 1f)
            {
                _cellsForSwap.RemoveAt(i);

                viewModel.RemoveMoveCellFromModel(data.Cell1.Row, data.Cell1.Column);
                viewModel.RemoveMoveCellFromModel(data.Cell2.Row, data.Cell2.Column);

                if (_cellsForSwap.Count == 0)
                {
                    return HandlerStatus.FINISHED;
                }
            }
        }

        _cellsForSwap.AddRange(_cellsForSwapQeueu);
        _cellsForSwapQeueu.Clear();

        return HandlerStatus.IN_PROGRESS;
    }

    public void AddCellsForSwap(MoveCellsViewParams newCellsForSwap)
    {
        _cellsForSwapQeueu.Add(newCellsForSwap);
    }
}