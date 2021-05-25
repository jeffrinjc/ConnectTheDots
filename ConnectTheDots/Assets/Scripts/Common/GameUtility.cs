using ConnectTheDots.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectTheDots.Common
{
    /// <summary>
    /// Utility class with helper functions 
    /// </summary>
    public static class GameUtility
    {
        public static readonly int GridRowCount;
        public static readonly int GridColumnCount;

        static GameUtility()
        {
            GridRowCount = GridColumnCount = 5;
        }

        public const string LevelDataFileName = "LevelData";
        public const string NodeColorMapperFileName = "NodeColorMapper";

        private static NodeColorMapper _nodeColorMapper;

        public static Color GetNodeColor(NodeId nodeColor)  //Gets the Node Color of the specified Color Id
        {
            if (_nodeColorMapper == null)
                _nodeColorMapper = Resources.Load<NodeColorMapper>(NodeColorMapperFileName);

            return _nodeColorMapper?.GetNodeColorData(nodeColor)?.Color ?? Color.white;
        }

        public static bool Contains(this List<LineData> lineDataList, NodeId id)    //Extention method for LineData List
        {
            return lineDataList.Exists(x => x.colorId == id);
        }

        public static LineData Get(this List<LineData> lineDataList, NodeId id)    //Extention method for LineData List
        {
            return lineDataList.Find(x => x.colorId == id);
        }

        public static bool ContainsTuple(this List<(int, int)> list, (int, int) item)
        {
            foreach (var v_item in list)
            {
                if (v_item.Item1 == item.Item1 && v_item.Item2 == item.Item2)
                    return true;
            }
            return false;
        }
    }
}