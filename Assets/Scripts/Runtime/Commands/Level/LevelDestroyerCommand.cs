/*using Runtime.Interfaces;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands.Level
{
    public class LevelDestroyerCommand : ICommand
    {
        private readonly LevelManager _levelManager;

        public LevelDestroyerCommand(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        public void Execute()
        {
            Object.Destroy(_levelManager.levelHolder.transform.GetChild(0).gameObject);
        }
    }
}*/
using UnityEngine;

namespace Runtime.Commands.Level
{
    public class LevelDestroyerCommand
    {
        private Transform _levelHolder;

        internal LevelDestroyerCommand(Transform levelHolder)
        {
            _levelHolder = levelHolder;
        }

        internal void Execute()
        {
            if (_levelHolder.transform.childCount <= 0) return;
            Object.Destroy(_levelHolder.transform.GetChild(0).gameObject);
        }
    }
}