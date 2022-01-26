using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leak : MonoBehaviour
{
    public TextMesh Name;

    public void TurnOff()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var emission = ps.emission;
        emission.enabled = false;
        GameManager.PipeDisposed();
    }

    public void ShowText()
    {
        Name.text = "Press to stop leakage";
    }

    public void DisappearText()
    {
        Name.text = "";
    }
}
