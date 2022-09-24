using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private Transform[] m_WayPoints;

    private int m_CurrentWayPointIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int nextWaypoint = m_CurrentWayPointIndex + 1;
        if(nextWaypoint > m_WayPoints.Length -1)
        {
            nextWaypoint = 0;
        }
        transform.position = Vector3.MoveTowards(transform.position, m_WayPoints[nextWaypoint].position, m_MoveSpeed * Time.deltaTime);
        if(transform.position == m_WayPoints[nextWaypoint].position)
        {
            m_CurrentWayPointIndex = nextWaypoint;
        }

        Vector3 direction = m_WayPoints[nextWaypoint].position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }
}
