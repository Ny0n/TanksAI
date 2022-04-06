using UnityEngine;

public abstract class MyLazySingleton<T> : MonoBehaviour where T : MyLazySingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject($"{typeof(T).Name} (created)");
                _instance = go.AddComponent<T>(); // This will call Awake()
            }

            return _instance;
        }
    }

    public abstract bool DoDestroyOnLoad { get; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
            if (!DoDestroyOnLoad)
                DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject); // Safeguard, if we do manually add an object in the scene
        }
    }
}
