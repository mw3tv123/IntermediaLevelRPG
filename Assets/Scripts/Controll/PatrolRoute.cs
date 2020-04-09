using UnityEngine;

namespace RPG.Control {
    /// <summary>
    /// Manage patrol paths and waypoints.
    /// </summary>
    public class PatrolRoute : MonoBehaviour {
        [SerializeField]
        [Range(0.05f, 1f)]
        private float waypointScale = 0.1f;

        // Call by Unity automaticly.
        private void OnDrawGizmos ( ) {
            for ( int i = 0; i < transform.childCount; i++ ) {
                Transform waypoint = GetCurrentWaypoint(i);
                Transform nextWaypoint = GetCurrentWaypoint(GetNextWaypointIndex(i));

                // Draw a sphere at current way point.
                Gizmos.DrawSphere(waypoint.position, waypointScale);

                // Draw a line to the next way point.
                Debug.DrawLine(waypoint.position, nextWaypoint.position);
            }
        }

        /// <summary>
        /// Return the next waypoint index of current waypoint index.
        /// Return 0 if the next waypoint index is out of range.
        /// </summary>
        /// <param name="index">Current waypoint index.</param>
        public int GetNextWaypointIndex ( int index ) {
            if ( index + 1 == transform.childCount )
                return 0;
            return index + 1;
        }

        /// <summary>
        /// Get the current waypoint at the index i from the Patrol Routes.
        /// </summary>
        /// <param name="i">Index of the waypoint.</param>
        /// <returns>Way point at index i.</returns>
        public Transform GetCurrentWaypoint ( int i ) => transform.GetChild(i);
    }
}
    