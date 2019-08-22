using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Resources {
    /// <summary>
    /// This contain Health behavior of an object (Player, Monster...)
    /// </summary>
    public class Health : MonoBehaviour, ISaveable {
        LazyValue<float> maxHealth;
        LazyValue<float> currentHealth;

        [Header("Status"), SerializeField] bool isDeath = false;

        [Header("Events List"), SerializeField] TakeDamageEvent takeDamage;

        /// <summary>
        /// Because SerializeField don't allow to use the generic UnityEvent,
        /// so we create a subclass with inherit generic type UnityEvent,
        /// and then mark this subclass to be Serializable by Unity.
        /// </summary>
        [Serializable]
        public class TakeDamageEvent : UnityEvent<float> { }

        public bool IsDeath {
            get { return isDeath; }
            set { isDeath = value; }
        }

        void Awake() {
            maxHealth = new LazyValue<float>(GetInitialHealth);
            currentHealth = new LazyValue<float>(() => maxHealth.value);
        }

        private float GetInitialHealth() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        void OnEnable() {
            GetComponent<BaseStats>().OnLevelUp += CalculateHealth;
        }

        void OnDisable() {
            GetComponent<BaseStats>().OnLevelUp -= CalculateHealth;
        }

        /// <summary>
        /// Return current health in percentage (%).
        /// </summary>
        public float GetPercentage() { return currentHealth.value / maxHealth.value * 100; }

        public float GetCurrentHP() { return currentHealth.value; }

        public float GetMaxHP() { return maxHealth.value; }

        /// <summary>
        /// Make an object take damage.
        /// </summary>
        /// <param name="damage">Amount of health will reduce.</param>
        public void TakeDamage( GameObject instigator, float damage ) {
            // When this object already dead, we do nothing.
            if (IsDeath) return;

            currentHealth.value = Mathf.Clamp(currentHealth.value - damage, 0, maxHealth.value);

            // Trigger spawn damage text.
            takeDamage.Invoke(damage);

            if (currentHealth.value == 0 ) {
                Die();
                AwardExperience(instigator);
            }
        }

        private void AwardExperience( GameObject instigator ) {
            Experience XP = instigator.GetComponent<Experience>();
            if ( XP == null ) return;

            XP.GainXP(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void CalculateHealth() {
            float currentHealthPercent = GetPercentage();

            maxHealth.value = GetComponent<BaseStats>().GetStat(Stat.Health);
            currentHealth.value = maxHealth.value * (currentHealthPercent / 100);
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

        public object CaptureState() {
            float[] healthStatus = new float[] { currentHealth.value, maxHealth.value };
            return healthStatus;
        }

        public void RestoreState( object state ) {
            float[] healthStatus = (float[])state;
            currentHealth.value = healthStatus[0];
            maxHealth.value = healthStatus[1];
            if ( currentHealth.value <= 0 ) Die();
        }
    }
}
