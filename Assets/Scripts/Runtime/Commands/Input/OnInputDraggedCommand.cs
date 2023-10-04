using Runtime.Interfaces;
using Runtime.Data.ValueObject;
using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Runtime.Commands.Input
{
    public class OnInputDraggedCommand : ICommand
    {
        private readonly InputData _data;
        private Vector2 _mousePosition;
        private Vector2 _moveVector;
        private float _currentVelocity;

        public OnInputDraggedCommand(InputData data)
        {
            _data = data;
        }

        public void Execute()
        {
            if (Mouse.current != null)
            {
                Vector2 currentMousePosition = Mouse.current.position.ReadValue();
                Vector2 mouseDeltaPos = currentMousePosition - _mousePosition;

                if (mouseDeltaPos.x > _data.HorizontalInputSpeed)
                    _moveVector.x = _data.HorizontalInputSpeed / 2f * mouseDeltaPos.x;
                else if (mouseDeltaPos.x < -_data.HorizontalInputSpeed)
                    _moveVector.x = -_data.HorizontalInputSpeed / 2f * -mouseDeltaPos.x;
                else
                    _moveVector.x = Mathf.SmoothDamp(_moveVector.x, 0f, ref _currentVelocity, _data.HorizontalInputClampStopValue);

                _moveVector.x = mouseDeltaPos.x;

                _mousePosition = currentMousePosition;

                InputSignals.Instance.onInputDragged?.Invoke(new HorizontalInputParams()
                {
                    HorizontalInputValue = _moveVector.x,
                    HorizontalInputClampSides = _data.HorizontalInputClampNegativeSides
                });
            }
        }

        public void Execute(int value)
        {
            throw new System.NotImplementedException();
        }
    }
}