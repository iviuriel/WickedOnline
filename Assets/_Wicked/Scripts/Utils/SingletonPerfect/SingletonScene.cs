using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonScene<T> : MonoBehaviour where T: SingletonScene<T>
{
    public virtual void Awake()
    {
        if(mInstance != null)
        {
            CheckDuplicates();
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void OnDomainReload()
    {
        _mInstance = null;
        GameObject go = GameObject.Find("PERSISTENT_SINGLETON");
        if(go != null)
        {
            Destroy(go);
        }
    }

    public virtual void OnDestroy()
    {
        mInstance = null;
    }

    static void CheckDuplicates()
    {
        T[] managers = GameObject.FindObjectsOfType(typeof(T)) as T[];

        if(managers.Length >1 && mInstance != null)
        {
            foreach(T manager in managers)
            {
                if(manager != _mInstance)
                {
                    Debug.Log("Destroy manager :" + manager.gameObject.name);
                    Destroy(manager.gameObject);
                }
                else
                {
                    _mInstance = manager;
                }
            }
        }
    }

    protected static SingletonScene<T> mInstance
    {
        get
        {
            if (!_mInstance)
            {
                T[] managers = GameObject.FindObjectsOfType(typeof(T)) as T[];

                if(managers.Length > 0)
                {
                    _mInstance = managers[0];
                }
            }
            return _mInstance;
        }
        set
        {
            _mInstance = value as T;
        }
    }

    private static T _mInstance;
}