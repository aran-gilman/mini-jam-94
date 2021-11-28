using UnityEngine;

public class Singleton : MonoBehaviour
{
    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag(tag).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
