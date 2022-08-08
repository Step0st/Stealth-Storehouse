using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Action PlayerCatch;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
           PlayerCatch?.Invoke();
        }
    }

}
