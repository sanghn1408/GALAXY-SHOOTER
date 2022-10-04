using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    //private GameManager m_GaneManager;
    // Start is called before the first frame update
    void Start()
    {
        //m_GaneManager = FindObjectOfType<GameManager>();
    }

    public void BtnHome_Pressed()
    {
       GameManager.Instance . Home();
    }
    public void BtnContinue_Pressed()
    {
       GameManager.Instance . Continue();
    }
}
