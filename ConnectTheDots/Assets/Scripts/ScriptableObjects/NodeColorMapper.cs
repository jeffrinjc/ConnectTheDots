using ConnectTheDots.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectTheDots.Gameplay
{
    /// <summary>
    /// Used to map customize the color of the nodes based on their color Ids
    /// </summary>
    [CreateAssetMenu(fileName = "NodeColorMapper", menuName = "ScriptableObject/NodeColorMapper")]
    public class NodeColorMapper : ScriptableObject
    {
        [SerializeField] private List<NodeColorData> _nodeColorMapper;

        public NodeColorData GetNodeColorData(NodeId nodeColor)
        {
            return _nodeColorMapper.Find(x => x.NodeColor == nodeColor);
        }
    }

    [Serializable]
    public class NodeColorData
    {
        public NodeId NodeColor;
        public Color Color;
    }
}