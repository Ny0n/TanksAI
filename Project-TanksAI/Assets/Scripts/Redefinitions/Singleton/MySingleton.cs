using UnityEngine;

public abstract class MySingleton<T> : MonoBehaviour where T : MySingleton<T>
{
    public static T Instance { get; private set; }

    protected virtual bool DoDestroyOnLoad { get; set; } = true;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
            if (!DoDestroyOnLoad)
                DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
