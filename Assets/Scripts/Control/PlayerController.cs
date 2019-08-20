using System;
using RPG.Combat;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Control {
    [RequireComponent(typeof(Mover))]
    public class PlayerController : MonoBehaviour {
        [Range(0, 1), SerializeField] float runSpeedFraction = 1f;

        Mover mover;
        Fighter combat;
        Health health;

        enum CursorType {
            None,
            Movement,
            Combat,
            UI,
        }

        [Serializable]
        struct CursorMapping {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;

        // Start is called before the first frame update
        void Start() {
            mover = GetComponent<Mover>();
            combat = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update() {
            if ( InteractWithUI() ) return;

            // If this Player is DEAD, he can't control anything else...
            if ( health.IsDeath ) {
                SetCursor(CursorType.None);
                return;
            }

            if ( InteractWithComponent() ) return;

            if ( Move() ) return;

            SetCursor(CursorType.None);
        }

        /// <summary>
        /// Generic way to interact with object which have IRaycastable component.
        /// </summary>
        /// <returns></returns>
        private bool InteractWithComponent() {
            RaycastHit[] hits = Physics.RaycastAll(GetRayToMouse());
            foreach ( RaycastHit hit in hits ) {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach ( IRaycastable raycastable in raycastables ) {
                    if ( raycastable.HandleRayCast(this) ) {
                        SetCursor(CursorType.Combat);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Define how cursor interact with UI.
        /// </summary>
        /// <returns></returns>
        private bool InteractWithUI() {
            if ( EventSystem.current.IsPointerOverGameObject() ) {
                SetCursor(CursorType.UI);
                return true;
            }

            return false;
        }

        // Casting a ray to the point where Player clicked and move to that point.
        private bool Move() {
            if ( Physics.Raycast(GetRayToMouse(), out RaycastHit hit) ) {
                if ( Input.GetMouseButton(0) )
                    mover.StartMovement(hit.point, runSpeedFraction);
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private void SetCursor( CursorType type ) {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        /// <summary>
        /// Get the cursor reference to the Cursor Type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private CursorMapping GetCursorMapping( CursorType type ) {
            foreach ( CursorMapping cursor in cursorMappings ) {
                if ( cursor.type == type )
                    return cursor;
            }
            return cursorMappings[0];
        }

        /// <summary>
        /// Create a Ray from middle of the scene to where the mouse point.
        /// </summary>
        private static Ray GetRayToMouse() {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}