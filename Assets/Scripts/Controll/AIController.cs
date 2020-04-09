using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control {
    /// <summary>
    /// Manage Mob AI basic controller.
    /// </summary>
    public class AIController : MonoBehaviour {
        #region Debugger Properties
        [SerializeField]
        private bool DebugMode = false;
        #endregion

        #region Behaviors Properties
        [SerializeField]
        private float FieldOfView = 10f;

        [SerializeField]
        private PatrolRoute patrolRoute;

        [SerializeField]
        private float WaypointTolerance = 1f;

        [SerializeField]
        private float WaypointDwellingTime = 5f;

        [SerializeField]
        private float SuspicionTime = 3f;           // The time this object stay at the last saw Player position.

        private float TimeSinceLastSawPlayer = Mathf.Infinity;
        private float TimeSinceArrivedAtWaypoint = Mathf.Infinity;
        private int CurrentWaypointIndex = 0;       // The index of recently waypoint.
        private Vector3 nextPosition => patrolRoute.GetCurrentWaypoint(CurrentWaypointIndex).position;
        #endregion

        #region Controllers
        private Health player;
        private Health self;
        private Fighter fighter;
        private Mover mover;
        #endregion

        #region Distance Properties
        // Return distance between this object to player.
        private float DistanceToPlayer => Vector3.Distance(transform.position, player.transform.position);

        // Return distance between this object to the current waypoint.
        private float DistanceToWaypoint => Vector3.Distance(transform.position, nextPosition);
        #endregion

        #region Checker Properties
        // Determine if Player is in this object's field of view or not.
        private bool PlayerIsInFOV => DistanceToPlayer < FieldOfView;

        // Determine if this object is suspicious when saw a Player or not.
        private bool IsSuspicious => TimeSinceLastSawPlayer < SuspicionTime;

        // Determine if this object dwelling at current waypoint long enough or not.
        private bool EnoughDwellingTime => TimeSinceArrivedAtWaypoint > WaypointDwellingTime;

        // Determine if this object is close to current waypoint or not.
        private bool AtTheWaypoint => DistanceToWaypoint < WaypointTolerance;
        #endregion

        private void Start ( ) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            self = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
        }

        private void Update ( ) {
            if ( self.IsDead ) return;  // If this object is dead, block it from do anything.

            if ( PlayerIsInFOV ) {      // If player is in field of view...
                Attack();
            }
            else if ( IsSuspicious ) {  // If this object still in suspicious...
                Suspicious();
            }
            else {                      // Player is out of view...
                Patrol();
            }
            UpdateTimers();
        }

        // Attack player in sight of view.
        private void Attack ( ) {
            TimeSinceLastSawPlayer = 0;
            fighter.StartFightAction(player);

            // For debug purpose.
            DebugCurrentTarget();
        }

        // Stay guard at current position.
        private void Suspicious ( ) {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        // Move between patrol routes.
        private void Patrol ( ) {
            if ( patrolRoute is null ) return;

            if ( AtTheWaypoint ) {
                TimeSinceArrivedAtWaypoint = 0;
                CurrentWaypointIndex = patrolRoute.GetNextWaypointIndex(CurrentWaypointIndex);
            }
            if ( EnoughDwellingTime ) {
                mover.StartMoveAction(nextPosition);
            }
        }

        private void UpdateTimers ( ) {
            TimeSinceLastSawPlayer += Time.deltaTime;
            TimeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        #region Debug
        // Draw a line to the current target player.
        private void DebugCurrentTarget ( ) {
            if ( DebugMode )
                Debug.DrawLine(transform.position, player.transform.position);
        }

        // Call by Unity, when this object is selected.
        private void OnDrawGizmosSelected ( ) {
            if ( !DebugMode )
                DrawGizmos();
        }

        // Call by Unity, always draw a gizmos if enable.
        private void OnDrawGizmos ( ) {
            if ( DebugMode )
                DrawGizmos();
        }

        private void DrawGizmos ( ) => Gizmos.DrawWireSphere(transform.position, FieldOfView);
        #endregion
    }
}