using UnityEngine;

public enum CellType
{
    EMPTY,
    FIRE,
    WATER
}

public enum HandlerStatus
{
    NONE,
    IN_PROGRESS,
    FINISHED
}

public enum SwipeDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class MoveCellsViewParams
{
    public CellView Cell1;
    public CellView Cell2;
    public Vector3 Cell1StartPosition;
    public Vector3 Cell2StartPosition;
    public float Duration;
    public float StartTime;
}

public class DestroyCellViewParams
{
    public CellView Cell;
    public float StartTime;
    public float Duration;
}

public class DestroyCellParams
{
    public int row;
    public int column;
}

public class MoveCellParams
{
    public int rowIndex1;
    public int columnIndex1;
    public int rowIndex2;
    public int columnIndex2;
}

public class SwipeParams
{
    public Vector2 startPosition;
    public SwipeDirection direction;
}

public class LevelParams
{
    public int[,] field;
}

public class SaveData
{
    public int level;
    public int[,] field;
}