using ConnectTheDots.Common;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectTheDots.Levels
{
    /// <summary>
    /// Model class of the Level Data Json
    /// </summary>
    public class LevelData
    {
        [JsonProperty("levels")] private List<Level> _levelList;

        public List<Level> LevelList => _levelList;
    }


    public class Level
    {
        [JsonProperty("level_id")] private ushort _levelId;
        [JsonProperty("nodes")] private List<Node> _nodesList;

        public ushort LevelId => _levelId;
        public List<Node> NodesList => _nodesList;
    }


    public class Node
    {
        [JsonProperty("node_id")] private NodeId _nodeId;
        [JsonProperty("first_pos")] private GridPos _firstPos;
        [JsonProperty("second_pos")] private GridPos _secondPos;

        public NodeId NodeId => _nodeId;
        public (int, int) FirstPos => (_firstPos.Row, _firstPos.Column);
        public (int, int) SecondPos => (_secondPos.Row, _secondPos.Column);
    }

    public class GridPos
    {
        [JsonProperty("row")] private int _row;
        [JsonProperty("column")] private int _column;

        public int Row => _row;
        public int Column => _column;
    }
}