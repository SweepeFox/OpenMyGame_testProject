using System.Linq;
using UnityEngine;

public class CellsFactory
{
    private readonly CellsConfigData _cellsConfigData;

    public CellsFactory(CellsConfigData cellsConfigData)
    {
        _cellsConfigData = cellsConfigData;
    }

    public CellView GetCellByType(CellType type)
    {
        return _cellsConfigData.Cells.Find(cellParams => cellParams.Type == type)?.Prefab;
    }
}