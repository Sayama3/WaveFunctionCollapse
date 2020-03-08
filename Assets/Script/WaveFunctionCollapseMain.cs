using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Procedural
{
    public class WaveFunctionCollapseMain : MonoBehaviour
    {
        [SerializeField] private int numberOfDifferentCubes = 10;
        [SerializeField] private int squareLenght = 8;
        private int seed = 0;

        [SerializeField] private List<GameObject> cubesUsed = new List<GameObject>();
        [SerializeField] private List<GameObject> allTheCubes = new List<GameObject>();

        private GameObject[,] mapCubes;

        [Button(ButtonSizes.Large)]
        public void MakeMap()
        {
            UseRandomCubes();
            if (cubesUsed.Count != numberOfDifferentCubes)
            {
                Debug.LogError("Il y a " + ((cubesUsed.Count < 10) ? "pas assez" : "trop") + " de cubes dans la liste (" + cubesUsed.Count + ").");
                return;
            }

            int indexMax = squareLenght - 1;

            mapCubes = new GameObject[squareLenght, squareLenght];
            List<Vector2> positionInitialCube = new List<Vector2>(1);
            List<Vector2> positionOfPossibilities = new List<Vector2>(squareLenght* squareLenght);

            
            for (int i = 0; i < squareLenght; i++)
            {
                for (int j = 0; j < squareLenght; j++)
                {
                    positionOfPossibilities[((i * squareLenght) - 1) + j] = new Vector2(i, j);
                }
            }
            var index = RandomizeIndex(positionOfPossibilities.Count);
            positionInitialCube[0] = positionOfPossibilities[index];

            CollapseMap(positionInitialCube[0],new Vector2[0]);


            bool CollapseMap(Vector2 cubeIndex, Vector2[] previousPositions)
            {
                int x = (int)cubeIndex.x;
                int y = (int)cubeIndex.y;
                bool[] sideCheck = new bool[4];

                for (Side side = 0; (int)side < 4; side++)
                {
                    switch (side)
                    {
                        case Side.Front:
                            sideCheck[(int)side] = (y + 1 <= indexMax);
                            break;
                        case Side.Back:
                            sideCheck[(int)side] = (y - 1 >= 0);
                            break;
                        case Side.Left:
                            sideCheck[(int)side]= (x - 1 >= 0);
                            break;
                        case Side.Right:
                            sideCheck[(int)side] = (x + 1 <= indexMax);
                            break;
                        default:
                            break;
                    }

                }



                return true;
            }




        }

        private int CheckPossibilities(Vector2 indexToSearch, GameObject previousCube, Side sideOfPreviousCube)
        {
            int numberOfPossibilites = 0;
            int previousIndiceRef = previousCube.GetComponent<CanBeNextTo>().adjoiningCubes[sideOfPreviousCube];

            GameObject actualCube = mapCubes[(int)indexToSearch.x, (int)indexToSearch.y];
            WaveFunctionCollapsePossibilites actualPossibilites = actualCube.GetComponent<WaveFunctionCollapsePossibilites>();

            if (actualPossibilites.possibleCube.Count > 1)
            {
                for (int i = 0; i < actualPossibilites.possibleCube.Count; i++)
                {
                    int possibilitieIndiceRef = actualPossibilites.possibleCube[i].GetComponent<CanBeNextTo>().adjoiningCubes[SideHelp.GetInverseSide(sideOfPreviousCube)];

                    numberOfPossibilites += (possibilitieIndiceRef == previousIndiceRef) ? 1 : 0;

                }
            }
            numberOfPossibilites -= (numberOfPossibilites == actualPossibilites.possibleCube.Count) ? 1 : 0;
            
            return numberOfPossibilites;
        }

        [Button(ButtonSizes.Large)]
        public void UseRandomCubes()
        {
            this.seed = SeedHelp.CreateSeed();

            List<GameObject> allTheCubeLeft = new List<GameObject>(allTheCubes);
            cubesUsed.Clear();
            
            for (int i = 0; i < numberOfDifferentCubes; i++)
            {
                var index = RandomizeIndex(allTheCubeLeft.Count);
                cubesUsed.Add(allTheCubeLeft[index]);
                allTheCubeLeft.RemoveAt(index);
            }


        }

        private int RandomizeIndex(int lenghtList)
        {
            int index = seed%lenghtList;
            seed = SeedHelp.EnhanceSeed(seed, index);
            return index;
        }



    }

    public static class SeedHelp
    {
        public static int CreateSeed(int min = 100000, int max = 999999)
        {
            int seed;
            Random.InitState((int)System.DateTime.Now.Ticks);

            seed = Random.Range(min, max);

            return seed;
        }

        public static int EnhanceSeed(int seed, int enhancer)
        {
            int enhanceSeed = seed;

            enhanceSeed += (enhancer + 1) * ((seed % 10) + 1);

            return enhanceSeed;
        }

    }
}

