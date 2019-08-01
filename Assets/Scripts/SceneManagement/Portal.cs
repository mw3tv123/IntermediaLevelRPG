using System.Collections;
using UnityEngine;
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
            // Preserve this portal object...
            DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();

            // Start and wait for fading out scene sequence complete...
            yield return fader.FadeOut(fadeTime);

            // Wait for loading scene object...
            yield return SceneManager.LoadSceneAsync(sceneIndex);

            Portal newPortal = GetNewPortal();
            // Loading Player's information...
            UpdatePlayer(newPortal);

            // When all jobs above completed, begin fading in...
            yield return fader.FadeIn(fadeTime);

            // Then destroy this portal.
            Destroy(gameObject);
        }

        // Reference to other portal in the other scene.
        private Portal GetNewPortal() {
            foreach (Portal portal in FindObjectsOfType<Portal>() ) {
                if ( portal == this ) continue; // Not current portal.
                if ( portal.destinationPortal != destinationPortal ) continue; // Not the same identify portal destination.
                return portal;
            }
            return null;
        }

        // Place Player at the spawn point of a portal.
        private void UpdatePlayer(Portal portal) {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = portal.spawnPoint.position;
            player.transform.rotation = portal.spawnPoint.rotation;
        }
    }
}
