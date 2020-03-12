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

        private List<GameObject>[,] mapCubes;

        int indexMax;
        List<Vector2Int> positionInitialCube = new List<Vector2Int>();
        List<GameObject> choosenInitialCube = new List<GameObject>();
        List<Vector2Int> positionOfPossibilities = new List<Vector2Int>();
        [Button(ButtonSizes.Large)]
        public void MakeMap()
        {
            if (cubesUsed.Count != numberOfDifferentCubes)
            {
                Debug.LogError("Il y a " + ((cubesUsed.Count < 10) ? "pas assez" : "trop") + " de cubes dans la liste (" + cubesUsed.Count + ").");
                return;
            }

            indexMax = squareLenght - 1;

            mapCubes = new List<GameObject>[squareLenght, squareLenght];
            mapCubes.Initialize();
            for (int i = 0; i < squareLenght; i++)
            {
                for (int j = 0; j < squareLenght; j++)
                {
                    mapCubes[i, j] = cubesUsed;
                }
            }


            positionInitialCube = new List<Vector2Int>();
            choosenInitialCube = new List<GameObject>();
            positionOfPossibilities = new List<Vector2Int>();
            

            for (int i = 0; i < squareLenght; i++)
            {
                for (int j = 0; j < squareLenght; j++)
                {
                    positionOfPossibilities.Add(new Vector2Int(i, j));
                }
            }
            var index = RandomizeIndex(positionOfPossibilities.Count);
            positionInitialCube.Add(positionOfPossibilities[index]);

            index = RandomizeIndex(cubesUsed.Count);
            choosenInitialCube.Add(cubesUsed[index]);

            for (int i = 0; i < squareLenght; i++)
            {
                for (int j = 0; j < squareLenght; j++)
                {
                    mapCubes[i, j] = new List<GameObject>(cubesUsed);
                }
            }


        Research:
            //Debug.Log(mapCubes[positionInitialCube[positionInitialCube.Count - 1].x, positionInitialCube[positionInitialCube.Count - 1].y][0].name);
            //foreach (var item in (mapCubes[positionInitialCube[positionInitialCube.Count - 1].x, positionInitialCube[positionInitialCube.Count - 1].y]))
            for (int i = 0; i < mapCubes[positionInitialCube[positionInitialCube.Count - 1].x, positionInitialCube[positionInitialCube.Count - 1].y].Count; i++)
            {
                var item = mapCubes[positionInitialCube[positionInitialCube.Count - 1].x, positionInitialCube[positionInitialCube.Count - 1].y][i];
                if (item != choosenInitialCube[choosenInitialCube.Count - 1])
                {
                    CollapseMap(positionInitialCube[positionInitialCube.Count - 1], new List<Vector2>(0), item);
                }
            }
            int possibilitesLeft = 0;
            List<Vector2Int> possibilityLeftReal = new List<Vector2Int>();
            for (int i = 0; i < positionOfPossibilities.Count; i++)
            {
                if (positionOfPossibilities[i] != Vector2.down)
                {
                    possibilitesLeft++;
                    possibilityLeftReal.Add(positionOfPossibilities[i]);
                }
            }
            if (possibilitesLeft > 0)
            {
                index = RandomizeIndex(possibilityLeftReal.Count);
                positionInitialCube.Add(possibilityLeftReal[index]);
                Debug.Log("Possibility left : " + possibilitesLeft);
                Debug.Log("index : " + positionInitialCube[positionInitialCube.Count - 1].x+ " , " + positionInitialCube[positionInitialCube.Count - 1].y);
                index = RandomizeIndex(mapCubes[positionInitialCube[positionInitialCube.Count - 1].x, positionInitialCube[positionInitialCube.Count - 1].y].Count);
                choosenInitialCube.Add(mapCubes[positionInitialCube[positionInitialCube.Count - 1].x, positionInitialCube[positionInitialCube.Count - 1].y][index]);
                goto Research;
            }

            Positionneur positionneur = GetComponent<Positionneur>();
            positionneur.SetListeCube(mapCubes, squareLenght);
            positionneur.GenerateList();
            positionneur.PositionnementCubes(squareLenght);


        }

        void CollapseMap(Vector2 cubeIndex, List<Vector2> previousPositions, GameObject objectToDestroy)
        {
            int x = (int)cubeIndex.x;
            int y = (int)cubeIndex.y;
            if (objectToDestroy == null || mapCubes[x,y].Count <= 1) return;


            List<Vector2> allPositions = new List<Vector2>(previousPositions)
            {
                cubeIndex
            };

            bool[] sideCheck = new bool[4];
            int[] sideNumberPossibilities = new int[4];
            List<GameObject>[] sidePossibilites = new List<GameObject>[4];
            Vector2[] nextPositions = new Vector2[4];



            for (int i = 0; i < sideCheck.Length; i++)
            {
                switch ((Side)i)
                {
                    case Side.Front:
                        sideCheck[i] = (y + 1 <= indexMax);
                        nextPositions[i] = new Vector2(x, y + 1);
                        break;
                    case Side.Back:
                        sideCheck[i] = (y - 1 >= 0);
                        nextPositions[i] = new Vector2(x, y - 1);
                        break;
                    case Side.Left:
                        sideCheck[i] = (x - 1 >= 0);
                        nextPositions[i] = new Vector2(x - 1, y);
                        break;
                    case Side.Right:
                        sideCheck[i] = (x + 1 <= indexMax);
                        nextPositions[i] = new Vector2(x + 1, y);
                        break;
                    default:
                        break;
                }
                foreach (var item in previousPositions)
                {
                    if (item == nextPositions[i])
                    {
                        sideCheck[i] = false;
                        break;
                    }
                }
                sidePossibilites[i] = (sideCheck[i]) ? CheckPossibilities(nextPositions[i], objectToDestroy, (Side)i) : new List<GameObject>(0);
                sideNumberPossibilities[i] = sidePossibilites[i].Count;

            }

            Side nextSide;

            do
            {
                nextSide = (Side)RandomizeIndex(sideCheck.Length);
            } while (sidePossibilites[(int)nextSide].Count > 0);


            for (int i = 0; i < sideCheck.Length; i++)
            {
                for (int j = 0; j < sideNumberPossibilities[(int)nextSide]; j++)
                {
                    var index = RandomizeIndex(sidePossibilites[(int)nextSide].Count);
                    CollapseMap(nextPositions[(int)nextSide], allPositions, sidePossibilites[(int)nextSide][index]);
                    sidePossibilites[(int)nextSide].RemoveAt(index);
                }
                nextSide = ((int)nextSide + 1 < 4) ? (nextSide + 1) : 0;
            }

            mapCubes[x, y].Remove(objectToDestroy);
            if (mapCubes[x, y].Count <= 1)
            {
                //Debug.Log(positionOfPossibilities[x * squareLenght + y]);
                //positionOfPossibilities.RemoveAt(x * squareLenght + y);
                positionOfPossibilities[x * squareLenght + y] = Vector2Int.down;
            }

        }

        private List<GameObject> CheckPossibilities(Vector2 indexToSearch, GameObject previousCube, Side sideOfPreviousCube)
        {
            int numberOfPossibilites = 0;
            List<GameObject> possibilities = new List<GameObject>();
            var previousIndiceRef = previousCube.GetComponent<CanBeNextTo>().adjoiningCubes[sideOfPreviousCube];

            List<GameObject> actualPossibilites = mapCubes[(int)indexToSearch.x, (int)indexToSearch.y];

            if (actualPossibilites.Count > 1)
            {
                for (int i = 0; i < actualPossibilites.Count; i++)
                {
                    var possibilitieIndiceRef = actualPossibilites[i].GetComponent<CanBeNextTo>().adjoiningCubes[SideHelp.GetInverseSide(sideOfPreviousCube)];

                    if (possibilitieIndiceRef == previousIndiceRef)
                    {
                        possibilities.Add(actualPossibilites[i]);
                        numberOfPossibilites++;
                    }
                }
            }
            numberOfPossibilites -= (numberOfPossibilites == actualPossibilites.Count) ? 1 : 0;

            return possibilities;
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
            int index = seed % lenghtList;
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

