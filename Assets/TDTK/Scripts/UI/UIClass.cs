using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



namespace TDTK{

	public delegate void Callback(GameObject uiObj);
	public delegate void CallbackInputDependent(GameObject uiObj, int pointerID);
	//public delegate void CallbackEventData(GameObject uiObj, PointerEventData eventData, int idx=0);
	
	#region UIObject
	[System.Serializable]
	public class UIObject{
		public GameObject rootObj;
		[HideInInspector] public Transform rootT;
		[HideInInspector] public RectTransform rectT;
		
		[HideInInspector] public CanvasGroup canvasG;
		
		//[HideInInspector] public Image imgBase;
		[HideInInspector] public Image imgMain;
		[HideInInspector] public Text lbMain;
		
		[HideInInspector] public UIItemCallback itemCallback;
		
		//[HideInInspector] public UIDropZone itemDropZone;
		//[HideInInspector] public UIDragNDrop itemDragNDrop;
		
		public UIObject(){}
		public UIObject(GameObject obj){ rootObj=obj; Init(); }
		
		public virtual void Init(){
			if(rootObj==null){ Debug.LogWarning("Unassgined rootObj"); return; }
			
			rootT=rootObj.transform;
			rectT=rootObj.GetComponent<RectTransform>();
			
			//imgBase=rootObj.GetComponent<Image>();
			
			foreach(Transform child in rectT){
				if(child.name=="Image") imgMain=child.GetComponent<Image>();
				else if(child.name=="Text") lbMain=child.GetComponent<Text>();
			}
		}
		
		public static UIObject Clone(GameObject srcObj, string name="", Vector3 posOffset=default(Vector3)){
			GameObject newObj=UI.Clone(srcObj, name, posOffset);
			return new UIObject(newObj);
		}
		
		public virtual void SetCallback(Callback enter=null, Callback exit=null, CallbackInputDependent down=null, CallbackInputDependent up=null){
			itemCallback=rootObj.GetComponent<UIItemCallback>();
			if(itemCallback==null) itemCallback=rootObj.AddComponent<UIItemCallback>();
			itemCallback.SetEnterCallback(enter);
			itemCallback.SetExitCallback(exit);
			itemCallback.SetDownCallback(down);
			itemCallback.SetUpCallback(up);
		}
		
		#region dragNdrop
		/*
		public void SetDropZone(CallbackEventData callback=null, CheckCallback enterCheckCB=null){
			itemDropZone=rootObj.GetComponent<UIDropZone>();
			if(itemDropZone==null) itemDropZone=rootObj.AddComponent<UIDropZone>();
			itemDropZone.SetDropCallback(callback);
			itemDropZone.SetEnterCheckCallback(enterCheckCB);
			
			foreach(Transform child in rectT){
				if(child.name=="HoverDummy"){
					itemDropZone.dummyT=child;
					child.gameObject.SetActive(false);
				}
				if(child.name=="HoverHighlight"){
					itemDropZone.hoverHighlight=child.gameObject;
					child.gameObject.SetActive(false);
				}
			}
		}
		public void SetDragNDrop(CallbackInputDependent start=null, CallbackInputDependent drag=null, CallbackEventData end=null, Transform parent=null){
			itemDragNDrop=rootObj.GetComponent<UIDragNDrop>();
			if(itemDragNDrop==null) itemDragNDrop=rootObj.AddComponent<UIDragNDrop>();
			itemDragNDrop.SetUIObj(this);
			itemDragNDrop.SetParentDrag(parent);
			itemDragNDrop.SetBeginCallback(start);
			itemDragNDrop.SetDragCallback(drag);
			itemDragNDrop.SetEndCallback(end);
			
			if(canvasG==null) canvasG=rootObj.AddComponent<CanvasGroup>();
		}
		*/
		#endregion
		
		public virtual void SetActive(bool flag){ rootObj.SetActive(flag); }
		
		public void SetSound(AudioClip eClip, AudioClip dClip){ itemCallback.SetSound(eClip, dClip); }
		
		public void DisableSound(bool disableHover, bool disablePress){ itemCallback.DisableSound(disableHover, disablePress); }
	}
	#endregion
	
	
	
	
	
	#region UIButton
	[System.Serializable]
	public class UIButton : UIObject{
		
		[HideInInspector] public Text lbAlt;
		[HideInInspector] public Text lbAlt2;
		
		[HideInInspector] public Image imgAlt;
		
		[HideInInspector] public Image imgHovered;
		[HideInInspector] public Image imgDisabled;
		[HideInInspector] public Image imgHighlight;
		
		[HideInInspector] public Button button;
		
		public UIButton(){}
		public UIButton(GameObject obj){ rootObj=obj; Init(); }
		
		public override void Init(){
			base.Init();
			
			button=rootObj.GetComponent<Button>();
			canvasG=rootObj.GetComponent<CanvasGroup>();
			
			foreach(Transform child in rectT){
				if(child.name=="TextAlt")				lbAlt=child.GetComponent<Text>();
				else if(child.name=="TextAlt2")		lbAlt2=child.GetComponent<Text>();
				else if(child.name=="ImageAlt")	imgAlt=child.GetComponent<Image>();
				else if(child.name=="Hovered") 		imgHovered=child.GetComponent<Image>();
				else if(child.name=="Disabled") 	imgDisabled=child.GetComponent<Image>();
				else if(child.name=="Highlight") 	imgHighlight=child.GetComponent<Image>();
			}
		}
		
		public static new UIButton Clone(GameObject srcObj, string name="", Vector3 posOffset=default(Vector3)){
			GameObject newObj=UI.Clone(srcObj, name, posOffset);
			return new UIButton(newObj);
		}
		
		public override void SetCallback(Callback enter=null, Callback exit=null, CallbackInputDependent down=null, CallbackInputDependent up=null){
			base.SetCallback(enter, exit, down, up);
			itemCallback.SetButton(button);
		}
		
		public override void SetActive(bool flag){
			//~ if(flag && imgHover!=null) imgHover.enabled=false;
			//~ if(flag && imgDisabled!=null) imgDisabled.enabled=false;
			base.SetActive(flag);
		}
		
	}
	#endregion
	
	
	#region callback
	public class UIItemCallback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler{
		private bool useCustomClip=false;
		public AudioClip enterClip;
		public AudioClip downClip;
		
		//private Button button;
		
		private Callback enterCB;
		private Callback exitCB;
		private CallbackInputDependent downCB;
		private CallbackInputDependent upCB;
		
		public void SetButton(Button but){}// button=but; }
		public void SetEnterCallback(Callback callback){ enterCB=callback; }
		public void SetExitCallback(Callback callback){ exitCB=callback; }
		public void SetDownCallback(CallbackInputDependent callback){ downCB=callback; }
		public void SetUpCallback(CallbackInputDependent callback){ upCB=callback; }
		
		public void OnPointerEnter(PointerEventData eventData){ 
			//if(enterClip!=null && button!=null && button.interactable) AudioManager.PlayUISound(enterClip);
			if(enterCB!=null) enterCB(thisObj);
		}
		public void OnPointerExit(PointerEventData eventData){ 
			if(exitCB!=null) exitCB(thisObj);
		}
		public void OnPointerDown(PointerEventData eventData){ 
			//if(downClip!=null && button!=null && button.interactable) AudioManager.PlayUISound(downClip);
			if(downCB!=null) downCB(thisObj, eventData.pointerId);
		}
		public void OnPointerUp(PointerEventData eventData){ 
			if(upCB!=null) upCB(thisObj, eventData.pointerId);
		}
		
		private GameObject thisObj;
		void Awake(){
			thisObj=gameObject;
			SetupAudioClip();
		}
		
		void SetupAudioClip(){
			if(useCustomClip) return;
			//enterClip=AudioManager.GetHoverButtonSound();
			//downClip=AudioManager.GetPressButtonSound();
		}
		
		public void SetSound(AudioClip eClip, AudioClip dClip){
			useCustomClip=true;	enterClip=eClip;	downClip=dClip;
		}
		
		public void DisableSound(bool disableHover, bool disablePress){
			if(disableHover) enterClip=null;
			if(disablePress) downClip=null;
		}
	}
	#endregion
	
}
