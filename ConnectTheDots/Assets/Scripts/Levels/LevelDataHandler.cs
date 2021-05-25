using ConnectTheDots.Common;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using System;

namespace ConnectTheDots.Levels
{
    /// <summary>
    /// Class to handle the Level Data Json
    /// </summary>
    public class LevelDataHandler : IDisposable
    {
        private LevelData _levelData;

        internal LevelData GetLevelData()
        {
            if (_levelData == null)
                _levelData = LoadDataFromJson();

            return _levelData;
        }

        private LevelData LoadDataFromJson()
        {
            var v_levelDataJson = Resources.Load<TextAsset>(GameUtility.LevelDataFileName);
            if (v_levelDataJson == null ||  string.IsNullOrEmpty(v_levelDataJson.text))
            {
                Debug.LogError("Error Loading the Level Data Json");
                return null;
            }

            try
            {
                _levelData = JsonConvert.DeserializeObject<LevelData>(v_levelDataJson.text);
                return _levelData;
            }
            catch (Exception exception)
            {
                Debug.LogFormat("Exception Caught On Parsing -> {0}", exception.ToString());
                return null;
            }
        }

        public void Dispose()
        {
            _levelData = null;
        }
    }
}