using RPG.Saving;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement {
    /// <summary>
    /// Wrapper of the saving systems.
    /// </summary>
    public class SavingWrapper : MonoBehaviour {
        [SerializeField] const string defaultSaveFile = "save";
        [SerializeField] float fadeTime = .2f;

        private void Awake() {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene() {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);

            // A flash flare for smoothing load scene.
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();

            yield return fader.FadeIn(fadeTime);
        }

        void Update() {
            if ( Input.GetKeyDown(KeyCode.S) )
                Save();
            if ( Input.GetKeyDown(KeyCode.L) )
                Load();
            if ( Input.GetKeyDown(KeyCode.Delete) )
                Delete();
        }

        public void Load() {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save() {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Delete() {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}
