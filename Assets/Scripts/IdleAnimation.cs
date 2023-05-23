using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimation : MonoBehaviour
{

    private new Renderer renderer;
    Color colourEnd;
    Color colourStart;
    bool forward = true;
    float alphaTarget = 0.3f;
    float rate = 0.2f;
    float i = 0f;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        colourEnd = renderer.material.color;
        colourStart = renderer.material.color;
        colourEnd.a = alphaTarget;
    }
    void Update()
    {
        // Blend towards the current target colour
        i = i + (Time.deltaTime * rate);
        renderer.material.color = Color.Lerp(colourStart, colourEnd, Mathf.PingPong(i * 2, 1));
        // If we've got to the current target colour, choose a new one
        if (i >= 1)
        {
            colourStart = colourEnd;
            if (!forward)
            {
                alphaTarget = 0.3f;
                forward = true;
            }
            else
            {
                alphaTarget = 0.5f;
                forward = false;
            }
            colourEnd.a = alphaTarget;
            i = 0;
        }
    }
}
