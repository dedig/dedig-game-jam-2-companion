using UnityEngine;
using System.Collections;

public class RequestMobile : MonoBehaviour {


	public static string ip;
	public static bool endGame;
	private string alvo;
	
	void Start () {
		endGame = false;
		StartCoroutine(connect ());
	}

	public IEnumerator SendMinion(string r){
		WWWForm form = new WWWForm();
		form.AddField("minion",r);
		WWW www = new WWW (ip+":46370/minion",form); //server
		yield return www;
	}

    IEnumerator connect()
	{
		WWWForm form = new WWWForm();
		form.AddField("Player","Mobile");
		WWW www = new WWW (ip+":46370/connect",form); //server
		yield return www;
		StartCoroutine(requestInfo ());
	}
	
	
	IEnumerator requestInfo(){
		while (true) {
			alvo = "mobile";
			yield return new WaitForSeconds (0.35f);
			WWWForm form = new WWWForm();
			form.AddField("Jogador","Mobile");
			WWW www = new WWW (ip+":46370/"+alvo,form); //server
			yield return www;
			string reciever = www.text;
			if(reciever!=""){
				if(reciever == "shake"){
					//objeto.GetComponent<>().shake();
				}
				if(reciever== "endGame"){
					endGame=true;
					//objeto.GetComponent<>().callback();
				}
			}
			
		}
	}
	
}
