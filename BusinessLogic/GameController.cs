using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using czu_sokoban.Domain;

namespace czu_sokoban.BusinessLogic
{
    /// <summary>
    /// Controls game logic including movement, collision detection, and win condition checking.
    /// </summary>
    public class GameController
    {
        private readonly Maps _maps;
        private readonly GameState _gameState;
        private readonly PeopleDatabase _database;
        private readonly Stopwatch _stopwatch;

        public GameController(Maps maps, GameState gameState, PeopleDatabase database)
        {
            _maps = maps ?? throw new ArgumentNullException(nameof(maps));
            _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _stopwatch = new Stopwatch();
        }

        public Stopwatch Stopwatch => _stopwatch;

        public bool ProcessPlayerMovement(Keys direction)
        {
            if (_maps.Player == null)
            {
                return false;
            }

            int movementDelta = 0;

            switch (direction)
            {
                case Keys.Left:
                    movementDelta = ProcessLeftMovement();
                    break;
                case Keys.Right:
                    movementDelta = ProcessRightMovement();
                    break;
                case Keys.Up:
                    movementDelta = ProcessUpMovement();
                    break;
                case Keys.Down:
                    movementDelta = ProcessDownMovement();
                    break;
                default:
                    return false;
            }

            if (movementDelta > 0)
            {
                _gameState.IncrementSteps(movementDelta);
                if (!_stopwatch.IsRunning)
                {
                    _stopwatch.Start();
                }
            }

            return CheckWinCondition();
        }

        private int ProcessLeftMovement()
        {
            _maps.Player.MoveLeft();
            _maps.UpdateBackgroundImage(_maps.Player);

            if (_maps.CollidedPlayerWall(_maps.Player, _maps.Walls))
            {
                _maps.Player.MoveRight(true);
                _maps.UpdateBackgroundImage(_maps.Player);
                return 0;
            }

            Box collidedBox = _maps.CollidedPlayerBox(_maps.Player, _maps.Boxes);
            if (collidedBox != null)
            {
                return ProcessBoxMovementLeft(collidedBox);
            }

            return 1;
        }

        private int ProcessRightMovement()
        {
            _maps.Player.MoveRight();
            _maps.UpdateBackgroundImage(_maps.Player);

            if (_maps.CollidedPlayerWall(_maps.Player, _maps.Walls))
            {
                _maps.Player.MoveLeft(true);
                _maps.UpdateBackgroundImage(_maps.Player);
                return 0;
            }

            Box collidedBox = _maps.CollidedPlayerBox(_maps.Player, _maps.Boxes);
            if (collidedBox != null)
            {
                return ProcessBoxMovementRight(collidedBox);
            }

            return 1;
        }

        private int ProcessUpMovement()
        {
            _maps.Player.MoveUp();
            _maps.UpdateBackgroundImage(_maps.Player);

            if (_maps.CollidedPlayerWall(_maps.Player, _maps.Walls))
            {
                _maps.Player.MoveDown(true);
                _maps.UpdateBackgroundImage(_maps.Player);
                return 0;
            }

            Box collidedBox = _maps.CollidedPlayerBox(_maps.Player, _maps.Boxes);
            if (collidedBox != null)
            {
                return ProcessBoxMovementUp(collidedBox);
            }

            return 1;
        }

        private int ProcessDownMovement()
        {
            _maps.Player.MoveDown();
            _maps.UpdateBackgroundImage(_maps.Player);

            if (_maps.CollidedPlayerWall(_maps.Player, _maps.Walls))
            {
                _maps.Player.MoveUp(true);
                _maps.UpdateBackgroundImage(_maps.Player);
                return 0;
            }

            Box collidedBox = _maps.CollidedPlayerBox(_maps.Player, _maps.Boxes);
            if (collidedBox != null)
            {
                return ProcessBoxMovementDown(collidedBox);
            }

            return 1;
        }

        private int ProcessBoxMovementLeft(Box box)
        {
            box.MoveLeft();
            _maps.UpdateBackgroundImage(box);

            if (IsBoxCollisionInvalid(box))
            {
                box.MoveRight();
                _maps.UpdateBackgroundImage(box);
                _maps.Player.MoveRight(true);
                _maps.UpdateBackgroundImage(_maps.Player);
                return 0;
            }

            return 0;
        }

        private int ProcessBoxMovementRight(Box box)
        {
            box.MoveRight();
            _maps.UpdateBackgroundImage(box);

            if (IsBoxCollisionInvalid(box))
            {
                box.MoveLeft();
                _maps.UpdateBackgroundImage(box);
                _maps.Player.MoveLeft(true);
                _maps.UpdateBackgroundImage(_maps.Player);
                return 0;
            }

            return 0;
        }

        private int ProcessBoxMovementUp(Box box)
        {
            box.MoveUp();
            _maps.UpdateBackgroundImage(box);

            if (IsBoxCollisionInvalid(box))
            {
                box.MoveDown();
                _maps.UpdateBackgroundImage(box);
                _maps.Player.MoveDown(true);
                _maps.UpdateBackgroundImage(_maps.Player);
                return 0;
            }

            return 0;
        }

        private int ProcessBoxMovementDown(Box box)
        {
            box.MoveDown();
            _maps.UpdateBackgroundImage(box);

            if (IsBoxCollisionInvalid(box))
            {
                box.MoveUp();
                _maps.UpdateBackgroundImage(box);
                _maps.Player.MoveUp(true);
                _maps.UpdateBackgroundImage(_maps.Player);
                return 0;
            }

            return 0;
        }

        private bool IsBoxCollisionInvalid(Box box)
        {
            Box collidedBox = _maps.CollidedBoxBox(box, _maps.Boxes);
            Box collidedWall = _maps.CollidedBoxWall(box, _maps.Walls);
            return collidedBox != null || collidedWall != null;
        }

        private bool CheckWinCondition()
        {
            bool hasWon = _maps.CheckWin(_maps.Boxes, _maps.FinalDestinations);
            
            if (hasWon)
            {
                _stopwatch.Stop();
                SaveLevelResult();
            }

            return hasWon;
        }

        private void SaveLevelResult()
        {
            double currentTime = Convert.ToDouble(_stopwatch.Elapsed.TotalSeconds.ToString("N3"));
            var bestResult = _database.GetLevelTimesAndStepsByPlayer(_gameState.CurrentSaveId, _gameState.CurrentLevelName);
            
            if (bestResult != null && bestResult.Count > 0)
            {
                double bestTime = Convert.ToDouble(bestResult[0].Time.ToString("N3"));
                
                if (bestTime > currentTime || bestTime == 0.0)
                {
                    _database.SetLevelTimeAndSteps(_gameState.CurrentSaveId, _gameState.CurrentLevelName, currentTime, _gameState.StepsCount);
                }
            }
        }

        public void ResetGame()
        {
            _gameState.Reset();
            _stopwatch.Reset();
        }
    }
}

