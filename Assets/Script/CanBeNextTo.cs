using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Procedural
{

    public enum Side
    {
        Front,
        Back,
        Left,
        Right
    }

    [ExecuteInEditMode]
    public class CanBeNextTo : MonoBehaviour
    {
        private Color[] colorsSideCubes = { Color.blue, Color.red, Color.green, Color.magenta };
        //public GameObject[] adjoiningCubes = new GameObject[4];
        public SideDictionnary adjoiningCubes = new SideDictionnary();
        
    }

    [Serializable]
    public class SideDictionnary : SerializableDictionary<Side, int> { }

    public static class SideHelp
    {
        public static Side GetInverseSide(Side givenSide)
        {
            Side inverSide = givenSide;

            switch (givenSide)
            {
                case Side.Front:
                    inverSide = Side.Back;
                    break;
                case Side.Back:
                    inverSide = Side.Front;
                    break;
                case Side.Left:
                    inverSide = Side.Right;
                    break;
                case Side.Right:
                    inverSide = Side.Left;
                    break;
                default:
                    Debug.LogError("Le cas ou le côté est " + givenSide.ToString() + " n'est pas pris en charge.");
                    break;

            }

            return inverSide;

        }


    }



}

