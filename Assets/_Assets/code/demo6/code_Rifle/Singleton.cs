using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<R> : MonoBehaviour where R : MonoBehaviour
{
    private static R _instance;
    public static R instance
    {
        get
        {
            if (_instance == null)
            {
                R ínstanceInScene =FindAnyObjectByType<R>();
                Registerinstance(ínstanceInScene);
            } 
              return _instance;
        }

      
    }

    private void Awake()
    {
        if(_instance == null)
        {
            Registerinstance((R)(MonoBehaviour)this);
        }
        else if(_instance!=this) 
        {
            Destroy(this);
        }
    }
    private static void Registerinstance(R newInstance)
    {
        //if (newInstance == null) { return; }
        if (newInstance == null)return;
        _instance=newInstance;
        DontDestroyOnLoad(_instance.transform.root);
    }    


}
