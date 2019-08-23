using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat {
    public class EnemyHealthDisplay : MonoBehaviour {
        Fighter fighter;
        Health targetHealth;
        Text healthUI;

        private void Start() {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            healthUI = GetComponent<Text>();
        }

        private void Update() {
            if ( fighter.GetTarget() == null ) {
                healthUI.text = "Enemy: N/A";
                return;
            }

            targetHealth = fighter.GetTarget().GetComponent<Health>();
            healthUI.text = string.Format("Enemy: {0:0}/{1:0}", targetHealth.GetCurrentHP(), targetHealth.GetMaxHP());
        }
    }
}