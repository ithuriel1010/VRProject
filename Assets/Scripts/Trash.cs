using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public TextMesh Name;

    public void ThrowAway()
    {
        gameObject.SetActive(false);

        //Send notification to game manager that trash is picked up
        GameManager.TrashDisposed();
    }

    public void ShowText()
    {
        Name.text = "Press Grab to throw away";
    }

    public void DisappearText()
    {
        Name.text = "";
    }
}
