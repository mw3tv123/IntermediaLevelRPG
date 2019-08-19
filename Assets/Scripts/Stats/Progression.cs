using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject {
        [SerializeField] ProgressionClass[] progressionClass = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> progressionTable = null;

        /// <summary>
        /// Get current level's health stat.
        /// </summary>
        /// <param name="charClass">Type of character.</param>
        /// <param name="stat">Type of stat.</param>
        /// <param name="level">Current character's level.</param>
        /// <returns>Health stat of the input level of character.</returns>
        public float GetStat( CharacterClass charClass, Stat stat, int level ) {
            BuildTable();
            return progressionTable[charClass][stat][level-1];
        }

        public int GetTotalLevel( CharacterClass charClass, Stat stat ) {
            BuildTable();

            float[] levelsList = progressionTable[charClass][stat];
            return levelsList.Length;
        }

        /// <summary>
        /// Build a in-memory progression table for optimize performance.
        /// </summary>
        private void BuildTable() {
            // If progression table was build before, we are not gonna built it again.
            if ( progressionTable != null ) return;

            progressionTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            // Built up progression table base on progression sciptable object.
            foreach ( ProgressionClass progressChar in progressionClass ) {
                var statTable = new Dictionary<Stat, float[]>();

                foreach ( ProgressionStat progressionStat in progressChar.stats )
                    statTable[progressionStat.stat] = progressionStat.levels;
                
                progressionTable[progressChar.characterClass] = statTable;
            }
        }

        /// <summary>
        /// Delegate to each character class stats.
        /// <example>
        /// Knight: Health, Damage,...
        /// </example>
        /// </summary>
        [Serializable]
        class ProgressionClass {
            // Type of this character class.
            public CharacterClass characterClass;

            // List of stats this character class carry.
            public ProgressionStat[] stats;
        }

        /// <summary>
        /// Delegate to a stat's progression.
        /// <example>
        /// Health: {10, 20, 30, ...}
        /// Damage: {2, 4, 6, ...}
        /// </example>
        /// </summary>
        [Serializable]
        class ProgressionStat {
            // This property is a enum, which can be any stat of a character: Health, Damage, Altributes,...
            public Stat stat;
            
            // Each element delegate a level, each level carry a value of property "stat" above.
            public float[] levels;
        }
    }
}