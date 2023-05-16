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
        Debug.Log(collision.gameObject.tag == "puzzlecollider");
        if (collision.gameObject.tag == "puzzlecollider")
        {
            if (_base.CheckPuzzlePiece(this, collision))
            {
                Debug.Log("Touching");
            }
            else
            {
                Debug.Log("Not Touching");
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "puzzlecollider")
        {
            _base.CheckPuzzlePiece(this, null);
            Debug.Log("Not Touching anymore");
        }
    }
}
