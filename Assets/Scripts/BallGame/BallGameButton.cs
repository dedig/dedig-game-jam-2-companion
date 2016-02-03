using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Seni.Game{
    [RequireComponent(typeof(CanvasGroup))]
    public class BallGameButton : MonoBehaviour {

        float time;
        int ballNumber;
        CanvasGroup cg;
        Text text;
        
        void Start(){
            cg = GetComponent<CanvasGroup>();            
        }
            
        public void GenerateBall(int number, Vector3 position, Sprite mySprite){            
            text = transform.GetChild(0).GetComponent<Text>();        
            GetComponent<Image>().sprite = mySprite;
            GetComponent<RectTransform>().anchoredPosition = position;        
            ballNumber = number;        
            text.text = number.ToString();                            
        }
        
        public void OnClick(){
            BallGameManager.sharedInstance.BallClick(ballNumber);
            transform.DOScale(Vector3.zero, 0.2f);
            cg.DOFade(0,0.2f);
            cg.interactable = false;
            cg.blocksRaycasts = false;
            if(Random.value > 0.5){
                Soundboard.s.PlaySound("ButtonClick");
            } else {
                Soundboard.s.PlaySound("ButtonClick2");
            }
        }

    }

}