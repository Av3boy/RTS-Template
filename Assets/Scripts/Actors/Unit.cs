using Assets.Scripts.Actors;
using Assets.Scripts.Enums;
using System.Collections;
using System.Linq;

using UnityEngine;

namespace Assets.Scripts
{
    public class Unit : Actor
    {
        public bool IsEnemy => !AlliedTeams.Contains(((Unit)TargetObject).Team);

        [SerializeField]
        private int _team;
        public int Team { get => _team; set => _team = value; }

        [SerializeField]
        private UnitType _unitType;
        public UnitType UnitType { get => _unitType; set => _unitType = value; }

        private Animator _animator;

        public Vector3? TargetPosition
        {
            get
            {
                if (TargetObject != null)
                    return TargetObject.transform.position;

                return _targetPosition;
            }

            set => _targetPosition = value;
        }

        private Vector3? _targetPosition;

        public Actor TargetObject { get; set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public IEnumerator AttackTarget()
        {
            while (TargetObject != null || TargetObject.Health > 0)
            {
                if (Vector3.Distance(TargetObject.transform.position, this.transform.position) <= AttackRange)
                    Move(TargetObject.transform.position, AttackRange);

                Unit targetUnit = (Unit)TargetObject;
                targetUnit.Health -= AttackDamage;

                Damage(targetUnit);

                if (targetUnit.Health <= 0)
                    targetUnit.Destroy();

                yield return null;
            }
        }

        public void Damage(Unit target)
        {
            StartCoroutine(DamageTarget());

            IEnumerator DamageTarget()
            {
                target.Renderer.material.color = Color.red;
                yield return new WaitForSeconds(0.5f);
                target.Renderer.material.color = Color.white; // Replace with correct material later.
            }
        }

        public void Move(Vector3 targetPosition, float stopRange = 0.1f)
        {
            StartCoroutine(MoveUntilDestinationReached());

            IEnumerator MoveUntilDestinationReached()
            {
                bool destinationReached = false;

                _animator.Play("Run");

                while (!destinationReached)
                {
                    if (Vector3.Distance(TargetObject.transform.position, this.transform.position) <= stopRange)
                        destinationReached = true;

                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
                    yield return null;
                }
            }
        }
    }
}