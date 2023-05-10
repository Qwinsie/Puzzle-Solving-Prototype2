using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public GameObject objectToSpawnPrefab;

  void OnCollisionEnter(Collision collision)
    {

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "3DObject")
        {
            StartCoroutine(DestroyObjects(collision));
        }
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
