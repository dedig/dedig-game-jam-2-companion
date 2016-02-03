using UnityEngine;
using System.Collections;

namespace Seni.Game{
    public class ScoreManager : MonoBehaviour {
        
        public int score = 0;
        
        public static ScoreManager sharedInstance;

        // Use this for initialization
        void Start () {
            if(sharedInstance==null){
                sharedInstance = this;
            } else {
                Debug.LogWarning("more than a single instance of ScoreManager found");
                Destroy(gameObject);
            }
        }        

        public void StartGame(){
            ResetValues();    
        }
        
        void ResetValues(){
            score = 0;
        }
        
    }
}

