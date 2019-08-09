using System;
using System.Collections;
using UnityEngine;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 3f;

        void OnTriggerEnter(Collider other) {
            if ( other.gameObject.tag == "Player" ) {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(Respawn(respawnTime));
            }
        }

        private IEnumerator Respawn(float time) {
            ShowPickup(false);
            yield return new WaitForSeconds(time);
            ShowPickup(true);
        }

        private void ShowPickup(bool isShow) {
            GetComponent<Collider>().enabled = isShow;
            foreach ( Transform child in transform )
                child.gameObject.SetActive(isShow);
        }
    }
}
