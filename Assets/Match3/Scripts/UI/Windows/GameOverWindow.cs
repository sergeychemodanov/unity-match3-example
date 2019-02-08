using System;
using TestCompany.Common.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TestCompany.Match3.UI
{
    public class GameOverWindow : WindowBase
    {
        [SerializeField] private Button _exitButton;

        private float _previousTimeScale;

        public void Initialize(Action onExitButtonClick)
        {
            _exitButton.onClick.RemoveAllListeners();
            _exitButton.onClick.AddListener(() =>
            {
                onExitButtonClick?.Invoke();
                Close();
            });
        }

        private void Start()
        {
            _previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        private void OnDestroy()
        {
            Time.timeScale = _previousTimeScale;
        }
    }
}