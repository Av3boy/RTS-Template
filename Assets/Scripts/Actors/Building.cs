using Assets.Scripts.Actors;
using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts
{
    public class Building : Actor
    {
        private RtsController _rtsController;

        public bool CanSpawnUnits => SpawnPoint != null;

        public Vector3 SpawnPoint { get; set; }

        private void Awake()
        {
            _rtsController = FindFirstObjectByType<RtsController>();
        }

        public void CreateUnit(Unit unit)
        {
            unit.transform.position = SpawnPoint;
            _rtsController._playerEntities[Enums.ActorType.Unit] = Instantiate(unit.gameObject);
        }
    }
}
