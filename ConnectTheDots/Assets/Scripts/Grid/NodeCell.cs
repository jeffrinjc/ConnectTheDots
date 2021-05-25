using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ConnectTheDots.Gameplay
{
    /// <summary>
    /// Component attached to the Nodes in the Grid
    /// </summary>
    public class NodeCell : Cell
    {
        internal void ResetNode()
        {
            _sprite.color = Color.white;
        }    
    }
}