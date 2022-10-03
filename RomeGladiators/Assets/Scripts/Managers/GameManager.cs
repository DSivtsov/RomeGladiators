using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Supporing module
 * To have a possibility to stop the StateMachine completly don't breake work of running processes, like Coroutune
 */
public class GameManager : MonoBehaviour
{
    private const int DyingTime = 5;

    public void FinishGame()
    {
        FindObjectOfType<SingletonGladiatorsManager>().gameObject.SetActive(false);
    }

    public void CallCoroutineGladiatorDying(GameObject gameObject)
    {
        StartCoroutine(GladiatorDying(gameObject));
    }
    private IEnumerator GladiatorDying(GameObject removedObject)
    {
        int DyingTimeLeftTicks = DyingTime;
        removedObject.GetComponent<MeshRenderer>().material = SingletonGladiatorsManager.Instance.GladiatorDyingMaterial;
        do
        {
            Vector3 _deltaDown = 2 * Vector3.down / DyingTimeLeftTicks;
            removedObject.transform.position += _deltaDown;
            DyingTimeLeftTicks--;
            yield return null;
        } while (DyingTimeLeftTicks > 0);
        removedObject.SetActive(false);
    }
}
