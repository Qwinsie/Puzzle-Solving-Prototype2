using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PuzzleBase : MonoBehaviour
{
    private bool IsVisitorMode = false;
    private bool IsPuzzleSolved = false;

    public GameObject objectToSpawnPrefab;
    public PuzzleCollider spherePrefab;

    private List<GameObject> PuzzlePieces = new();
    private Dictionary<PuzzleCollider, GameObject> PuzzlePieceSpheres = new();
    // private List<Vector3> Colliders = new();
    private Dictionary<PuzzleCollider, bool> _pieces = new();

    public void StartPosition()
    {
        IsVisitorMode = true;
    }

    public void SavePosition()
    {
        GetPuzzlePiecesPositions();
        // TODO: Save PuzzlePiece Positions in backend
    }

    void GetPuzzlePiecesPositions()
    {
        // RemoveAllSpheres();
        PuzzlePieces.AddRange(GameObject.FindGameObjectsWithTag("puzzlepiece"));
        for (int i = 0; i < PuzzlePieces.Count; i++)
        {
            PuzzleCollider collider = CreateSphere(PuzzlePieces[i]);
            PuzzlePieceSpheres.Add(collider, PuzzlePieces[i].transform.GetChild(0).gameObject);
            _pieces.Add(collider, false);
        }

    }

    private void RemoveAllSpheres()
    {

        // if (PuzzlePieceSpheres != null)
        // {
        //     for (int i = 0; i < PuzzlePieceSpheres.Count; i++)
        //     {
        //         Destroy(PuzzlePieceSpheres[i].gameObject);
        //     };
        //     PuzzlePieceSpheres.Clear();
        // };
    }

    private PuzzleCollider CreateSphere(GameObject puzzlepiece)
    {
        PuzzleCollider sphere = Instantiate(spherePrefab, puzzlepiece.transform.position, puzzlepiece.transform.rotation);

        sphere.SetBase(this);
        sphere.transform.SetParent(this.gameObject.transform);
        sphere.transform.localScale = puzzlepiece.transform.localScale / 140;

        return sphere;
    }

    public bool CheckPuzzlePiece(PuzzleCollider yellow, Collision blue)
    {
        if (IsVisitorMode)
        {
            if (!_pieces.ContainsKey(yellow))
            {
                return false;
            }

            if (blue == null)
            {
                _pieces[yellow] = false;
                return false;
            }

            if (!PuzzlePieceSpheres.ContainsKey(yellow))
            {
                return false;
            }

            GameObject collisionObject = PuzzlePieceSpheres[yellow];
            if (blue.gameObject != collisionObject)
            {
                return false;
            }

            Quaternion AnglePieceRotation = blue.transform.rotation;
            Quaternion AngleBaseRotation = yellow.transform.rotation;
            float AngleMarge = 50f;
            if (Quaternion.Angle(AnglePieceRotation, AngleBaseRotation) < AngleMarge)
            {
                _pieces[yellow] = true;
                CheckPuzzle();
                return true;
            }
            else
            {
                _pieces[yellow] = false;
                return false;
            }
        }
        return false;
    }

    private bool CheckPuzzle()
    {
        IsPuzzleSolved = _pieces.All(x => x.Value);
        if (IsPuzzleSolved)
        {
            StartCoroutine(DestroyObjects());
        };

        return IsPuzzleSolved;
    }

    IEnumerator DestroyObjects()
    {
        for (int i = 0; i < PuzzlePieces.Count; i++)
        {
            StartCoroutine(ScaleDownAnimation(0.5f, PuzzlePieces[i]));
        }

        StartCoroutine(ScaleDownAnimation(0.5f, this.gameObject));
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
        for (int i = 0; i < PuzzlePieces.Count; i++)
        {
            Destroy(PuzzlePieces[i]);
        }
        SpawnResultObject();
    }

    private void SpawnResultObject()
    {
        Instantiate(objectToSpawnPrefab, this.gameObject.transform.position, Quaternion.identity);
    }

    IEnumerator ScaleDownAnimation(float time, GameObject gameobject)
    {
        float i = 0;
        float rate = 1 / time;

        Vector3 fromScale = gameobject.transform.localScale;
        Vector3 toScale = Vector3.zero;
        Quaternion fromRotation = Quaternion.Euler(0, 180, 0);
        Quaternion toRotation = Quaternion.identity;

        while (i < 1)
        {
            i += Time.deltaTime * rate;
            gameobject.transform.localScale = Vector3.Lerp(fromScale, toScale, i);
            gameobject.transform.rotation = Quaternion.Lerp(fromRotation, toRotation, i);
            yield return 0;
        }
    }
}
