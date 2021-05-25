using ConnectTheDots.Common;
using ConnectTheDots.Gameplay;
using ConnectTheDots.Levels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectTheDots.UI
{
    /// <summary>
    /// UI to select the Levels loaded from Json
    /// </summary>
    public class LevelSelectionUIMenu : BaseUIPanel
    {
        [SerializeField] private LevelSelectionUIHead _levelSelectionPrefab;
        [SerializeField] private Transform _container;
        private LevelDataHandler _levelDataHandler;

        private void Start()
        {
            _levelDataHandler = new LevelDataHandler();
            InitializeUI();
        }

        private void InitializeUI()
        {
            foreach (var v_level in _levelDataHandler.GetLevelData().LevelList)
            {
                var v_levelUI = Instantiate(_levelSelectionPrefab, _container);
                v_levelUI.SetUI(v_level, OnLevelSelected);
            }
        }

        private void OnLevelSelected(Level level)
        {
            ReferenceFactory.Get<GridController>().Initialize(level);
            ReferenceFactory.Get<GameplayUIPanel>().Show();
            Close();
        }

        private void OnDestroy()
        {
            _levelDataHandler?.Dispose();
        }
    }
}