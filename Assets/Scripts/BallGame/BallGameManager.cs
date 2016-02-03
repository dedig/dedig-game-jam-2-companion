using Seni.Web;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Seni.Game{
    public class BallGameManager : MonoBehaviour {
        
        public static BallGameManager sharedInstance;
        
        public Sprite[] tiposDeBotao;
        public BallGameButton ballButton;
        public Text text, timerText; //text is for score
        public Button[] invokableButtons;
        public GameObject endGameScreen;       

        int currentBall = 1;
        int score = 0;
        
        bool isTimerRunning;
        float timeRemaining;
            
        const int upperTimeLimit = 20;
        const int lowerTimeLimit = 5;
        
        const int upperItemLimit = 20;
        const int lowerItemLimit = 5;
        
        public static int[] minionPrices = { 30, 90, 140, 120 };             
        
        void Awake(){
            sharedInstance = this;
        }
        
        void Start(){
            StartGame();
        } 
        
        void ShakeGame(){
            
        }    
        
        void RestartGame(){
            foreach(Transform child in transform){
                Destroy(child.gameObject);
            }
            BallGenerator();
        }
        
        public void StartGame(){
            BallGenerator();
            VerifyIfMinionsAreInvokable();
        }
        
        public void EndGame(){
            foreach(Transform child in transform){
                Destroy(child.gameObject);
            }
            //reset values
            currentBall = 1;
            score = 0;
            isTimerRunning = false;
            timeRemaining = 0;
            endGameScreen.SetActive(true);
            Soundboard.s.PlaySound("End"); 
            ServerManager.sharedInstance.StopGame();                       
        }
        
        int itemQuantity;
        public void BallClick(int clickedBall){
            Debug.Log(clickedBall);
            if(clickedBall==currentBall){
                //nice
                currentBall++;
                score += 10;
                if(clickedBall==itemQuantity){
                    score+=100;
                    RestartGame();
                }
            } else {
                RestartGame();
                //shit man
            }        
            text.text = score.ToString();   
            VerifyIfMinionsAreInvokable();    
        }
            
        void BallGenerator(){
            System.Random rand = new System.Random();
            itemQuantity = rand.Next(lowerItemLimit, upperItemLimit);
            for(int i = 0; i < itemQuantity; i++){
                BallGameButton bgb =  (BallGameButton) Instantiate (ballButton);
                bgb.transform.SetParent(this.transform);
                bgb.transform.localScale = Vector3.one;
                bgb.transform.localPosition = Vector3.zero;
                bgb.GenerateBall(itemQuantity - i, GetRandomPositionForBall(), RandomizeSprite()); 
            }
            currentBall = 1;
            isTimerRunning = true;
            timeRemaining = RandomizeTime();
        }
        
        Sprite RandomizeSprite(){
            System.Random rand = new System.Random();
            return tiposDeBotao[rand.Next(0, tiposDeBotao.Length)];
        }
        
        float RandomizeTime(){
            System.Random rand = new System.Random();
            return (float) rand.Next(lowerTimeLimit, upperTimeLimit);
        }
        
        void Update(){
            if(isTimerRunning){
                timeRemaining -= Time.deltaTime;
                timerText.text = ((int) timeRemaining).ToString();
                if(timeRemaining < 0){
                    RestartGame();
                }
            }
        }
        
        Vector3 lastRandomPosition = Vector3.zero;
        Vector3 GetRandomPositionForBall(){        
            int upperBoundary = 300;
            System.Random rand = new System.Random();
            //must not exceed boundaries
            Vector3 v3 = new Vector3(rand.Next(-upperBoundary, upperBoundary), rand.Next(-upperBoundary, upperBoundary), 0);
            //must not overlap
            while((v3+lastRandomPosition).magnitude<50){
                v3 = new Vector3(rand.Next(-upperBoundary, upperBoundary), rand.Next(-upperBoundary, upperBoundary), 0);
            }
            lastRandomPosition = v3;
            return v3;
        }
        
        public void SpawnMinion(int minionCode){
            //enviar pro sv como número
            switch(minionCode){
                case 1:
                    if(PayForMinion(minionPrices[minionCode-1])){
                        Soundboard.s.PlaySound("SwordBuy");
                        ServerManager.sharedInstance.SendMinion(minionCode);  
                    } 
                    break;
                case 2:
                    if(PayForMinion(minionPrices[minionCode-1])){
                        Soundboard.s.PlaySound("AxeBuy");
                        ServerManager.sharedInstance.SendMinion(minionCode);  
                    } 
                    break;
                case 3:
                    if(PayForMinion(minionPrices[minionCode-1])){
                        Soundboard.s.PlaySound("MageBuy");
                        ServerManager.sharedInstance.SendMinion(minionCode);  
                    } 
                    break;
                case 4:
                    if(PayForMinion(minionPrices[minionCode-1])){
                        Soundboard.s.PlaySound("HammerBuy");
                        ServerManager.sharedInstance.SendMinion(minionCode);  
                    } 
                    break;
                default:
                    if(PayForMinion(minionPrices[minionCode-1])){
                        Soundboard.s.PlaySound("GenericBuy");                        
                        ServerManager.sharedInstance.SendMinion(minionCode);  
                    } 
                    break;
            }
            VerifyIfMinionsAreInvokable();
        }
        
        bool PayForMinion(int minionPrice){
            Debug.Log(minionPrice);
            if(score - minionPrice >= 0){
                score -= minionPrice;
                text.text = score.ToString();                
                return true;    
            }
            return false;
        }
        
        void VerifyIfMinionsAreInvokable(){            
            for(int i = 1; i < 5; i++){
                Debug.Log(i);
                invokableButtons[i-1].interactable = isMinionAvailable(i);        
            }
        }
        
        bool isMinionAvailable(int minionCode){            
            return score - minionPrices[minionCode-1] >= 0;
        }

    }
}