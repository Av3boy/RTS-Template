using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Actors
{
    public class Actor : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 5.0f;
        public float Speed { get => _speed; set => _speed = value; }

        [SerializeField]
        private float _attack = 10.0f;
        public float AttackDamage { get => _attack; set => _attack = value; }

        [SerializeField]
        private float _health = 100.0f;
        public float Health { get => _health; set => _health = value; }

        [SerializeField]
        private ActorType _actorType;
        public ActorType ActorType { get => _actorType; set => _actorType = value; }

        public IEnumerable<int> AlliedTeams { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;

                if (Renderer == null)
                    return;

                Renderer.material.color = _isSelected ? Color.green : Color.white;
            }
        }

        [SerializeField]
        private float _attackRange = 1.0f;
        public float AttackRange { get => _attackRange; set => _attackRange = value; }

        public Renderer Renderer { get; set; }

        private void Awake()
        {
            Renderer = GetComponent<Renderer>();
        }

        public void Destroy()
        {
            GameObject.Destroy(this);
        }
    }
}
