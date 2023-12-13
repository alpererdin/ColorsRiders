using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.Events;

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
             
            CollectableSignals.Instance.OnAfterDroneArea += onafter;
        }

        public void onafter()
        {
           
            animator.SetTrigger(CollectableAnimationStates.Run.ToString());
        }
        public void OnChangeAnimationState(CollectableAnimationStates animationState, GameObject collectableGameObject)
        {
            if (gameObject == collectableGameObject)
            {
                animator.SetTrigger(animationState.ToString());
            }
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
           // CollectableSignals.Instance. 
        }
    }
}