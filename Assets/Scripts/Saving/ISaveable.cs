using UnityEngine;

namespace RPG.Saving {
    public interface ISaveable {
        /// <summary>
        /// Capture this object's properties and serialize it to an object for storing.
        /// </summary>
        /// <returns></returns>
        object CaptureState();

        /// <summary>
        /// Deserialize an object's properties to it original stage.
        /// </summary>
        /// <param name="state"></param>
        void RestoreState( object state );
    }
}