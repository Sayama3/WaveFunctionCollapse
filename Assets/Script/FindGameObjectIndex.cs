using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Procedural
{
    public static class FindGameObjectIndex
    {
        
        public static int IndexGameObject(List<GameObject> TheList, GameObject THeGameObject)
        {
            bool gameObjectNotFind = false;
             int theIndex = -1;
            for (int i = 0; i < TheList.Count; i++)
            {
                if (TheList[i].gameObject == THeGameObject)
                {
                    gameObjectNotFind = false;
                    theIndex = i;
                    i = TheList.Count;
                }
                else
                {
                    gameObjectNotFind = true;
                }
            }

            if (gameObjectNotFind)
            {
                return -1;
            }
            else
            {
                return theIndex;
            }
        }
    }
}