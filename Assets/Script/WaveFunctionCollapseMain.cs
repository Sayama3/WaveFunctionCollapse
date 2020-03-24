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

        Vector2Int indexMax;
        List<Vector2Int> positionInitialCube = new List<Vector2Int>();
        List<GameObject> choosenInitialCube = new List<GameObject>();
        List<Vector2Int> positionOfPossibilities = new List<Vector2Int>();
        
        
        
        [Button(ButtonSizes.Large)]
        public void MakeMap()
        {
            seed = SeedHelp.CreateSeed();
            UseRandomCubes();

            if (cubesUsed.Count != numberOfDifferentCubes)
            {
                Debug.LogError("Il y a " + ((cubesUsed.Count < numberOfDifferentCubes) ? "pas assez" : "trop") + " de cubes dans la liste (" + cubesUsed.Count + ").");
                return;
            }

            indexMax = new Vector2Int(squareLenght - 1, squareLenght - 1);
            BuildMap();

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
            //Debug.Log(index + " ; " + positionOfPossibilities.Count);
            positionInitialCube.Add(
                positionOfPossibilities[index]);

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
                index = RandomizeIndex(possibilitesLeft);
                positionInitialCube.Add(possibilityLeftReal[index]);
                //Debug.Log("Possibility left : " + possibilitesLeft);
                //Debug.Log("index : " + positionInitialCube[positionInitialCube.Count - 1].x + " , " + positionInitialCube[positionInitialCube.Count - 1].y);
                index = RandomizeIndex(mapCubes[positionInitialCube[positionInitialCube.Count - 1].x, positionInitialCube[positionInitialCube.Count - 1].y].Count);
                choosenInitialCube.Add(mapCubes[positionInitialCube[positionInitialCube.Count - 1].x, positionInitialCube[positionInitialCube.Count - 1].y][index]);
                goto Research;
            }

            Positionneur positionneur = GetComponent<Positionneur>();
            positionneur.SetListeCube(mapCubes, squareLenght);
            positionneur.GenerateList();
            positionneur.PositionnementCubes(squareLenght);


        }

        //[Button(ButtonSizes.Large)]
        private void BuildMap()
        {
            mapCubes = new List<GameObject>[squareLenght, squareLenght];
            mapCubes.Initialize();
            for (int i = 0; i < squareLenght; i++)
            {
                for (int j = 0; j < squareLenght; j++)
                {
                    mapCubes[i, j] = cubesUsed;
                }
            }
        }

        void CollapseMap(Vector2Int cubeIndex, List<Vector2> previousPositions, GameObject objectToDestroy)
        {
            int x = cubeIndex.x;
            int y = cubeIndex.y;
            if (objectToDestroy == null || mapCubes[x,y].Count <= 1) return;


            List<Vector2> allPositions = new List<Vector2>(previousPositions)
            {
                cubeIndex
            };

            bool[] sideCheck = new bool[4];
            int[] sideNumberPossibilities = new int[4];
            List<GameObject>[] sidePossibilites = new List<GameObject>[4];
            for (int i = 0; i < sidePossibilites.Length; i++)
            {
                sidePossibilites[i] = new List<GameObject>();
            }
            Vector2Int[] nextPositions = new Vector2Int[4];



            for (int i = 0; i < sideCheck.Length; i++)
            {
                nextPositions[i] = SideHelp.NextIndex(cubeIndex, (Side)i);
                
                if (nextPositions[i].x <= indexMax.x && nextPositions[i].y <= indexMax.y && nextPositions[i].x >=0 && nextPositions[i].y >=0)
                {
                    sideCheck[i] = true;
                }
                else
                {
                    sideCheck[i] = false;
                }

                foreach (var item in previousPositions)
                {
                    if (item == nextPositions[i])
                    {
                        sideCheck[i] = false;
                        break;
                    }
                }
                
                sidePossibilites[i] = (sideCheck[i]) ? GetPossibilities(cubeIndex, objectToDestroy, (Side)i) : new List<GameObject>(0);
                sideNumberPossibilities[i] = sidePossibilites[i].Count;

            }

            Side nextSide = (Side)RandomizeIndex(sideCheck.Length);
            
            int h = 0;
            while (sidePossibilites[(int)nextSide].Count > 0);
            {
                if (h == 4)
                {
                    goto SwitchTheCollaps;
                }
                nextSide = ((int)nextSide + 1 < 4) ? ++nextSide : 0;
                ++h;
                
            } 


            for (int i = 0; i < sideCheck.Length; i++)
            {
                for (int j = 0; j < sideNumberPossibilities[(int)nextSide]; j++)
                {
                    var index = RandomizeIndex(sidePossibilites[(int)nextSide].Count);

                    if (CubeHaveAnyPossibility(sidePossibilites[(int)nextSide][index], nextPositions[(int)nextSide], SideHelp.GetInverseSide(nextSide),objectToDestroy))
                    {
                        CollapseMap(nextPositions[(int)nextSide], allPositions, sidePossibilites[(int)nextSide][index]);
                    }
                    sidePossibilites[(int)nextSide].RemoveAt(index);
                }
                nextSide = ((int)nextSide + 1 < 4) ? (nextSide + 1) : 0;
            }
            
            SwitchTheCollaps:
            
            mapCubes[x, y].Remove(objectToDestroy);
            if (mapCubes[x, y].Count <= 1)
            {
                //Debug.Log(positionOfPossibilities[x * squareLenght + y]);
                //positionOfPossibilities.RemoveAt(x * squareLenght + y);
                positionOfPossibilities[x * squareLenght + y] = Vector2Int.down;
            }

        }

        private List<GameObject> GetPossibilities(Vector2Int indexOfCube, GameObject previousCube, Side sideOfCube)
        {
            //int numberOfPossibilites = 0;
            List<GameObject> possibilities = new List<GameObject>();
            var cubePossibilites = previousCube.GetComponent<CanBeNextTo>().adjoiningCubes[sideOfCube];

            Vector2Int indexToSearch = SideHelp.NextIndex(indexOfCube, sideOfCube);
            //Debug.Log(indexToSearch);
            List<GameObject> actualPossibilites = mapCubes[(int)indexToSearch.x, (int)indexToSearch.y];

            Side sideToSearch = SideHelp.GetInverseSide(sideOfCube);

            if (actualPossibilites.Count > 1)
            {
                for (int i = 0; i < actualPossibilites.Count; i++)
                {
                    var cube = actualPossibilites[i];
                    bool found = false;
                    foreach (var item in cubePossibilites)
                    {
                        if (item == cube)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        possibilities.Add(cube);
                    }
                }
            }
            //numberOfPossibilites -= (numberOfPossibilites == actualPossibilites.Count) ? 1 : 0;

            return possibilities;
        }

        
        private bool CubeHaveAnyPossibility(GameObject cubeToCheck,Vector2Int cubeIndex,Side sideToSearch, GameObject cubeExeption)
        {
            int x = cubeIndex.x;
            int y = cubeIndex.y;
            int index = FindGameObjectIndex.IndexGameObject(mapCubes[x, y],cubeToCheck);
            Vector2Int oldIndex = SideHelp.NextIndex(cubeIndex, sideToSearch);
            bool foundPossibilities = false;

            List<GameObject> possibilites = cubeToCheck.GetComponent<CanBeNextTo>().adjoiningCubes[sideToSearch];

            foreach(var aCube in mapCubes[oldIndex.x, oldIndex.y])
            {
                if (aCube != cubeExeption)
                {
                    foreach (var possibilite in possibilites)
                    {
                        if (possibilite == aCube)
                        {
                            foundPossibilities = true;
                            break;
                        }
                    }

                    if (foundPossibilities) break;
                    
                }
            }
            
            return foundPossibilities;
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
            return Mathf.Abs(index);
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

