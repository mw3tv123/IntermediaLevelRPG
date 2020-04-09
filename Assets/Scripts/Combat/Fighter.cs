using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    /// <summary>
    /// Manage combat activities like attack damage [AD], attack range [AR], attack speed [AS], attack type,...
    /// </summary>
    public class Fighter : MonoBehaviour, IAction {
        #region Basic properties
        [SerializeField]
        private float attackRange = 1.5f;

        [SerializeField]
        private float attackDamage = 1f;
        
        [SerializeField]
        private float attackCooldownTime = 1f;
        #endregion

        #region Properties Checker
        private float timeSinceLastAttack = Mathf.Infinity;

        // Determine if this object's attack is cooldown or not.
        private bool IsAttackCooledDown => timeSinceLastAttack > attackCooldownTime;

        // Determine if distance from this object to target is in attack range or not.
        private bool IsInAttackRange => Vector3.Distance(transform.position, target.transform.position) < attackRange;

        /// <summary>
        /// Determise if the target can be attacked or not.
        /// </summary>
        public bool CanNotAttack => target is null || target.IsDead;
        #endregion

        private Animator animator;
        private Mover mover;
        private Health target;

        private void Start ( ) {
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
        }

        private void Update ( ) {
            timeSinceLastAttack += Time.deltaTime;

            // If this object don't have a target or its target dead, then it do nothing.
            if ( CanNotAttack ) return;

            // Attack if target is in range.
            if ( IsInAttackRange ) {
                mover.Cancel();
                transform.LookAt(target.transform);
                Attack();
            }
            // Move close to target.
            else {
                mover.MoveTo(target.transform.position);
            }
        }

        /// <summary>
        /// Start and register fight action to Action Scheduler.
        /// </summary>
        /// <param name="target">The target of this object.</param>
        public void StartFightAction ( Health target ) {
            GetComponent<ActionScheduler>().StartAction(this);
            this.target = target;
        }

        /// <summary>
        /// Start an attack sequence: trigger animation, deal damage.
        /// </summary>
        private void Attack ( ) {
            if ( IsAttackCooledDown ) {
                timeSinceLastAttack = 0;
                UpdateAnimator();
            }
        }

        /// <summary>
        /// Trigger attack animation.
        /// </summary>
        private void UpdateAnimator ( ) {
            animator.ResetTrigger("StopAttack");
            animator.SetTrigger("Attack");
        }

        /// <summary>
        /// Attack animation event occur when the hand reach it point.
        /// </summary>
        private void Hit ( ) {
            if ( target != null ) {
                target.TakeDamage(attackDamage);
                // Target.GetComponent<Animator>().SetTrigger("GetHit");
            }
        }

        public void Cancel ( ) {
            target = null;
            animator.ResetTrigger("Attack");
            animator.SetTrigger("StopAttack");
        }
    }
}