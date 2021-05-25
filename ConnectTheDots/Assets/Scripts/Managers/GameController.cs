using ConnectTheDots.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConnectTheDots.UI;

namespace ConnectTheDots.Gameplay
{
    /// <summary>
    /// Handles the core gameplay functionalities
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        private List<LineData> _lineDataList = new List<LineData>();
        private NodeId _currentNodeId;
        private GameplayUIPanel _gameplayUIPanel;

        private void Start()
        {
            _gameplayUIPanel = ReferenceFactory.Get<GameplayUIPanel>();
        }

        internal void OnLineDrawStarted(GridCell gridCell)
        {
            if (gridCell == null || !gridCell.HasNode) return;
            _currentNodeId = gridCell.NodeData.NodeId;
            var v_lineData = _lineDataList.Get(_currentNodeId);
            if (v_lineData == null)     //Spawn a line renderer and keep track of it in the list.
            {
                var v_lineRenderer = Instantiate(_lineRenderer, transform);
                v_lineRenderer.material.color = GameUtility.GetNodeColor(_currentNodeId);

                v_lineData = new LineData(_currentNodeId, v_lineRenderer);
                _lineDataList.Add(v_lineData);
            }

            if (v_lineData.IsFlowCompleted())
                return;

            v_lineData.AddCell(gridCell);   //Add the cells for drawing the line on top of them.
            v_lineData.DrawLine();
        }

        internal void OnlineDrawing(GridCell gridCell)
        {
            var v_lineData = _lineDataList.Get(_currentNodeId);
            if (v_lineData == null) return;

            if (v_lineData.IsFlowCompleted() || IsDiagonalCell(gridCell)) return;   //If flow is completed or If its a Diagonal Cell then return.

            if (v_lineData.IsExistingCell(gridCell) || (gridCell.HasNode && gridCell.NodeData.NodeId != _currentNodeId)) //If line is drawn on a different colored Node then stop drawing the line.
            {
                _currentNodeId = NodeId.None;
                v_lineData.DeleteLine();
                return;
            }

            if (gridCell.LineColorId != NodeId.None && gridCell.LineColorId != _currentNodeId)  //If line overlaps with a different colored Line then delete it.
                _lineDataList.Get(gridCell.LineColorId)?.DeleteLine();

            v_lineData.AddCell(gridCell);   //Add the cells for drawing the line on top of them.
            v_lineData.DrawLine();
            CheckAndUpdateValues();
        }

        internal void OnLineDrawEnded(GridCell gridCell)
        {
            var v_lineData = _lineDataList.Get(_currentNodeId);

            if (v_lineData != null && !v_lineData.IsFlowCompleted())    //If flow is not completed delete the existing line.
                v_lineData.DeleteLine();

            _currentNodeId = NodeId.None;
            CheckAndUpdateValues();
        }

        /// <summary>
        /// Checks if the cell is Diagonal to the previous one
        /// </summary>
        private bool IsDiagonalCell(GridCell cell)
        {
            var v_lineData = _lineDataList.Get(_currentNodeId);
            var (row, column) = cell.GetCellId();

            var v_diagonalCells = new List<(int, int)> { (row + 1, column + 1), (row + 1, column - 1), (row - 1, column + 1), (row - 1, column - 1) };

            if (v_lineData.PreviousCell == null) return false;
            return v_diagonalCells.ContainsTuple(v_lineData.PreviousCell.GetCellId());
        }

        private void CheckAndUpdateValues()
        {
            var v_flowCount = 0;
            var v_cellsFilledCount = 0;
            foreach (var v_lineData in _lineDataList)
            {
                if (v_lineData.IsFlowCompleted()) v_flowCount += 1;
                v_cellsFilledCount += v_lineData.GetCellsFilledCount();
            }
            var v_pipeFlowValue = ((float)v_cellsFilledCount / (GameUtility.GridRowCount * GameUtility.GridColumnCount)) * 100;
            _gameplayUIPanel.UpdateValues(v_flowCount, v_pipeFlowValue);

            if (v_flowCount == GameUtility.GridRowCount && v_cellsFilledCount == GameUtility.GridRowCount * GameUtility.GridColumnCount) 
            {
                _gameplayUIPanel?.OnGameOver(); //Win Condition...
            }
        }

        public void ResetGame()
        {
            _lineDataList.ForEach(x => x.Dispose());
            _lineDataList.Clear();
            CheckAndUpdateValues();
        }

        private void OnDestroy()
        {
            _gameplayUIPanel = null;
            ReferenceFactory.Refresh();
        }
    }

    /// <summary>
    /// Class to handle all the Line functions
    /// </summary>
    public class LineData : IDisposable
    {
        internal NodeId colorId;
        private LineRenderer lineRenderer;
        private HashSet<GridCell> gridCells;
        internal GridCell PreviousCell { get; private set; }

        public LineData(NodeId nodeId, LineRenderer lineRenderer)
        {
            colorId = nodeId;
            this.lineRenderer = lineRenderer;
            gridCells = new HashSet<GridCell>();
        }

        public void AddCell(GridCell cell)
        {
            PreviousCell = cell;
            gridCells.Add(cell); 
        }

        public void DrawLine()
        {
            lineRenderer.positionCount = gridCells.Count;
            int index = 0;
            foreach (var v_cell in gridCells)
            {
                v_cell.SetLineColor(colorId);
                var v_pos = new Vector3(v_cell.CenterPoint.x, v_cell.CenterPoint.y, -1);
                lineRenderer.SetPosition(index, v_pos);
                index += 1;
            }
        }

        public void DeleteLine()
        {
            lineRenderer.positionCount = 0;
            foreach (var v_cell in gridCells)
                v_cell.SetLineColor(NodeId.None);
            gridCells.Clear();
        }

        public bool IsExistingCell(GridCell cell)
        {
            return gridCells.Contains(cell);
        }

        public bool IsFlowCompleted()
        {
            var v_nodeCounter = 0;
            foreach (var v_cell in gridCells)
            {
                if (v_cell.HasNode)
                    v_nodeCounter += 1;
            }
            return v_nodeCounter >= 2;
        }

        public int GetCellsFilledCount()
        {
            var v_count = 0;
            foreach (var v_cell in gridCells)
            {
                if (v_cell.LineColorId != NodeId.None)
                    v_count += 1;
            }
            return v_count;
        }

        public void Dispose()
        {
            PreviousCell = null;
            DeleteLine();
        }
    }
}