using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PuzzleBase : MonoBehaviour
{
    private bool IsVisiterMode = false;
    private bool IsPuzzleSolved = false;

    public GameObject objectToSpawnPrefab;
    public PuzzleCollider spherePrefab;

    private List<GameObject> PuzzlePieces = new();
    private List<PuzzleCollider> PuzzlePieceSpheres = new();
    public static List<Vector3> Colliders = new();
    private Dictionary<PuzzleCollider, bool> _pieces = new();

    public void startPosition()
    {
        IsVisiterMode = true;
    }

    public void savePosition()
    {
        List<Vector3> PuzzlePiecesPositions = getPuzzlePiecesPositions();
        // TODO: Save PuzzlePiece Positions in backend
    }

    List<Vector3> getPuzzlePiecesPositions()
    {
        RemoveAllSpheres();
        PuzzlePieces.AddRange(GameObject.FindGameObjectsWithTag("puzzlepiece"));
        for (int i = 0; i < PuzzlePieces.Count; i++)
        {
            CreateSphere(PuzzlePieces[i], PuzzlePieces[i].transform.position, PuzzlePieces[i].transform.localScale, PuzzlePieces[i].transform.rotation);
            Colliders.Add(PuzzlePieces[i].transform.position);
        }

        return Colliders;
    }

    private void RemoveAllSpheres()
    {

        if (Colliders != null) { Colliders.Clear(); };
        if (PuzzlePieceSpheres != null)
        {
            for (int i = 0; i < PuzzlePieceSpheres.Count; i++)
            {
                // TODO: Do not only destroy the script attached to the GameObject, but also the GameObject itself.
                Destroy(PuzzlePieceSpheres[i]);
            };
            PuzzlePieceSpheres.Clear();
        };
    }

    private void CreateSphere(GameObject puzzlepiece, Vector3 position, Vector3 scale, Quaternion rotation)
    {
        PuzzleCollider sphere = Instantiate(spherePrefab, position, rotation);
        PuzzlePieceSpheres.Add(sphere);
        _pieces.Add(sphere, false);
        sphere.SetBase(this);
        sphere.transform.SetParent(this.gameObject.transform);
        sphere.transform.localScale = scale / 140;
    }

    public bool SetCorrect(PuzzleCollider piece)
    {
        if (IsVisiterMode)
        {
            if (_pieces.ContainsKey(piece))
            {
                Debug.Log(_pieces.All(x => x.Value));
                _pieces[piece] = true;
            }
        }

        return CheckPuzzle();
    }

    public bool SetIncorrect(PuzzleCollider piece)
    {
        if (IsVisiterMode)
        {
            if (_pieces.ContainsKey(piece))
            {
                _pieces[piece] = false;
            }
        }

        return CheckPuzzle();
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
