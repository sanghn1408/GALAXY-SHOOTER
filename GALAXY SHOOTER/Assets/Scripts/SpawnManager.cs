using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPool
{
    public EnemyController prefab;
    public List<EnemyController> inactiveObjs;
    public List<EnemyController> activeObjs;
    public EnemyController Spawn(Vector3 pos, Transform parent)
    {
        if (inactiveObjs.Count == 0)
        {
            EnemyController obj = GameObject.Instantiate(prefab, parent);
            obj.transform.position = pos;
            activeObjs.Add(obj);
            return obj;
        }
        else
        {
            EnemyController obj = inactiveObjs[0];
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(parent);
            obj.transform.position = pos;
            activeObjs.Add(obj);
            inactiveObjs.RemoveAt(0);
            return obj;
        }

    }
    public void Release(EnemyController obj)
    {
        if (activeObjs.Contains(obj))
        {
            activeObjs.Remove(obj);
            inactiveObjs.Add(obj);
            obj.gameObject.SetActive(false);
        }
    }
    public void Clear()
    {
        while (activeObjs.Count > 0)
        {
            EnemyController obj = activeObjs[0];
            obj.gameObject.SetActive(false);
            activeObjs.RemoveAt(0);
            inactiveObjs.Add(obj);
        }
    }
}


[System.Serializable]
public class ProjectilesPool
{
    public ProjectileController prefab;
    public List<ProjectileController> inactiveObjs;
    public List<ProjectileController> activeObjs;
    public ProjectileController Spawn(Vector3 position, Transform parent)
    {
        if (inactiveObjs.Count == 0)
        {
            ProjectileController newObj = GameObject.Instantiate(prefab, parent);
            newObj.transform.position = position;
            activeObjs.Add(newObj);
            return newObj;
        }
        else
        {
            ProjectileController oldObj = inactiveObjs[0];
            oldObj.gameObject.SetActive(true);
            oldObj.transform.SetParent(parent);
            oldObj.transform.position = position;
            activeObjs.Add(oldObj);
            inactiveObjs.RemoveAt(0);
            return oldObj;
        }

    }
    public void Release(ProjectileController obj)
    {
        if (activeObjs.Contains(obj))
        {
            activeObjs.Remove(obj);
            inactiveObjs.Add(obj);
            obj.gameObject.SetActive(false);
        }
    }
    public void Clear()
    {
        while(activeObjs.Count>0)
        {
            ProjectileController obj = activeObjs[0];
            obj.gameObject.SetActive(false);
            activeObjs.RemoveAt(0);
            inactiveObjs.Add(obj);
        }
    }
}



[System.Serializable]
public class ParticleFXsPool
{
    public ParticleFX prefab;
    public List<ParticleFX> inactiveObjs;
    public List<ParticleFX> activeObjs;
    public ParticleFX Spawn(Vector3 position, Transform parent)
    {
        if (inactiveObjs.Count == 0)
        {
            ParticleFX newObj = GameObject.Instantiate(prefab, parent);
            newObj.transform.position = position;
            activeObjs.Add(newObj);
            return newObj;
        }
        else
        {
            ParticleFX oldObj = inactiveObjs[0];
            oldObj.gameObject.SetActive(true);
            oldObj.transform.SetParent(parent);
            oldObj.transform.position = position;
            activeObjs.Add(oldObj);
            inactiveObjs.RemoveAt(0);
            return oldObj;
        }

    }
    public void Release(ParticleFX obj)
    {
        if (activeObjs.Contains(obj))
        {
            activeObjs.Remove(obj);
            inactiveObjs.Add(obj);
            obj.gameObject.SetActive(false);
        }
    }
    public void Clear()
    {
        while (activeObjs.Count > 0)
        {
            ParticleFX obj = activeObjs[0];
            obj.gameObject.SetActive(false);
            activeObjs.RemoveAt(0);
            inactiveObjs.Add(obj);
        }
    }
}

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager m_Instance;
    public static SpawnManager Instance
    {
        get
        {
            if(m_Instance == null)
            {
                m_Instance = FindObjectOfType<SpawnManager>();
            }
            return m_Instance;
        }

    }
    [SerializeField] private bool m_Active;
    //[SerializeField] private EnemyController m_EnemyPrefabs;
    [SerializeField] private EnemyPool m_EnemiesPool;
    [SerializeField] private ProjectilesPool m_PlayerProjectilesPool;
    [SerializeField] private ProjectilesPool m_EnemyProjectilesPool;
    [SerializeField] private int m_MinTotalEnemies;
    [SerializeField] private int m_MaxTotalEnemies;
    [SerializeField] private float m_EnemySpawnInterval;
    [SerializeField] private EnemyPath[] m_Path;
    [SerializeField] private int m_TotalGroupds;
    [SerializeField] private ParticleFXsPool m_HitFXsPool;
    [SerializeField] private ParticleFXsPool m_ShootingFXsPool;
    [SerializeField] private ParticleFXsPool m_ExplosionFXsPool;
    [SerializeField] private PlayerController m_playerControllerPrefab;

    public PlayerController Player => m_Player;

    private bool m_IsSpawningEnemies;
    private PlayerController m_Player;
    private WaveData m_CurWave;

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }           
        else if (m_Instance != this)
        {
            Destroy(gameObject);
        }            
    }

     public void StartBattle(WaveData wave, bool resetPosition)
    {
        m_CurWave = wave;
        m_MinTotalEnemies = m_CurWave.minTotalEnemies;
        m_MaxTotalEnemies = m_CurWave.maxTotalEnemies;
        m_TotalGroupds = m_CurWave.totalGroups;
        if(m_Player == null)
        {
            m_Player = Instantiate(m_playerControllerPrefab);
        }
        if (resetPosition)
        {
            m_Player.transform.position = Vector3.zero;
        }
        StartCoroutine(IESpawnGroups(m_TotalGroupds));

    }
    private IEnumerator IESpawnGroups(int pGroups )
    {
        m_IsSpawningEnemies = true;
        for (int i = 0;i<pGroups; i++)
        {
            int TotalEnemies = Random.Range(m_MinTotalEnemies, m_MaxTotalEnemies);

            EnemyPath path = m_Path[Random.Range(0, m_Path.Length)];
            yield return StartCoroutine(IESpawnEnemies( TotalEnemies, path));
            yield return new WaitForSeconds(3f / m_CurWave.speedMutiplier);
            if (i < pGroups - 1)
            {
                yield return new WaitForSeconds(3f);
            }               
        }
        m_IsSpawningEnemies = false;
    }
    private IEnumerator IESpawnEnemies(int TotalEnemies,EnemyPath path)
    {
        
        for(int i =0;i<TotalEnemies;i++)
        {
            yield return new WaitUntil(() => m_Active);
            yield return new WaitForSeconds(m_EnemySpawnInterval/m_CurWave.speedMutiplier);
            //EnemyController enemy = Instantiate(m_EnemyPrefabs, transform);
            EnemyController enemy = m_EnemiesPool.Spawn(path.WayPoints[0].position, transform);
            enemy.Init(path.WayPoints,m_CurWave.speedMutiplier);
        }
    }

    public void ReleaseEnemy(EnemyController obj)
    {
        m_EnemiesPool.Release(obj);
    }
    public void ReleaseEnemyController(EnemyController enemy)
    {
        m_EnemiesPool.Release(enemy);
    }
    public ProjectileController SpawnEnemyProjectile(Vector3 position)
    {
        ProjectileController obj = m_EnemyProjectilesPool.Spawn(position, transform);
        obj.SetFromPlayer(false);
        return obj;
    }
    public void ReleaseEnemyProjectile(ProjectileController projectile)
    {
        m_EnemyProjectilesPool.Release(projectile);
    }
    public ProjectileController SpawnPlayerProjectile(Vector3 position)
    {
        ProjectileController obj = m_PlayerProjectilesPool.Spawn(position, transform);
        obj.SetFromPlayer(true);
        return obj;
    }
    public void ReleasePlayerProjectile(ProjectileController projectile)
    {
        m_PlayerProjectilesPool.Release(projectile);
    }
    public  ParticleFX SpawmHitFX(Vector3 postion)
    {
        ParticleFX fx= m_HitFXsPool.Spawn(postion, transform);
        fx.SetPool(m_HitFXsPool);
        return fx;
    }
    public void ReleaseHitFX(ParticleFX obj)
    {
        m_HitFXsPool.Release(obj);
    }
    public ParticleFX SpawmShootingFX(Vector3 postion)
    {
        ParticleFX fx = m_ShootingFXsPool.Spawn(postion, transform);
        fx.SetPool(m_ShootingFXsPool);
        return fx;
    }
    public void ReleaseShootingtFX(ParticleFX obj)
    {
        m_ShootingFXsPool.Release(obj);
    }
    public void ReleaseShootingFX(ParticleFX obj)
    {
        m_ShootingFXsPool.Release(obj);
    }

    public ParticleFX SpawnExplosionFX(Vector3 position)
    {
        ParticleFX fx = m_ExplosionFXsPool.Spawn(position, transform);
        fx.SetPool(m_ExplosionFXsPool);
        return fx;
    }
    public bool Isclear()
    {
        if (m_IsSpawningEnemies || m_EnemiesPool.activeObjs.Count > 0)
            return false;
        return true;
    }
    public void Clear()
    {
        m_EnemiesPool.Clear();
        m_EnemyProjectilesPool.Clear();
        m_ExplosionFXsPool.Clear();
        m_HitFXsPool.Clear();
        m_PlayerProjectilesPool.Clear();
        m_ShootingFXsPool.Clear();
        StopAllCoroutines();
    }
}
