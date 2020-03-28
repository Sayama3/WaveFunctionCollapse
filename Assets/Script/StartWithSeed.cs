using System;
using System.Collections;
using System.Collections.Generic;
using Procedural;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class StartWithSeed : MonoBehaviour
{
    public GameObject Gestionnaire;
    public void LetsStart()
    {
        string txt = GameObject.FindWithTag("FindMe").GetComponent<Text>().text;
        int seed = Int32.Parse(txt);
        Gestionnaire.GetComponent<AnotherWaveFunctionCollapse>().MakeMap(seed);
    }
}
