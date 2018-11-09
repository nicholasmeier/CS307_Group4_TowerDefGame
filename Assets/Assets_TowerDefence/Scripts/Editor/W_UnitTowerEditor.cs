using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace TDTK {
	
	public class UnitTowerEditorWindow : TDEditorWindow {
		
		[MenuItem ("Tools/TDTK/TowerEditor", false, 10)]
		static void OpenUnitTowerEditor () { Init(); }
		
		private static UnitTowerEditorWindow window;
		
		public static void Init (int prefabID=-1) {
			window = (UnitTowerEditorWindow)EditorWindow.GetWindow(typeof (UnitTowerEditorWindow), false, "TowerEditor");
			window.minSize=new Vector2(420, 300);
			
			TDE.Init();
			
			InitLabel();
			
			if(prefabID>=0) window.selectID=TowerDB.GetPrefabIndex(prefabID);
			
			window.SelectItem(window.selectID);
		}
		
		
		private static string[] towerTypeLabel;
		private static string[] towerTypeTooltip;
		
		private static string[] targetGroupLabel;
		private static string[] targetGroupTooltip;
		
		private static void InitLabel(){
			int enumLength = Enum.GetValues(typeof(UnitTower._TowerType)).Length;
			towerTypeLabel=new string[enumLength];
			towerTypeTooltip=new string[enumLength];
			for(int i=0; i<enumLength; i++){
				towerTypeLabel[i]=((UnitTower._TowerType)i).ToString();
				if((UnitTower._TowerType)i==UnitTower._TowerType.Turret) 	towerTypeTooltip[i]="Typical tower, attack creep directly by fire shootObject";
				if((UnitTower._TowerType)i==UnitTower._TowerType.AOE) 		towerTypeTooltip[i]="Apply its effect to all creep within it's area of effective";
				if((UnitTower._TowerType)i==UnitTower._TowerType.Support) 	towerTypeTooltip[i]="Buff friendly towers in range";
				if((UnitTower._TowerType)i==UnitTower._TowerType.Resource) towerTypeTooltip[i]="Generate resource ovetime";
				if((UnitTower._TowerType)i==UnitTower._TowerType.Mine) 		towerTypeTooltip[i]="Explode and apply aoe effects when a creep wanders into it's range\nDoesn't block up path when built on a walkable platform";
				if((UnitTower._TowerType)i==UnitTower._TowerType.Block) 		towerTypeTooltip[i]="Has no any particular function other than as a structure to block up path.";
			}
			
			enumLength = Enum.GetValues(typeof(UnitTower._TargetGroup)).Length;
			targetGroupLabel=new string[enumLength];
			targetGroupTooltip=new string[enumLength];
			for(int i=0; i<enumLength; i++){
				targetGroupLabel[i]=((UnitTower._TargetGroup)i).ToString();
				if((UnitTower._TargetGroup)i==UnitTower._TargetGroup.Ground) 	targetGroupTooltip[i]="Target ground units only";
				if((UnitTower._TargetGroup)i==UnitTower._TargetGroup.Air) 		targetGroupTooltip[i]="Target air units only";
				if((UnitTower._TargetGroup)i==UnitTower._TargetGroup.All) 		targetGroupTooltip[i]="Target both air and ground units";
			}
		}
		
		
		
		public void OnGUI(){
			TDE.InitGUIStyle();
			
			if(!CheckIsPlaying()) return;
			if(window==null) Init();
			
			
			List<UnitTower> towerList=TowerDB.GetList();
			selectID=Mathf.Clamp(selectID, 0, towerList.Count-1);
			
			
			Undo.RecordObject(this, "window");
			Undo.RecordObject(TowerDB.GetDB(), "towerDB");
			if(towerList.Count>0) Undo.RecordObject(towerList[selectID], "tower");
			
			
			if(GUI.Button(new Rect(Math.Max(260, window.position.width-120), 5, 100, 25), "Save")) TDE.SetDirty();
			
			UnitTower newTower=null;
			TDE.Label(5, 7, 150, 17, "Add New Tower:", "Drag tower prefab to this slot to add it to the list");
			newTower=(UnitTower)EditorGUI.ObjectField(new Rect(115, 7, 150, 17), newTower, typeof(UnitTower), false);
			if(newTower!=null) Select(NewItem(newTower));
			
			
			float startX=5;	float startY=55;
			
			if(minimiseList){ if(GUI.Button(new Rect(startX, startY-20, 30, 18), ">>")) minimiseList=false; }
			else{ if(GUI.Button(new Rect(startX, startY-20, 30, 18), "<<")) minimiseList=true; }
			
			Vector2 v2=DrawTowerList(startX, startY, towerList);
			startX=v2.x+25;
			
			if(towerList.Count==0) return;
			
			
			Rect visibleRect=new Rect(startX, startY, window.position.width-startX, window.position.height-startY);
			Rect contentRect=new Rect(startX, startY, contentWidth, contentHeight);
			
			scrollPos = GUI.BeginScrollView(visibleRect, scrollPos, contentRect);
			
				v2=DrawUnitConfigurator(startX, startY, towerList[selectID]);
				contentWidth=v2.x-startX;
				contentHeight=v2.y-55;
			
			GUI.EndScrollView();
			
			
			if(GUI.changed) TDE.SetDirty();
		}
		
		
		private float maxX=0;
		private bool showTypeDesp=false;
		private Vector2 DrawUnitConfigurator(float startX, float startY, UnitTower unit){
			
			startY=TDE.DrawBasicInfo(startX, startY, unit);
			
			TDE.Label(startX+12, startY, width, height, "Hide In Inspector:", "Check to hide the tower in Inspector (It won't be showing up in TowerManager)");
			unit.hideInInspector=EditorGUI.Toggle(new Rect(startX+spaceX+12, startY, width, height), unit.hideInInspector);
			
			int type=(int)unit.towerType;
			cont=new GUIContent("Tower Type:", "Type of the tower. Each type of tower serve a different function");
			contL=TDE.SetupContL(towerTypeLabel, towerTypeTooltip);
			EditorGUI.LabelField(new Rect(startX+12, startY+=spaceY, width, height), cont);
			type = EditorGUI.Popup(new Rect(startX+spaceX+12, startY, width, height), new GUIContent(""), type, contL);
			unit.towerType=(UnitTower._TowerType)type;
			
			showTypeDesp=EditorGUI.ToggleLeft(new Rect(startX+spaceX+width+12, startY, width, 20), "Show Description", showTypeDesp);
			if(showTypeDesp){
				EditorGUI.HelpBox(new Rect(startX, startY+=spaceY, width+spaceX, 40), towerTypeTooltip[(int)unit.towerType], MessageType.Info);
				startY+=45-height;
			}
			
			
			cont=new GUIContent("MaxCount In Scene:", "The maximum number of this tower prefab is allowed in any particular scene.\nSet to >0 to take effect");
			EditorGUI.LabelField(new Rect(startX+12, startY+=spaceY, width, height), cont);
			GUI.color=unit.limitInScene>0 ? Color.white : Color.grey;
			unit.limitInScene=EditorGUI.IntField(new Rect(startX+spaceX+12, startY, widthS, height), unit.limitInScene);	GUI.color=Color.white;
			
			
			startY=DrawGeneralSetting(startX, startY+10, unit);
			
			if(unit.statsList.Count==0) unit.statsList.Add(new Stats());
			startY=DrawTowerStats(startX, startY+spaceY, unit);
			
			startY=DrawTowerVisualEffect(startX, startY+spaceY, unit)+spaceY;
			
			startY=DrawUnitAnimation(startX, startY, unit)+spaceY;
			
			startY+=spaceY;
			
				GUIStyle style=new GUIStyle("TextArea");	style.wordWrap=true;
				cont=new GUIContent("Unit description (for runtime and editor): ", "");
				EditorGUI.LabelField(new Rect(startX, startY, 400, 20), cont);
				unit.desp=EditorGUI.TextArea(new Rect(startX, startY+spaceY-3, 270, 150), unit.desp, style);
			
			return new Vector2(maxX, startY+170);
		}
		
		
		
		float blockWidth;
		float blockHeight;
		private bool foldStats=true;
		protected float DrawTowerStats(float startX, float startY, UnitTower unit){
			string textF="Tower Stats And Upgrade";//+(!foldStats ? "(show)" : "(hide)");
			foldStats=EditorGUI.Foldout(new Rect(startX, startY+=spaceY, spaceX, height), foldStats, textF, TDE.foldoutS);
			if(!foldStats) return startY;
			
			startX+=15;
			
			TDE.Label(startX, startY+=spaceY, width, height, "Next Upgrade:", "The tower prefab this tower can be upgraded to");
			for(int i=0; i<unit.upgradeTowerList.Count; i++){
				if(unit.upgradeTowerList[i]==null){ unit.upgradeTowerList.RemoveAt(i);	i-=1; continue; }
				
				TDE.Label(startX+spaceX-20, startY, width, height, " - ", "");
				int idx=TowerDB.GetPrefabIndex(unit.upgradeTowerList[i]);
				idx=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), idx, TowerDB.label);
				if(TowerDB.GetItem(idx)!=unit && !unit.upgradeTowerList.Contains(TowerDB.GetItem(idx))) 
					unit.upgradeTowerList[i]=TowerDB.GetItem(idx);
				
				if(GUI.Button(new Rect(startX+spaceX+width+5, startY, height, height), "-")){
					unit.upgradeTowerList.RemoveAt(i);	i-=1; continue; 
				}
				
				startY+=spaceY;
			}
			
			int newIdx=-1;
			if(unit.upgradeTowerList.Count>0) TDE.Label(startX+spaceX-65, startY, width, height, " Add New:", "");
			newIdx=EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), newIdx, TowerDB.label);
			if(newIdx>=0 && TowerDB.GetItem(newIdx)!=unit && !unit.upgradeTowerList.Contains(TowerDB.GetItem(newIdx))){
				Debug.Log("new index  "+newIdx+"     "+TowerDB.GetItem(newIdx));
				unit.upgradeTowerList.Add(TowerDB.GetItem(newIdx));
			}
			
			startY+=10;
			
			//~ if(GUI.Button(new Rect(startX+spaceX+15, startY, width, height), "Add Level")) {
			if(GUI.Button(new Rect(startX, startY+spaceY, width, height), "Add Level")) {
				unit.statsList.Add(new Stats()); 
				unit.statsList[unit.statsList.Count-1].ResetAsBaseStat();
				foldStats=true;
			}
			
			startY+=spaceY;
			
			blockWidth=spaceX+2*widthS+26;
			
			float cachedX=startX;
			float cachedY=(startY+=spaceY);
			for(int i=0; i<unit.statsList.Count; i++){
				GUI.Box(new Rect(startX, cachedY, blockWidth, blockHeight), "");
				
				EditorGUI.LabelField(new Rect(startX+6, startY+=3, width, height), "Level "+(i+1), TDE.headerS);
				
				GUI.color=new Color(1f, .25f, .25f, 1f);
				if(GUI.Button(new Rect(startX+blockWidth-1.5f*widthS-3, startY, widthS*1.5f, 14), "remove")){
					if(unit.statsList.Count>1){ unit.statsList.RemoveAt(i); i-=1; }
				}
				GUI.color=Color.white;
				
				_EType tType=_EType.TAOE;
				if(unit.IsTurret()) tType=_EType.TTurret;
				else if(unit.IsSupport()) tType=_EType.TSupport;
				else if(unit.IsResource()) tType=_EType.TRsc;
				else if(unit.IsMine()) tType=_EType.TMine;
				else if(unit.IsBlock()) tType=_EType.TBlock;
				
				
				float bHeight=DrawStats(startX, startY+spaceY+3, unit.statsList[i], tType, true)-cachedY+5;
				if(bHeight>blockHeight) blockHeight=bHeight;
				
				startY=cachedY;
				
				startX+=blockWidth+10;
				
				maxX=startX;
			}
			startY+=blockHeight;
			startX=cachedX-15;
			
			return startY;
		}
		
		
		
		private bool foldBasicSetting=true;
		protected float DrawGeneralSetting(float startX, float startY, UnitTower unit){
			string textF="General Tower Setting ";//+(!foldBasicSetting ? "(show)" : "(hide)");
			foldBasicSetting=EditorGUI.Foldout(new Rect(startX, startY+=spaceY, spaceX, height), foldBasicSetting, textF, TDE.foldoutS);
			if(!foldBasicSetting) return startY;
			
			
			startX+=12;
			
			//TDE.Label(startX, startY+=spaceY, width, height, "Turret:", "");
			//unit.isTurret=EditorGUI.Toggle(new Rect(startX+spaceX, startY, widthS, height), unit.isTurret);
			//TDE.Label(startX, startY+=spaceY, width, height, "AOE:", "");
			//unit.isAOE=EditorGUI.Toggle(new Rect(startX+spaceX, startY, widthS, height), unit.isAOE);
			//TDE.Label(startX, startY+=spaceY, width, height, "Support:", "");
			//unit.isSupport=EditorGUI.Toggle(new Rect(startX+spaceX, startY, widthS, height), unit.isSupport);
			//TDE.Label(startX, startY+=spaceY, width, height, "Resource:", "");
			//unit.isResource=EditorGUI.Toggle(new Rect(startX+spaceX, startY, widthS, height), unit.isResource);
			//TDE.Label(startX, startY+=spaceY, width, height, "Mine:", "");
			//unit.isMine=EditorGUI.Toggle(new Rect(startX+spaceX, startY, widthS, height), unit.isMine);
			////TDE.Label(startX, startY+=spaceY, width, height, "Turret:", "");
			////unit.isSpawner=EditorGUI.Toggle(new Rect(startX+spaceX, startY, widthS, height), unit.isSpawner);
			
			//startY+=10;
			
			if(unit.IsTurret() || unit.IsAOE() || unit.IsMine()){
				int tgtGroup=(int)unit.targetGroup;
				cont=new GUIContent("Target Group:", "The target group of the tower");
				contL=TDE.SetupContL(targetGroupLabel, targetGroupTooltip);
				EditorGUI.LabelField(new Rect(startX, startY+=spaceY, width, height), cont);
				tgtGroup = EditorGUI.Popup(new Rect(startX+spaceX, startY, width, height), new GUIContent(""), tgtGroup, contL);
				unit.targetGroup=(UnitTower._TargetGroup)tgtGroup;
			}
			
			startY=DrawUnitSetting(startX-12, startY, unit);
			
			return startY;
		}
		
		
		private bool foldVisual=false;
		protected float DrawTowerVisualEffect(float startX, float startY, UnitTower tower){
			string textF="Visual and Audio Setting ";//+(!foldVisual ? "(show)" : "(hide)");
			foldVisual=EditorGUI.Foldout(new Rect(startX, startY+=spaceY, spaceX, height), foldVisual, textF, TDE.foldoutS);
			if(!foldVisual) return startY;
			
			startX+=12;
			
			startY=5+DrawVisualObject(startX, startY+=spaceY, tower.effectBuilding, "Building Effect", "OPTIONAL: The effect object to spawn when the tower is entering building/upgrading/selling state");
			startY=5+DrawVisualObject(startX, startY+=spaceY, tower.effectBuilt, "Built Effect", "OPTIONAL: The effect object to spawn when the tower has complete a building/upgrading process");
			startY=5+DrawVisualObject(startX, startY+=spaceY, tower.effectBuilt, "Sold Effect", "OPTIONAL: The effect object to spawn when the tower has been sold");
			startY=5+DrawVisualObject(startX, startY+=spaceY, tower.effectDestroyed, "Destroyed Effect", "OPTIONAL: The effect object to spawn when the tower has been sold");
			
			startY+=10;
			
			TDE.Label(startX, startY+=spaceY, width, height, "Sound-Building:", "OPTIONAL - The audio clip to play when the tower starts building");
			tower.soundBuilding=(AudioClip)EditorGUI.ObjectField(new Rect(startX+spaceX, startY, width, height), tower.soundBuilding, typeof(AudioClip), true);
			
			TDE.Label(startX, startY+=spaceY, width, height, "Sound-Built:", "OPTIONAL - The audio clip to play when the tower finishes building");
			tower.soundBuilt=(AudioClip)EditorGUI.ObjectField(new Rect(startX+spaceX, startY, width, height), tower.soundBuilt, typeof(AudioClip), true);
			
			TDE.Label(startX, startY+=spaceY, width, height, "Sound-Upgrading:", "OPTIONAL - The audio clip to play when the tower starts upgrading");
			tower.soundUpgrading=(AudioClip)EditorGUI.ObjectField(new Rect(startX+spaceX, startY, width, height), tower.soundUpgrading, typeof(AudioClip), true);
			
			TDE.Label(startX, startY+=spaceY, width, height, "Sound-Upgraded:", "OPTIONAL - The audio clip to play when the tower finishes upgraded");
			tower.soundUpgraded=(AudioClip)EditorGUI.ObjectField(new Rect(startX+spaceX, startY, width, height), tower.soundUpgraded, typeof(AudioClip), true);
			
			TDE.Label(startX, startY+=spaceY, width, height, "Sound-Sold:", "OPTIONAL - The audio clip to play when the tower is sold");
			tower.soundSold=(AudioClip)EditorGUI.ObjectField(new Rect(startX+spaceX, startY, width, height), tower.soundSold, typeof(AudioClip), true);
			
			TDE.Label(startX, startY+=spaceY, width, height, "Sound-Destroyed:", "OPTIONAL - The audio clip to play when the tower is destroyed");
			tower.soundDestroyed=(AudioClip)EditorGUI.ObjectField(new Rect(startX+spaceX, startY, width, height), tower.soundDestroyed, typeof(AudioClip), true);
			
			return startY;
		}
		
		
		
		
		
		protected Vector2 DrawTowerList(float startX, float startY, List<UnitTower> towerList){
			List<EItem> list=new List<EItem>();
			for(int i=0; i<towerList.Count; i++){
				EItem item=new EItem(towerList[i].prefabID, towerList[i].unitName, towerList[i].icon);
				list.Add(item);
			}
			return DrawList(startX, startY, window.position.width, window.position.height, list);
		}
		
		
		
		public static int NewItem(UnitTower tower){ return window._NewItem(tower); }
		private int _NewItem(UnitTower tower){
			if(TowerDB.GetList().Contains(tower)) return selectID;
			
			tower.prefabID=TDE.GenerateNewID(TowerDB.GetPrefabIDList());
			
			TowerDB.GetList().Add(tower);
			TowerDB.UpdateLabel();
			
			return TowerDB.GetList().Count-1;
		}
		
		protected override void DeleteItem(){
			TowerDB.GetList().RemoveAt(deleteID);
			TowerDB.UpdateLabel();
		}
		
		protected override void SelectItem(){ SelectItem(selectID); }
		private void SelectItem(int newID){ 
			selectID=newID;
			if(TowerDB.GetList().Count<=0) return;
			
			selectID=Mathf.Clamp(selectID, 0, TowerDB.GetList().Count-1);
			UpdateObjHierarchyList(TowerDB.GetList()[selectID].transform);
			
			blockHeight=0;
		}
		
		protected override void ShiftItemUp(){ 	if(selectID>0) ShiftItem(-1); }
		protected override void ShiftItemDown(){ if(selectID<TowerDB.GetList().Count-1) ShiftItem(1); }
		private void ShiftItem(int dir){
			UnitTower tower=TowerDB.GetList()[selectID];
			TowerDB.GetList()[selectID]=TowerDB.GetList()[selectID+dir];
			TowerDB.GetList()[selectID+dir]=tower;
			selectID+=dir;
			blockHeight=0;
		}
		
		
		
	}
	
}