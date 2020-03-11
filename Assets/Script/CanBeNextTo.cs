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
    public class CanBeNextTo : SerializedMonoBehaviour
    {
        private Color[] colorsSideCubes = { Color.blue, Color.red, Color.green, Color.magenta };

        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout,KeyLabel = "Side",ValueLabel = "Cubes Associates")]
        public Dictionary<Side, List<GameObject>> adjoiningCubes = new Dictionary<Side, List<GameObject>>();


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + Vector3.up / 2, transform.localScale.x / 4);
        }

    }


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

        public static string ShowRealName(Side givenSide)
        {
            string realName = "";
            switch (givenSide)
            {
                case Side.Front:
                    realName = "devant";
                    break;
                case Side.Back:
                    realName = "derrière";
                    break;
                case Side.Left:
                    realName = "à gauche";
                    break;
                case Side.Right:
                    realName = "à droite";
                    break;
            }
            return realName;
        }
    }



}

