using UnityEngine;

namespace RPG.Attributes {
    public class HealthBar : MonoBehaviour {
        [SerializeField] Health health = null;
        [SerializeField] RectTransform healthBar = null;
        [SerializeField] Canvas canvas = null;

        // Update is called once per frame
        void Update() {
            if ( (health.IsDeath) || health.GetCurrentHP() == health.GetMaxHP() ) {
                canvas.enabled = false;
                return;
            }

            canvas.enabled = true;
            healthBar.localScale = new Vector3(health.GetPercentage() / 100, 1, 1);
        }
    }
}
