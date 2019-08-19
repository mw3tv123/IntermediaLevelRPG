using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic {
    /// <summary>
    /// Handle about what happen when cinematic take place.
    /// </summary>
    public class ControlRemover : MonoBehaviour {
        GameObject player;

        void Start() {
            player = GameObject.FindWithTag("Player");
        }

        void OnEnable() {
            // Registe an event delegate of PlayerDirector: when cinematic played and when it done.
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        void OnDisable() {
            // Registe an event delegate of PlayerDirector: when cinematic played and when it done.
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        /// <summary>
        /// Disable all Player actions when cinematic take place.
        /// </summary>
        /// <param name="director"></param>
        void DisableControl( PlayableDirector director ) {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        // Enable all Player actions when cinematic done.
        void EnableControl( PlayableDirector director ) {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}

