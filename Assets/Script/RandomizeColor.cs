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
        private void Start()
        {
            ChangeAleaColor();
        }

        [Button(ButtonSizes.Gigantic)]
        void ChangeAleaColor()
        {
            GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
            //Debug.Log("Done");
        }
    }
}

