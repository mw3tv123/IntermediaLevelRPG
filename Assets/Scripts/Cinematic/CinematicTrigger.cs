using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic {
    // Control cinematic moment.
    public class CinematicTrigger : MonoBehaviour {
        [SerializeField] bool isTriggerred = false;

        // Trigger when Player touch event collider
        private void OnTriggerEnter(Collider other){
            if (!isTriggerred && other.gameObject.tag == "Player") {
                GetComponent<PlayableDirector>().Play();
                isTriggerred = true;
            }
        }
    }
}

