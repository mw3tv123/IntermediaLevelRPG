using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement {
    // Use to fade out and fade in between scene loading.
    public class Fader : MonoBehaviour {
        CanvasGroup canvasGroup;

        void Start() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // Ajust the Alpha value from 0 to 1 each frame (White out scene).
        public IEnumerator FadeOut( float time ) {
            while (canvasGroup.alpha < 1 ) {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        // Ajust the Alpha value from 0 to 1 each frame (bring back normal scene).
        public IEnumerator FadeIn( float time ) {
            while ( canvasGroup.alpha > 0 ) {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}