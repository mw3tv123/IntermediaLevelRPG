using RPG.Saving;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement {
    public class Portal : MonoBehaviour {
        enum DestinationIdentifier {
            // Use to indentify portal between scene and link it together.
            A, B
        }

        [SerializeField] float fadeTime = 3f;
        [SerializeField] int sceneIndex = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destinationPortal;

        private void OnTriggerEnter( Collider other ) {
            if ( other.gameObject.tag == "Player" )
                StartCoroutine(Transistion());
        }

        // A sequence of stage when transisting between scene.
        private IEnumerator Transistion() {
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            // Preserve this portal object...
            DontDestroyOnLoad(gameObject);

            // Start and wait for fading out scene sequence complete...
            yield return fader.FadeOut(fadeTime);

            // Save current scene state before change to next scene...
            savingWrapper.Save();

            // Wait for loading scene object...
            yield return SceneManager.LoadSceneAsync(sceneIndex);

            // Load previous stage of the new scene...
            savingWrapper.Load();

            Portal newPortal = GetNewPortal();

            // Loading Player's information...
            UpdatePlayer(newPortal);

            // Save new scene as Player's last scene for later load.
            savingWrapper.Save();

            // When all jobs above completed, begin fading in...
            yield return fader.FadeIn(fadeTime);

            // Then destroy this portal.
            Destroy(gameObject);
        }

        // Reference to other portal in the other scene.
        private Portal GetNewPortal() {
            foreach (Portal portal in FindObjectsOfType<Portal>() ) {
                // We won't spawn at...
                if ( portal == this ) continue; // ...the same portal.
                if ( portal.destinationPortal != destinationPortal ) continue; // ...difference ID portal destination.
                return portal;      // ...but at difference portal with the same ID portal destination.
            }
            return null;
        }

        // Place Player at the spawn point of a portal.
        private void UpdatePlayer(Portal portal) {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = portal.spawnPoint.position;
            player.transform.rotation = portal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
