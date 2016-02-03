using UnityEngine;
using System.Collections;

namespace Seni.Utilities{
    public static class TransformGoodies {
        
        ///<summary>
        ///Randomizes the position of the Transform's children in the Hierarchy.
        ///Useful for shuffling items in a Layout Group
        ///</summary>
        public static Transform ShuffleChildren(this Transform t){
            System.Random rand = new System.Random();
            for(int i = 0; i < t.childCount; i++){
                t.GetChild(rand.Next(0, t.childCount-1)).SetSiblingIndex(i);
            }
            return t; 
        }

    }    
}