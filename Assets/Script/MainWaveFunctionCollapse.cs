using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Procedural
{
    public class MainWaveFunctionCollapse : MonoBehaviour
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

            CollapseMap(positionInitialCube[0]);


            bool CollapseMap(Vector2 startingIndex)
            {
                int x = (int)startingIndex.x;
                int y = (int)startingIndex.y;



                return true;
            }
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

