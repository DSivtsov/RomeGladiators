using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
public class ClickSetTarget : MonoBehaviour {
    RaycastHit hitInfo = new RaycastHit();
    UnityEngine.AI.NavMeshAgent agent;
    MoveFollowTarget moveTest;
    GameObject currentAimCube;

    void Start () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
        moveTest = GetComponent<MoveFollowTarget>();
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
                agent.destination = destination;
                if (!currentAimCube)
                {
                    currentAimCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                }
                currentAimCube.transform.position = destination;
                moveTest.SetDestinantion(currentAimCube.transform);
            }
        }
    }
}
