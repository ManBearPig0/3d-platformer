using System.Collections;
using UnityEngine;

public class FallingObstacle : MonoBehaviour
{
    private float OriginalHeight;

    void Start()
    {
        OriginalHeight = transform.position.y;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            StartCoroutine(Crumble());
        }
    }

    IEnumerator Crumble()
    {
        yield return new WaitForSeconds(2f);
        float CrumbledTime = Time.time;
        while (Time.time - CrumbledTime < 4f)
        {
            transform.Translate(0, -0.1f, 0);
            yield return null;
        }
        while (transform.position.y < OriginalHeight)
        {
            transform.Translate(0, 0.1f, 0);
            yield return null;
        }
    }
}
