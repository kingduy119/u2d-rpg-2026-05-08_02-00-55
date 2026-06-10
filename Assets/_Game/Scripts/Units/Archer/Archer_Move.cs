using System.Collections;
using UnityEngine;

public class Archer_Move : MonoBehaviour
{
    public Vector2 zoneSize = new Vector2(5f, 5f);

    private Vector2 target;
    public Vector2 startPosition;
    private bool isPaused;
    public float pauseDuration = 1.5f;

    private BaseMovement m_BaseMovement;

    void Start()
    {
        m_BaseMovement = GetComponent<BaseMovement>();
        target = GetRandomTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            m_BaseMovement.SetState(State.Idle);
            return;
        }

        if (Vector2.Distance(transform.position, target) < .1f)
            StartCoroutine(PauseAndPickNewDestination());

        Move();
    }

    IEnumerator PauseAndPickNewDestination()
    {
        isPaused = true;
        m_BaseMovement.SetState(State.Idle);


        yield return new WaitForSeconds(pauseDuration);

        target = GetRandomTarget();
        isPaused = false;
    }

    void Move()
    {
        m_BaseMovement.SetDirection(target - (Vector2)transform.position);
        m_BaseMovement.SetState(State.Moving);
    }

    private Vector2 GetRandomTarget()
    {
        float randomX = Random.Range(startPosition.x - zoneSize.x / 2, startPosition.x + zoneSize.x / 2);
        float randomY = Random.Range(startPosition.y - zoneSize.y / 2, startPosition.y + zoneSize.y / 2);
        return new Vector2(randomX, randomY);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector3)startPosition, new Vector3(zoneSize.x, zoneSize.y, 0));
    }
}
