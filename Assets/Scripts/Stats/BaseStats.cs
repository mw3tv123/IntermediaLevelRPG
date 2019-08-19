using GameDevTV.Utils;
using System;
using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour {
        [SerializeField, Range(1, 100)] int startLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;
        [SerializeField] bool canUseModifiers = false;

        LazyValue<int> currentLevel;
        Experience experience;
        public event Action OnLevelUp;

        void Awake() {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        void Start() {
            currentLevel.ForceInit();
        }

        void OnEnable() {
            // Subcribe to Experience event Action...
            if ( experience != null )
                experience.OnExperienceGained += UpdateLevel;
        }

        void OnDisable() {
            if ( experience != null )
                experience.OnExperienceGained -= UpdateLevel;
        }

        /// <summary>
        /// Automatic calculate level whenever Player's experiences changed.
        /// </summary>
        void UpdateLevel() {
            int newLevel = CalculateLevel();
            if ( newLevel > currentLevel.value ) {
                currentLevel.value = newLevel;
                LevelUpEffect();
                OnLevelUp();
            }
        }

        /// <summary>
        /// Create a particle FX whenever Player level up.
        /// </summary>
        private void LevelUpEffect() {
            Instantiate(levelUpEffect, transform);
        }

        public float GetStat( Stat stat ) {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }

        private float GetBaseStat( Stat stat ) {
            return progression.GetStat(characterClass, stat, GetLevel());
        }

        /// <summary>
        /// Summary all percenage modifier of a specific STAT.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        private float GetPercentageModifier( Stat stat ) {
            if ( !canUseModifiers ) return 0;
            float total = 0;
            foreach ( IModifierProvider provider in GetComponents<IModifierProvider>() ) {
                foreach ( float modifier in provider.GetPercentageModifier(stat) )
                    total += modifier;
            }
            return total;
        }

        /// <summary>
        /// Summary all modifiers of a specific STAT.
        /// </summary>
        /// <param name="stat">The stat that going to get all modifiers.</param>
        /// <returns>Total of all modifiers of target stat.</returns>
        private float GetAdditiveModifier( Stat stat ) {
            // Only Player can use modifier...
            // TODO: Give NPC abilities to use modifiers like Player.
            if ( !canUseModifiers ) return 0;
            float total = 0;
            foreach ( IModifierProvider provider in GetComponents<IModifierProvider>() ) {
                foreach ( float modifier in provider.GetAdditiveModifier(stat) )
                    total += modifier;
            }
            return total;
        }

        public int GetLevel() { return currentLevel.value; }

        /// <summary>
        /// Caculate player level base on current experiences.
        /// </summary>
        /// <returns>Return current level or total level if max level have reached.</returns>
        public int CalculateLevel() {
            if ( experience == null )
                return startLevel;

            float currentXP = experience.XP;
            int totalLevels = progression.GetTotalLevel(characterClass, Stat.ExperienceToLevelUp);

            for ( int level = 1; level <= totalLevels; level++ ) {
                float XPToLevelUp = progression.GetStat(characterClass, Stat.ExperienceToLevelUp, level);
                if ( currentXP <= XPToLevelUp )
                    return level;
            }
            return totalLevels;
        }
    }
}