using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//using UnityStandardAssets.ImageEffects;

namespace TDTK{

	public class UI : MonoBehaviour {
		
		public static UI instance;
		
		public static void Init(){
			if(instance!=null) return;
			
			GameObject obj=new GameObject("UI_Utility");
			instance=obj.AddComponent<UI>();
		}
		
		
		public static float GetScaleFactor(){ return UIControl.GetScaleReferenceWidth()/Screen.width; }
		
		
		//inputID=-1 - mouse cursor, 	inputID>=0 - touch finger index
		public static bool IsCursorOnUI(int inputID=-1){
			EventSystem eventSystem = EventSystem.current;
			return ( eventSystem.IsPointerOverGameObject( inputID ) );
		}
		
		public static GameObject Clone(GameObject srcObj, string name="", Vector3 posOffset=default(Vector3)) {
			GameObject newObj=(GameObject)MonoBehaviour.Instantiate(srcObj);
			newObj.name=name=="" ? srcObj.name : name ;
			
			newObj.transform.SetParent(srcObj.transform.parent);
			newObj.transform.localPosition=srcObj.transform.localPosition+posOffset;
			newObj.transform.localScale=srcObj.transform.localScale;
			//newObj.transform.localScale=new Vector3(1, 1, 1);
			
			return newObj;
		}
		
		
		//0 - bottom left
		//1 - top left
		//2 - top right
		//3 - bottom right
		public static Vector3 GetCorner(RectTransform rectT, int corner=0){
			Vector3[] fourCornersArray=new Vector3[4];
			rectT.GetWorldCorners(fourCornersArray);
			return fourCornersArray[corner];
		}
		
		
		
		public static string IntToString(int val){
			if(val<1000) return val.ToString();
				
			string t1="";
			int val1=val%1000;
			if(val1<10) t1="00"+val1.ToString();
			else if(val1<100) t1="0"+val1.ToString();
			else t1=val1.ToString();
			
			string t2=(val/1000).ToString();
			return t2+","+t1;
		}
		
		
		public static string GetColor1String(){ return "<color=#ff9600ff>"; }		//255, 150, 0		</color>
		public static string GetColor2String(){ return "<color=#ffC864ff>"; }		//255, 200, 100
		public static string GetColor3String(){ return "<color=#ffe1afff>"; }		//255, 225, 175
		public static string GetColor4String(){ return "<color=#ff3232ff>"; }		//255, 64, 64
		
		public static string TextWColor1(string txt){ return GetColor1String()+txt+"</color>"; }
		public static string TextWColor2(string txt){ return GetColor2String()+txt+"</color>"; }
		public static string TextWColor3(string txt){ return GetColor3String()+txt+"</color>"; }
		public static string TextWColor4(string txt){ return GetColor4String()+txt+"</color>"; }
		
		public static string HighlightText(string txt){ return "<B><I>"+txt+"</I></B>"; }
		public static string HighlightTextColor1(string txt){ return "<B><I>"+TextWColor1(txt)+"</I></B>"; }
		public static string HighlightTextColor2(string txt){ return "<B><I>"+TextWColor2(txt)+"</I></B>"; }
		public static string HighlightTextColor3(string txt){ return "<B><I>"+TextWColor3(txt)+"</I></B>"; }
		public static string HighlightTextColor4(string txt){ return "<B><I>"+TextWColor4(txt)+"</I></B>"; }
		
		
		public static int GetItemIndex(GameObject uiObj, List<UIObject> objList){
			for(int i=0; i<objList.Count; i++){ if(objList[i].rootObj==uiObj) return i;}
			return 0;
		}
		public static int GetItemIndex(GameObject uiObj, List<UIButton> objList){
			for(int i=0; i<objList.Count; i++){ if(objList[i].rootObj==uiObj) return i;}
			return 0;
		}
		
		
		//public static void Message(string msg){ UIMessage.DisplayMessage(msg); }
		
		
		
		public static IEnumerator WaitForRealSeconds(float time){
			Init();	float start = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup < start + time) yield return null;
		}
		
		
		#region canvasgroup fade
		public static void FadeOut(CanvasGroup canvasGroup, float duration=0.25f, GameObject obj=null){
			Init();	instance.StartCoroutine(instance._Fade(canvasGroup, 1f/duration, 1, 0, obj));
		}
		public static void FadeIn(CanvasGroup canvasGroup, float duration=0.25f, GameObject obj=null){ 
			Init();	instance.StartCoroutine(instance._Fade(canvasGroup, 1f/duration, 0, 1, obj)); 
		}
		public static void Fade(CanvasGroup canvasGroup, float duration=0.25f, float startValue=0.5f, float endValue=0.5f){ 
			Init();	instance.StartCoroutine(instance._Fade(canvasGroup, 1f/duration, startValue, endValue));
		}
		IEnumerator _Fade(CanvasGroup canvasGroup, float timeMul, float startValue, float endValue, GameObject obj=null){
			if(endValue>0 && obj!=null) obj.SetActive(true);
			
			float duration=0;
			while(duration<1){
				canvasGroup.alpha=Mathf.Lerp(startValue, endValue, duration);
				duration+=Time.unscaledDeltaTime*timeMul;
				yield return null;
			}
			canvasGroup.alpha=endValue;
			
			if(endValue<=0 && obj!=null) obj.SetActive(false);
		}
		#endregion
		
		
		#region blur
		/*
		public static void FadeBlur(UnityStandardAssets.ImageEffects.BlurOptimized blurEff, float startValue=0, float targetValue=0){
			Init();	if(blurEff==null || instance==null) return;
			instance.StartCoroutine(instance.FadeBlurRoutine(blurEff, startValue, targetValue));
		}
		//change the blur component blur size from startValue to targetValue over 0.25 second
		IEnumerator FadeBlurRoutine(UnityStandardAssets.ImageEffects.BlurOptimized blurEff, float startValue=0, float targetValue=0){
			blurEff.enabled=true;
			
			float duration=0;
			while(duration<1){
				float value=Mathf.Lerp(startValue, targetValue, duration);
				blurEff.blurSize=value;
				duration+=Time.unscaledDeltaTime*4f;	//multiply by 4 so it only take 1/4 of a second
				yield return null;
			}
			blurEff.blurSize=targetValue;
			
			if(targetValue==0) blurEff.enabled=false;
			if(targetValue==1) blurEff.enabled=true;
		}
		*/
		#endregion
		
	}

}