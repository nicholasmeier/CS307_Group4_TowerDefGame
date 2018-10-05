using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TDTK;

namespace TDTK{

	public class UITowerSelect : UIScreen {
		
		public Text lbTowerName;
		public Text lbTowerDesp;
		
		[Space(5)]
		public bool allowTargetModeSwitch;
		public GameObject targetModeObj;
		public UIButton buttonTargetMode;
		
		[Space(5)]
		public bool allowTargetDirSwitch;
		public GameObject targetDirObj;
		public Slider sliderTargetDir;
		public Text lbTargetDir;
		
		[Space(5)]
		public List<UIButton> upgradeButtons=new List<UIButton>();
		
		//filled up during runtime, used for dragNdrop mode only
		//public List<UnitTower> buildableList=new List<UnitTower>();	
		
		private UnitTower sTower;
		
		public UIButton buttonSell;
		
		private static UITowerSelect instance;
		
		public override void Awake(){
			base.Awake();
			
			instance=this;
		}
		
		public override void Start(){ 
			base.Start();
			
			//~ Transform buttonParent=upgradeButtons[0].transform.parent;
			for(int i=0; i<3; i++){
				if(i>0) upgradeButtons.Add(UIButton.Clone(upgradeButtons[0].rootObj, "BuildButton"+(i+1)));
				
				upgradeButtons[i].Init();
				upgradeButtons[i].SetCallback(this.OnHoverUpgradeButton, this.OnExitUpgradeButton, this.OnUpgradeButton, null);
				
				upgradeButtons[i].rectT.SetSiblingIndex(i);
			}
			
			buttonSell.Init();
			buttonSell.SetCallback(this.OnHoverSellButton, this.OnExitSellButton, this.OnSellButton, null);
			
			buttonTargetMode.Init();
			buttonTargetMode.SetCallback(null, null, this.OnTargetModeButton, null);
			
			sliderTargetDir.onValueChanged.AddListener(delegate {TargetDirSliderValueChange(); });
			
			thisObj.SetActive(false);
		}
		
		
		public void OnHoverUpgradeButton(GameObject butObj){
			int idx=UI.GetItemIndex(butObj, upgradeButtons);
			UITooltip.ShowUpgrade(sTower, idx, UI.GetCorner(upgradeButtons[idx].rectT, 2), 0, new Vector3(0, .25f, 0));
		}
		public void OnExitUpgradeButton(GameObject butObj){
			UITooltip.Hide();
		}
		public void OnUpgradeButton(GameObject butObj, int pointerID=-1){
			if(!showing) return;
			int idx=UI.GetItemIndex(butObj, upgradeButtons);
			
			List<float> upgradeCost=sTower.GetUpgradeCost(idx);
			if(CheckCost(upgradeCost)){
				//RscManager.SpendRsc(upgradeCost);
				sTower.Upgrade(idx);
				SelectControl.ClearUnit();
				Hide();
			}
			
			UITooltip.Hide();
		}
		
		
		private bool CheckCost(List<float> cost){
			if(!RscManager.HasSufficientRsc(cost)){
				Debug.Log("Insufficient resources");
				return false;
			}
			return true;
		}
		
		
		public void OnHoverSellButton(GameObject butObj){
			UITooltip.ShowSell(sTower, UI.GetCorner(buttonSell.rectT, 2), 0, new Vector3(0, .25f, 0));
		}
		public void OnExitSellButton(GameObject butObj){
			UITooltip.Hide();
		}
		public void OnSellButton(GameObject butObj, int pointerID=-1){
			if(!showing) return;
			UITooltip.Hide();
			sTower.Sell();
			SelectControl.ClearUnit();
			Hide();
		}
		
		
		public void OnTargetModeButton(GameObject butObj, int pointerID=-1){
			sTower.CycleTargetMode();
			UpdateTargetModeDisplay();
		}
		public void UpdateTargetModeDisplay(){
			//public enum _TargetMode{ NearestToDestination, NearestToSelf, MostHP, LeastHP, Random, }
			if(sTower.targetMode==Unit._TargetMode.NearestToDestination) buttonTargetMode.lbMain.text="Nearest To Goal";
			else if(sTower.targetMode==Unit._TargetMode.NearestToSelf) 	buttonTargetMode.lbMain.text="Nearest";
			else if(sTower.targetMode==Unit._TargetMode.MostHP) 			buttonTargetMode.lbMain.text="Strongest";
			else if(sTower.targetMode==Unit._TargetMode.LeastHP) 			buttonTargetMode.lbMain.text="Weakest";
			else if(sTower.targetMode==Unit._TargetMode.Random) 			buttonTargetMode.lbMain.text="Random";
		}
		
		public void TargetDirSliderValueChange(){
			sTower.targetingDir=sliderTargetDir.value;
			lbTargetDir.text=sTower.targetingDir.ToString("f0");
			
			SelectControl.RefreshUnit();
		}
		
		
		void UpdateDisplay(){
			lbTowerName.text=sTower.unitName;
			lbTowerDesp.text=sTower.desp;
			
			int upgradeType=sTower.GetUpgradeType();	//0-to next level, 1-to next tower
			int upgradeCount=sTower.GetUpgradeOptionCount();
			
			for(int i=0; i<upgradeButtons.Count; i++){
				if(i<upgradeCount){
					if(upgradeType==0){
						upgradeButtons[i].lbMain.text=sTower.GetUpgradeCost()[0].ToString("f0");//"next level";
						upgradeButtons[i].imgMain.enabled=false;
						upgradeButtons[i].imgAlt.enabled=true;
					}
					else if(upgradeType==1){
						UnitTower nextTower=sTower.GetUpgradeTower(i);
						upgradeButtons[i].lbMain.text=nextTower.GetCost()[0].ToString("f0");
						upgradeButtons[i].imgMain.sprite=nextTower.icon;
						upgradeButtons[i].imgMain.enabled=true;
						upgradeButtons[i].imgAlt.enabled=false;
					}
					
					upgradeButtons[i].SetActive(true);
				}
				else upgradeButtons[i].SetActive(false);
			}
			
			buttonSell.lbMain.text=sTower.GetSellValue()[0].ToString("f0");
			buttonSell.SetActive(!sTower.disableSelling);
			
			targetModeObj.SetActive(sTower.IsTurret() && allowTargetModeSwitch);
			UpdateTargetModeDisplay();
			
			targetDirObj.SetActive(sTower.IsTurret() && sTower.UseDirectionalTargeting() && allowTargetDirSwitch);
			sliderTargetDir.value=sTower.targetingDir;
		}
		
		
		private bool showing=false;
		
		public static void Show(UnitTower tower, bool instant=false){ if(instance!=null) instance._Show(tower, instant); }
		public void _Show(UnitTower tower, bool instant=false){
			sTower=tower;
			
			UpdateDisplay();
			
			showing=true;
			
			//base._Show();	
			base._Show(thisObj.activeInHierarchy);
			//base._Show(instant);
		}
		public static void Hide(bool instant=false){
			if(instance!=null){ 
				instance.showing=false;
				if(instance.thisObj.activeInHierarchy) instance._Hide(instant);
			}
		}
		
	}

}
