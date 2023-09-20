using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Collectables
{
    public class CollectableAnimationController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Animator animator;

        #endregion

        #endregion

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CollectableSignals.Instance.onChangeCollectableAnimationState += OnChangeAnimationState;
        }

        private void OnChangeAnimationState(CollectableAnimationStates animationState)
        {
            animator.SetTrigger(animationState.ToString());
        }

        private void UnSubscribeEvents()
        {
            CollectableSignals.Instance.onChangeCollectableAnimationState -= OnChangeAnimationState;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        internal void OnReset()
        {
            CollectableSignals.Instance.onChangeCollectableAnimationState?.Invoke(CollectableAnimationStates.Idle);
        }
    }
}