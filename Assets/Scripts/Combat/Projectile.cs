using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour {
        [SerializeField] float speed = 2f;
        [SerializeField] float damage = 0f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float lifeTime = 10f;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] GameObject[] destroyOnHit = null;

        Transform target = null;
        Health target_health;
        GameObject instigator = null;

        void Start() {
            target_health = target.GetComponent<Health>();
            PointAtTarget();
        }

        // Update is called once per frame
        void Update() {
            if ( target == null ) return;

            // If this projectile able to follow target...
            if ( isHoming && !target_health.IsDeath)
                PointAtTarget();
            // Moving forward that direction.
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void PointAtTarget() {
            // Point this projectile to point at target direction.
            transform.LookAt(GetLocation());
        }

        /// <summary>
        /// Return target's current position in world space.
        /// </summary>
        private Vector3 GetLocation() {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            return target.transform.position + Vector3.up * (targetCapsule.height * 2) / 3;
        }

        /// <summary>
        /// Set target of this projectile.
        /// </summary>
        /// <param name="target">For easy access target location.</param>
        public void SetTarget(Transform target, GameObject instigator, float damage) {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, lifeTime);
        }

        // Execute a sequence of work when this projectile hit an object.
        private void OnTriggerEnter( Collider other ) {
            // If the object this projectile is not its target, then it does nothing.
            if ( other.transform != target ) return;

            speed = 0;

            // Create an impact hit visual effect.
            if ( hitEffect != null )
                Instantiate(hitEffect, GetLocation(), transform.rotation);

            // Dealt damage to target.
            target_health.TakeDamage(instigator, damage);

            foreach ( GameObject toDestroy in destroyOnHit )
                Destroy(toDestroy);

            // Remove this projectile out of world space.
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
