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
        Right,
        Null
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

        public static Vector2Int NextIndex(Vector2Int index, Side givenSide)
        {
            Vector2Int newIndex = index;
            switch (givenSide)
            {
                case Side.Front:
                    newIndex += Vector2Int.up;
                    break;
                case Side.Back:
                    newIndex += Vector2Int.down;
                    break;
                case Side.Left:
                    newIndex += Vector2Int.left;
                    break;
                case Side.Right:
                    newIndex += Vector2Int.right;
                    break;
            }
            return newIndex;
        }

        public static Side SideIncrease(Side givenSide, bool keepTheNullSide = false)
        {
            int max = (keepTheNullSide) ? 5 : 4;
            Side nextSide = ((int) givenSide + 1 < max) ? givenSide + 1 : 0;
            return nextSide;
        }
        public static Side SideDecrease(Side givenSide, bool keepTheNullSide = false)
        {
            int max = (keepTheNullSide) ? 5 : 4;
            Side nextSide = ((int) givenSide - 1 >= 0) ? givenSide - 1 : (Side)(max-1);
            return nextSide;
        }

        public static Side sideBetweenTwoPoint(Vector2Int positionReference, Vector2Int positionTest)
        {
            Side side = Side.Null;
            if (positionReference == positionTest) return Side.Null;
            if (positionReference.x == positionTest.x)
            {
                
                int test = positionReference.y - positionTest.y;
                if ( Mathf.Abs(test) == 1)
                {
                    if (test < 0)
                    {
                        side = Side.Front;
                    }
                    else
                    {
                        side = Side.Back;
                    }
                }
                
            }
            else if(positionReference.y == positionTest.y)
            {
                int test = positionReference.x - positionTest.x;
                if (Mathf.Abs(test) == 1)
                {
                    if (test < 0)
                    {
                        side = Side.Right;
                    }
                    else
                    {
                        side = Side.Left;
                    }
                }
            }
            else
            {
                side = Side.Null;
                // Debug.LogError("On tente de savoir un côté entre deux point qui ne sont pas a côté");
            }

            return side;
        }
    }



}

