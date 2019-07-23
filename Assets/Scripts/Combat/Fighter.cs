using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] float attackRange = 2f;
        [SerializeField] Transform target;

        void Update() {
            if (target == null) return;
            if (Vector3.Distance(target.position, transform.position) > attackRange)
                GetComponent<Mover>().MoveToPoint(target.position);
            else
                GetComponent<Mover>().Cancel();

        }

        public void Attack(CombatTarget combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel() {
            target = null;
        }
    }
}

