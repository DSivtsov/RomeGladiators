using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
public class ClickToMove : MonoBehaviour {
    RaycastHit hitInfo = new RaycastHit();
    UnityEngine.AI.NavMeshAgent agent;

    void Start () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
    }

    Vector3 time;
    Ray ray;
    void Update () {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                Vector3 destination = hitInfo.point;
                //destination = transform.position + Vector3.forward * 20;
                agent.destination = destination;
                GameObject Cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Cube.transform.position = destination;
                Debug.Log($"agent.nextPosition={agent.nextPosition} agent.nextPosition={agent.destination}");
            }


        }
    }
}
