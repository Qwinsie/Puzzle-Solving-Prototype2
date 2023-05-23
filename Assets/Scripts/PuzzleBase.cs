using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
// using UnityEngine.UI.Button;

public class PuzzleBase : MonoBehaviour
{
    // private Button button;
    private PuzzleManager _puzzleManager;
    private bool IsVisitorMode = false;
    private bool IsPuzzleSolved = false;
    public PuzzleCollider spherePrefab;

    private List<GameObject> PuzzlePieces = new();
    private Dictionary<PuzzleCollider, GameObject> PuzzlePieceSpheres = new();
    private Dictionary<PuzzleCollider, bool> _pieces = new();

    private void Start() {
    //     Button button = myButton.getComponent<Button>();
    //     //next, any of these will work:
    //     button.onClick += myMethod;
    //     button.onClick.AddListener(myMethod);
    }

    internal void SetManager(PuzzleManager puzzleManager)
    {
        _puzzleManager = puzzleManager;
    }
    public void StartPosition()
    {
        IsVisitorMode = true;
    }

    public bool GetIsPuzzleSolved()
    {
        return IsPuzzleSolved;
    }

    public void SavePosition()
    {
        GetPuzzlePiecesPositions();
        // TODO: Save PuzzlePiece Positions in backend
    }

    void GetPuzzlePiecesPositions()
    {
        RemoveAllSpheres();
        PuzzlePieces.AddRange(GameObject.FindGameObjectsWithTag("puzzlepiece"));
        for (int i = 0; i < PuzzlePieces.Count; i++)
        {
            PuzzleCollider collider = CreateSphere(PuzzlePieces[i]);
            // TODO: Do not look on the first child.
            PuzzlePieceSpheres.Add(collider, PuzzlePieces[i].transform.GetChild(0).gameObject);
            _pieces.Add(collider, false);
        }

    }

    private void RemoveAllSpheres()
    {
        if (PuzzlePieceSpheres != null)
        {
            foreach(KeyValuePair<PuzzleCollider, GameObject> entry in PuzzlePieceSpheres)
            {
                Destroy(entry.Key.gameObject);
            }
            PuzzlePieceSpheres.Clear();
            PuzzlePieces.Clear();
            _pieces.Clear();
        };
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
                Debug.Log(1);
                _pieces[yellow] = true;
                CheckPuzzle();
                return true;
            }
            else
            {
                Debug.Log(2);
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
        if (_puzzleManager.GetResultToSpawnPrefab() == null)
        {
            Debug.LogWarning("Prefab is not assigned");
        } else {
            GameObject spawn = Instantiate(_puzzleManager.GetResultToSpawnPrefab(), this.gameObject.transform.position, Quaternion.identity);
            spawn.AddComponent<DragAndDrop>();
        }
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
