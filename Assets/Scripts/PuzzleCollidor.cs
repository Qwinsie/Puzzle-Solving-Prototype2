using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCollidor : MonoBehaviour
{
    private PuzzleBase _base;

    internal void SetBase(PuzzleBase puzzleBase)
    {
        _base = puzzleBase;
    }

    void Awake()
    {
        Debug.Log("Created Sphere");
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint c in collision.contacts)
        {
            Debug.Log(c.thisCollider.name);
        }

        // Debug.Log(collision.gameObject.name);
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "puzzlecollidor")
        {
            Quaternion AnglePieceRotation =  this.transform.rotation;
            Quaternion AngleBaseRotation =  collision.transform.rotation;
            float AngleMarge = 50f;
            if (Quaternion.Angle(AnglePieceRotation, AngleBaseRotation) < AngleMarge)
            {
                Debug.Log("Correct Rotation");
                _base.SetCorrect(this);
            }
            Debug.Log("Touching");
        }
    }
}
