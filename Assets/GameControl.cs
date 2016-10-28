using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
    public static class GameControl
    {
        public static GameObject GameOverPanel;
    public static void Reset()
    {
        SceneManager.LoadScene(0);
    }

    }

