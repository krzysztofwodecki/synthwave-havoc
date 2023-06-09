using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter.Enemy
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemyGroupAvoidanceController : MonoBehaviour
    {
        [SerializeField] private LayerMask _enemyPlayerMask;
        private HashSet<Collider> _collidersInRange;
        private SphereCollider _collider;
        [SerializeField] private float _innerRadius = 0.3f;
        [SerializeField] private float _strength = 2f;

        private void Awake()
        {
            _collidersInRange = new HashSet<Collider>();
            _collider = GetComponent<SphereCollider>();
        }

        private void Update()
        {
            if (DebugSettings.DrawEnemyAvoidanceDebugLines)
            {
                var vector = GetAvoidanceVector();
                Debug.DrawRay(transform.position, vector, Color.red);
                foreach(var collider in _collidersInRange)
                {
                    Debug.DrawLine(transform.position, collider.transform.position, Color.white);
                }
            }
    }


        private void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger && other.gameObject.transform != transform.parent && (1 << other.gameObject.layer & _enemyPlayerMask.value) != 0)
            {
                _collidersInRange.Add(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.isTrigger && other.gameObject.transform != transform.parent && (1 << other.gameObject.layer & _enemyPlayerMask.value) != 0)
            {
                if (_collidersInRange.Contains(other))
                    _collidersInRange.Remove(other);
            }
        }

        public Vector3 GetAvoidanceVector()
        {

            var sum = Vector3.zero;
            var radius = _collider.radius;
            foreach (var collider in _collidersInRange)
            {
                var difference = this.transform.position - collider.transform.position;
                var direction = difference.normalized;
                var distance = difference.magnitude;
                var force = 0f;
                if (distance < _innerRadius)
                    force = 1f;
                else if (distance > radius)
                    force = 0f;
                else
                    force = (radius - distance) / (radius - _innerRadius);

                sum += direction * force;
            }

            return sum;
        }
    }
}