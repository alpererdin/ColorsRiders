using System;
using System.Collections.Generic;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Signals;
using Runtime.Managers;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private float _positionValuesX;

        private bool _isTouching;

        private float _currentVelocity;  
        private Vector2? _mousePosition;  
        private Vector3 _moveVector; //ref  

        [Header("Data")] private InputData _data;
        private bool _isFirstTimeTouchTaken;
        private bool _isAvailableForTouch;

        [SerializeField] public Joystick joyStick;
        [SerializeField] private GameObject joystickContainer;
        

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetInputData();
             
        }

        private void Start()
        {
            GameStateManager.SetGameState(GameStateManager.GameState.Runner);
            joystickContainer.SetActive(false);
        }
        private InputData GetInputData() => Resources.Load<CD_Input>("Data/CD_Input").Data;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onPlay += OnPlay;
            InputSignals.Instance.onChangeInputState += OnChangeInputState;
            
      
        }

        private void OnPlay()
        {
            _isAvailableForTouch = true;
        }


        private void OnChangeInputState(bool state)
        {
            _isAvailableForTouch = state;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onReset -= OnReset;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            InputSignals.Instance.onChangeInputState -= OnChangeInputState;
            
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }


        private void Update()
        {
           
          
            if (!_isAvailableForTouch) return;
            
            GameStateManager.GameState gameState = GameStateManager.CurrentGameState;

            if (gameState == GameStateManager.GameState.Idle)
            {
                
                joystickContainer.SetActive(true);
                InputSignals.Instance.onInputTaken?.Invoke();

                
            }
            else
            {
                joystickContainer.SetActive(false);
            }

            if (gameState == GameStateManager.GameState.Runner)
            {

                if (Input.GetMouseButtonUp(0) && !IsPointerOverUIElement())
                {
                    _isTouching = false;

                    InputSignals.Instance.onInputReleased?.Invoke();
                }

                if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
                {
                    _isTouching = true;
                    InputSignals.Instance.onInputTaken?.Invoke();
                    if (!_isFirstTimeTouchTaken)
                    {
                        _isFirstTimeTouchTaken = true;
                        InputSignals.Instance.onFirstTimeTouchTaken?.Invoke();
                    }

                    _mousePosition = Input.mousePosition;
                }

                if (Input.GetMouseButton(0) && !IsPointerOverUIElement())
                {
                    if (_isTouching)
                    {

                        if (_mousePosition != null)
                        {
                            Vector2 mouseDeltaPos = (Vector2)Input.mousePosition - _mousePosition.Value;


                            if (mouseDeltaPos.x > _data.HorizontalInputSpeed)
                                _moveVector.x = _data.HorizontalInputSpeed / 10f * mouseDeltaPos.x;
                            else if (mouseDeltaPos.x < -_data.HorizontalInputSpeed)
                                _moveVector.x = -_data.HorizontalInputSpeed / 10f * -mouseDeltaPos.x;
                            else
                                _moveVector.x = Mathf.SmoothDamp(_moveVector.x, 0f, ref _currentVelocity,
                                    _data.HorizontalInputClampStopValue);

                            _mousePosition = Input.mousePosition;

                            InputSignals.Instance.onInputDragged?.Invoke(new HorizontalInputParams()
                            {
                                HorizontalInputValue = _moveVector.x,
                                HorizontalInputClampSides = _data.HorizontalInputClampNegativeSides,
                            });
                        }

                    }
                }
            }

        }
        
        private bool IsPointerOverUIElement()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }

        private void OnReset()
        {
            _isTouching = false;
            _isFirstTimeTouchTaken = false;
        }
    }
}