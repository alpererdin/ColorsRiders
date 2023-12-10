 
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