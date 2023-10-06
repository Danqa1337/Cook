using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMusicPlayer : MonoBehaviour
{
    private void Start()
    {
        AudioManager.StopAllImmediate();
    }
}