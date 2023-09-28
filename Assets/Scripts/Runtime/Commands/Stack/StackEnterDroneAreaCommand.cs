using DG.Tweening;
using Runtime.Signals;
 
using System.Collections.Generic;
 
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands
{
    public class StackEnterDroneAreaCommand
    {
        private List<Transform> _collectable;
        private List<Transform> _tempList;
        
        public StackEnterDroneAreaCommand(ref List<Transform> collectable, ref List<Transform> tempList)
        {
            _collectable = collectable;
            _tempList = tempList;
        }

        public void OnStackEnterDroneArea(Transform collectable)
        {
            if (!_collectable.Contains(collectable)) return;
            _tempList.Add(collectable);
            _collectable.Remove(collectable);
            _collectable.TrimExcess();
            _tempList.TrimExcess();
           /* collectable.DOMove(
                    new Vector3(mat.position.x, collectable.position.y,
                        collectable.position.z + UnityEngine.Random.Range(6, 10)), 3f);
             */
            if (_collectable.Count == 0)
            {
                StackSignals.Instance.onLastCollectableEnterDroneArea?.Invoke();
            }
        }
    }
}