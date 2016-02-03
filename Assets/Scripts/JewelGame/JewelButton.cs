using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

namespace Seni.Game{
    public class JewelButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
        
        public enum JewelType{
            Red,
            Green,
            Blue,
            Alpha, //yes alpha is a color havent you heard of rgba colorspace you weirdo
            Empty
        }
        
        public JewelType type;
        public int x, y;
        public bool destroyable = false;
        
        GameObject firstGo;
        public void OnBeginDrag(PointerEventData eventData){            
            firstGo = eventData.pointerPress;                                 
        }

        public void OnEndDrag(PointerEventData eventData){            
            JewelManager.sharedInstance.SwitchJewels(firstGo, eventData.pointerCurrentRaycast.gameObject);
        }
        
        public void OnDrag(PointerEventData eventData){} //this call needs to exist so that begin and end handlers work properly
        
        public void SetMyType(JewelType jt){
            Image i = GetComponent<Image>();
            switch (jt){
                case JewelType.Red:
                    i.color = Color.red;;
                    break;
                case JewelType.Green:
                    i.color = Color.green;
                    break;
                case JewelType.Blue:
                    i.color = Color.blue;
                    break;
                default: //alpha
                    i.color = Color.yellow;
                    break;                    
            }
            type = jt;
        }
        
        public void SetIndex(int x, int y){
            this.x = x;
            this.y = y;         
        }
        
        public void Drop(){
            x++; 
            transform.DOLocalMoveY(transform.localPosition.y - 95, 0.3f, true);
        }
        
        public void UpdatePosition(){
            if(y <= JewelManager.jewelPoolX - 1){
                JewelButton lowerJB = JewelManager.sharedInstance.GetJewelInCoordinates(x+1,y);
                if(lowerJB==null){
                    Drop();
                }                
            }
        }
        
        #region explosion
        
        
        public void Explode(){
            if(destroyable){
                Image i = GetComponent<Image>();
                i.color = Color.clear;                                
            }
        }
        #endregion

    }   
}