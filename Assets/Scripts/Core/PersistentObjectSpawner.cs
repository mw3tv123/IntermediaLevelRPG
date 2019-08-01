using System;
using UnityEngine;

namespace RPG.Core {
    public class PersistentObjectSpawner : MonoBehaviour {
        [SerializeField] GameObject persistentObjectPrefab;

        static bool hasSpawned = false;

        void Awake() {
            if ( hasSpawned ) return;

            SpawnPersistentObject();

            hasSpawned = true; 
        }

        private void SpawnPersistentObject() {
            // Create an instance of this object an carry it through scene.
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}