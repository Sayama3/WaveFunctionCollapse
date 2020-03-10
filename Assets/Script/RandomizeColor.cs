using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Procedural
{
    [ExecuteInEditMode]
    public class RandomizeColor : MonoBehaviour
    {
        // Start is called before the first frame update
        //private void Start()
        //{
        //    ChangeAleaColor();
        //}

        [Button(ButtonSizes.Gigantic)]
        void ChangeAleaColor()
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            GetComponent<MeshRenderer>().sharedMaterial.color = Random.ColorHSV();
            //Debug.Log("Done");
        }
    }
}

