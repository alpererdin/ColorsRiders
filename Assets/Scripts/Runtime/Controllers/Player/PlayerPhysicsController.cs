using System;
using DG.Tweening;
using Runtime.Commands.Stack;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Rigidbody managerRigidbody;
        [SerializeField] private PlayerManager manager;
        //[SerializeField] private GateManager gatemanager;
        
         

        #endregion

        #region Private Variables
        private Transform _stackHolder;

        private readonly string _obstacle = "Obstacle";
        private readonly string _atm = "ATM";
        private readonly string _collectable = "Collectable";
        private readonly string _conveyor = "Conveyor";
        private readonly string _stageArea = "StageArea";
        private readonly string _gate = "Gate";
        private readonly string _drone = "DroneArea";
        private readonly string _idle = "Idle";
        private readonly string _finish = "Finish";
        #endregion

        #endregion

        private void Awake()
        {
            _stackHolder= GameObject.Find("StackManager").transform;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(_stageArea))
            {
                PlayerSignals.Instance.onChangePlayerAnimationState(PlayerAnimationStates.Crouch);
               
            }
            
        }
       private void OnTriggerExit(Collider other)
       {
           if (other.CompareTag(_stageArea))
           {
               CoreGameSignals.Instance.onExitTurretArea?.Invoke();
               PlayerSignals.Instance.onChangePlayerAnimationState(PlayerAnimationStates.Run);
               manager.PlayerSpeedExitStageArea();
               // manager.SetStackPosition();
               
           }

           if (other.CompareTag(_drone))
           {
               Destroy(other.transform.parent.gameObject);
           }
       }

       private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_idle))
            {
                GameStateManager.SetGameState(GameStateManager.GameState.Idle);
            } 
            if (other.CompareTag(_finish))
            {
                CoreGameSignals.Instance.onSizeUpPlayer?.Invoke();
            }
            if (other.CompareTag("JumpArea"))
            {
               StackSignals.Instance.JumperArea?.Invoke();
            }
            if (other.CompareTag(_drone))
            {
               // CoreGameSignals.Instance.onMiniGameEntered?.Invoke();
                CoreGameSignals.Instance.onEnterDroneArea?.Invoke();
              
                
              /*   DOVirtual.DelayedCall(.5f,
                    () =>manager.PlayerSpeedStageArea());
               DOVirtual.DelayedCall(2f,
                    () =>manager.PlayerDroneStageArea());*/
             
                
                /*DOVirtual.DelayedCall(.5f,
                    () => CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStates.DroneArea));
                DOVirtual.DelayedCall(4.5f,
                    () => CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStates.Follow));*/
                //PlayerSignals.Instance.onChangePlayerAnimationState(PlayerAnimationStates.Run);
            
               // return;
                
            }
          /*  if (other.CompareTag(_stageArea))
            {
             
                manager.ForceCommand.Execute();
                CoreGameSignals.Instance.onStageAreaEntered?.Invoke();
                InputSignals.Instance.onDisableInput?.Invoke();

                DOVirtual.DelayedCall(3, () =>
                {
                    var result = other.transform.parent.GetComponentInChildren<PoolController>()
                        .TakeResults(manager.StageValue);

                    if (result)
                    {
                        CoreGameSignals.Instance.onStageAreaSuccessful?.Invoke(manager.StageValue);
                        InputSignals.Instance.onEnableInput?.Invoke();
                    }
                    else
                    {
                        CoreGameSignals.Instance.onLevelFailed?.Invoke();
                    }
                });
                return;
            }*/
          if (other.CompareTag(_stageArea))
          {
              //CoreGameSignals.Instance.onEnterTurretArea?.Invoke();
              PlayerSignals.Instance.onChangePlayerAnimationState(PlayerAnimationStates.Crouch);
              manager.PlayerSpeedStageArea();
          }

          if (other.CompareTag(_gate))
          {
              PlayerSignals.Instance.OnGatePassed?.Invoke(other.GetComponent<GateManager>().currentColorType);
           
          }
          if (other.CompareTag(_obstacle))
          {
              managerRigidbody.transform.DOMoveZ(managerRigidbody.transform.position.z - 10f, 1f)
                  .SetEase(Ease.OutBack);
              return;
          }

          if (other.CompareTag(_atm))
          {
              CoreGameSignals.Instance.onAtmTouched?.Invoke(other.gameObject);
              return;
          }

          if (other.CompareTag(_collectable) )
          {
               
              if (manager.currentColor == other.GetComponentInParent<CollectableManager>().currentColorType)
              {
                    
                  other.tag = "Collected";
                  StackSignals.Instance.onInteractionCollectable?.Invoke(other.transform.parent.gameObject);
                  PlayerSignals.Instance.onSetTotalScore?.Invoke(1);
              }
              else
              {
                  PlayerSignals.Instance.onPlayConditionChanged?.Invoke(false);
                  PlayerSignals.Instance.onMoveConditionChanged?.Invoke(false);
                  Destroy(other.transform.parent.gameObject);
                  managerRigidbody.transform.DOMoveZ(managerRigidbody.transform.position.z - 2.3f, .4f)
                      .SetEase(Ease.Linear).OnComplete(() =>
                      {
                          PlayerSignals.Instance.onPlayConditionChanged?.Invoke(true);
                          PlayerSignals.Instance.onMoveConditionChanged?.Invoke(true);
                          
                      });
                  StackSignals.Instance.onInteractionObstacle?.Invoke(_stackHolder.transform.gameObject);
                  PlayerSignals.Instance.onSetTotalScore?.Invoke(-1);
                  
              }
                
              return;
          }

          if (other.CompareTag(_conveyor))
          {
              CoreGameSignals.Instance.onMiniGameEntered?.Invoke();
              DOVirtual.DelayedCall(1.5f,
                  () => CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStates.MiniGame));
              DOVirtual.DelayedCall(2.5f,
                  () => CameraSignals.Instance.onSetCinemachineTarget?.Invoke(CameraTargetState.FakePlayer));
              return;
          }

             
            
        }
    }
}