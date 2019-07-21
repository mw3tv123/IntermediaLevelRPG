using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour {
    NavMeshAgent agent;     // Reference to this object's NavMeshAgent for navigating/moving object around.
    Animator animator;
    Ray controller_ray;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButton(0))
            Move();
        UpdataAnimator();
    }

    private void Move() {
        controller_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(controller_ray, out RaycastHit target, 50)) {
            agent.SetDestination(target.point);
        }
    }
    private void UpdataAnimator() {
        float speed = transform.InverseTransformDirection(agent.velocity).z;
        animator.SetFloat("speed", speed);
    }
}
