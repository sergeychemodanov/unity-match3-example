using System;
using System.Collections.Generic;
using System.Linq;
using TestCompany.Common;
using TestCompany.Common.UI;
using TestCompany.Match3.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TestCompany.Match3.UI
{
    public class LevelInfoWindow : WindowBase
    {
        [SerializeField] private Button _pauseButton;

        [SerializeField] private Text _looseConditionValueText;

        [SerializeField] private LevelInfoWindowItem _targetItemPrefab;

        [SerializeField] private Transform _targetItemsContainer;

        private IGameOverChecker _gameOverChecker;

        private IWinChecker _winChecker;

        private readonly List<LevelInfoWindowItem> _targetItems = new List<LevelInfoWindowItem>();


        public void Initialize(IGameOverChecker gameOverChecker, IWinChecker winChecker, Action onPauseButtonClick)
        {
            Clear();

            _pauseButton.onClick.AddListener(() => { onPauseButtonClick(); });

            _gameOverChecker = gameOverChecker;
            _gameOverChecker.OnRemainingValueChanged += OnLoseConditionCounterChange;
            OnLoseConditionCounterChange(_gameOverChecker.RemainingValue);

            _winChecker = winChecker;
            _winChecker.OnTargetsChange += OnTargetsChange;

            foreach (var targetItem in _winChecker.Targets)
            {
                var newTargetItem = Instantiate(_targetItemPrefab, _targetItemsContainer, false);
                newTargetItem.Initialize(targetItem);
                _targetItems.Add(newTargetItem);
            }
        }


        private void OnDestroy()
        {
            Clear();
        }

        private void Clear()
        {
            _pauseButton.onClick.RemoveAllListeners();

            if (_gameOverChecker != null)
                _gameOverChecker.OnRemainingValueChanged -= OnLoseConditionCounterChange;

            if (_winChecker != null)
                _winChecker.OnTargetsChange -= OnTargetsChange;

            _targetItemsContainer.ClearChildObjects();
            _targetItems.Clear();
        }

        private void OnLoseConditionCounterChange(int val)
        {
            _looseConditionValueText.text = val.ToString();
        }

        private void OnTargetsChange(List<Dto.Item> targets)
        {
            foreach (var target in targets)
            {
                var targetItem = _targetItems.FirstOrDefault(i => i.Item.StaticItem.Id == target.StaticItem.Id);
                if (targetItem != null)
                    targetItem.Initialize(target);
            }
        }
    }
}