#undef TRACE
using UnityEngine;
using UnityEngine.AI;

public class BattleField
{
    private const int GladiatorYPos = 1;
    private Vector3 worldPositionZerro;
    private int maxRNDX;
    private int maxRNDZ;
    private int minRNDX;
    private int minRNDZ;

    private System.Random random;

    public BattleField()
    {
        this.worldPositionZerro = SingletonGladiatorsManager.Instance.BattfleFieldData.WordldPosZerro;
        this.maxRNDX = SingletonGladiatorsManager.Instance.BattfleFieldData.MaxPosX + 1;
        this.maxRNDZ = SingletonGladiatorsManager.Instance.BattfleFieldData.MaxPosZ + 1;
        this.minRNDX = SingletonGladiatorsManager.Instance.BattfleFieldData.MinPosX;
        this.minRNDZ = SingletonGladiatorsManager.Instance.BattfleFieldData.MinPosZ;
        if (SingletonGladiatorsManager.Instance.RandomSeedPos != 0)
            this.random = new System.Random(SingletonGladiatorsManager.Instance.RandomSeedPos);
        else
            this.random = new System.Random();
    }

    private Vector3 GetRandomPositionOnField() => worldPositionZerro + 
        new Vector3(random.Next(minRNDX,maxRNDX), GladiatorYPos, random.Next(minRNDZ, maxRNDZ));

    private bool PositionOnFieldNotWalkable(Vector3 newPos)
    {
        int WalkableMask = 1 << NavMesh.GetAreaFromName("Walkable");
        if (NavMesh.FindClosestEdge(newPos, out _, WalkableMask))
        {
            GladiatorTest(newPos, Color.green);
            return false;
        }
        else
        {
            GladiatorTest(newPos, Color.red);
            return true; 
        }
    }

    [System.Diagnostics.Conditional("TRACE")]
    private static void GladiatorTest(Vector3 newPos, Color color)
    {
        GameObject newGladiator = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        newGladiator.GetComponent<MeshRenderer>().material.color = color;
        newGladiator.transform.position = newPos + Vector3.up;
    }

    public int allAttemps;
    private Vector3 GetRandomPositionOnFieldWalkable()
    {
        Vector3 newPos;
        do
        {
            allAttemps++;
            newPos = GetRandomPositionOnField();
        } while (PositionOnFieldNotWalkable(newPos));
        return newPos;
    }

    public Vector3 GetNewGladiatorPos()
    {
        //Debug.Log(GetRandomPositionOnFieldWalkable());
        //GladiatorTest(GetRandomPositionOnField(), Color.yellow);
        return GetRandomPositionOnFieldWalkable();
    }
}

