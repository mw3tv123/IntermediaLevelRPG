using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic {
    // Handle about what happen when cinematic take place.
    public class ControlRemover : MonoBehaviour {
        GameObject player;

        void Start() {
            // Registe an event delegate of PlayerDirector: when cinematic played and when it done.
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
        }

        // Disable all Player actions when cinematic take place.
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

