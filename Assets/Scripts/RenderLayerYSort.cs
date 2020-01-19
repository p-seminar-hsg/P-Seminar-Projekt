using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Ersteller: Benedikt Wille <br/>
/// Zuletzt geändert am: 19.01.2020 <br/>
/// </summary>
public class RenderLayerYSort : MonoBehaviour
{
    private void Start()
    {

        GraphicsSettings.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);
        Camera.main.transparencySortMode = TransparencySortMode.CustomAxis;
        Camera.main.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);
    }
}
