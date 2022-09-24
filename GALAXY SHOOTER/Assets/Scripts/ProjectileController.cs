using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private Vector2 m_Direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(m_Direction * Time.deltaTime * m_MoveSpeed);
    }
    public void Fire()
    {
        Destroy(gameObject, 3f);
    }
}
