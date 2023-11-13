using Runtime.Data.ValueObject;
using DG.Tweening;
using UnityEngine;

namespace Runtime.Commands
{
    public class JumpCommand
    {
        #region Self Variables

        #region Private Variables
        private StackData _playerData;
        private Transform _transform;
        #endregion
        #endregion
        
        public JumpCommand(ref StackData stackData,Transform transform)
        {
            _playerData = stackData;
            _transform = transform;
        }

        public void Execute()
        {
          
          
            _transform.DOMoveY(8, 2)
                .SetEase(Ease.OutExpo).SetAutoKill();
            
            DOVirtual.DelayedCall(1, () =>
               _transform.DOMoveY(1, 1)
                    .SetEase(Ease.InCubic).SetAutoKill());
            
          /*  DOVirtual.DelayedCall(1,()=> 
                _transform.DORotate( new Vector3(20,_transform.rotation.y,_transform.rotation.z), 1,RotateMode.Fast) );*/
        }

         
    }
}