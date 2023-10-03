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
            _transform.DOMoveY(5, 1)
                .SetEase(Ease.OutQuad).SetAutoKill();
            DOVirtual.DelayedCall(1, () =>
                _transform.DOMoveY(1, 1)
                    .SetEase(Ease.InOutCubic).SetAutoKill());
        }
    }
}