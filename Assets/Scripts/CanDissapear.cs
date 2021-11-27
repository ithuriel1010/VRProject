using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDissapear : MonoBehaviour
{
    public TextMesh Name;

    public void ThrowAway()
    {
        Destroy(gameObject);
    }

    public void ShowText()
    {
        Name.text = "Press Grab to throw away";
        ///Name.SendMessage("Hello Transform");
    }

    public void DisappearText()
    {
        Name.text = "";
        ///Name.SendMessage("Hello Transform");
    }
    //public void ShowText()
    //{
    //    TextMesh textMesh = gameObject.GetComponent(typeof(TextMesh)) as TextMesh;

    //    textMesh.text = ;
    //}
}
