using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement {
    // Use to fade out and fade in between scene loading.
    public class Fader : MonoBehaviour {
        CanvasGroup canvasGroup;

        void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// Fadeout immediately (for load scene from file).
        /// </summary>
        public void FadeOutImmediate() {
            canvasGroup.alpha = 1;
        }

        /// <summary>
        /// Ajust the Alpha value from 0 to 1 each frame (White out scene).
        /// </summary>
        /// <param name="time">Fading time.</param>
        public IEnumerator FadeOut( float time ) {
            while (canvasGroup.alpha < 1 ) {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        /// <summary>
        /// Ajust the Alpha value from 0 to 1 each frame (bring back normal scene).
        /// </summary>
        /// <param name="time">Fading time.</param>
        public IEnumerator FadeIn( float time ) {
            while ( canvasGroup.alpha > 0 ) {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}