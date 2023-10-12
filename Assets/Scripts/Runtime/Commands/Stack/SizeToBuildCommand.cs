using System.Collections.Generic;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;
using DG.Tweening;
namespace Runtime.Commands.Stack
{
    public class SizeToBuildCommand
    {
      
        private StackManager _stackManager;
        private List<GameObject> _collectableStack;
        private Transform _levelHolder;
        
        private List<GameObject> _tempStack;
         

        public SizeToBuildCommand(StackManager stackManager, ref List<GameObject> collectableStack,
            ref List<GameObject> tempStack)
        {
            _stackManager = stackManager;
            _collectableStack = collectableStack;
            _levelHolder = GameObject.Find("LevelHolder").transform;
            _tempStack = tempStack;

        }

        public void Execute(Transform target)
      {
          #region oldWay commandLines
/* Debug.Log(target);
            _tempStack[0].SetActive(true);

        
            _tempStack[0].transform.DOMove(
                new Vector3(_tempStack[0].transform.position.x,
                    _tempStack[0].transform.position.y, _tempStack[0].transform.position.z + 4),
                .2f).OnComplete(() =>
            {
                CoreGameSignals.Instance.onSizeDownPlayer?.Invoke(); 
           
            
            });
          
         
            _tempStack.RemoveAt(0);
            _tempStack.TrimExcess();
 
            
            PlayerSignals.Instance.onSetTotalScore?.Invoke(-1);*/
          

          #endregion
            
            if (_tempStack.Count == 0)
            {
                Debug.Log("No objects in _temp ");
                return;
            }
            GameObject objectToMove = _tempStack[0];
            _tempStack.RemoveAt(0);
            Vector3 targetPosition = target.position  ;  
            objectToMove.SetActive(true);
            objectToMove.transform.DOMove(targetPosition, 0.2f).OnComplete(() =>
            {
                CoreGameSignals.Instance.onSizeDownPlayer?.Invoke();
            });
            _tempStack.TrimExcess();
            PlayerSignals.Instance.onSetTotalScore?.Invoke(-1);

            if (_tempStack.Count == 0)
            {
                Debug.Log("new level");
                CoreGameSignals.Instance.onReset.Invoke();
                GameStateManager.SetGameState(GameStateManager.GameState.Runner);
                 CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
                _tempStack.TrimExcess();
                 
            }
      }
    }
}