using System;
using RPG.Combat;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control {
    [RequireComponent(typeof(Mover))]
    public class PlayerController : MonoBehaviour {
        [Range(0, 1), SerializeField] float runSpeedFraction = 1f;

        Mover mover;
        Fighter combat;
        Health health;

        [Serializable]
        struct CursorMapping {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavPathLength = 20f;

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
            RaycastHit[] hits = RaycastAllSorted();
            foreach ( RaycastHit hit in hits ) {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach ( IRaycastable raycastable in raycastables ) {
                    if ( raycastable.HandleRayCast(this) ) {
                        SetCursor(raycastable.GetCursorType());
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
            if ( RaycastNavMesh( out Vector3 hit ) ) {
                if ( Input.GetMouseButton(0) )
                    mover.StartMovement(hit, runSpeedFraction);
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Cast a ray to terrain, then sample the closest NavMesh hit to the ray hit point.
        /// </summary>
        /// <param name="target">Position of the NavMesh which was caculated.</param>
        /// <returns>True if Raycast Hit in allow distance, otherwise False.</returns>
        private bool RaycastNavMesh( out Vector3 target ) {
            // Default value for the position.
            target = new Vector3();

            // If Raycast Hit a terrain, objects,...
            if ( Physics.Raycast(GetRayToMouse(), out RaycastHit hit) ) {
                // If sample to the closest NavMesh in the allow distance...
                if ( NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, 1f, NavMesh.AllAreas) ) {
                    target = navMeshHit.position;

                    // If the path to the NavMesh position in allow distance...
                    if ( IsPathInAllowDistance(target) ) return true;
                }
            }

            // If not hit a raycast or not in allow distance...
            return false;
        }

        /// <summary>
        /// Set texture for the cursor.
        /// </summary>
        /// <param name="type">Type of the cursor (see CursorType enum).</param>
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

        /// <summary>
        /// Get all objects hit by Raycast, build an array distances and then sort the hits.
        /// </summary>
        /// <returns>An ordered list of hit objects.</returns>
        private RaycastHit[] RaycastAllSorted() {
            RaycastHit[] hits = Physics.RaycastAll(GetRayToMouse());

            float[] distances = new float[hits.Length];
            // Build up distances array.
            for ( int i = 0; i < distances.Length; i++ )
                distances[i] = hits[i].distance;

            Array.Sort(distances, hits);
            return hits;
        }

        /// <summary>
        /// Calculate path from current position to the target NavMesh point.
        /// </summary>
        /// <param name="target">Position of the target NavMesh.</param>
        /// <returns>False if:
        /// - No path to the target NavMesh point (*1, *2);
        /// - Distances to target NavMesh point greater than allow distances.
        /// Otherwise return True.</returns>
        private bool IsPathInAllowDistance( Vector3 target) {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);

            // *1 No path to target.
            if ( !hasPath ) return false;
            // *2 Path to target incompleted.
            if ( path.status != NavMeshPathStatus.PathComplete ) return false;
            // Distance to to target greater than maximum distances allowed.
            if ( GetPathLength(path) > maxNavPathLength ) return false;

            return true;
        }

        /// <summary>
        /// Get the total distances  of all corner between current position and the target NavMesh position.
        /// </summary>
        /// <param name="path">Target NavMesh position.</param>
        /// <returns>(<b>float</b>)Total distances.</returns>
        private float GetPathLength( NavMeshPath path ) {
            float distance = 0;

            // If path have fewer corners than 2...
            if ( path.corners.Length < 2 ) return distance;

            // Calculate distances between all corners...
            for ( int i = 0; i < path.corners.Length - 1; i++ )
                distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);

            return distance;
        }
    }
}