using System.Collections;
using System.Collections.Generic;
using Procedural;
using UnityEngine;

public class SetCameraPosition : MonoBehaviour
{
    public void SetPositionStart(int lenghtList = 8)
    {
        if (Camera.main != null)
            Camera.main.transform.position =
                new Vector3((float) (lenghtList-1) / 2, lenghtList + 0.5f, (float) (lenghtList-1) / 2);

        GetComponent<AnotherWaveFunctionCollapse>().squareLenght = lenghtList;
    }

    

    public void SetPositionChangeRule()
    {
        if (Camera.main != null) Camera.main.transform.position = new Vector3(51.5f, 4.5f, 1.5f);
    }
    
}
