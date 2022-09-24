using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private ProjectileController m_Projectile;
    [SerializeField] private Transform m_FiringPoint;
    [SerializeField] private float m_FiringCooldown;

    private float m_TempCooldown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical);
        transform.Translate(direction * Time.deltaTime * m_MoveSpeed);

        if(Input.GetKey(KeyCode.Space))
        {
            if(m_TempCooldown <=0)
            {
                Fire();
                m_TempCooldown = m_FiringCooldown;
            }
        }

        m_TempCooldown -= Time.deltaTime;
    }

    private void Fire()
    {
        ProjectileController projectile =Instantiate(m_Projectile, m_FiringPoint.position, Quaternion.identity, null);
        projectile.Fire();
    }
}
