using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPiece : MonoBehaviour
{
    public bool isColor = false;
    public void ChangeColor (Color color) {
        GetComponent<MeshRenderer>().material.color=color;
        isColor=true;
    }
}
