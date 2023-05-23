using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{

    public GameObject spherePrefab;
    public PuzzleBase PuzzleBasePrefab;
    private PuzzleBase _puzzlebase;
    public GameObject PuzzlePiecePrefab;
    public GameObject PuzzlePiece2Prefab;
    public GameObject ResultToSpawnPrefab;
    public List<GameObject> Puzzlepieces = new();

    public GameObject GetResultToSpawnPrefab()
    {
        if (ResultToSpawnPrefab == null)
        {   
            return null;
        } 
        else 
        {
            return ResultToSpawnPrefab;
        }
    }
    public void CreatePuzzle()
    {
        _puzzlebase = Instantiate(PuzzleBasePrefab, Vector3.zero, Quaternion.identity);
        _puzzlebase.SetManager(this);
    }

    public void SpawnNextPiece()
    {
        if (_puzzlebase)
        {
            GameObject _puzzlepiece = Instantiate(PuzzlePiece2Prefab, Vector3.zero, Quaternion.identity);
            _puzzlepiece = CreateSphere(_puzzlepiece);
            Puzzlepieces.Add(_puzzlepiece);
        }
        if (Puzzlepieces.Count == 2)
        {
            GameObject _puzzlepiece = Instantiate(PuzzlePiecePrefab, Vector3.zero, Quaternion.identity);
            CreateSphere(_puzzlepiece);
            Puzzlepieces.Add(_puzzlepiece);
        }
    }

    private GameObject CreateSphere(GameObject puzzlepiece)
    {
        GameObject sphere = Instantiate(spherePrefab, puzzlepiece.transform.position, puzzlepiece.transform.rotation);
        sphere.transform.SetParent(puzzlepiece.transform);
        sphere.transform.localScale = puzzlepiece.transform.localScale / 140;

        return sphere;
    }
}
