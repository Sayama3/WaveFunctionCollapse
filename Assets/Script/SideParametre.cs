using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Procedural
{
    public class SideParametre : MonoBehaviour
    {
        public Side ParametreChoisie()
        {
            if (gameObject.name.Contains("haut"))
            {
                return Side.Front;
            }else if (gameObject.name.Contains("bas"))
            {
                return Side.Back;
            }else if (gameObject.name.Contains("droite"))
            {
                return Side.Right;
            }else if (gameObject.name.Contains("gauche"))
            {
                return Side.Left;
            }
            else
            {
                return Side.Null;
            }
        }
    }
}
