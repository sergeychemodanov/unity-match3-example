using System;
using UnityEngine;
using UnityEngine.UI;
using TestCompany.Common.UI;

namespace TestCompany.Match3.UI
{
    public class WinWindow : WindowBase
    {
        [SerializeField] private Button _nextButton;

        [SerializeField] private Button _exitButton;

        private float _previousTimeScale;

        public void Initialize(Action onExitButtonClick, bool nextLevelExist = false, Action onNextButtonClick = null)
        {
            _exitButton.onClick.RemoveAllListeners();
            _exitButton.onClick.AddListener(() =>
            {
                onExitButtonClick?.Invoke();
                Close();
            });

            _nextButton.onClick.RemoveAllListeners();

            _nextButton.gameObject.SetActive(nextLevelExist);
            _nextButton.onClick.AddListener(() =>
            {
                onNextButtonClick?.Invoke();
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