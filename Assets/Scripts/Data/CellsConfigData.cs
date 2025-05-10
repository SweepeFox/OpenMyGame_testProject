using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Data/CellsConfigData")]
public class CellsConfigData : ScriptableObject
{
    // TODO: temp, need use SerializableList package
    [SerializeField] private List<CellParams> _cells;

    public List<CellParams> Cells => _cells;
}

[Serializable]
public class CellParams
{
    public CellType Type;
    public CellView Prefab;
}