using UnityEngine;

public class ColorChange : MonoBehaviour
{
    private Material _material;

    void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    public void ChangeColor()
    {
        _material.color = Color.red;
    }
}