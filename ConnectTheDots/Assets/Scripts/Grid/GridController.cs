using ConnectTheDots.Common;
using ConnectTheDots.Levels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectTheDots.Gameplay
{
    /// <summary>
    /// Class to handle all the Grid related functionalities 
    /// </summary>
    public class GridController : MonoBehaviour
    {
        [SerializeField] private float _gridSpacing;
        [SerializeField] private GridCell _gridCellPrefab;
        [SerializeField] private NodeCell _nodePrefab;

        private List<GridCell> _gridCells;
        private List<NodeCell> _nodes;
        private GameController _gameController;

        private void Start()
        {
            _gameController = ReferenceFactory.Get<GameController>();
        }

        internal void Initialize(Level level)
        {
            InitializeGrid();
            InitializeNodes(level.NodesList);
            _gameController.ResetGame();
        }

        /// <summary>
        /// Initializes the grid with the given spacing
        /// </summary>
        private void InitializeGrid()
        {            
            if (_gridCells != null && _gridCells.Count > 0)
            {
                _gridCells.ForEach(x => x.ResetGrid());
                return;
            }

            float xPos, yPos;
            _gridCells = _gridCells ?? new List<GridCell>();

            for (int row = 0; row < GameUtility.GridRowCount; row++)
            {
                yPos = row * _gridSpacing;
                for (int column = 0; column < GameUtility.GridColumnCount; column++)
                {
                    xPos = column * _gridSpacing;

                    var v_gridCell = Instantiate(_gridCellPrefab, transform);
                    v_gridCell.transform.localPosition = new Vector3(xPos, yPos, 0);
                    v_gridCell.SetCellId(row, column);
                    v_gridCell.AddEventListeners(OnNodeSelected, OnGridEnter, OnGridExit);
                    v_gridCell.gameObject.name = $"Grid: ({row}, {column})";

                    _gridCells.Add(v_gridCell);
                }
            }
        }
        
        /// <summary>
        /// Spawns all the nodes in the given positions 
        /// </summary>
        /// <param name="nodes"></param>
        private void InitializeNodes(List<Node> nodes)
        {
            _nodes = _nodes ?? new List<NodeCell>();
            ResetNodes();
            foreach (var v_node in nodes)
            {
                foreach (var v_gridCell in _gridCells)
                {
                    if (v_gridCell.IsEquals(v_node.FirstPos) || v_gridCell.IsEquals(v_node.SecondPos))
                    {
                        var v_nodeCell = Instantiate(_nodePrefab, transform);
                        v_nodeCell.transform.position = v_gridCell.transform.position;
                        v_nodeCell.SetLineColor(v_node.NodeId);
                        var v_cellId = v_gridCell.GetCellId();
                        v_nodeCell.SetCellId(v_cellId);
                        v_nodeCell.gameObject.name = $"Node: ({v_cellId.Item1}, {v_cellId.Item2})";

                        v_gridCell.SetNodeData(v_node);

                        _nodes.Add(v_nodeCell);
                    }
                }
            }
        }

        private void ResetNodes()
        {
            if (_nodes?.Count <= 0) return;
            foreach (var v_node in _nodes)
            {
                Destroy(v_node.gameObject);
            }
            _nodes?.Clear();
        }

        /// <summary>
        /// Callback when a Node is clicked
        /// </summary>
        private void OnNodeSelected(GridCell gridCell)
        {
            _gameController.OnLineDrawStarted(gridCell);
        }

        /// <summary>
        /// Callback when player is dragging on the grid cells
        /// </summary>
        private void OnGridEnter(GridCell gridCell)
        {
            _gameController.OnlineDrawing(gridCell);
        }

        /// <summary>
        /// Callback when dragging is stopped
        /// </summary>
        private void OnGridExit(GridCell gridCell)
        {
            _gameController.OnLineDrawEnded(gridCell);
        }
    }
}