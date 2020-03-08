using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Procedural
{
    [ExecuteInEditMode]
    public class Positionneur : MonoBehaviour
    {
        [SerializeField] private List<GameObject> cubes = new List<GameObject>();
        [SerializeField] private Transform camera;
        public static bool isIndexReady = false;

        private void Start()
        {
            ResetPosition(new Vector3(0, 10));
        }

        [Button]
        public bool PositionnementCubes(int tailleDeLArrete)
        {
            ResetPosition(new Vector3(0, tailleDeLArrete + 10, 0));
            PositionnementMainCamera(tailleDeLArrete);

            for (int index = 0; index < cubes.Count; ++index)
            {
                if (index >= (tailleDeLArrete * tailleDeLArrete))
                {
                    //Debug.LogError("Index = " + index);
                    //Debug.LogError("Taille du carre = " + (tailleDeLarrete * tailleDeLarrete));
                    Debug.LogError("Il y a " + (cubes.Count - index) + " cubes qui ne sont pas compris dans le cube");
                    return false;
                }
                int positionZ = 0;
                int positionX = index;

                while (positionX >= tailleDeLArrete)
                {
                    ++positionZ;
                    positionX -= tailleDeLArrete;
                }

                cubes[index].transform.position = new Vector3(positionX, 0, positionZ);
            }


            if (tailleDeLArrete * tailleDeLArrete > cubes.Count)
            {
                Debug.LogError("Le carré est trop grand, il manque " + (tailleDeLArrete * tailleDeLArrete - cubes.Count) + " cubes");
                return false;
            }
            else
            {
                return true;
            }

        }

        [Button]
        public void ResetPosition(Vector3 resetPosition = new Vector3())
        {
            PositionnementMainCamera(0);
            foreach (GameObject item in cubes)
            {
                item.transform.position = resetPosition;
            }
        }

        private void AfficherPosition(char Axe, float position)
        {
            Debug.Log("pour " + name + " " + Axe + " = " + position);

        }
        public void SetListeCube(GameObject[] cubesArray)
        {
            if (cubes == null)
            {
                cubes = new List<GameObject>();
            }
            else if (cubes.Count > 0)
            {
                foreach (var item in cubes)
                {
                    DestroyImmediate(item);
                }
                cubes.Clear();
            }
            for (int i = 0; i < cubesArray.Length; i++)
            {
                cubes.Add(cubesArray[i]);
            }
        }
        public void PositionnementMainCamera(int tailleDeLArette)
        {
            //Debug.Log("Positionnement de la camera en cours");
            //camera = Camera.main.transform;
            

            float positionCotee = Mathf.Max((((float)tailleDeLArette - 1) / 2), 0);

            Vector3 position = new Vector3();
            position.y = tailleDeLArette + 0.5f;
            position.x = positionCotee;
            position.z = positionCotee;

            camera.position = position;

        }
    }
}


