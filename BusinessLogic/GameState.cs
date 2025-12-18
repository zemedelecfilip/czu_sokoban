using System.Collections.Generic;

namespace czu_sokoban.BusinessLogic
{
    /// <summary>
    /// Represents the current state of the game.
    /// </summary>
    public class GameState
    {
        private int _stepsCount;
        private string _currentLevelName;
        private int _currentSaveId;

        public int StepsCount
        {
            get { return _stepsCount; }
            set { _stepsCount = value; }
        }

        public string CurrentLevelName
        {
            get { return _currentLevelName; }
            set { _currentLevelName = value; }
        }

        public int CurrentSaveId
        {
            get { return _currentSaveId; }
            set { _currentSaveId = value; }
        }

        public void Reset()
        {
            _stepsCount = 0;
        }

        public void IncrementSteps(int steps)
        {
            _stepsCount += steps;
        }
    }
}

