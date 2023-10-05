using System.Collections.Generic;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;
using DG.Tweening;
namespace Runtime.Commands.Stack
{
    public class StackToSizeCommand
    {
      
        private StackManager _stackManager;
        private List<GameObject> _collectableStack;
        private Transform _levelHolder;
        private PlayerManager _playerManager;
         

        public StackToSizeCommand(StackManager stackManager, ref List<GameObject> collectableStack,PlayerManager playerManager)
        {
            _stackManager = stackManager;
            _collectableStack = collectableStack;
            _levelHolder = GameObject.Find("LevelHolder").transform;
            _playerManager = playerManager;
        }

        public void Execute()
        {

            int score = _playerManager._score;

            for (int i = 0; i < score; i++)
            {

                _collectableStack[0].transform.SetParent(_levelHolder.transform.GetChild(0));
                _collectableStack[0].transform
                    .DOMove(
                        new Vector3(_collectableStack[0].transform.position.x,
                            _collectableStack[0].transform.position.y, _collectableStack[0].transform.position.z + 4),
                        .2f).OnComplete(() => { CoreGameSignals.Instance.onSizeUpPlayer?.Invoke(); });

                _collectableStack.RemoveAt(0);
                _collectableStack.TrimExcess();
                _stackManager.UpdateStack();
                PlayerSignals.Instance.onSetTotalScore?.Invoke(-1);
            }


        }
    }
}