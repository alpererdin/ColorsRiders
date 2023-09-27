using System.Collections;
using System.Collections.Generic;
using Runtime.Commands.Stack;
using Runtime.Controllers.Player;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Signals;

using UnityEngine;

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

        
        #endregion

        #region Private Variables

        private PlayerData _data;
        private const string PlayerDataPath = "Data/CD_Player";
        private ColorData _colorData;

        private int _score=1;
        private byte _currentValue = 0;
     
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
             
            
            _material.color = _colorData.Color;
            
        }

        private void GetPlayerColorData() => _colorData = Resources.Load<CD_Color>("Data/CD_Color").Colors[(int)currentColor];

        private PlayerData GetPlayerData() => Resources.Load<CD_Player>(PlayerDataPath).Data;

        private void SendPlayerDataToControllers()
        {
            movementController.SetMovementData(_data.MovementData);
        }

        public void PlayerSpeedStageArea()
        {
            _data.MovementData.ForwardSpeed = _data.MovementData.ForwardSpeed / 2f;
            SendPlayerDataToControllers();
        }
        public void PlayerSpeedExitStageArea()
        {
            _data.MovementData.ForwardSpeed = _data.MovementData.ForwardSpeed * 2f;
            SendPlayerDataToControllers();
        }
        public void PlayerDroneStageArea()
        {
            _data.MovementData.ForwardSpeed = 0f;
            SendPlayerDataToControllers();
        } 
        public void PlayerExitDroneStageArea()
        {
            _data.MovementData.ForwardSpeed = 14f;
            SendPlayerDataToControllers();
        }
       
         
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
            StartCoroutine(WaitForFinal());
        }

        private void OnSetTotalScore(int value)
        {
            
            _score += value;
            meshController.SetTotalScore(_score);
        
        }

       
        private void OnReset()
        {
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
                 case MaterialColorTypes.blue:
                     return Color.blue;
                 case MaterialColorTypes.black:
                     return Color.black;
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
            Vector2 pos = new Vector2(position.x, position.z);
            StackSignals.Instance.onStackFollowPlayer?.Invoke(pos);
        }
        
        private IEnumerator WaitForFinal()
        {
           // PlayerSpeedStageArea();
          //  gameObject.SetActive(false);
             gameObject.transform.GetChild(0).gameObject.SetActive(false);
            
            //PlayerDroneStageArea();
            
           // PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationStates.Crouch);
            yield return new WaitForSeconds(3f);
           // PlayerExitDroneStageArea();
            Debug.Log("3sn bekledik");
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationStates.Run);
            // CoreGameSignals.Instance.onMiniGameStart?.Invoke();
        }
    }
}