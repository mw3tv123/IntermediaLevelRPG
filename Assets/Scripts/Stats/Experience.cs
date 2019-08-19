using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Stats {
    public class Experience : MonoBehaviour, ISaveable {
        [SerializeField] float currentXP = 0;

        // Define a delegate.
        // public delegate void ExperienceGainedDelegate();
        // Using Action - a predefine delegate.
        public event Action OnExperienceGained;

        public float XP { get { return currentXP; } }

        public void GainXP( float experiencePoint ) {
            currentXP += experiencePoint;

            // Notify other class about experiences changes...
            OnExperienceGained();
        }

        public object CaptureState() { return currentXP; }

        public void RestoreState(object state) { currentXP = (float)state; }
    }
}