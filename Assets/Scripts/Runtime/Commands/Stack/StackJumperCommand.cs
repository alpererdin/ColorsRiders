using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.ValueObject;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackJumperCommand
    {
        private StackData _data;
        private List<GameObject> _collectableStack;
        private Transform _levelHolder;

        public StackJumperCommand(StackData stackData, ref List<GameObject> collectableStack)
        {
            _data = stackData;
            _collectableStack = collectableStack;
            _levelHolder = GameObject.Find("LevelHolder").transform;
        }

        public void Execute(int last, int index)
        {
            for (int i = last; i > index; i--)
            {
               
                Vector3 newPosition = _collectableStack[i].transform.position;
                newPosition.z += _data.CollectableOffsetInStack; 
                _collectableStack[i].transform.DOMove(newPosition, 0.5f);

                
                SwapItems(i, i - 1);
            }
            
            
        }
        private void SwapItems(int indexA, int indexB)
        {
            GameObject temp = _collectableStack[indexA];
            _collectableStack[indexA] = _collectableStack[indexB];
            _collectableStack[indexB] = temp;
        }
    }
}