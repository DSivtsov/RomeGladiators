// MeasureSpace
using UnityEngine;
using UnityEngine.AI;

public class MeasureSpace : MonoBehaviour
{
    private bool notFound = false;

    void DrawCircle(Vector3 center, float radius, Color color)
    {
        Vector3 prevPos = center + new Vector3(radius, 0, 0);
        for (int i = 0; i < 30; i++)
        {
            float angle = (float)(i + 1) / 30.0f * Mathf.PI * 2.0f;
            Vector3 newPos = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Debug.DrawLine(prevPos, newPos, color);
            prevPos = newPos;
        }
    }

    void Update()
    {
        NavMeshHit hit;
        int WalkableMask = 1 << NavMesh.GetAreaFromName("Walkable");
        if (NavMesh.FindClosestEdge(transform.position, out hit, WalkableMask))
        {
            DrawCircle(transform.position, hit.distance, Color.red);
            Debug.DrawRay(hit.position, Vector3.up, Color.red);
            notFound = false;
        }
        else
        {
            if (!notFound)
            {
                notFound = true;
                Debug.Log("notFound = true");
            }
        }
    }
}