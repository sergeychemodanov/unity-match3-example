using System;
using System.Linq;
using UnityEngine;
using TestCompany.Match3.Static;
using TestCompany.Match3.UI;

namespace TestCompany.Match3.Core
{
    public class GameController
    {
        public CellsField CellsField { get; }

        public IGameOverChecker GameOverChecker { get; private set; }

        public IWinChecker WinChecker { get; private set; }


        public GameController()
        {
            CellsField = new GameObject("CellsField").AddComponent<CellsField>();
            Initialize();
        }


        private void Initialize()
        {
            Clear();

            App.Instance
                .WindowManager
                .Show<StartWindow>()
                .Initialize(App.Instance.StaticDataProvider, PlayLevel);
        }

        private void OnGameOver()
        {
            App.Instance
                .WindowManager
                .Show<GameOverWindow>()
                .Initialize(Initialize);
        }

        private void OnWin()
        {
            var currentLevelNumber = CellsField.LevelData.Number;
            var isNextLevelExist = App.Instance
                .StaticDataProvider
                .Levels
                .Any(l => l.Number > currentLevelNumber);

            App.Instance
                .WindowManager
                .Show<WinWindow>()
                .Initialize(
                    Initialize,
                    isNextLevelExist,
                    () => {
                        Clear();

                        var nextLevel = App.Instance
                            .StaticDataProvider
                            .Levels
                            .FirstOrDefault(l => l.Number == currentLevelNumber + 1);

                        PlayLevel(nextLevel);
                    });
        }


        private void Clear()
        {
            if (GameOverChecker != null)
            {
                GameOverChecker.Clear();
                GameOverChecker = null;
            }

            if (WinChecker != null)
            {
                WinChecker.Clear();
                WinChecker = null;
            }

            CellsField.Clear();
            App.Instance.WindowManager.CloseAll();
        }


        private void PlayLevel(LevelData levelData)
        {
            CellsField.Play(levelData);

            switch (levelData.LoseConditionType)
            {
                case LoseConditionType.LimitedTime:
                    GameOverChecker = new LimitedTimeGameOverChecker(levelData.LoseConditionValue);
                    break;
                case LoseConditionType.LimitedMoves:
                    GameOverChecker = new LimitedMovesGameOverChecker(CellsField, levelData.LoseConditionValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            GameOverChecker.OnGameOver += OnGameOver;
            WinChecker = new TargetsWinChecker(CellsField, levelData.Targets.ToList());
            WinChecker.OnWin += OnWin;

            App.Instance
                .WindowManager
                .Show<LevelInfoWindow>()
                .Initialize(GameOverChecker, WinChecker, Pause);
        }

        private void Pause()
        {
            App.Instance
                .WindowManager
                .Show<PauseWindow>()
                .Initialize(Initialize);
        }
    }
}