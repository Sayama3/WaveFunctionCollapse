using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Procedural
{
    [ExecuteInEditMode]
    public class SeeIndice : MonoBehaviour
    {
        private List<GameObject> cubesWithIndice = new List<GameObject>();
        private List<List<Side>> associateSide = new List<List<Side>>();
        [SerializeField] private GameObject indice = null;

        //[Button(ButtonSizes.Large)]
        private void Update()
        {
            cubesWithIndice.Clear();
            associateSide.Clear();
            var allCube = GameObject.FindGameObjectsWithTag("Cube");
            foreach (var item in allCube)
            {
                CanBeNextTo sided = item.GetComponent<CanBeNextTo>();
                for (int i = 0; i < sided.adjoiningCubes.Count; i++)
                {
                    if (sided.adjoiningCubes[(Side)i] != null)
                    {
                        for (int j = 0; j < sided.adjoiningCubes[(Side)i].Count; j++)
                        {
                            if (sided.adjoiningCubes[(Side)i][j] == indice)
                            {
                                cubesWithIndice.Add(item);
                                associateSide.Add(new List<Side>());
                                associateSide[associateSide.Count - 1].Add((Side)i);
                            }
                        }
                    }
                }
                
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < cubesWithIndice.Count; i++)
            {

                Vector3 center = cubesWithIndice[i].transform.position;
                for (int j = 0; j < associateSide[i].Count; j++)
                {
                    Vector3 newCenter;

                    switch (associateSide[i][j])
                    {
                        case Side.Front:
                            newCenter = (center + Vector3.up + (Vector3.forward / 2) - Vector3.forward * 0.1f);
                            Gizmos.DrawLine(newCenter - (Vector3.right * 0.4f), newCenter + (Vector3.right * 0.4f));
                            break;
                        case Side.Back:
                            newCenter = (center + Vector3.up + (-Vector3.forward / 2) + Vector3.forward * 0.1f);
                            Gizmos.DrawLine(newCenter - (Vector3.right * 0.4f), newCenter + (Vector3.right * 0.4f));
                            break;
                        case Side.Left:
                            newCenter = (center + Vector3.up - (Vector3.right / 2) + Vector3.right * 0.1f);
                            Gizmos.DrawLine(newCenter - (Vector3.forward * 0.4f), newCenter + (Vector3.forward * 0.4f));
                            break;
                        case Side.Right:
                            newCenter = (center + Vector3.up + (Vector3.right / 2) - Vector3.right * 0.1f);
                            Gizmos.DrawLine(newCenter - (Vector3.forward * 0.4f), newCenter + (Vector3.forward * 0.4f));
                            break;
                    }
                }
            }
        }
    }
}


