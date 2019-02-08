using System;
using TestCompany.Common;
using UnityEngine;
using UnityEngine.UI;
using TestCompany.Common.UI;
using TestCompany.Match3.Static;

namespace TestCompany.Match3.UI
{
    public class StartWindow : WindowBase
    {
        [SerializeField] private Button _levelPrefab;

        [SerializeField] private Transform _levelsContainer;

        private Action<LevelData> _onLevelClick;

        public void Initialize(IStaticDataProvider staticDataProvider, Action<LevelData> onLevelClick)
        {
            Clear();
            _onLevelClick = onLevelClick;

            foreach (var staticLevel in staticDataProvider.Levels)
            {
                var levelButton = Instantiate(_levelPrefab, _levelsContainer, false);

                var text = levelButton.GetComponentInChildren<Text>();
                if (text != null)
                    text.text = staticLevel.Number.ToString();

                levelButton.onClick.AddListener(() =>
                {
                    _onLevelClick?.Invoke(staticLevel);
                    Close();
                });
            }
        }

        private void Clear()
        {
            _levelsContainer.ClearChildObjects();
        }
    }
}