using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFX : MonoBehaviour
{
    [SerializeField] private float m_LifeTime;

    private float m_CurrentLifeTime;
    private ParticleFXsPool m_Pool;
    // Start is called before the first frame update
    private void OnEnable()
    {
        m_CurrentLifeTime = m_LifeTime;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_CurrentLifeTime <=0)
        {
            m_Pool.Release(this);
        }
        m_CurrentLifeTime -= Time.deltaTime;
    }

    public void SetPool(ParticleFXsPool pool)
    {
        m_Pool = pool;
    }
}
