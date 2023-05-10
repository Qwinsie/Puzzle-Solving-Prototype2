using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PuzzleBase : MonoBehaviour
{
    private bool visitorMode = false;
    public GameObject objectToSpawnPrefab;
    private bool finishedPuzzle = false;
    public PuzzleCollidor spherePrefab;
    private GameObject[] PuzzlePieces;
    public static List<Vector3> Collidors = new List<Vector3>();
    private Dictionary<PuzzleCollidor, bool> _pieces = new();

    List<Vector3> getPuzzlePiecesPositions () {
        
        PuzzlePieces = GameObject.FindGameObjectsWithTag("puzzlepiece");
        for (int i = 0; i < PuzzlePieces.Length; i++)
        {
            CreateSphere(PuzzlePieces[i], PuzzlePieces[i].transform.position,PuzzlePieces[i].transform.localScale, PuzzlePieces[i].transform.rotation );
            Collidors.Add(PuzzlePieces[i].transform.position);
        }
        
        return Collidors;
    }

    private void CreateSphere (GameObject puzzlepiece, Vector3 position, Vector3 scale, Quaternion rotation)
    {
        PuzzleCollidor sphere = Instantiate(spherePrefab, position, rotation);
        _pieces.Add(sphere, false);
        sphere.SetBase(this);
        sphere.transform.SetParent(this.gameObject.transform);
        sphere.transform.localScale = scale / 140;
    }

    public bool SetCorrect(PuzzleCollidor piece)
    {
        if (_pieces.ContainsKey(piece))
        {
            _pieces[piece] = true;
        }

        return CheckPuzzle();
    }

    private bool CheckPuzzle()
    {
        finishedPuzzle = _pieces.All(x => x.Value);
        if (visitorMode)
        {
            // SpawnResultObject(this.gameObject.);
            // TODO: play animation 
            // Add Comment
            Debug.Log("Animation");
        };

        return finishedPuzzle;
    }

    public void savePosition()
    {
        List<Vector3> PuzzlePiecesPositions = getPuzzlePiecesPositions();
        // Save PuzzlePiece Positions in backend
    }

    public void startPosition()
    {
        Debug.Log("Starting Position");
        visitorMode = true;
    }

    private void SpawnResultObject(Collision collision)
    {
        Instantiate(objectToSpawnPrefab,  Vector3.Lerp(this.gameObject.transform.position, collision.transform.position, 0.5f),  Quaternion.identity);
    }

    IEnumerator DestroyObjects(Collision collision)
    {
        StartCoroutine(ScaleDownAnimation(0.5f, this.gameObject)); 
        StartCoroutine(ScaleDownAnimation(0.5f, collision.gameObject));
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
        Destroy(collision.gameObject);
        SpawnResultObject(collision);
    }

    IEnumerator ScaleDownAnimation(float time, GameObject gameobject)
    {
        float i = 0;
        float rate = 1 / time;

        Vector3 fromScale = gameobject.transform.localScale;
        Vector3 toScale = Vector3.zero;
        Quaternion fromRotation = Quaternion.Euler(0, 180, 0);
        Quaternion toRotation = Quaternion.identity;

        while (i<1)
        {
            i += Time.deltaTime * rate;
            gameobject.transform.localScale = Vector3.Lerp(fromScale, toScale, i);
            gameobject.transform.rotation = Quaternion.Lerp(fromRotation, toRotation, i);
            yield return 0;
        }
    }
}
