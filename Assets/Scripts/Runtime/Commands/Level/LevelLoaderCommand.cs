 

using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Level
{
    public class LevelLoaderCommand
    {
        private Transform _levelHolder;

        internal LevelLoaderCommand(Transform levelHolder)
        {
            _levelHolder = levelHolder;
        }

        internal void Execute(byte levelIndex)
        {
            Object.Instantiate(Resources.Load<GameObject>($"Prefabs/LevelPrefabs/level {levelIndex}"), _levelHolder,
                true);
            CameraSignals.Instance.onSetCinemachineTarget?.Invoke(CameraTargetState.Player);
        }
    }
}