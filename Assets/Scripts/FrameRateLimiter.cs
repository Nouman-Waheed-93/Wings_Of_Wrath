using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    [SerializeField]
    private int fps;
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = fps;
    }

}
