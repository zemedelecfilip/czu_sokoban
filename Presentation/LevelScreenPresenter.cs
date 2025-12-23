using System;
using System.Windows.Forms;
using czu_sokoban.BusinessLogic;

namespace czu_sokoban.Presentation
{
    /// <summary>
    /// Handles presentation logic for the level screen.
    /// </summary>
    public class LevelScreenPresenter
    {
        private readonly Panel _levelPanel;
        private readonly GameState _gameState;
        private readonly GameController _gameController;
        private Label _stepsLabel;
        private Label _timeLabel;

        public LevelScreenPresenter(Panel levelPanel, GameState gameState, GameController gameController)
        {
            _levelPanel = levelPanel ?? throw new ArgumentNullException(nameof(levelPanel));
            _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));
            _gameController = gameController ?? throw new ArgumentNullException(nameof(gameController));
        }

        public void InitializeLevelScreen(string levelName, Maps maps)
        {
            _gameState.CurrentLevelName = levelName;
            PrepareLevel(levelName, maps);
            CreateLabels();
        }

        private void PrepareLevel(string mapName, Maps maps)
        {
            var database = new PeopleDatabase();
            int[,] levelData = database.GetLevel(mapName);
            maps.AddObjectsToList(levelData, levelData.GetLength(0));
            maps.AddToControls(_levelPanel);
        }

        private void CreateLabels()
        {
            int screenWidth = Storage.ScreenWidth;
            int screenHeight = Storage.ScreenHeight;

            _stepsLabel = new Label
            {
                Text = "Steps: 0",
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(screenWidth / 18, screenHeight / 4),
                AutoSize = true,
                ForeColor = System.Drawing.Color.Black
            };
            _levelPanel.Controls.Add(_stepsLabel);

            _timeLabel = new Label
            {
                Text = "Time: 0.000 s",
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(screenWidth / 18, 11 * screenHeight / 28),
                AutoSize = true,
                ForeColor = System.Drawing.Color.Black
            };
            _levelPanel.Controls.Add(_timeLabel);
        }

        public void UpdateLabels()
        {
            if (_stepsLabel != null)
            {
                _stepsLabel.Text = $"Steps: {_gameState.StepsCount}";
            }

            if (_timeLabel != null)
            {
                _timeLabel.Text = $"Time: {_gameController.Stopwatch.Elapsed.TotalSeconds:F3} s";
            }
        }
    }
}

