using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Procedural
{
    public class SelectionCarreParametre : MonoBehaviour
    {
        [SerializeField] private Material PremierCarreSelectione;
        [SerializeField] private Material PremierCarreSelectioneSelectionAutreCarre;
        [SerializeField] private Material AutreCarreSelectionnable;// rend les carrés selectionable bleu
        private int CarreSelectione = 0;
        private Material ConteneurMaterielCube;
        private Material ConteneurMaterielParametre;
        private List<GameObject> ListCarreAdd = new List<GameObject>();
        private List<Material> ListMaterielAdd = new List<Material>();

        private GameObject ConteneurCarreSelectione;
        private GameObject ParametreSelectione;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Parametre")) && this.CarreSelectione == 1)
                {
                    Debug.Log("Miam2");
                    this.ConteneurCarreSelectione.transform.GetComponent<MeshRenderer>().material = this.PremierCarreSelectioneSelectionAutreCarre;
                    this.ParametreSelectione = hit.transform.gameObject;
                    this.ConteneurMaterielParametre = hit.transform.GetComponent<MeshRenderer>().material;
                    hit.transform.GetComponent<MeshRenderer>().material = this.PremierCarreSelectione;
                    this.CarreSelectione = 2;
                }else
                if (Physics.Raycast(ray, out RaycastHit hitt, Mathf.Infinity, LayerMask.GetMask("Carre")))
                {
                    Debug.Log(hitt);
                    if (this.CarreSelectione == 0)
                    {
                        Debug.Log("Miam");
                        this.ConteneurCarreSelectione = hitt.transform.gameObject;
                        foreach (Transform Child in hitt.transform)
                        {
                            Child.gameObject.SetActive(true);
                        }
                        this.ConteneurMaterielCube = hitt.transform.GetComponent<MeshRenderer>().material;
                        hitt.transform.GetComponent<MeshRenderer>().material = this.PremierCarreSelectione;
                        hitt.transform.gameObject.layer = 0;
                        this.CarreSelectione = 1;
                    }
                    else if(this.CarreSelectione == 2)
                    {
                        Debug.Log("Miam3");
                        hitt.transform.gameObject.layer = 0;
                        this.ListCarreAdd.Add(hitt.transform.gameObject);
                        this.ListMaterielAdd.Add(hitt.transform.GetComponent<MeshRenderer>().material);
                        // rendre les carrés selectionable bleu
                        if (this.ConteneurCarreSelectione.GetComponent<CanBeNextTo>().adjoiningCubes
                            [this.ParametreSelectione.GetComponent<SideParametre>().ParametreChoisie()].Contains(hitt.transform.gameObject))
                        {
                            this.ConteneurCarreSelectione.GetComponent<CanBeNextTo>().adjoiningCubes
                                [this.ParametreSelectione.GetComponent<SideParametre>().ParametreChoisie()].Remove(hitt.transform.gameObject);
                            hitt.transform.GetComponent<CanBeNextTo>().adjoiningCubes
                                [SideHelp.GetInverseSide(this.ParametreSelectione.GetComponent<SideParametre>().ParametreChoisie())].Remove(this.ConteneurCarreSelectione);
                        }
                        else
                        {
                            this.ConteneurCarreSelectione.GetComponent<CanBeNextTo>().adjoiningCubes
                                [this.ParametreSelectione.GetComponent<SideParametre>().ParametreChoisie()].Add(hitt.transform.gameObject);
                            hitt.transform.GetComponent<CanBeNextTo>().adjoiningCubes
                                [SideHelp.GetInverseSide(this.ParametreSelectione.GetComponent<SideParametre>().ParametreChoisie())].Add(this.ConteneurCarreSelectione);
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (this.CarreSelectione <= 0)
                {
                    Debug.Log("Pas Possible");
                }else
                if (this.CarreSelectione == 1)
                {
                    this.ConteneurCarreSelectione.layer = 8;
                    this.ConteneurCarreSelectione.GetComponent<MeshRenderer>().material = this.ConteneurMaterielCube;
                    foreach (Transform Child in this.ConteneurCarreSelectione.transform)
                    {
                        if (Child.GetComponent<BoxCollider>() != null)
                        {
                            Child.gameObject.SetActive(false);
                        }
                    }
                    this.CarreSelectione--;
                }else 
                if (this.CarreSelectione == 2)
                {
                    this.CarreSelectione--;
                    this.ParametreSelectione.GetComponent<MeshRenderer>().material = this.ConteneurMaterielParametre;
                    for (int i = 0; this.ListCarreAdd.Count > i; i++)
                    {
                        Debug.Log(this.ListCarreAdd[i]);
                        this.ListCarreAdd[i].layer = 8;
                        this.ListCarreAdd[i].GetComponent<MeshRenderer>().material = this.ListMaterielAdd[i];
                    }
                }

            }

        }
    }
}
