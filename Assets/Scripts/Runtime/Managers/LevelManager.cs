using DG.Tweening;
using Runtime.Commands.Level;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [Header("Holder")] [SerializeField] private Transform levelHolder;

        [Space] [SerializeField] private byte totalLevelCount;

        #endregion

        #region Private Variables

        private LevelLoaderCommand _levelLoader;
        private LevelDestroyerCommand _levelDestroyer;
        private byte _currentLevel;
        private LevelData _levelData;
        #endregion

        #endregion

        private void Awake()
        {
            Init();
            _currentLevel = GetActiveLevel();
            _levelData = GetLevelData();
        }
        private LevelData GetLevelData()
        {
            return Resources.Load<CD_Level>("Data/CD_Level").Levels[_currentLevel];
        }
        private byte GetActiveLevel()
        {
            return (byte)_currentLevel;
        }

        private void Init()
        {
       
            _levelLoader = new LevelLoaderCommand(levelHolder);
            _levelDestroyer = new LevelDestroyerCommand(levelHolder);
        }
        
       

        private void OnEnable()
        {
            SubscribeEvents();

            _currentLevel = GetLevelID();
            CoreGameSignals.Instance.onLevelInitialize?.Invoke(_currentLevel);
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += _levelLoader.Execute;
            CoreGameSignals.Instance.onClearActiveLevel += _levelDestroyer.Execute;
           CoreGameSignals.Instance.onGetLevelID += GetLevelID;
            CoreGameSignals.Instance.onNextLevel += OnNextLevel;
            CoreGameSignals.Instance.onRestartLevel += OnRestartLevel;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize -= _levelLoader.Execute;
            CoreGameSignals.Instance.onClearActiveLevel -= _levelDestroyer.Execute;
           CoreGameSignals.Instance.onGetLevelID -= GetLevelID;
            CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
            CoreGameSignals.Instance.onRestartLevel -= OnRestartLevel;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }


       /* private byte GetLevelID()
        {
            if (!ES3.FileExists()) return 0;
            return (byte)(ES3.KeyExists("Level") ? ES3.Load<int>("Level") % totalLevelCount : 0);
        }
*/
        private byte GetLevelID()
        {
            return (byte)((byte)_currentLevel % totalLevelCount);
        }

        private void OnNextLevel()
        {
            _currentLevel++;
            SaveSignals.Instance.onSaveGameData?.Invoke();
            CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
        }

        private void OnRestartLevel()
        {
            SaveSignals.Instance.onSaveGameData?.Invoke();
            CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
           CoreGameSignals.Instance.onLevelInitialize?.Invoke(GetLevelID());
        }
        private void Start()
        {
        CameraSignals.Instance.onSetCinemachineTarget?.Invoke(CameraTargetState.Player);
           // CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
            //CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 1);
        }
    }
}