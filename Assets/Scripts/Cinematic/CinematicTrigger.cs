using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic {
    /// <summary>
    /// Control cinematic moment.
    /// </summary>
    public class CinematicTrigger : MonoBehaviour {
        [SerializeField] bool isTriggerred = false;

        /// <summary>
        /// Trigger when Player touch event collider
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other){
            if (!isTriggerred && other.gameObject.tag == "Player") {
                GetComponent<PlayableDirector>().Play();
                isTriggerred = true;
            }
        }
    }
}

