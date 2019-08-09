using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using UnityEngine;

namespace RPG.Combat {
    /// <summary>
    /// Combat system for deal with move next to target, attack target and some other stuffs.
    /// </summary>
    [RequireComponent(typeof(ActionScheduler))]
    public class Fighter : MonoBehaviour, IAction, ISaveable {
        [Header("Attack Stats")]
        [SerializeField] float attackSpeed = 1f;

        [Header("Weapon Slot")]
        [SerializeField] Transform rightHand = null;
        [SerializeField] Transform leftHand = null;
        [SerializeField] Weapon currentWeapon = null;
        [SerializeField] Weapon defaultWeapon = null;

        Transform target;
        Animator animator;
        Mover mover;
        float attackCooldown;

        void Awake() {
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
            attackCooldown = attackSpeed;

            // Instantiate default weapon in player's hand.
            if ( currentWeapon == null ) EquipWeapon(defaultWeapon);
        }

        void Update() {
            attackCooldown += Time.deltaTime;

            // If we don't have any target...
            if (target == null) return;
            // If our target is death...
            if (target.GetComponent<Health>().IsDeath) return;
            // If our target is far away from our attack range...
            if (Vector3.Distance(target.position, transform.position) > currentWeapon.GetAttackRange())
                mover.MoveToPoint(target.position, 1f);
            // If target in attack range...
            else {
                mover.CancelThisAction();
                AnimateAttack();
            }
        }

        /// <summary>
        /// Equip new weapon.
        /// </summary>
        /// <param name="weapon">New equipment.</param>
        public void EquipWeapon(Weapon weapon) {
            currentWeapon = weapon;
            weapon.Spawn(rightHand, leftHand, animator);
        }

        /// <summary>
        /// Start "attack target" mode.
        /// </summary>
        /// <param name="combatTarget"></param>
        public void Attack(GameObject combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        /// <summary>
        /// Stop all attack action.
        /// </summary>
        public void CancelThisAction() {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
            mover.CancelThisAction();
            target = null;
        }

        /// <summary>
        /// Trigger attack animation in this object's Animator.
        /// </summary>
        void AnimateAttack() {
            transform.LookAt(target.transform);
            // Sync attack animation with our object's attack speed.
            if (attackCooldown >= attackSpeed) {
                animator.ResetTrigger("stopAttack");
                animator.SetTrigger("attack");
                attackCooldown = 0;
            }
        }

        /// <summary>
        /// Melee attack Animation Event.
        /// </summary>
        void Hit() {
            if ( target == null ) return;

            if ( currentWeapon.HasProjectile() )
                currentWeapon.LauchProjectile(rightHand, leftHand, target);
            else
                target.GetComponent<Health>().TakeDamage(currentWeapon.GetDamage());
        }

        /// <summary>
        /// Range attack Animation Event.
        /// </summary>
        void Shoot() {
            Hit();
        }

        public object CaptureState() {
            return currentWeapon.name;
        }

        public void RestoreState( object state ) {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}

