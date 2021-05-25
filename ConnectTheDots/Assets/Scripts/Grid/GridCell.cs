using ConnectTheDots.Common;
using ConnectTheDots.Levels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ConnectTheDots.Gameplay
{
    /// <summary>
    /// Component attached to each Grid Cells of the Grid
    /// Handles the Touch Inputs using IPointer callbacks 
    /// </summary>
    public class GridCell : Cell, IPointerEnterHandler, IPointerUpHandler, IPointerDownHandler
    {
        private Action<GridCell> OnNodeSelected;
        private Action<GridCell> OnGridEnter;
        private Action<GridCell> OnGridExit;

        internal Node NodeData { get; private set; }
        internal bool HasNode => NodeData != null;
        internal NodeId LineColorId { get; private set; } 

        internal override void SetLineColor(NodeId lineColorId)
        {
            base.SetLineColor(lineColorId);
            LineColorId = lineColorId;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (HasNode)
                OnNodeSelected?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnGridEnter?.Invoke(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnGridExit?.Invoke(this);
        }

        internal void AddEventListeners(Action<GridCell> onNodeSelected, Action<GridCell> onGridEnter, Action<GridCell> onGridExit)
        {
            OnNodeSelected = onNodeSelected;
            OnGridEnter = onGridEnter;
            OnGridExit = onGridExit;
        }

        internal void SetNodeData(Node node)
        {
            NodeData = node;
        }

        internal void ResetGrid()
        {
            NodeData = null;
            LineColorId = NodeId.None;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnNodeSelected = null;
            OnGridEnter = null;
            OnGridExit = null;
        }
    }
}