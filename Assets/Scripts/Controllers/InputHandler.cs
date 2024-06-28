using Assets.Scripts.Controllers;
using Assets.Scripts.Enums;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class InputHandler : MonoBehaviour
    {
        private bool _initialized;

        private RtsInput _input;
        private RtsController _rtsController;
        private CameraController _cameraController;

        private Dictionary<InputAction, Action<InputAction.CallbackContext>> _actionHandlersPerformed;
        private Dictionary<InputAction, Action<InputAction.CallbackContext>> _actionHandlersCanceled;

        private void Awake()
        {
            _initialized = true;

            _input = new RtsInput();
            _rtsController = FindFirstObjectByType<RtsController>();
            _cameraController = FindFirstObjectByType<CameraController>();

            _actionHandlersCanceled = new()
            {
                { _input.Player.OnLeftMouseButton, _ => _rtsController.OnLeftMouseButttonCanceled() },
            };

            _actionHandlersPerformed = new()
            {
                { _input.Player.OnLeftMouseButton, _ => _rtsController.OnLeftMouseButtonPerformed() },

                { _input.Player.OnRightMouseButton, _ => _rtsController.OnRightMouseDownPerformed() },

                { _input.Player.CameraMoveUp, _ => _cameraController.Move(Direction.Up) },
                { _input.Player.CameraMoveDown, _ => _cameraController.Move(Direction.Down) },
                { _input.Player.CameraMoveLeft, _ => _cameraController.Move(Direction.Left) },
                { _input.Player.CameraMoveRight, _ => _cameraController.Move(Direction.Right) },

                { _input.Player.DeleteActor, _ => _rtsController.DeleteSelectedUnits() },

                { _input.Player.SelectGroup1, _ => _rtsController.SelectGroup(1) },
                { _input.Player.SelectGroup2, _ => _rtsController.SelectGroup(2) },
                { _input.Player.SelectGroup3, _ => _rtsController.SelectGroup(3) },
                { _input.Player.SelectGroup4, _ => _rtsController.SelectGroup(4) },
            };
        }

        private void Start()
        {
            _input.Enable();

            foreach (var kvp in _actionHandlersPerformed)
                kvp.Key.performed += kvp.Value;

            foreach (var kvp in _actionHandlersCanceled)
                kvp.Key.canceled += kvp.Value;
        }

        private void OnDestroy()
        {
            if (!_initialized)
                return;

            foreach (var kvp in _actionHandlersPerformed)
                kvp.Key.performed -= kvp.Value;
            
            foreach (var kvp in _actionHandlersCanceled)
                kvp.Key.canceled -= kvp.Value;

            _input.Disable();
        }
    }
}
