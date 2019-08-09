using RPG.Saving;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement {
    /// <summary>
    /// Wrapper of the saving systems.
    /// </summary>
    public class SavingWrapper : MonoBehaviour {
        [SerializeField] const string defaultSaveFile = "save";
        [SerializeField] float fadeTime = 1f;

        IEnumerator Start() {
            // A flash flare for smoothing load scene.
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();

            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);

            yield return fader.FadeIn(fadeTime);
        }

        void Update() {
            if ( Input.GetKeyDown(KeyCode.S) )
                Save();
            if ( Input.GetKeyDown(KeyCode.L) )
                Load();
        }

        public void Load() {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save() {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
    }
}
