using System;
using System.ComponentModel;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace FarlandsCoreMod.ui;

public class ModsConfigUI : MonoBehaviour
{
    public void Start()
    {
        generateBackground();
    }

    private void generateBackground()
    {
        var image = gameObject.AddComponent<Image>();
    }
}
