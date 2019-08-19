using UnityEngine;

namespace RPG.Core {
    /// <summary>
    /// Automatic destroy particle object after it done effecting (?!)
    /// </summary>
    public class DestroyAfterEffect : MonoBehaviour {
        [SerializeField] GameObject targetToDestroy = null;

        void Update() {
            if ( !GetComponent<ParticleSystem>().IsAlive() ) {
                if ( targetToDestroy != null )
                    Destroy(targetToDestroy);
                else
                    Destroy(gameObject);
            }
        }
    }
}
