using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.ValueObject;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackMaterialUpdate
    {
        private StackData _data;
        private List<GameObject> _collectableStack;
        private Transform _levelHolder;

        public StackMaterialUpdate(StackData stackData, ref List<GameObject> collectableStack)
        {
            _data = stackData;
            _collectableStack = collectableStack;
            _levelHolder = GameObject.Find("LevelHolder").transform;
        }


        public void Execute(int last, int index)
        {
            for (int i = last; i > index; i--)
            {
         
                _collectableStack[i].transform.DOJump(
                    new Vector3(
                        Random.Range(-_data.JumpItemsClampX, _data.JumpItemsClampX + 1),
                        .5f,
                        _collectableStack[i].transform.position.z + Random.Range(10, 15)),
                    _data.JumpForce,
                    Random.Range(1, 3), 0.05f
                );
                _collectableStack[i].transform.DOScale(Vector3.one, 0);
                _collectableStack.RemoveAt(i);
                _collectableStack.TrimExcess();
            }
        }
    }
}