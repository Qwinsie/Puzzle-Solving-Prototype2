using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCollider : MonoBehaviour
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
        if (collision.gameObject.tag == "puzzlecollider")
        {
            Quaternion AnglePieceRotation = this.transform.rotation;
            Quaternion AngleBaseRotation = collision.transform.rotation;
            float AngleMarge = 50f;
            if (Quaternion.Angle(AnglePieceRotation, AngleBaseRotation) < AngleMarge)
            {
                Debug.Log("Correct Rotation");
                _base.SetCorrect(this);
            }
            Debug.Log("Touching");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "puzzlecollider")
        {
            _base.SetIncorrect(this);
            Debug.Log("Not Touching anymore");
        }
    }
}
