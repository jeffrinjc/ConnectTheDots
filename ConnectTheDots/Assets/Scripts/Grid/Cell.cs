using ConnectTheDots.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectTheDots.Gameplay
{
    /// <summary>
    /// Base class for the Grid cell and Node cell 
    /// Contains base functionalities of the cell
    /// </summary>
    public abstract class Cell : MonoBehaviour
    {
        protected (int, int) _cellId;
        protected SpriteRenderer _sprite;

        protected virtual void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }

        internal virtual void SetLineColor(NodeId lineColorId)
        {
            _sprite.color = GameUtility.GetNodeColor(lineColorId);
        }

        public Vector3 CenterPoint => _sprite.bounds.center;

        internal (int, int) GetCellId() => _cellId;

        internal void SetCellId(int row, int column)
        {
            _cellId = (row, column);
        }

        internal void SetCellId((int, int) cellId)
        {
            _cellId = cellId;
        }

        internal bool IsEquals(int row, int column)
        {
            return _cellId.Item1 == row && _cellId.Item2 == column;
        }

        internal bool IsEquals((int, int) cellId)
        {
            return _cellId.Item1 == cellId.Item1 && _cellId.Item2 == cellId.Item2;
        }

        protected virtual void OnDestroy() { }
    }
}