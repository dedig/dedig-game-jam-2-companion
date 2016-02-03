using Seni.Web;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Seni.UI{
   
    public class MainMenuActions : MonoBehaviour{
        
        public InputField ipInput;
        public GameObject connectorGameObject, connectionWindowGameObject, endGameScreen, connectMenu, gameSpace;
        IConnector connector;
        UIConnectorStart connectionWindow;
        
        
        void Start(){
            connector = connectorGameObject.GetComponent<IConnector>();
            connectionWindow = connectionWindowGameObject.GetComponent<UIConnectorStart>();
        }
        
        public void Connect(){           
            if(!string.IsNullOrEmpty(ipInput.text)){
                connectionWindow.StartConnection();
                connector.Connect(ipInput.text);
            }
        }
        
        public void GoBackToMainMenu(){
            CanvasGroup cg = connectMenu.transform.GetComponent<CanvasGroup>();
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true; 
            endGameScreen.SetActive(false);
            gameSpace.SetActive(false);   
        }
                
    }
}