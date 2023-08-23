using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomResolution : MonoBehaviour
{
    [SerializeField]
    private int height;
    [SerializeField]
    private int width;

    void Start()
    {
        Screen.SetResolution(width, height, false);
    }
}
