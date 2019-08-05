using UnityEngine;

namespace RPG.Core {
    /// <summary>
    /// This contain Health behavior of an object (Player, Monster...)
    /// </summary>
    public class Health : MonoBehaviour {
        [Header("Heaths Settings")]
        [Range(50, 125), SerializeField] float maxHealth = 100f;
        [SerializeField] float currentHealth;

        [Header("Status")]
        [SerializeField] bool isDeath = false;

        public bool IsDeath {
            get { return isDeath; }
            set { isDeath = value; }
        }

        void Start() {
            currentHealth = maxHealth;
        }

        /// <summary>
        /// Make an object take damage.
        /// </summary>
        /// <param name="damage">Amount of health will reduce.</param>
        public void TakeDamage(float damage) {
            // When this object already dead, we do nothing.
            if (IsDeath) return;

            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
            if (currentHealth == 0)
                Die();
        }

        /// <summary>
        /// Active Death animation, remove collider to prevent future action on this object
        /// and set "death" to True to mark this is actually "dead".
        /// </summary>
        public void Die() {
            IsDeath = true;
            GetComponent<Animator>().SetTrigger("death");

            // Disable control of this object on Death Event.
            GetComponent<ActionScheduler>().CancelCurrentAction();

            // Remove this object Collider so that Player's RayCasting won't obstruct by this object collider.
            if (GetComponent<CapsuleCollider>() != null)
                GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
