using Seni.UI;
using Seni.Game;
using UnityEngine;
using System.Collections;
using System;

namespace Seni.Web{
    public class ServerManager : MonoBehaviour, IConnector {
        
        public static ServerManager sharedInstance; 
                
        public GameObject responseWindowGameObject;
        UIConnectionResponse responseWindow;       
        RequestMobile rm;
        
        void Awake(){
            sharedInstance = this;
        }
        
        void Start(){
            if(responseWindowGameObject!=null){
                responseWindow = responseWindowGameObject.GetComponent<UIConnectionResponse>();
            }
        }                 
                
        public void Connect(string ipToConnect){
            //game connection routine            
            Debug.Log(ipToConnect);
            RequestMobile.ip = "http://" + ipToConnect;            
            rm = gameObject.AddComponent<RequestMobile>();                        
            responseWindow.Connected();
        }
        
        public void SendMinion(int minionCode){
            StartCoroutine(rm.SendMinion((minionCode-1).ToString()));
        }
        
        void Update(){
            if((rm!=null)&&(RequestMobile.endGame==true)){
                BallGameManager.sharedInstance.EndGame();
                StopGame();         
            }
        }
        
        public void StopGame(){            
            Destroy(rm);
            rm = null;
            RequestMobile.endGame = false;
        }

    }
}