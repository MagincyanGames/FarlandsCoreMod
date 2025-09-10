using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FarlandsCoreMod.Managers;

public class Manager : MonoBehaviour
{
    public void Awake()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    public virtual void OnSceneChange(Scene oldScene, Scene newScene) { }
}
