using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    // An Fighter class.
    [RequireComponent(typeof(ActionScheduler))]
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] float attackRange = 2f;
        [SerializeField] float attackSpeed = 1f;
        [SerializeField] float damage = 10f;

        [SerializeField] Transform target;

        Animator animator;
        Mover mover;
        float attackCooldown;

        void Start() {
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
            attackCooldown = attackSpeed;
        }

        void Update() {
            attackCooldown += Time.deltaTime;

            // If we don't have any target...
            if (target == null) return;
            // If our target is death...
            if (target.GetComponent<Health>().IsDeath) return;
            // If our target is far away from our attack range...
            if (Vector3.Distance(target.position, transform.position) > attackRange)
                mover.MoveToPoint(target.position);
            // If target in attack range...
            else {
                mover.CancelThisAction();
                AnimateAttack();
            }
        }

        // Set our target to deal damage.
        public void Attack(GameObject combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        // Cancel attack action currently take place.
        public void CancelThisAction() {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
            target = null;
        }

        // Trigger attack animation in this object's Animator.
        void AnimateAttack() {
            transform.LookAt(target.transform);
            // Sync attack animation with our object's attack speed.
            if (attackCooldown >= attackSpeed) {
                animator.ResetTrigger("stopAttack");
                animator.SetTrigger("attack");
                attackCooldown = 0;
            }
        }

        // Animation Event
        void Hit() {
            if (target != null)
                target.GetComponent<Health>().TakeDamage(damage);
        }
    }
}

