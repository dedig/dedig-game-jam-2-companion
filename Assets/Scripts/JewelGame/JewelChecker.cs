using UnityEngine;
using System.Collections;

namespace Seni.Game{
    
    public enum TypeOfMatch{
        Seven,
        Six,
        Five,
        Four,
        Three,
        Future,
        None
    }
    
    public class JewelChecker {

        public static TypeOfMatch CheckForMatch(JewelButton jb){
            //         //check next ones in row
            //         //check next ones in column
            //         //have an index that counts the number of stuffs                                 
            return TypeOfMatch.None;
        }

    }
}