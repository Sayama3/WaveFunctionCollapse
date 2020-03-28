using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace Procedural
{
    public class AnotherWaveFunctionCollapse : MonoBehaviour
    {
        [SerializeField] private Text seedWriter;
        
        [SerializeField]private Transform parent;
        
        private int compteur = 0;
        [SerializeField] private int numberOfDifferentCubes = 3;
        public int squareLenght = 5;
        private int seed = 0;

        [SerializeField] private List<GameObject> cubesUsed = new List<GameObject>();
        [SerializeField] private List<GameObject> allTheCubes = new List<GameObject>();

        private List<GameObject>[,] mapCubes;

        Vector2Int indexMax;
        List<Vector2Int> positionInitialCube = new List<Vector2Int>();
        List<GameObject> choosenInitialCube = new List<GameObject>();
        List<Vector2Int> positionOfPossibilities = new List<Vector2Int>();
        
        
        
        [Button(ButtonSizes.Large)]
        public void MakeMap(int seedChoosen = -1)
        {
            GetComponent<DernierRecours>().DestroyEveryCubes();
            
            compteur = 0;


            seed = (seedChoosen < 1) ? SeedHelp.CreateSeed() : seedChoosen;
            // seed = 15364;
            Debug.LogWarning("Attention, la seed utilisé est la suivante : " + seed);
            seedWriter.text = "La seed actuelle est : " + seed;
            UseRandomCubes();

            if (cubesUsed.Count != numberOfDifferentCubes)
            {
                Debug.LogError("Il y a " + ((cubesUsed.Count < numberOfDifferentCubes) ? "pas assez" : "trop") +
                               " de cubes dans la liste (" + cubesUsed.Count + ").");
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

            int possibilitesLeft = 0;
            List<Vector2Int> possibilityLeftReal = new List<Vector2Int>(positionOfPossibilities);
            
            
            do
            {
                for (int g = 0; g < 1; g++)
                {

                    var index = RandomizeIndex(possibilityLeftReal.Count);
                    //Debug.Log(index + " ; " + positionOfPossibilities.Count);
                    positionInitialCube.Add(
                        possibilityLeftReal[index]);

                    index = RandomizeIndex(mapCubes[positionInitialCube[positionInitialCube.Count - 1].x,
                        positionInitialCube[positionInitialCube.Count - 1].y].Count);

                    choosenInitialCube.Add(mapCubes[positionInitialCube[positionInitialCube.Count - 1].x,
                        positionInitialCube[positionInitialCube.Count - 1].y][index]);

                    //Debug.Log("La position initial est : " + positionInitialCube[positionInitialCube.Count - 1]);
                    //Debug.Log("Le cube de départ choisi est " + choosenInitialCube[choosenInitialCube.Count - 1].name);
                    //if (choosenInitialCube.Count - 1 == 0)
                    //{
                    //    // choosenInitialCube[0].GetComponent<MeshRenderer>().material.color = Color.black;
                    //    Debug.Log("Le premier choisi est " + choosenInitialCube[0].name);
                    //}

                    // int remover = 0;
                    var testList = new List<GameObject>(mapCubes[positionInitialCube[positionInitialCube.Count - 1].x,
                        positionInitialCube[positionInitialCube.Count - 1].y]);
                    for (int i = 0;
                        i < testList.Count;
                        i++)
                    {
                        var item = testList[i];
                        if (item != choosenInitialCube[choosenInitialCube.Count - 1])
                        {
                            CollapseMap(positionInitialCube[positionInitialCube.Count - 1], positionInitialCube, item);
                            // ++remover;
                        }
                    }

                    possibilitesLeft = 0;
                    possibilityLeftReal.Clear();
                    for (int i = 0; i < positionOfPossibilities.Count; i++)
                    {
                        if (positionOfPossibilities[i] != Vector2.down)
                        {
                            possibilitesLeft++;
                            possibilityLeftReal.Add(positionOfPossibilities[i]);
                        }
                    }

                    //Debug.Log(possibilitesLeft);
                }
                
            } while (possibilitesLeft > 0);

            for (int i = 0; i < squareLenght; i++)
            {
                for (int j = 0; j < squareLenght; j++)
                {
                    for (int k = 0; k < mapCubes[i,j].Count; k++)
                    {
                        var pos = mapCubes[i, j][k].transform.position;
                        pos.y = k;
                        mapCubes[i,j][k].transform.position = pos;
                    }
                }
            }
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
                    List<GameObject> randomCubes = new List<GameObject>();
                    for (int k = 0; k < cubesUsed.Count; k++)
                    {
                        var cube = Instantiate(cubesUsed[k], new Vector3(i, k, j), Quaternion.identity,parent);
                        randomCubes.Add(cube);
                        cube.SetActive(true);
                    }
                    
                    mapCubes[i, j] = randomCubes;
                }
            }
        }

        void CollapseMap(Vector2Int cubeIndex, List<Vector2Int> previousPositions, GameObject objectToDestroy)
        {
        Begining:
        //Debug.Log("je m'apprete a detruire " + objectToDestroy.name + " en coordonnée "+ cubeIndex);
            int x = cubeIndex.x;
            int y = cubeIndex.y;
            if (objectToDestroy == null || mapCubes[x,y].Count <= 1) return;
            
            List<Side> possibleSides = new List<Side>(); 

            List<Vector2Int> allPositions = new List<Vector2Int>(previousPositions)
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
                if (sideCheck[i])
                {
                    if (sideNumberPossibilities[i] == mapCubes[nextPositions[i].x,nextPositions[i].y].Count)
                    {
                        sideNumberPossibilities[i]--;
                        sidePossibilites[i].RemoveAt(RandomizeIndex(sidePossibilites[i].Count));
                    }
                }
                if (sideCheck[i] && sideNumberPossibilities[i] > 0)
                {
                    possibleSides.Add((Side)i);
                }

            }

            if (possibleSides.Count > 0)
            {
                var anIndex = RandomizeIndex(possibleSides.Count);
                var theSide = possibleSides[anIndex];
                int firstOne = -1;
                int numberOfSidePossible = possibleSides.Count;

                for (int i = 0; i < numberOfSidePossible; i++)
                {

                    /*Debug.Log(SideHelp.ShowRealName(theSide) + " soit au coordonnée " +
                              nextPositions[(int) theSide] +
                              " " +
                              ((sideCheck[(int) theSide] && sideNumberPossibilities[(int) theSide] > 0)
                                  ? "il y a " + sideNumberPossibilities[i] + "possibilité"
                                  : "il n'y a pas de possibilité"));*/

                    if ((sideCheck[(int) theSide] && sideNumberPossibilities[(int) theSide] > 0))
                    {
                        ++compteur;
                        //Debug.LogWarning("Le compteur est à " + compteur);
                        //Debug.Log("les possibilités sont :");
                    }


                    if (sideCheck[(int) theSide] && sideNumberPossibilities[(int) theSide] > 0)
                    {
                        firstOne = RandomizeIndex(sideNumberPossibilities[(int) theSide]);
                        for (int j = 0; j < sideNumberPossibilities[(int) theSide]; j++)
                        {
                            if (sidePossibilites[(int) theSide][firstOne] != null)
                            {
                                CollapseMap(nextPositions[(int) theSide], allPositions,
                                    sidePossibilites[(int) theSide][firstOne]);
                                firstOne = IncreaseIndex(firstOne, sideNumberPossibilities[(int) theSide]);
                                
                            }

                        }
                        possibleSides.Remove(theSide);
                        --anIndex;
                    }

                    if (possibleSides.Count > 0)
                    {
                        anIndex = IncreaseIndex(anIndex, possibleSides.Count);
                        theSide = possibleSides[anIndex];
                    }
                    else
                    {
                        break;
                    }

                }
                
            }

            // List<Side> sidesExeption = new List<Side>();
            // foreach (var position in previousPositions)
            // {
            //     Side side = SideHelp.sideBetweenTwoPoint(cubeIndex, position);
            //     if (side != Side.Null)
            //     {
            //         sidesExeption.Add(side);
            //     }
            // }
            // if (CubeHaveAnyPossibility(objectToDestroy,cubeIndex,sidesExeption))
            if(false)
            {
                 goto Begining;
            }
            else
            {
                mapCubes[x,y].Remove(objectToDestroy);
                if (mapCubes[x, y].Count <= 1)
                {
                    positionOfPossibilities[x * squareLenght + y] = Vector2Int.down;
                }
                DestroyImmediate(objectToDestroy,false);
            }

        }

        private List<GameObject> GetPossibilities(Vector2Int indexOfCube, GameObject theCube, Side sideOfCube)
        {
            //int numberOfPossibilites = 0;
            List<GameObject> possibilities = new List<GameObject>();
            var cubePossibilites = theCube.GetComponent<CanBeNextTo>().adjoiningCubes[sideOfCube];

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
                        if (item.name.Contains(cube.name) || cube.name.Contains(item.name))
                        {
                            found = !CubeHaveAnyPossibilitySided(cube, indexToSearch, sideToSearch, theCube);
                            break;
                            
                        }
                    }
                    if (found)
                    {
                        possibilities.Add(cube);
                    }
                }
            }

            return possibilities;
        }

        
        private bool CubeHaveAnyPossibilitySided(GameObject cubeToCheck,Vector2Int cubeIndex,Side sideToSearch, GameObject cubeExeption)
        {
            int x = cubeIndex.x;
            int y = cubeIndex.y;
            int index = FindGameObjectIndex.IndexGameObject(mapCubes[x, y],cubeToCheck);
            Vector2Int oldIndex = SideHelp.NextIndex(cubeIndex, sideToSearch);
            bool foundPossibilities = false;
            if (oldIndex.x <= indexMax.x && oldIndex.x >= 0 && oldIndex.y <= indexMax.y && oldIndex.y >= 0)
            {
                List<GameObject> possibilites = cubeToCheck.GetComponent<CanBeNextTo>().adjoiningCubes[sideToSearch];

                foreach (var aCube in mapCubes[oldIndex.x, oldIndex.y])
                {
                    if (!aCube.name.Contains(cubeExeption.name) && !cubeExeption.name.Contains(aCube.name))
                    {
                        foreach (var possibilite in possibilites)
                        {
                            if (possibilite.name.Contains(aCube.name) || aCube.name.Contains(possibilite.name))
                            {
                                foundPossibilities = true;
                                break;
                            }
                        }

                        if (foundPossibilities) break;
                    }
                }
            }

            return foundPossibilities;
        }

        private bool CubeHaveAnyPossibility(GameObject cubeToCheck, Vector2Int cubeIndex, List<Side> sideExeption)
        {
            
            bool haveAPossibilitie = false;
            for (int i = 0; i < 4; i++)
            {
                Side side = (Side) i;
                bool found = false;
                for (int j = 0; j < sideExeption.Count; j++)
                {
                    if (side == sideExeption[j])
                    {
                        found = true;
                        break;
                    }
                    
                }
                
                if (!found)
                {
                    haveAPossibilitie = CubeHaveAnyPossibilitySided(cubeToCheck, cubeIndex, side, new GameObject());
                    if (haveAPossibilitie)
                    {
                        break;
                    }
                }
            }

            return haveAPossibilitie;
        }
        
        public void UseRandomCubes()
        {
            // this.seed = SeedHelp.CreateSeed();

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

        private int IncreaseIndex(int index, int lenghtList)
        {
            int newIndex = (index + 1 < lenghtList) ? index + 1 : 0;
            newIndex = (newIndex < 0) ? 0 : newIndex;
            return newIndex;
        }
        
        private Side GetRandomSideExcluding(Side[] excludeSide, bool includeNull = false)
        {
            int max = (includeNull) ? 5 : 4;
            List<Side> possibleSide = new List<Side>();
            for (int i = 0; i < max; i++)
            {
                bool canBeTake = true;
                foreach (var side in excludeSide)
                {
                    if (side == (Side)i)
                    {
                        canBeTake = false;
                        break;
                    }
                }
                if (canBeTake)
                {
                    possibleSide.Add((Side)i);
                }
            }
            return possibleSide[RandomizeIndex(possibleSide.Count)];
        }
        private Side GetRandomSideExcluding(List<Side> excludeSide, bool includeNull = false)
        {
            int max = (includeNull) ? 5 : 4;
            List<Side> possibleSide = new List<Side>();
            for (int i = 0; i < max; i++)
            {
                bool canBeTake = true;
                foreach (var side in excludeSide)
                {
                    if (side == (Side)i)
                    {
                        canBeTake = false;
                        break;
                    }
                }
                if (canBeTake)
                {
                    possibleSide.Add((Side)i);
                }
            }
            return possibleSide[RandomizeIndex(possibleSide.Count)];
        }

    }

    
}

