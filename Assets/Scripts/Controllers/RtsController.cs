using Assets.Scripts.Actors;
using Assets.Scripts.Enums;
using Assets.Scripts.Extensions;
using Assets.Scripts.UI;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using static Assets.Scripts.Extensions.DictionaryExtensions;

namespace Assets.Scripts.Controllers
{
    public class RtsController : MonoBehaviour
    {
        private Camera _camera;

        private Vector3 _mouseStart;
        private bool _leftButtonDown;

        private IEnumerable<Unit> _selectedUnits { get; set; }
        internal Dictionary<ActorType, GameObject> _playerEntities { get; set; }

        private Dictionary<int, IEnumerable<Unit>> _groups { get; set; }

        [SerializeField]
        private UIController _uiController;

        [SerializeField]
        private float _cameraDistance = 10.0f;

        private void Awake()
        {
            _camera = Camera.main;
            _uiController = GetComponent<UIController>();

            _selectedUnits = Enumerable.Empty<Unit>();
            _playerEntities = new();
            _groups = new();
        }

        private Ray GetMouseScreenPointRay()
            => _camera.ScreenPointToRay(Input.mousePosition);

        #region Unit Selection

        public void OnLeftMouseButtonPerformed()
        {
            _leftButtonDown = true;
            _mouseStart = Input.mousePosition;
            SelectedUnitsChanged();
        }

        private void SelectedUnitsChanged()
        {
            _uiController.SelectedUnitsText.text = _selectedUnits.Count().ToString();
            
            foreach (var unit in _selectedUnits)
            {
                unit.IsSelected = true;
            }
        }

        public void OnLeftMouseButttonCanceled()
        {
            _mouseStart = Vector3.zero;
            _leftButtonDown = false;
            _uiController.MouseDragRect = null;

        }

        private void Update()
        {
            if (_leftButtonDown)
            {
                GetSelectedUnits();
            }
        }

        private void Start()
        {
            var units = FindObjectsByType(typeof(Unit), FindObjectsSortMode.None).ToActorKeyValuePairs(ActorType.Unit);
            var buildings = FindObjectsByType(typeof(Building), FindObjectsSortMode.None).ToActorKeyValuePairs(ActorType.Building);

            _playerEntities = DictionaryExtensions.KeyValuePairsToDictionary(units, buildings);
        }

        private IEnumerable<GameObject> GetEntitiesOfType(ActorType actorType)
            => _playerEntities.Where(entity => entity.Key == actorType)
                              .Select(dict => dict.Value);
        private bool IsUnitInSearchArea(GameObject unit, Rect searchArea)
            => searchArea.Contains(_camera.WorldToScreenPoint(unit.transform.position));

        private void GetSelectedUnits()
        {
            Vector3 mouseEnd = Input.mousePosition;
            Rect selectionRect = RectExtensions.GetRectFromMousePositions(_mouseStart, mouseEnd);
            _uiController.MouseDragRect = selectionRect;

            var units = GetEntitiesOfType(ActorType.Unit);
            _selectedUnits = units.Where(unit => IsUnitInSearchArea(unit, selectionRect))
                                  .Select(unitGameObject => unitGameObject.GetComponent<Unit>());

            SelectedUnitsChanged();

            Debug.Log("Units found in rect: {units}", _selectedUnits.Count());
        }

        #endregion Unit Selection

        #region Unit Movement

        public void OnRightMouseDownPerformed()
        {
            Ray ray = GetMouseScreenPointRay();
            Debug.DrawRay(ray);

            bool targetHit = Physics.Raycast(ray, out RaycastHit hitInfo, _cameraDistance);

            if (targetHit)
            {
                Debug.Log("Right clicked at: {point}, target: {name}", hitInfo.collider.name, hitInfo.point);

                if (_selectedUnits.Any())
                {
                    Actor actor = hitInfo.collider.GetComponent<Actor>();
                    if (actor == null)
                    {
                        MoveUnits(hitInfo);
                    }
                }
            }
            else
                Debug.Log("No target found");
        }

        private void MoveUnits(RaycastHit hitInfo)
        {
            foreach (var unit in _selectedUnits)
                unit.Move(hitInfo.point);
        }

        #endregion Unit Movement

        #region Delete

        public void DeleteSelectedUnits()
        {
            foreach (var unit in _selectedUnits)
            {
                unit.Destroy();

                // TODO: Handle groups
            }
        }

        #endregion Delete

        public void SelectGroup(int group)
        {
            if (_groups.TryGetValue(group, out IEnumerable<Unit> units))
                _selectedUnits = units;
        }

        public void AssignToGroup()
        {

        }

    }
}