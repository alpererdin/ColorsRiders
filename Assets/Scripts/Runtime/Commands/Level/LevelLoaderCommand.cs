/*using Runtime.Enums;
using Runtime.Interfaces;
using Runtime.Managers;
using Runtime.Signals;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Commands.Level
{
    public class LevelLoaderCommand : ICommand
    {
        private readonly LevelManager _levelManager;

        public LevelLoaderCommand(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        public void Execute(byte parameter)
        {
            var resourceRequest = Resources.LoadAsync<GameObject>($"Prefabs/LevelPrefabs/level {parameter}");
            resourceRequest.completed += operation =>
            {
                var newLevel = Object.Instantiate(resourceRequest.asset.GameObject(),
                    Vector3.zero, Quaternion.identity);
                if (newLevel != null) newLevel.transform.SetParent(_levelManager.levelHolder.transform);
                CameraSignals.Instance.onSetCinemachineTarget?.Invoke(CameraTargetState.Player);
            };
        }
    }
}*/

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