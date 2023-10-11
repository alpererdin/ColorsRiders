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
        
        private List<GameObject> _tempStack;
         

        public StackToSizeCommand(StackManager stackManager, ref List<GameObject> collectableStack,
            ref List<GameObject> tempStack)
        {
            _stackManager = stackManager;
            _collectableStack = collectableStack;
            _levelHolder = GameObject.Find("LevelHolder").transform;
            _tempStack = tempStack;

        }

        public void Execute()
        
        {

         _collectableStack[0].transform.SetParent(_levelHolder.transform.GetChild(0));
        
        _collectableStack[0].transform.DOMove(
            new Vector3(_collectableStack[0].transform.position.x,
                _collectableStack[0].transform.position.y, _collectableStack[0].transform.position.z + 4),
            .2f).OnComplete(() =>
        {
            CoreGameSignals.Instance.onSizeUpPlayer?.Invoke(); 
           
            
        });
        _tempStack.Add(_collectableStack[0]);
        _collectableStack[0].SetActive(false);
        _collectableStack.RemoveAt(0);
        _collectableStack.TrimExcess();
        _stackManager.UpdateStack();
            
           // PlayerSignals.Instance.onSetTotalScore?.Invoke(-1);

           if (_collectableStack.Count == 0)
           {
               //Debug.Log("sonobje de gecebilirdi");
               Debug.Log("Start idle mover ");
               GameStateManager.SetGameState(GameStateManager.GameState.Idle);
               //
               PlayerSignals.Instance.onPlayConditionChanged?.Invoke(true);
               //++
               _collectableStack.TrimExcess();
                 
           }
        }
    }
}