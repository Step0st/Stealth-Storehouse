using System;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public Action PlayerEscape;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerEscape?.Invoke();
            Debug.Log("INVOLE");
        }

        Debug.Log("Escaped");
    }
}