using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI {
    public class DamageText : MonoBehaviour {
        [SerializeField] Text damageText = null;

        /// <summary>
        /// Destroy this text object after it completed animate.
        /// </summary>
        public void DestroyText() { Destroy(gameObject); }

        /// <summary>
        /// Set the value that appear in the text.
        /// </summary>
        public void SetValue( float amount ) { damageText.text = string.Format("{0}", amount); }
    }
}