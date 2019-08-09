using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving {
    // Enable this script to rung in both Editor mode and Playing mode.
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour {
        // Unique ID for each saveable object.
        [SerializeField] string uniqueIdentifier = "";
        static Dictionary<string, SaveableEntity> UUIDTable = new Dictionary<string, SaveableEntity>();

        public string GetUniqueIdentifier() {
            return uniqueIdentifier;
        }

        public object CaptureState() {
            Dictionary<string, object> serializedStates = new Dictionary<string, object>();
            foreach ( ISaveable component in GetComponents<ISaveable>() ) {
                serializedStates[component.GetType().ToString()] = component.CaptureState();
            }
            return serializedStates;
        }

        public void RestoreState( object serializeStates ) {
            Dictionary<string, object> originalStates = (Dictionary<string, object>)serializeStates;
            foreach ( ISaveable component in GetComponents<ISaveable>() ) {
                string typeName = component.GetType().ToString();
                if ( originalStates.ContainsKey(typeName) ) {
                    component.RestoreState(originalStates[typeName]);
                }
            }
        }

#if UNITY_EDITOR
        void Update() {
            // UUID won't change in playing mode...
            if ( Application.isPlaying ) return;

            // We only want to create UUID in instance of an prefab
            // so we check if current scene path of this object is null or not.
            if ( string.IsNullOrEmpty(gameObject.scene.path) ) return;

            // This enable the UUID property of this object could be serializable.
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            // We only generate UUID when this object's UUID hasn't been created before.
            if ( string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue)) {
                property.stringValue = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            UUIDTable[property.stringValue] = this;
        }
#endif

        private bool IsUnique( string candidate ) {
            // UI not existed...
            if ( !UUIDTable.ContainsKey(candidate) ) return true;

            // This object already existed in table...
            if ( UUIDTable[candidate] == this ) return true;

            // UI existed but doesn't contain any Object...
            if ( UUIDTable[candidate] == null ) {
                // Remove that UI for it not contain any Object...
                UUIDTable.Remove(candidate);
                return true;
            }

            // Object in the table doesn't not the same UI as this Object...
            if ( UUIDTable[candidate].GetUniqueIdentifier() != candidate ) {
                UUIDTable.Remove(candidate);
                // Remove that UI...
                return true;
            }

            return false;
        }
    }
}