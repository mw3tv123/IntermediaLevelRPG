using RPG.Control;
using System.Collections;
using UnityEngine;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour, IRaycastable {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 3f;

        void OnTriggerEnter(Collider other) {
            if ( other.gameObject.tag == "Player" )
                Pickup(other.GetComponent<Fighter>());
        }

        private void Pickup( Fighter other ) {
            other.EquipWeapon(weapon);
            StartCoroutine(Respawn(respawnTime));
        }

        private IEnumerator Respawn(float time) {
            ShowPickup(false);
            yield return new WaitForSeconds(time);
            ShowPickup(true);
        }

        /// <summary>
        /// Show/Hide this pickup object.
        /// </summary>
        /// <param name="isShow"></param>
        private void ShowPickup(bool isShow) {
            GetComponent<Collider>().enabled = isShow;
            foreach ( Transform child in transform )
                child.gameObject.SetActive(isShow);
        }

        public bool HandleRayCast( PlayerController player ) {
            if ( Input.GetMouseButtonDown(0) ) {
                Pickup(player.GetComponent<Fighter>());
            }
            return true;
        }
    }
}
