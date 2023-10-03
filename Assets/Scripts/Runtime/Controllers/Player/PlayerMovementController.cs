using System;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Managers;
using Runtime.Signals;

using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private new Rigidbody rigidbody;

        #endregion

        #region Private Variables

        [Header("Data")]  private PlayerMovementData _data;
         private bool _isReadyToMove, _isReadyToPlay;
         private float _inputValue;
        private Vector2 _clampValues;
        [SerializeField] private GameStates currentGameState;
        private Vector3 _movementDirection;

        public void UpdateIdleInputValue(IdleInputParams inputParam) => _movementDirection = inputParam.joystickMovement;
        #endregion

        #endregion

        internal void SetMovementData(PlayerMovementData movementData)
        {
            _data = movementData;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PlayerSignals.Instance.onPlayConditionChanged += OnPlayConditionChanged;
            PlayerSignals.Instance.onMoveConditionChanged += OnMoveConditionChanged;
        }

        private void OnPlayConditionChanged(bool condition) => _isReadyToPlay = condition;
        private void OnMoveConditionChanged(bool condition) => _isReadyToMove = condition;

        private void UnSubscribeEvents()
        {
            PlayerSignals.Instance.onPlayConditionChanged -= OnPlayConditionChanged;
            PlayerSignals.Instance.onMoveConditionChanged -= OnMoveConditionChanged;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        public void UpdateInputValue(HorizontalInputParams inputParams)
        {
            _inputValue = inputParams.HorizontalInputValue;
            _clampValues = inputParams.HorizontalInputClampSides;
        }

        private void Update()
        {
            if (_isReadyToPlay)
            {
                manager.SetStackPosition();
            }
        }

        private void FixedUpdate()
        {
            if (_isReadyToPlay)
            {
                if (_isReadyToMove)
                {
                    if (currentGameState == GameStates.Runner)
                    {
                        Move();
                    }
                    else if (currentGameState == GameStates.Idle)
                    {
                        IdleMove();
                    }
                }
                else
                {
                    if (currentGameState == GameStates.Runner)
                    {
                        StopSideways();
                    }
                    else if (currentGameState == GameStates.Idle)
                    {
                        Stop();
                    }
                }
            }
            else
                Stop();
        }
        
        private void IdleMove()
        {
            Vector3 velocity = rigidbody.velocity;
            velocity = new Vector3(_movementDirection.x * _data.ForwardSpeed, velocity.y,
                _movementDirection.z * _data.ForwardSpeed);
            rigidbody.velocity = velocity;

            if (_movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(_movementDirection);
                transform.rotation = toRotation;
                return;
            }
        }

        private void Move()
        {
            var velocity = rigidbody.velocity;
            velocity = new Vector3(_inputValue * _data.SidewaysSpeed, velocity.y,
                _data.ForwardSpeed);
            rigidbody.velocity = velocity;

            Vector3 position;
            position = new Vector3(
                Mathf.Clamp(rigidbody.position.x, _clampValues.x,
                    _clampValues.y),
                (position = rigidbody.position).y,
                position.z);
            rigidbody.position = position;
        }

        private void StopSideways()
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, _data.ForwardSpeed);
            rigidbody.angularVelocity = Vector3.zero;
        }

        public void StopHorizontal()
        {
            _data.ForwardSpeed = 0;
            
        }

      
        private void Stop()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        public void OnReset()
        {
            Stop();
            _isReadyToPlay = false;
            _isReadyToMove = false;
        }
    }
}