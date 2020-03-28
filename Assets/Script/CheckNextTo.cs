using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace Procedural
{
    public class CheckNextTo : MonoBehaviour
    {
        [Button(ButtonSizes.Gigantic)]
        public void Check()
        {
            CanBeNextTo nextTo = GetComponent<CanBeNextTo>();
            List<GameObject> problematicsCubes = new List<GameObject>();
            List<Side> problematicsSide = new List<Side>();
            for (int i = 0; i < nextTo.adjoiningCubes.Count; i++)
            {
                Side side = (Side)i;
                Side inverseSide = SideHelp.GetInverseSide((side));
                for (int j = 0; j < nextTo.adjoiningCubes[side].Count; j++)
                {
                    GameObject cubeTest = nextTo.adjoiningCubes[side][j];
                    CanBeNextTo nextToTest = cubeTest.GetComponent<CanBeNextTo>();
                    bool found = false;
                    for (int k = 0; k < nextToTest.adjoiningCubes[inverseSide].Count; k++)
                    {
                        GameObject cubeTestSecond = nextToTest.adjoiningCubes[inverseSide][k];
                        if (cubeTestSecond.name.Contains(gameObject.name) || gameObject.name.Contains(cubeTestSecond.name))
                        {
                            found = true;
                            break;
                        }

                    }
                    if (!found)
                    {
                        problematicsCubes.Add(cubeTest);
                        problematicsSide.Add(inverseSide);
                    }

                }
                
            }
            if (problematicsCubes.Count >0)
            {
                Debug.LogError("Il y a un problème avec " + name + ", " + problematicsCubes.Count + " cubes ne se comprènent pas, les voici.");
                for (int i = 0; i < problematicsCubes.Count; i++)
                {
                    Debug.LogError(problematicsCubes[i].name + " n'est pas associé " + SideHelp.ShowRealName(problematicsSide[i]));
                }
            }
            else
            {
                Debug.Log(name + " est tout bon");
            }
        }
    }
}

