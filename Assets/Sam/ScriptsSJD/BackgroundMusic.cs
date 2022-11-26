using UnityEngine;

namespace ScriptsSJD
{
    public class BackgroundMusic : MonoBehaviour
    {
        public static BackgroundMusic _backgroundMusic;
        public static BackgroundMusic instance;

       void Awake()
        {
            if (_backgroundMusic != null)
            {
                _backgroundMusic = this;
                DontDestroyOnLoad(_backgroundMusic);
            }
        }
    }
}
