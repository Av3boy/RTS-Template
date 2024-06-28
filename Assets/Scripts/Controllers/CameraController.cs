using UnityEngine;

using Assets.Scripts.Enums;

using System;

namespace Assets.Scripts.Controllers
{
    public class CameraController : MonoBehaviour
    {
        private Camera _camera;

        [SerializeField]
        private float _cursorMovementThreshold = 50.0f;

        [SerializeField]
        private float _cameraSpeed = 1.0f;

        private void Awake()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector3? newPosition = null;

            if (Screen.width - mousePosition.x <= _cursorMovementThreshold)
                newPosition = GetHorizontalMovementPosition(Direction.Left);
            else if (mousePosition.x <= _cursorMovementThreshold)
                newPosition = GetHorizontalMovementPosition(Direction.Right);
            
            if (mousePosition.y <= _cursorMovementThreshold)
                newPosition = GetVerticalMovementPosition(Direction.Down, newPosition ?? _camera.transform.position);
            else if (Screen.height - mousePosition.y <= _cursorMovementThreshold)
                newPosition = GetVerticalMovementPosition(Direction.Up, newPosition ?? _camera.transform.position);

            if (newPosition.HasValue)
                _camera.transform.position = GetNewCamPosition(newPosition.Value);
        }

        public void Move(Direction direction)
        {
            Vector3 newPosition = direction switch
            {
                Direction.Left => GetHorizontalMovementPosition(Direction.Left),
                Direction.Right => GetHorizontalMovementPosition(Direction.Right),
                Direction.Up => GetVerticalMovementPosition(Direction.Down, _camera.transform.position),
                Direction.Down => GetVerticalMovementPosition(Direction.Up, _camera.transform.position),
                _ => throw new InvalidOperationException("Unexpected direction matched."),
            };

            _camera.transform.position = GetNewCamPosition(newPosition);
        }

        private Vector3 GetHorizontalMovementPosition(Direction direction)
            => new Vector3(_camera.transform.position.x + (direction == Direction.Left ? _cameraSpeed : -_cameraSpeed),
                           _camera.transform.position.y,
                           _camera.transform.position.z);

        private Vector3 GetVerticalMovementPosition(Direction direction, Vector3 position)
            => new Vector3(position.x,
                           position.y,
                           position.z + (direction == Direction.Up ? _cameraSpeed : -_cameraSpeed));

        private Vector3 GetNewCamPosition(Vector3 newPosition)
            => Vector3.Slerp(_camera.transform.position, newPosition, _cameraSpeed * Time.deltaTime);
    }
}