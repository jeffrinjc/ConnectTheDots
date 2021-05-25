using ConnectTheDots.Levels;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectTheDots.UI
{
    public class LevelSelectionUIHead : MonoBehaviour
    {
        [SerializeField] private Text _levelId;
        [SerializeField] private Button _button;
        private Level _level;
        private Action<Level> OnLevelSelected;

        private void Start()
        {
            _button.onClick.AddListener(OnLevelButtonClicked);
        }

        internal void SetUI(Level level, Action<Level> onLevelSelected = null)
        {
            if (level == null) return;
            _level = level;
            OnLevelSelected = onLevelSelected;
            _levelId.text = _level.LevelId.ToString();
        }

        private void OnLevelButtonClicked()
        {
            OnLevelSelected?.Invoke(_level);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnLevelButtonClicked);
            _level = null;
        }
    }
}