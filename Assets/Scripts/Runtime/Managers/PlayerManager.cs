using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Commands;
using Runtime.Commands.Stack;
using Runtime.Controllers.Player;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Signals;

using UnityEngine;
using MK.Toon;

namespace Runtime.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables
        public  Material _material;
 
        public MaterialColorTypes currentColor;
        #region Serialized Variables

        [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private PlayerPhysicsController physicsController;
        [SerializeField] private PlayerMeshController meshController;
       
        public AudioClip impact;
        public AudioClip finishSound;
        public AudioClip pic;
        public AudioClip pics;
        public AudioClip sound3;
        public AudioClip sound7;
        
        
        public AudioSource audioSource;
        #endregion

        #region Private Variables

        private JumpCommand _jumpCommand;
        private PlayerData _data;
        private StackData _stackDatadata;
        private const string PlayerDataPath = "Data/CD_Player";
        private ColorData _colorData;

        public ParticleSystem front,back;

      //  public ParticleSystem one;
 
        public int _score=1;
   
        #endregion

        #endregion

        private void Awake()
        {
            _data = GetPlayerData();
            SendPlayerDataToControllers();
            GetPlayerColorData();
            Init();
        }

        
        public void Init()
        {
            _jumpCommand = new JumpCommand(ref _stackDatadata,transform);
            
            _material.color = _colorData.Color;
            
             
             
        }

        private void GetPlayerColorData() => _colorData = Resources.Load<CD_Color>("Data/CD_Color").Colors[(int)currentColor];

        private PlayerData GetPlayerData() => Resources.Load<CD_Player>(PlayerDataPath).Data;

        #region DeleteThis

        private void SendPlayerDataToControllers()
        {
            movementController.SetMovementData(_data.MovementData);
            meshController.SetData(_data.MeshData);
        }

        public void PlayerSpeedStageArea()
        {
            _data.MovementData.ForwardSpeed = _data.MovementData.ForwardSpeed / 2f;
            //_data.MovementData.RotateSpeed = _data.MovementData.RotateSpeed / 2f;
            //_data.MovementData.SidewaysSpeed = _data.MovementData.SidewaysSpeed / 2f;
            SendPlayerDataToControllers();
        }
        public void PlayerSpeedExitStageArea()
        {
            _data.MovementData.ForwardSpeed = _data.MovementData.ForwardSpeed * 2f;
            //
         //   _data.MovementData.RotateSpeed = _data.MovementData.RotateSpeed * 2f;
           // _data.MovementData.SidewaysSpeed = _data.MovementData.SidewaysSpeed * 2f;
            SendPlayerDataToControllers();
        }
        public void PlayerDroneStageArea()
        {
          //  _data.MovementData.RotateSpeed = 0f;
            //_data.MovementData.ForwardSpeed =0f;
           // SendPlayerDataToControllers();
        
            _data.MovementData.ForwardSpeed = 14f;  
            SendPlayerDataToControllers();
    
            StartCoroutine(FinishStageArea());
        }

        private void PlayerRealDroneStage()
        {
            _data.MovementData.ForwardSpeed = 0f;
            SendPlayerDataToControllers();
            
        }

        private IEnumerator FinishStageArea()
        {
            float targetSpeed = 0f;
            float duration = 2f;

            float startSpeed = _data.MovementData.ForwardSpeed;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float newSpeed = Mathf.Lerp(startSpeed, targetSpeed, elapsedTime / duration);
                _data.MovementData.ForwardSpeed = newSpeed;
                SendPlayerDataToControllers();

                elapsedTime += Time.deltaTime;
                yield return null;
            }
 
            _data.MovementData.ForwardSpeed = targetSpeed;
            SendPlayerDataToControllers();
        }
        public void PlayerExitDroneStageArea()
        {
            Vector3 targetPosition = new Vector3(0,transform.position.y, transform.position.z + 20f);
            gameObject.transform.DOMove(targetPosition, 1)
                .OnComplete(() =>
                {
                    _data.MovementData.ForwardSpeed = 14f;
                   // _data.MovementData.RotateSpeed = 20f;
                    SendPlayerDataToControllers();
                });
       
        }

        #endregion
        
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onInputTaken += () => PlayerSignals.Instance.onMoveConditionChanged?.Invoke(true);
            InputSignals.Instance.onInputReleased += () => PlayerSignals.Instance.onMoveConditionChanged?.Invoke(false);
            InputSignals.Instance.onInputDragged += OnInputDragged;
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onLevelSuccessful +=
                () => PlayerSignals.Instance.onPlayConditionChanged?.Invoke(true);
            CoreGameSignals.Instance.onLevelFailed +=
                () => PlayerSignals.Instance.onPlayConditionChanged?.Invoke(false);
            CoreGameSignals.Instance.onReset += OnReset;
            PlayerSignals.Instance.onSetTotalScore += OnSetTotalScore;
            CoreGameSignals.Instance.onMiniGameEntered += OnMiniGameEntered;
            PlayerSignals.Instance.OnGatePassed += OnChangePlayerColor;
            CoreGameSignals.Instance.onEnterDroneArea += OnEnterDroneArea;
            CoreGameSignals.Instance.onExitDroneArea += OnExitDroneArea;
            StackSignals.Instance.JumperArea += _jumpCommand.Execute;
            CoreGameSignals.Instance.onSizeUpPlayer += anotherCommand;
            CoreGameSignals.Instance.onSizeDownPlayer += renameThisCommand;
            CoreGameSignals.Instance.onOutLineKiller += outlineKiller;
            StackSignals.Instance.playPicSound  += picSound;
            StackSignals.Instance.playPicsSound  += picsSound;

            PlayerSignals.Instance.DroneAreaPlayerMove += moveOnDroneArea;


        }

        private void moveOnDroneArea()
        {
            transform.DOMove(new Vector3(transform.position.x,
                transform.position.y,
                transform.position.z + 8), 2).OnComplete(() =>
            {
                StackSignals.Instance.onLastCollectableEnterDroneArea?.Invoke();
            });
        }

        private void picSound()
        {
            audioSource.PlayOneShot(pic,.7F);
        }
        private void picsSound()
        {
            audioSource.PlayOneShot(pics,1F);
        }


        public void outlineKiller(OutlineData type)
        {
            if (type == OutlineData.Normal)
            {
                _material.DOFloat(0, "_OutlineSize", 1f);
                return;
            }
            else if (type == OutlineData.Baked)
            {
                _material.DOFloat(100, "_OutlineSize", 1f);
                return;
            }
        }

      /*  public void buildParticle()
        {
            one.Play();
             
        }*/
        public void playForwardParticle()
        {
            front.Play();
        }
        public void playbackParticle()
        {
            back.Play();
        }

        private void renameThisCommand()
        {
            meshController.ScaleDownPlayer();
        }

        private void anotherCommand()
        {
             meshController.ScaleUpPlayer();
             
        }

        private void OnExitDroneArea()
        {
            
            PlayerExitDroneStageArea();
            
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
           
            PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationStates.Run);
            DOVirtual.DelayedCall(.5f,
                () => CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStates.Follow));

            
           DOVirtual.DelayedCall(1.5f, 
               () =>StackSignals.Instance.onUpdateStack?.Invoke());
        
        }

        private void OnEnterDroneArea()
        {
         //   gatestate();
            DOVirtual.DelayedCall(.5f,
                () => CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStates.DroneArea));
           // StartCoroutine(WaitForFinal());
           //PlayerDroneStageArea();
           PlayerRealDroneStage();
          gameObject.transform.GetChild(0).gameObject.SetActive(false);
           
      
        }

        private void OnPlay()
        {
            PlayerSignals.Instance.onPlayConditionChanged?.Invoke(true);
            PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationStates.Run);
        }

        private void OnInputDragged(HorizontalInputParams inputParams)
        {
            movementController.UpdateInputValue(inputParams);
        }

        private void OnMiniGameEntered()
        {
            //PlayerSignals.Instance.onPlayConditionChanged?.Invoke(false);
            Debug.Log("delete w/ienum");
            DOVirtual.DelayedCall(.5f,
                () => CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStates.DroneArea));
            StartCoroutine(WaitForFinal());
        }

        private void ExitDrone()
        {
            DOVirtual.DelayedCall(3f,
                () =>PlayerDroneStageArea());
        }
        private void OnSetTotalScore(int value)
        {
            
            _score += value;
            audioSource.PlayOneShot(impact, 0.7F);
       
            meshController.SetTotalScore(_score);
        }

        public void playFinishSound()
        {
            audioSource.PlayOneShot(finishSound, 0.5F);
        }

        public void GateSound()
        {
            audioSource.PlayOneShot(sound3, 0.6F);
        }

        public void jumpSound()
        {
            audioSource.PlayOneShot(sound7, 0.6F);
        }
      
        private void OnReset()
        {
            _material.DOFloat(100, "_OutlineSize", 1f);
            movementController.OnReset();
            animationController.OnReset();
        }

        private void UnSubscribeEvents()
        {
            InputSignals.Instance.onInputTaken -= () => PlayerSignals.Instance.onMoveConditionChanged?.Invoke(true);
            InputSignals.Instance.onInputReleased -= () => PlayerSignals.Instance.onMoveConditionChanged?.Invoke(false);
            InputSignals.Instance.onInputDragged -= OnInputDragged;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onLevelSuccessful -=
                () => PlayerSignals.Instance.onPlayConditionChanged?.Invoke(true);
            CoreGameSignals.Instance.onLevelFailed -=
                () => PlayerSignals.Instance.onPlayConditionChanged?.Invoke(false);
            CoreGameSignals.Instance.onReset -= OnReset;
            PlayerSignals.Instance.onSetTotalScore -= OnSetTotalScore;
            CoreGameSignals.Instance.onMiniGameEntered -= OnMiniGameEntered;
            PlayerSignals.Instance.OnGatePassed -= OnChangePlayerColor;
            CoreGameSignals.Instance.onEnterDroneArea -= OnEnterDroneArea;
            CoreGameSignals.Instance.onExitDroneArea -= OnExitDroneArea;
            StackSignals.Instance.JumperArea -= _jumpCommand.Execute;
            CoreGameSignals.Instance.onSizeUpPlayer -= anotherCommand;
            CoreGameSignals.Instance.onSizeDownPlayer -= renameThisCommand;
            CoreGameSignals.Instance.onOutLineKiller -= outlineKiller;
            StackSignals.Instance.playPicSound  -= picSound;
            StackSignals.Instance.playPicsSound  -= picsSound;
          
            PlayerSignals.Instance.DroneAreaPlayerMove -= moveOnDroneArea;
        }

         private void OnChangePlayerColor(MaterialColorTypes type)
        {
         
            Color color = GetColorForMaterialColorType(type); 
            meshController.ChangeMaterialColor(color);
            currentColor = type;
        }

        
         
         private Color GetColorForMaterialColorType(MaterialColorTypes colorType)
         {
             switch (colorType)
             {
                 case MaterialColorTypes.red:
                     return Color.red;
                 case MaterialColorTypes.green:
                     return Color.green;
                 case MaterialColorTypes.yellow:
                     return Color.yellow;
                 default:
                     return Color.white;  
             }
         }


        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        internal void SetStackPosition()
        {
            var position = transform.position;
            Vector3 pos = new Vector3(position.x,position.y, position.z);
            StackSignals.Instance.onStackFollowPlayer?.Invoke(pos);
        } 
        internal void SetSizeStackPosition()
        {
            var position = transform.position;
            Vector3 pos = new Vector3(position.x,position.y, position.z);
            StackSignals.Instance.onSizeStackFollowPlayer?.Invoke(pos);
        } 
       
        
        private IEnumerator WaitForFinal()
        {
            ExitDrone();
            
            
           // PlayerSpeedStageArea();
          //  gameObject.SetActive(false);
             //gameObject.transform.GetChild(0).gameObject.SetActive(false);
             //PlayerDroneStageArea();
            
            //PlayerDroneStageArea();
            
           // PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationStates.Crouch);
            yield return new WaitForSeconds(1f);
            
           
           // PlayerExitDroneStageArea();
            Debug.Log("5sn bekledik");
            //gameObject.transform.position += new Vector3(0, 0, 10f);
            //gameObject.transform.GetChild(0).gameObject.SetActive(true);
            PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationStates.Run);
            // CoreGameSignals.Instance.onMiniGameStart?.Invoke();
        }
    }
}