using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TDTK;

namespace TDTK{

	public class UIAbilityButton : UIScreen {
		
		public List<UIButton> abilityButtons=new List<UIButton>();
		private List<Slider> cooldownSlider=new List<Slider>();
		
		public UIButton buttonClearSelect;
		
		private static UIAbilityButton instance;
		
		public override void Awake(){
			base.Awake();
			
			instance=this;
		}
		
		public override void Start(){ 
			base.Start();
			
			if(!AbilityManager.IsEnabled()){
				thisObj.SetActive(false);
				return;
			}
			
			List<Ability> list=AbilityManager.GetAbilityList();
			for(int i=0; i<list.Count; i++) AddAbilityButton(i, list[i].icon, list[i].GetUseLimitText());	//list[i].name);
			
			buttonClearSelect.Init();
			buttonClearSelect.SetCallback(null, null, this.OnClearSelectButton, null);
			buttonClearSelect.SetActive(false);
			
			canvasGroup.alpha=1;
		}
		
		public static void NewAbility(Ability ability){
			instance.AddAbilityButton(instance.abilityButtons.Count, ability.icon, ability.GetUseLimitText());
		}
		
		private void AddAbilityButton(int idx, Sprite icon, string txt){
			if(idx>0) abilityButtons.Add(UIButton.Clone(abilityButtons[0].rootObj, "Button"+(idx)));
			abilityButtons[idx].Init();
			
			abilityButtons[idx].SetCallback(this.OnHoverAbilityButton, this.OnExitAbilityButton, this.OnAbilityButton, null);
			
			if(icon!=null) abilityButtons[idx].imgMain.sprite=icon;
			abilityButtons[idx].lbMain.text=txt;
			
			cooldownSlider.Add(abilityButtons[idx].rootT.GetChild(0).gameObject.GetComponent<Slider>());
		}
		
		
		public void OnHoverAbilityButton(GameObject butObj){
			int idx=UI.GetItemIndex(butObj, abilityButtons);
			UITooltip.Show(AbilityManager.GetAbility(idx), UI.GetCorner(abilityButtons[idx].rectT, 2), 3, new Vector3(0, .25f, 0));
		}
		public void OnExitAbilityButton(GameObject butObj){
			int idx=UI.GetItemIndex(butObj, abilityButtons);
			if(AbilityManager.GetPendingTargetAbilityIndex()==idx || UIControl.GetPendingAbilityIdx()==idx) return;
			UITooltip.Hide();
		}
		
		private int touchModeButtonIdx=-1;
		public void OnAbilityButton(GameObject butObj, int pointerID=-1){
			int idx=UI.GetItemIndex(butObj, abilityButtons);
			
			
			if(pendingTgtSelectIdx>=0){
				abilityButtons[pendingTgtSelectIdx].imgHighlight.gameObject.SetActive(false);
				pendingTgtSelectIdx=-1;
			}
			
			if(UIControl.InTargetSelectionMode()){
				if(AbilityManager.GetPendingTargetAbilityIndex()==idx || UIControl.GetPendingAbilityIdx()==idx){
					abilityButtons[idx].imgHighlight.gameObject.SetActive(false);
					UIControl.ClearSelectedAbility();
					buttonClearSelect.SetActive(false);
					return;
				}
			}
			
			Ability._Status status=AbilityManager.IsReady(idx);
			
			if(status!=Ability._Status.Ready){
				if(status==Ability._Status.OnCooldown) GameControl.InvalidAction("Ability is on cooldown");
				if(status==Ability._Status.InsufficientRsc) GameControl.InvalidAction("Insufficient resource");
				if(status==Ability._Status.UseLimitReached) GameControl.InvalidAction("Use limit exceeded");
				return;
			}
			
			if(UIControl.InTouchMode()){
				if(!AbilityManager.RequireTargetSelection(idx) && touchModeButtonIdx!=idx){
					if(touchModeButtonIdx>=0) ClearTouchModeSelect();
					
					touchModeButtonIdx=idx;
					abilityButtons[touchModeButtonIdx].imgHighlight.gameObject.SetActive(true);
					OnHoverAbilityButton(butObj);
					buttonClearSelect.SetActive(true);
					return;
				}
				
				ClearTouchModeSelect();
			}
			
			if(AbilityManager.RequireTargetSelection(idx)){
				UIControl.SelectAbility(idx);
				abilityButtons[idx].imgHighlight.gameObject.SetActive(true);
				buttonClearSelect.SetActive(true);
				
				pendingTgtSelectIdx=idx;
			}
			else{
				AbilityManager.ActivateAbility(idx);
				abilityButtons[idx].lbMain.text=AbilityManager.GetAbility(idx).GetUseLimitText();
			}
		}
		
		private int pendingTgtSelectIdx=-1;
		
		//public static bool PendingTouchModeInput(){ return instance.touchModeButtonIdx>=0; }
		
		public static void ClearTouchModeSelect(){ instance._ClearTouchModeSelect(); }
		public void _ClearTouchModeSelect(){
			if(touchModeButtonIdx<0) return;
			abilityButtons[touchModeButtonIdx].imgHighlight.gameObject.SetActive(false);
			touchModeButtonIdx=-1;
			OnExitAbilityButton(null);
		}
		
		
		void OnEnable(){
			TDTK.onActivateAbilityE += OnActivateAbility;
		}
		void OnDisable(){
			TDTK.onActivateAbilityE -= OnActivateAbility;
		}
		
		
		void Update(){
			List<Ability> list=AbilityManager.GetAbilityList();
			for(int i=0; i<list.Count; i++){
				if(!list[i].OnCooldown()){
					if(cooldownSlider[i].value<=1) cooldownSlider[i].value=1;
					if(abilityButtons[i].imgAlt.enabled && !list[i].UseLimitReached()) abilityButtons[i].imgAlt.enabled=false;
				}
				else cooldownSlider[i].value=list[i].GetCDRatio();
			}
		}
		
		
		void OnActivateAbility(Ability ability){
			abilityButtons[ability.instanceID].lbMain.text=ability.GetUseLimitText();
			abilityButtons[ability.instanceID].imgAlt.enabled=true;
			abilityButtons[ability.instanceID].imgHighlight.gameObject.SetActive(false);
			
			UITooltip.Hide();
			buttonClearSelect.SetActive(false);
			
			pendingTgtSelectIdx=-1;
		}
		
		
		
		public static void ClearSelect(){ instance.OnClearSelectButton(null); }
		
		public void OnClearSelectButton(GameObject butObj, int pointerID=-1){
			if(!AbilityManager.IsEnabled()) return;
			
			if(touchModeButtonIdx>=0) ClearTouchModeSelect();
			else{
				UIControl.ClearSelectedAbility();
				for(int i=0; i<abilityButtons.Count; i++){
					abilityButtons[i].imgHighlight.gameObject.SetActive(false);
				}
			}
			
			buttonClearSelect.SetActive(false);
			
			UITooltip.Hide();
		}
		
		
		//public static void Show(SelectInfo info, bool instant=false){ if(instance!=null) instance._Show(info, instant); }
		//~ public void _Show(SelectInfo info, bool instant=false){
			//base._Show();
			////base._Show(instant);
		//}
		//public static void Hide(bool instant=false){ 
			//if(instance!=null && instance.thisObj.activeInHierarchy) instance._Hide(instant);
		//}
		
		
		
	}

}
