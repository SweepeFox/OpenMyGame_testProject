using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine;
using System.Linq;

public class GameView : MonoBehaviour
{
    private const float MOVE_CELL_DURATION = 0.25f;
    private const float DESTROY_CELL_DURATION = 0.75f;

    [SerializeField] private float detectSwipeMinDistance;

    [SerializeField] private SwipeDetectorView _swipeDetectorView;
    [SerializeField] private GamefieldView _gamefieldView;

    private SwapCellsViewHandler _swalCellsHandler;
    private DestroyCellsViewHandler _destroyCellsHandler;

    private CellsFactory _cellsFactory;
    private GameViewModel _viewModel;
    private CellView[,] _cells;

    public void Init(GameViewModel viewModel, CellsFactory cellsFactory)
    {
        _viewModel = viewModel;
        _cellsFactory = cellsFactory;

        _cells = new CellView[_viewModel.Rows, _viewModel.Columns];

        var gamefieldModel = new GamefieldModel();
        var gamefieldViewModel = new GamefieldViewModel(gamefieldModel);
        _gamefieldView.Init(_viewModel.Rows, _viewModel.Columns, gamefieldViewModel);

        var swipeDetectorModel = new SwipeDetectorModel(detectSwipeMinDistance);
        var swipeDetectorViewModel = new SwipeDetectorViewModel(swipeDetectorModel);
        _swipeDetectorView.Init(swipeDetectorViewModel);

        _swalCellsHandler = new SwapCellsViewHandler();
        _destroyCellsHandler = new DestroyCellsViewHandler();

        _viewModel.MoveCellsView.OnChanged += OnMoveCellsChanged;
        _viewModel.DestroyCellsView.OnChanged += OnDestroyCellsChanged;
        _swipeDetectorView.SwipeParams.OnChanged += OnSwipe;

        StartCoroutine(CreateCells());
    }

    private IEnumerator CreateCells()
    {
        for (int i = _viewModel.Rows - 1; i >= 0; i--)
        {
            for (int j = 0; j < _viewModel.Columns; j++)
            {
                var cellType = (CellType)_viewModel.FieldView.Value[i, j];

                var cellModel = new CellModel(i, j);
                var cellViewModel = new CellViewModel(cellModel);
                var cellView = Instantiate(_cellsFactory.GetCellByType(cellType), _gamefieldView.transform);

                cellView.Init(cellViewModel);
                _cells[i, j] = cellView;
            }
        }

        yield return new WaitForEndOfFrame();

       foreach (var cell in _cells)
        {
            _gamefieldView.CellsPositions[cell.Row, cell.Column] = cell.transform.position;
        }

        _gamefieldView.DisableLayoutGroup();
        _viewModel.CheckFallingCells();
    }

    private void OnMoveCellsChanged(List<MoveCellParams> moveCells)
    {
        foreach (var moveCellData in moveCells)
        {
            var cell1 = _cells[moveCellData.rowIndex1, moveCellData.columnIndex1];
            var cell2 = _cells[moveCellData.rowIndex2, moveCellData.columnIndex2];

            _cells[moveCellData.rowIndex1, moveCellData.columnIndex1] = cell2;
            _cells[moveCellData.rowIndex2, moveCellData.columnIndex2] = cell1;

            cell1.SetGamefieldPosition(moveCellData.rowIndex2, moveCellData.columnIndex2);
            cell2.SetGamefieldPosition(moveCellData.rowIndex1, moveCellData.columnIndex1);

            var tempSiblingIndex = cell1.transform.GetSiblingIndex();
            cell1.transform.SetSiblingIndex(cell2.transform.GetSiblingIndex());
            cell2.transform.SetSiblingIndex(tempSiblingIndex);

            var cellsToTarget = Mathf.Abs(moveCellData.rowIndex1 - moveCellData.rowIndex2) + Mathf.Abs(moveCellData.columnIndex1 - moveCellData.columnIndex2);

            _swalCellsHandler.AddCellsForSwap(new MoveCellsViewParams()
            {
                Cell1 = cell1,
                Cell2 = cell2,
                Cell1StartPosition = _gamefieldView.CellsPositions[moveCellData.rowIndex1, moveCellData.columnIndex1],
                Cell2StartPosition = _gamefieldView.CellsPositions[moveCellData.rowIndex2, moveCellData.columnIndex2],
                Duration = MOVE_CELL_DURATION * cellsToTarget,
                StartTime = Time.time
            });
        }
    }

    private void OnDestroyCellsChanged(List<DestroyCellParams> destroyCells)
    {
        foreach (var destroyCellData in destroyCells)
        {
            var cell = _cells[destroyCellData.row, destroyCellData.column];
            if (cell != null)
            {
                _destroyCellsHandler.AddCellForDestroy(new DestroyCellViewParams
                {
                    Cell = cell,
                    StartTime = Time.time,
                    Duration = DESTROY_CELL_DURATION
                });

                cell.PlayDeathAnimation(DESTROY_CELL_DURATION);
            }
        }
    }

    private void OnSwipe(SwipeParams swipeParams)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = swipeParams.startPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        if (results.Count == 0)
        {
            return;
        }

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
            {
                if (result.gameObject.TryGetComponent(out CellView cell))
                {
                    _viewModel.MoveCell(cell, swipeParams.direction);
                    break;
                }
            }
        }
    }

    #region MonoBehaviour
    private void Update()
    {
        var swapCellsStatus = _swalCellsHandler.Handle(_viewModel);
        if (swapCellsStatus == HandlerStatus.FINISHED)
        {
            if (!_viewModel.CheckFallingCells())
            {
                _viewModel.CheckCombinations();
            }
        }

        var destroyCellsStatus = _destroyCellsHandler.Handle(_viewModel);
        if (destroyCellsStatus == HandlerStatus.FINISHED)
        {
            var notEmptyBlocksCount = 0;
            for (int row = 0; row < _cells.GetLength(0); row++)
            {
                for (int col = 0; col < _cells.GetLength(1); col++)
                {
                    if (_viewModel.FieldView.Value[row, col] != 0)
                    {
                        notEmptyBlocksCount++;
                    }
                }
            }

            if (notEmptyBlocksCount == 0)
            {
                _viewModel.OnLevelCompleted();
                return;
            }

            _viewModel.CheckFallingCells();
        }
    }

    private void OnDestroy()
    {
        _viewModel.MoveCellsView.OnChanged -= OnMoveCellsChanged;
        _viewModel.DestroyCellsView.OnChanged -= OnDestroyCellsChanged;
        _swipeDetectorView.SwipeParams.OnChanged -= OnSwipe;

        _viewModel.Dispose();
    }
    #endregion
}