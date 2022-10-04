using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private Material m_BigStartsBg;
    [SerializeField] private Material m_MedStartsBg;
    [SerializeField] private float m_BigStartsBgScrollSpeed;
    [SerializeField] private float m_MedStartsBgScrollSpeed;

    private int m_MainTexId;
    // Start is called before the first frame update
    void Start()
    {
        m_MainTexId = Shader.PropertyToID("_MainTex");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = m_BigStartsBg.GetTextureOffset(m_MainTexId);
        offset += new Vector2(0, m_BigStartsBgScrollSpeed * Time.deltaTime);
        m_BigStartsBg.SetTextureOffset(m_MainTexId, offset);

         offset = m_MedStartsBg.GetTextureOffset(m_MainTexId);
        offset += new Vector2(0, m_MedStartsBgScrollSpeed * Time.deltaTime);
        m_MedStartsBg.SetTextureOffset(m_MainTexId, offset);
    }
}
