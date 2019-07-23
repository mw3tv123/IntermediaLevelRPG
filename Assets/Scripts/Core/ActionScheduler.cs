using UnityEngine;

namespace RPG.Core {
    public class ActionScheduler : MonoBehaviour {
        IAction lastAction;

        public void StartAction(IAction action) {
            if (lastAction == action) return;
            if (lastAction != null)
                lastAction.Cancel();
            lastAction = action;
        }
    }
}