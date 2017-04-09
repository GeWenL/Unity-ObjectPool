using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class RecyclableObj<T> where T : RecyclableObj<T>, new()
{
    static UnityEngine.UI.ObjectPool<T> s_pool;         // pool

    private static Transform s_transRecycleParent;      // default recycle pos
    private static GameObject s_objPrefab;              // prefab

    // create defautl recycle pos
    static RecyclableObj()
    {
        GameObject o = GameObject.Find("recycle_pool");
        if (o == null)
        {
            o = new GameObject("recycle_pool");
        }

        GameObject.DontDestroyOnLoad(o);
        s_transRecycleParent = o.transform;
    }

    // gameobject
    public GameObject Obj;

    // get T from pool
    public static T GenObj(GameObject objPrefab = null) 
    {
        // 设置prefab
        s_objPrefab = objPrefab;

        if (s_transRecycleParent == null || s_objPrefab == null)
        {
            Develop.LogErrorF("not init pool : {0}", typeof(T));
            return default(T);
        }

        if (s_pool == null)
        {
            s_pool = new UnityEngine.UI.ObjectPool<T>(
                (t) => { t.__Init(); },
                (t) => { t.__Release(); }
            );
        }

        return (T)s_pool.Get();
    }

    bool m_isReleased = false;
    // recycle selef
    public void __Recycle() 
    {
        if (m_isReleased)
        {
            return;
        }

        s_pool.Release((T)this);
        m_isReleased = true;
    }

    // default creator
    public void __Init() {
        if (Obj == null)
        {
            Obj = GameObject.Instantiate(s_objPrefab);
        }

        Obj.SetActive(true);
        m_isReleased = false;
    }

    public void __Release() 
    {
        // destroy
        if (Obj == null)
        {
            return;
        }

        Obj.transform.SetParent(s_transRecycleParent, false);
        Obj.SetActive(false);     
    }
    
}
