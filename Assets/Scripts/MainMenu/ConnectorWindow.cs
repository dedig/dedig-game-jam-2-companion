using UnityEngine;
using System.Collections;
using System;

namespace Seni.UI{
    [RequireComponent(typeof(CanvasGroup))]
    public class ConnectorWindow : MonoBehaviour, UIConnectorStart, UIConnectionResponse{
        
        public GameObject mainMenu, game;        
        CanvasGroup cg;
        
        void Start(){
            cg = gameObject.GetComponent<CanvasGroup>();
            cg.blocksRaycasts = false;
            cg.interactable = false;
            cg.alpha = 0;
        }
        
        public void StartConnection(){
            //open the window and wait for response
            CanvasGroup cg = mainMenu.GetComponent<CanvasGroup>();
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;            
            OpenWindow();           
        }
        
        public void Connected(){
            CloseWindow();
            game.SetActive(true);            
            //open the game 
        }
        
        public void ConnectionFail(){
            CloseWindow();
            mainMenu.SetActive(true);            
            //close the window
            //go back to ip enter field
        }
                
        void OpenWindow(){
            //animate window opening            
            cg.alpha = 1;
        }
        
        void CloseWindow(){
            //animate window closing
            cg.alpha = 0;
        }
    }
}