using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class PlayerController : MonoBehaviour
    {
         public Action<int, int> OnHPChanged;
        
        [SerializeField] private float m_MoveSpeed;
        //[SerializeField] private ProjectileController m_Projectile;
        [SerializeField] private Transform m_FiringPoint;
        [SerializeField] private float m_FiringCooldown;
        [SerializeField] private int m_Hp;

        private int m_CurrentHp;
        private float m_TempCooldown;
    //private SpawnManager m_SpawnManager;
    //private GameManager m_GameManager;
    //private AudioManager m_AudioManager;
    
    // Start is called before the first frame update
    void Start()
        {
        m_CurrentHp = m_Hp;
        if (OnHPChanged != null)
            {
                OnHPChanged(m_CurrentHp,m_Hp);
            }
        
        //m_AudioManager = FindObjectOfType<AudioManager>();        
        //m_SpawnManager = FindObjectOfType<SpawnManager>();
        //m_GameManager = FindObjectOfType<GameManager>();
    }

        // Update is called once per frame
        void Update()
        {
            if(!GameManager.Instance.IsActive())
            {
                return; 
            }
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 direction = new Vector2(horizontal, vertical);
            transform.Translate(direction * Time.deltaTime * m_MoveSpeed);

            if (Input.GetKey(KeyCode.Space))
            {
                if (m_TempCooldown <= 0)
                {
                    Fire();
                    m_TempCooldown = m_FiringCooldown;
                }
            }

            m_TempCooldown -= Time.deltaTime;
        }

        private void Fire()
        {
            //ProjectileController projectile = Instantiate(m_Projectile, m_FiringPoint.position, Quaternion.identity, null);
            ProjectileController projectile = SpawnManager.Instance.SpawnPlayerProjectile(m_FiringPoint.position);
            projectile.Fire(1);

            SpawnManager.Instance.SpawmShootingFX(m_FiringPoint.position);
            AudioManager.Instance.PlayLazerSFX();
        }

        public void Hit(int damage)
        {
            m_CurrentHp -= damage;  
            if (OnHPChanged != null)
            {
                OnHPChanged(m_CurrentHp, m_Hp);
            }
            if (m_CurrentHp <= 0)
            {
                Destroy(gameObject);

                SpawnManager.Instance.SpawnExplosionFX(transform.position);
                GameManager.Instance.Gameover(false);
                AudioManager.Instance.PlayExplosionSFX();
            }
         AudioManager.Instance.PlayHitSFX();
        }
    }
