using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving {
    /// <summary>
    /// Core saving system for saving stuffs.
    /// </summary>
    public class SavingSystem : MonoBehaviour {
        // Enable to serialize multi properties.
        BinaryFormatter formatter = new BinaryFormatter();

        /// <summary>
        /// Load the last scene player was before loading other things.
        /// </summary>
        /// <param name="saveFile"></param>
        public IEnumerator LoadLastScene( string saveFile ) {
            // Get previous states...
            Dictionary<string, object> states = LoadFile(saveFile);

            // A safe check for last scene index.
            if ( states.ContainsKey("LastSceneBuildIndex") ) {
                int buildIndex = (int)states["LastSceneBuildIndex"];

                // Only load scene if privious scene difference from current scene...
                if ( SceneManager.GetActiveScene().buildIndex != buildIndex )
                    yield return SceneManager.LoadSceneAsync(buildIndex);
            }

            // Then load other stuffs.
            RestoreState(states);
        }

        /// <summary>
        /// Save data to a specific <b>saveFile</b>.
        /// </summary>
        /// <param name="saveFile">File that data store in.</param>
        public void Save( string saveFile ) {
            Dictionary<string, object> states = LoadFile(saveFile);
            CaptureState(states);
            SaveFile(saveFile, states);
        }

        /// <summary>
        /// Load data from a specific file.
        /// </summary>
        /// <param name="saveFile">File that data store in.</param>
        public void Load( string saveFile ) {
            RestoreState(LoadFile(saveFile));
        }

        private Dictionary<string, object> LoadFile( string saveFile ) {
            string path = GetPathFromSaveFile(saveFile);
            
            // If the save file doesn't exist, just create a empty dictionary STATES.
            if ( !File.Exists(path) ) return new Dictionary<string, object>();

            // Else load all data from the save file.
            using ( FileStream stream = File.Open(path, FileMode.Open) ) {
                return (Dictionary<string, object>) formatter.Deserialize(stream);
            }
        }

        private void SaveFile( string saveFile, object states ) {
            string path = GetPathFromSaveFile(saveFile);
            using ( FileStream stream = File.Open(path, FileMode.Create) ) {
                formatter.Serialize(stream, states);
            }
        }

        // Store captured states in an dictionary for easy access.
        private void CaptureState( Dictionary<string, object> states ) {
            // Find all saveable object in current scene and store that into a state.
            foreach ( SaveableEntity item in FindObjectsOfType<SaveableEntity>() ) {
                states[item.GetUniqueIdentifier()] = item.CaptureState();
            }

            states["LastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        // Restore states back to to its original states.
        private void RestoreState( Dictionary<string, object> capturedStates ) {
            foreach ( SaveableEntity item in FindObjectsOfType<SaveableEntity>() ) {
                string itemID = item.GetUniqueIdentifier();

                // A safe check.
                if ( capturedStates.ContainsKey(itemID) )
                    item.RestoreState(capturedStates[itemID]);
            }
        }

        /// <summary>
        /// Get the full path of the file.
        /// </summary>
        /// <param name="saveFile"></param>
        /// <returns>Alsolute path.</returns>
        private string GetPathFromSaveFile( string saveFile ) {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}
