using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Procedural
{
    [ExecuteInEditMode]
    public class DernierRecours : MonoBehaviour
    {
        [Button(ButtonSizes.Gigantic)]
        public void DestroyEveryCubes()
        {
            GameObject[] allTheCubesInScene = GameObject.FindGameObjectsWithTag("Cube");
            foreach (var item in allTheCubesInScene)
            {
                DestroyImmediate(item);
            }
        }
    }

}

