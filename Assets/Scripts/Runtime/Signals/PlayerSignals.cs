using Runtime.Enums;
using Runtime.Extentions;
using Runtime.Managers;
using UnityEngine.Events;
using UnityEngine;

namespace Runtime.Signals
{
    public class PlayerSignals : MonoSingleton<PlayerSignals>
    {
        public UnityAction<PlayerAnimationStates> onChangePlayerAnimationState = delegate { };
        public UnityAction<bool> onPlayConditionChanged = delegate { };
        public UnityAction<bool> onMoveConditionChanged = delegate { };
        public UnityAction<int> onSetTotalScore = delegate { };

        public UnityAction<MaterialColorTypes> OnGatePassed = delegate {  };
    }
}