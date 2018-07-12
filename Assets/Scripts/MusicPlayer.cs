using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer musicPlayer;

    void Awake()
    {
        if (musicPlayer != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            musicPlayer = this;
        }
    }
}
