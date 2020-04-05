using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core {
    public class CameraController : MonoBehaviour {
        [SerializeField]
        private Transform target;
        
        // Update is called once per frame
        private void LateUpdate ( ) {
            transform.position = target.position;
        }
    }
}
