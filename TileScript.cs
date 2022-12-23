using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public int rowID;
    public int columnID;

    public GoToScript gt;

    
    //cachiramo GoTo skriptu
    void Start()
    {
        gt.GetComponent<GoToScript>();   
    }

    //funkcija se poziva kad se klikne mis
    private void OnMouseDown()
    {
        gt.MoveTo(columnID, rowID);
    }
}
