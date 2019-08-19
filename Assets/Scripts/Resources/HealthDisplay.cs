using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources {
    public class HealthDisplay : MonoBehaviour {
        Health health;

        private void Start() {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update() {
            GetComponent<Text>().text = string.Format("Health: {0:0}/{1:0}", health.GetCurrentHP(), health.GetMaxHP());
        }
    }
}