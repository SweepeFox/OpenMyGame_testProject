using System.Collections.Generic;
using UnityEngine;

public class DestroyCellsViewHandler
{
    private readonly List<DestroyCellViewParams> _cellsForDestroy = new List<DestroyCellViewParams>();

    public HandlerStatus Handle(GameViewModel viewModel)
    {
        if (_cellsForDestroy.Count == 0)
        {
            return HandlerStatus.NONE;
        }

        for (int i = _cellsForDestroy.Count - 1; i >= 0; i--)
        {
            var data = _cellsForDestroy[i];
            var destroyTime = Mathf.Clamp01((Time.time - data.StartTime) / data.Duration);

            if (destroyTime >= 1f)
            {
                data.Cell.Break();
                _cellsForDestroy.RemoveAt(i);

                viewModel.RemoveDestroyingCellFromModel(data.Cell.Row, data.Cell.Column);

                if (_cellsForDestroy.Count == 0)
                {
                    return HandlerStatus.FINISHED;
                }
            }
        }

        return HandlerStatus.IN_PROGRESS;
    }

    public void AddCellForDestroy(DestroyCellViewParams cellForDestroy)
    {
        _cellsForDestroy.Add(cellForDestroy);
    }
}