using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCollider : MonoBehaviour
{
    private PuzzleBase _base;
    private bool IsCorrect; 

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
        if(!_base.GetIsPuzzleSolved())
        {
            if (collision.gameObject.tag == "puzzlecollider")
            {
                if (_base.CheckPuzzlePiece(this, collision))
                {
                    Debug.Log("Correct");
                }
                else
                {
                    Debug.Log("Incorrect");
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(!_base.GetIsPuzzleSolved())
        {
            if (collision.gameObject.tag == "puzzlecollider")
            {
                _base.CheckPuzzlePiece(this, null);
                Debug.Log("Not correct anymore");
                IsCorrect = false;
            }
        }
    }
}
