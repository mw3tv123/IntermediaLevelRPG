using UnityEngine;

namespace RPG.Core {
    /// <summary>
    /// Automatic destroy particle object after it done effecting (?!)
    /// </summary>
    public class DestroyAfterEffect : MonoBehaviour {
        void Update() {
            if ( !GetComponent<ParticleSystem>().IsAlive() )
                Destroy(gameObject);
        }
    }
}
