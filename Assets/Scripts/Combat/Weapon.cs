using UnityEngine;

namespace RPG.Combat {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new weapon", order = 0)]
    public class Weapon : ScriptableObject {
        [Header("Weapon's Properties")]
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] bool isRightHanded = true;

        // Is this weapon has projectiles?
        [SerializeField] Projectile projectile = null;

        [Header("Weapon's Stats")]
        [SerializeField] float attackRange = 2f;
        [SerializeField] float damage = 10f;
        [SerializeField] float percentageBonus = 0;

        const string weaponName = "Weapon";

        public float GetAttackRange() { return attackRange; }
        public float GetDamage() { return damage; }
        public float GetPercentageBonus() { return percentageBonus; }

        /// <summary>
        /// Spawn a weapon at the hand of the object and override current animator with this weapon animator.
        /// </summary>
        /// <param name="rightHand">Right hand's position.</param>
        /// <param name="leftHand">Left hand's position.</param>
        /// <param name="animator">The override animator of this weapon.</param>
        public void Spawn( Transform rightHand, Transform leftHand, Animator animator ) {
            DestroyOldWeapon(rightHand, leftHand);

            // Create an instance from weapon prefab...
            if ( weaponPrefab != null ) {
                GameObject weapon = Instantiate(weaponPrefab, GetHandPosition(rightHand, leftHand));
                weapon.name = weaponName;
            }
            // Override character animator controller if current weapon have an override animator.
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if ( weaponOverride != null )
                animator.runtimeAnimatorController = weaponOverride;
            // If this weapon doesn't have override controller, get runtime animator controller of the previous override animator controller.
            else if ( overrideController != null )
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }

        /// <summary>
        /// Destroy old weapon if it existed.
        /// </summary>
        /// <param name="rightHand"></param>
        /// <param name="leftHand"></param>
        private void DestroyOldWeapon( Transform rightHand, Transform leftHand ) {
            Transform oldWeapon = rightHand.Find(weaponName);
            if ( oldWeapon == null )
                oldWeapon = leftHand.Find(weaponName);
            if ( oldWeapon == null ) return;

            oldWeapon.name = "Destroyed";
            Destroy(oldWeapon.gameObject);
        }

        /// <summary>
        /// Get this weapon's handler position.
        /// </summary>
        /// <param name="rightHand">Right hand's position.</param>
        /// <param name="leftHand">Left hand's position.</param>
        /// <returns>This weapon's handler position.</returns>
        private Transform GetHandPosition( Transform rightHand, Transform leftHand ) {
            return isRightHanded ? rightHand : leftHand;
        }

        /// <summary>
        /// Check if this WEAPON can shoot projectiles or not.
        /// </summary>
        public bool HasProjectile() {
            return projectile != null;
        }

        /// <summary>
        /// Create a projectile and lauch it at the handler position forward target.
        /// </summary>
        /// <param name="rightHand">Right hand's position.</param>
        /// <param name="leftHand">Left hand's position.</param>
        /// <param name="target">Target location.</param>
        public void LauchProjectile( Transform rightHand, Transform leftHand, Transform target, GameObject instigator, float calculatedDamage ) {
            Projectile anProjectile = Instantiate(projectile, GetHandPosition(rightHand, leftHand).position, Quaternion.identity);
            anProjectile.SetTarget(target, instigator, calculatedDamage);
        }
    }
}