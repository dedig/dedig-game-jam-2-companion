using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Seni.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace Seni.Game{    
    public class JewelManager : MonoBehaviour {
        
        public static JewelManager sharedInstance;
        
        public GameObject jewelPrefab, verticalScroll, horizontalScroll;
        public Transform buttonsGrid;
        public int score {private set; get;}
        
        public const int jewelPoolX = 8, jewelPoolY = 7; //jewel objects pool size (grid size 7x8)
        
        public List<JewelButton> jewelPool = new List<JewelButton>(); //jewel objects pool

        // Use this for initialization
        void Start () {
            score = 0;
            if(sharedInstance==null){
                sharedInstance = this;
            } else {
                Debug.LogWarning("more than a single instance of JewelManager found");
                Destroy(gameObject);
            }
            if(jewelPrefab!=null && buttonsGrid!=null){
                jewelPool = new List<JewelButton>(); //jewel objects pool                                
                RandomizeJewels();     
                IndexateJewels();
            } else {
                Debug.LogError("no jewelbutton or buttonsgrid found");
            }            
        }
         
        void IndexateJewels(){       
            int transformIndex = 0;             
            for(int i = 0; i <= jewelPoolX-1; i++){
                for(int j = 0; j <= jewelPoolY-1; j++){             
                    jewelPool.Add(buttonsGrid.GetChild(transformIndex).GetComponent<JewelButton>());
                    jewelPool[transformIndex].SetMyType(GetRandomJewelType());                      
                    jewelPool[transformIndex].SetIndex(i,j);                  
                    transformIndex++;                             
                }
            }
        }
        
        JewelButton.JewelType GetRandomJewelType(){
            System.Random rand = new System.Random();
            int i = rand.Next(0, 4);
            Debug.Log(i);
            switch(i){
                case 0:
                    return JewelButton.JewelType.Red;
                case 1:
                    return JewelButton.JewelType.Green;
                case 2:
                    return JewelButton.JewelType.Blue;
                default:
                    return JewelButton.JewelType.Alpha;
            }
        }
        
        void RandomizeJewels(){
            Debug.Log("Randomizing Jewel Order");
            transform.ShuffleChildren();            
        }

        bool VerifyValues(){
            Debug.LogWarning("Verifying for matches or possible matches");            
                        
            int consecutiveJewels = 0;
            JewelButton.JewelType previousType = JewelButton.JewelType.Empty;
            bool wereAnyObjectsTagged = false;
            
            for(int i = 0; i < jewelPoolX; i++){
                consecutiveJewels = 0;
                previousType = JewelButton.JewelType.Empty;
                for(int j = 0; j < jewelPoolY; j++){
                    //get item from jewelpool where item.x == i and item.y == j                    
                    JewelButton jb = GetJewelInCoordinates(i, j);
                    if(jb==null) break;                    
                    if(jb.type==previousType){
                        consecutiveJewels++;
                    } else {
                        consecutiveJewels = 0;
                    }
                    
                    if(consecutiveJewels==2){
                        wereAnyObjectsTagged = true;
                        jb.destroyable = true;
                        JewelButton jback = (from item in jewelPool where (item.x == i) && (item.y==j-1) select item).First();
                        jback.destroyable = true;
                        JewelButton jreallyback = (from item in jewelPool where (item.x == i) && (item.y==j-2) select item).First();
                        jreallyback.destroyable = true;
                    } else if (consecutiveJewels > 2){
                        jb.destroyable = true;
                    }
                    
                    previousType = jb.type;
                }                   
            }

            for(int j = 0; j < jewelPoolY; j++){
                consecutiveJewels = 0;
                previousType = JewelButton.JewelType.Empty;
                for(int i = 0; i < jewelPoolX; i++){
                    //get item from jewelpool where item.x == i and item.y == j                   
                    JewelButton jb = GetJewelInCoordinates(i, j);
                    if(jb==null) break;                                        
                    if(jb.type==previousType){
                        consecutiveJewels++;
                    } else {
                        consecutiveJewels = 0;
                    }
                    
                    if(consecutiveJewels==2){
                        wereAnyObjectsTagged = true;
                        jb.destroyable = true;
                        JewelButton jback = (from item in jewelPool where (item.x == i-1) && (item.y==j) select item).First();
                        jback.destroyable = true;
                        JewelButton jreallyback = (from item in jewelPool where (item.x == i-2) && (item.y==j) select item).First();
                        jreallyback.destroyable = true;
                    } else if (consecutiveJewels > 2){
                        jb.destroyable = true;
                    }
                    
                    previousType = jb.type;
                }                   
            }

            return wereAnyObjectsTagged;
        }
        
        JewelButton GetJewelButtonFromGameObject(GameObject go){
            //jewelbuttons might sometimes return the text of the second selected little damned object (i think i should just return the jewelbutton itself in the firstgo tho)
            if(go!=null){
                if(go.name=="Text"){
                    return go.transform.parent.GetComponent<JewelButton>();
                } else {
                    JewelButton jb = go.GetComponent<JewelButton>();
                    if(jb==null){
                        return null;                     
                    } else {
                        return jb;
                    }
                }                
            }
            return null;        
        }
        
        public void SwitchJewels(GameObject firstJewel, GameObject secondJewel){
            StartCoroutine(JewelSwitcher(firstJewel, secondJewel));
        }
        
        IEnumerator JewelSwitcher(GameObject firstJewel, GameObject secondJewel){
            //get jewel components            
            JewelButton fj = GetJewelButtonFromGameObject(firstJewel);            
            JewelButton sj = GetJewelButtonFromGameObject(secondJewel);
            bool areSiblings = false;            
            //check if jewels are sibling to each other
            if((sj!=null)&&(fj!=null)){
                if ((sj.x == fj.x + 1) && (sj.y == fj.y)) areSiblings = true;
                if ((sj.x == fj.x - 1) && (sj.y == fj.y)) areSiblings = true;
                if ((sj.x == fj.x) && (sj.y == fj.y + 1)) areSiblings = true;
                if ((sj.x == fj.x) && (sj.y == fj.y - 1)) areSiblings = true;           
                
                if(areSiblings){
                    Debug.Log("Jewels are direct siblings");
                    //animate switch
                    AnimatedJewelSwitch(fj.gameObject, sj.gameObject); //already gotten their correct components, why not?
                    SwitchValues(fj, sj);
                    yield return new WaitForSeconds(0.3f);
                    //switch values
                    //check for validity
                    bool jewelsWillBeDestroyed = VerifyValues();
                    Debug.Log(jewelsWillBeDestroyed);
                    if(jewelsWillBeDestroyed){
                        //leave as it is and destroy the destroyables
                        foreach(JewelButton jb in jewelPool){
                            if(jb.destroyable && jb!=null){
                                jb.Explode();
                                score++;
                            }
                        }
                        DropJewels();                        
                    } else {
                        SwitchValues(fj, sj);                    
                        //if not valid switch back and animate back
                        yield return new WaitForSeconds(0.4f);        
                        AnimatedJewelSwitch(fj.gameObject, sj.gameObject); //already gotten their correct components, why not? //yes i copied this 
                    }                    
                } else { 
                    Debug.Log ("not siblings");
                }                
            } else {
                Debug.Log ("clicked not in jewel");
            }
        }
        
        void AnimatedJewelSwitch(GameObject firstJewel, GameObject secondJewel){
            float time = 0.3f;
            Vector3 firstPos = firstJewel.transform.position;            
            Vector3 secondPos = secondJewel.transform.position;            
            firstJewel.transform.DOMove(secondPos, time, true); 
            secondJewel.transform.DOMove(firstPos, time, true); 
        }
        
        void SwitchValues(JewelButton first,JewelButton second){                       
            int auxx, auxy;            
            auxy = first.y;
            auxx = first.x;
            first.x = second.x;
            first.y = second.y;
            second.x = auxx;
            second.y = auxy;            
        }
        
        List<JewelButton> droppableJewels = new List<JewelButton>();
        void DropNewJewels(){        
            //after finish
            foreach(JewelButton jb in droppableJewels){                
                jewelPool.Add(jb);
            }
        }
        
        void DropJewels(){
            /*for(int i = jewelPool.Count() - 1; i >= 0; i--){                
                if(jewelPool[i].destroyable){
                    Debug.Log(i);
                    DropAbove(jewelPool[i]);  
                }     
            }
            for(int i = 0; i < jewelPoolX; i++){
                VerifyForEmptySpaceInRow(i);
            }            
            */
            
            foreach(JewelButton jb in jewelPool){
                jb.UpdatePosition();
            }
            
        }
        
        void DropAbove(JewelButton jb){
            for(int i = jb.x; i >= 0; i--){
                JewelButton job = GetJewelInCoordinates(i, jb.y);
                if(job!=null) job.Drop();    
            }            
        }
        
        void ResetDestroyables(){
            foreach(JewelButton jb in jewelPool){
                jb.destroyable = false;
            }
        }
        
        public JewelButton GetJewelInCoordinates(int x, int y){ 
            if((x>=0)&&(y>=0)){
                var obj = (from item in jewelPool where ((item.x==x)&&(item.y==y)) select item);
                foreach(JewelButton o in obj){
                    if(o!=null){
                        return o;
                    } else {
                        return null;
                    }                    
                    
                }                
            } 
            return null;       
        }
        
        int holeCount = 0;
        void VerifyForEmptySpaceInRow(int x){
            var items = from item in jewelPool where (item.x == x) select item;
            int index = 0;
            foreach (JewelButton button in items){
                if(button.y!=index){
                    Debug.LogWarning("empty: " + button.y + "," + index);
                    JewelButton jb = GetJewelInCoordinates(x, index-1);
                    if(jb!=null) jb.Drop();
                    holeCount++;
                }
                index++; //forgive me for this
            }
            
        }
        
        void Update(){
            if(Input.GetButton("Jump")){
                DropJewels();
            }
        }

    }
}