using System.Collections;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    private float MoveDistance;
    private Vector3 OriginalPosition;

    void Start()
    {
        MoveDistance = 4f;
        OriginalPosition = transform.position;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            while (Vector3.Distance(transform.position, OriginalPosition) < MoveDistance)
            {
                transform.Translate(0.1f, 0, 0);
                yield return null;
            }
            yield return new WaitForSeconds(2f);
            transform.Translate(-0.1f, 0, 0); // Obstacle has to move back in range.

            while (Vector3.Distance(transform.position, OriginalPosition) < MoveDistance)
            {
                transform.Translate(-0.1f, 0, 0);
                yield return null;
            }
            yield return new WaitForSeconds(2f);
            transform.Translate(0.1f, 0, 0);
        }
    }
}
