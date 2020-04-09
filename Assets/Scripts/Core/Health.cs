using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core {
    /// <summary>
    /// Manage an object's health status like Health Point [HP], Magic Point [MP], Stamina Point [SP],...
    /// </summary>
    public class Health : MonoBehaviour {
        [SerializeField]
        private float healthPoint = 100f;

        [SerializeField]
        private bool UseRagDollPhysic = false;

        public bool IsDead => healthPoint == 0;

        public void TakeDamage ( float damage ) {
            if ( healthPoint > 0 ) {
                healthPoint = Mathf.Max(healthPoint - damage, 0);
                if ( IsDead ) {
                    GetComponent<Animator>().SetTrigger("Die");
                    GetComponent<NavMeshAgent>().enabled = false;
                    GetComponent<ActionScheduler>().CancelCurrentAction();
                    #region  Prototype, for using with rag-doll only.
                    if ( UseRagDollPhysic ) {
                        GetComponent<Animator>().enabled = false;
                        GetComponent<NavMeshAgent>().enabled = false;
                        GetComponent<CapsuleCollider>().enabled = false;
                    }
                    #endregion
                }
            }
        }
    }
}