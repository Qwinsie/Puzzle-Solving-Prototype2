using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAction : MonoBehaviour
{

    public GameObject particleEffect;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(particleEffect, transform.position, particleEffect.transform.rotation);
        StartCoroutine(ScaleUpAnimation(1.5f));
    }

    IEnumerator ScaleUpAnimation(float time)
    {
        float i = 0;
        float rate = 1 / time;

        Vector3 fromScale = Vector3.zero;
        Vector3 toScale = transform.localScale;
        Quaternion fromRotation = Quaternion.identity;
        Quaternion toRotation = Quaternion.Euler(0, 180, 0);

        while (i<1)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(fromScale, toScale, i);
            transform.rotation = Quaternion.Lerp(fromRotation, toRotation, i);
            yield return 0;
        }
    }
}
