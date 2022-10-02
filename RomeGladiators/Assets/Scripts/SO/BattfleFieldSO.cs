using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMTools;

[CreateAssetMenu(fileName = "BattfleFieldSO", menuName = "SO/BattfleFieldSO")]
public class BattfleFieldSO : ScriptableObject
{
    //Set Min base on initial gladisators size - radius capsule 0.5 
    [ReadOnly(BaseColor.white)] public int MinPosX = 1;
    [ReadOnly(BaseColor.white)] public int MinPosZ = 1;
    [ReadOnly(BaseColor.white)] public int MaxPosX = 99;
    [ReadOnly(BaseColor.white)] public int MaxPosZ = 99;
    [ReadOnly(BaseColor.white)] public Vector3 WordldPosZerro;
    /// <summary>
    /// World position of center field
    /// </summary>
    /// <param name="_groundTransform">Transform probuilder plane</param>
    public void SetWordldPosZerro(Transform _groundTransform)
    {
        WordldPosZerro = _groundTransform.position;
    }
}
