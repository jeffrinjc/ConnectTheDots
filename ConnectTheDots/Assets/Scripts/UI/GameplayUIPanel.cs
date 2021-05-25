using ConnectTheDots.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectTheDots.UI
{
    public class GameplayUIPanel : BaseUIPanel
    {
        [SerializeField] private Text _flowValueText;
        [SerializeField] private Text _pipeValueText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _levelSelectButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private GameObject _gameOverScreen;

        public override void Show()
        {
            base.Show();
            _gameOverScreen.SetActive(false);
            _restartButton?.onClick.AddListener(OnRestartButtonClicked);
            _levelSelectButton?.onClick.AddListener(OnLevelSelectionClicked);
            _backButton?.onClick.AddListener(OnBackClicked);

            _flowValueText.text = $"0/{GameUtility.GridRowCount}";
            _pipeValueText.text = $"0%";
        }

        public override void Close()
        {
            base.Close();
            _restartButton?.onClick.RemoveListener(OnRestartButtonClicked);
            _levelSelectButton?.onClick.RemoveListener(OnLevelSelectionClicked);
            _backButton?.onClick.RemoveListener(OnBackClicked);
        }

        internal void UpdateValues(int flowCompleted, float pipeFilled)
        {
            _flowValueText.text = $"{flowCompleted}/{GameUtility.GridRowCount}";
            _pipeValueText.text = $"{pipeFilled}%";
        }

        internal void OnGameOver()
        {
            _gameOverScreen.SetActive(true);
        }

        private void OnBackClicked()
        {
            _gameOverScreen.SetActive(false);
            OnLevelSelectionClicked();
        }

        private void OnRestartButtonClicked()
        {
            ReferenceFactory.Get<Gameplay.GameController>()?.ResetGame();
        }

        private void OnLevelSelectionClicked()
        {
            ReferenceFactory.Get<LevelSelectionUIMenu>()?.Show();
        }
    }
}