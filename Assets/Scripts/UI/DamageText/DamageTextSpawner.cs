using UnityEngine;

namespace RPG.UI {
    public class DamageTextSpawner : MonoBehaviour {
        [SerializeField] DamageText damageTextPrefab = null;
        
        /// <summary>
        /// Spawn an damage text UI above object.
        /// </summary>
        /// <param name="damage">Damage which will appear on text.</param>
        public void SpawnDamageText ( float damage ) {
            DamageText damageText = Instantiate<DamageText>(damageTextPrefab, transform);
            damageText.SetValue(damage);
        }
    }
}