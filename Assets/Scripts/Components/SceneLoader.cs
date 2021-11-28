using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string nextScene;

    private void Update()
    {
        if (Input.GetButtonDown("Select"))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
