using System;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour {
    NavMeshAgent agent;     // Reference to this object's NavMeshAgent for navigating/moving object around.

    Ray controller_ray;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0))
            Move();
    }

    private void Move() {
        controller_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(controller_ray, out RaycastHit target, 50)) {
            agent.SetDestination(target.point);
        }
    }
}
