using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    List<Waypoint> path = new List<Waypoint>();
    [SerializeField] [Range(0f, 5f)] float speed = 1f;

    Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void OnEnable()
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowPath());
    }

    void FindPath()
    {
        path.Clear();
        path
            .AddRange(
                GameObject
                    .FindGameObjectWithTag("Path")
                    .GetComponentsInChildren<Waypoint>());
    }

    void ReturnToStart()
    {
        transform.position = path.First().transform.position;
    }

    void FinishPath()
    {
        enemy.StealGold();
                gameObject.SetActive(false);
    }
    
    IEnumerator FollowPath()
    {
        foreach (Waypoint waypoint in path)
        {
            yield return MoveTo(waypoint);
        }

        FinishPath();
    }

    IEnumerator MoveTo(Waypoint waypoint)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = waypoint.transform.position;
        float travelPercent = 0f;

        transform.LookAt(endPosition);

        while (travelPercent < 1f)
        {
            travelPercent += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
            yield return new WaitForEndOfFrame();
        }
    }
}