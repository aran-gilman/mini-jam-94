using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string nextScene;

    private SoundEffectsPlayer soundEffectsPlayer;

    private void Start()
    {
        soundEffectsPlayer = GameObject.FindGameObjectWithTag("SoundEffectsPlayer").GetComponent<SoundEffectsPlayer>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Select"))
        {
            soundEffectsPlayer.PlaySuccess();
            SceneManager.LoadScene(nextScene);
        }
    }
}
