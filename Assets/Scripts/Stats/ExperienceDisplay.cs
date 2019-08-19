using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats {
    public class ExperienceDisplay : MonoBehaviour {
        Experience playerXP;

        private void Start() {
            playerXP = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update() {
            GetComponent<Text>().text = string.Format("Experiences: {0:0}", playerXP.XP);
        }
    }
}