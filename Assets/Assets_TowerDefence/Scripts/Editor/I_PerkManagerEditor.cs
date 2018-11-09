using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

namespace TDTK{
	
	[CustomEditor(typeof(PerkManager))]
	public class PerkManagerEditor : _TDInspector {

		private PerkManager instance;
		
		public override void Awake(){
			base.Awake();
			instance = (PerkManager)target;
		}
		
		private bool showList=true;
		
		public override void OnInspectorGUI(){
			base.OnInspectorGUI();
			
			if(instance==null){ Awake(); return; }
			
			GUI.changed = false;
			
			Undo.RecordObject(instance, "PerkManager");
			
			EditorGUILayout.Space();
			
				cont=new GUIContent("Game Scene:", "Check to to indicate if the scene is not an actual game scene\nIntend if the a perk menu scene, purchased perk wont take effect ");
				instance.inGameScene=EditorGUILayout.Toggle(cont, instance.inGameScene);
			
				cont=new GUIContent("Carry Over:", "Check to have carry the progress made in previous level to this level, the progress made in this level will be carry over to the next level.\n\nIf this is the first level, the specified setting value is used instead");
				instance.carryOver=EditorGUILayout.Toggle(cont, instance.carryOver);
			
			EditorGUILayout.Space();
			
			
				GUILayout.BeginHorizontal();
					
					GUILayout.BeginVertical();
						
						EditorGUIUtility.labelWidth+=35;
						cont=new GUIContent("Use RscManager For Cost:", "Check use the resources in RscManager for perk cost");
						instance.useRscManagerForCost=EditorGUILayout.Toggle(cont, instance.useRscManagerForCost);
						EditorGUIUtility.labelWidth-=35;
						
						cont=new GUIContent("Resource:", "The resource used  to cast perk");
						if(instance.useRscManagerForCost) EditorGUILayout.LabelField("Resource:", "-");
						else instance.rsc=EditorGUILayout.IntField(cont, instance.rsc);
						
					GUILayout.EndVertical();
					
					if(!instance.useRscManagerForCost){
						Sprite icon=PerkDB.GetRscIcon();
						icon=(Sprite)EditorGUILayout.ObjectField(icon, typeof(Sprite), true, GUILayout.Width(40), GUILayout.Height(40));
						PerkDB.SetRscIcon(icon);
					}
				
				GUILayout.EndHorizontal();
				
			
			EditorGUILayout.Space();
				
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("", GUILayout.MaxWidth(10));
				showList=EditorGUILayout.Foldout(showList, "Show Perk List");
				EditorGUILayout.EndHorizontal();
				if(showList){
					
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.Space();
						if(GUILayout.Button("EnableAll") && !Application.isPlaying){
							instance.unavailablePrefabIDList=new List<int>();
						}
						if(GUILayout.Button("DisableAll") && !Application.isPlaying){
							instance.unavailablePrefabIDList=PerkDB.GetPrefabIDList();
						}
						EditorGUILayout.Space();
					EditorGUILayout.EndHorizontal();
						
					EditorGUILayout.Space();
			
					
					List<Perk> perkList=PerkDB.GetList();
					for(int i=0; i<perkList.Count; i++){
						if(perkList[i].hideInInspector) continue;
						
						Perk perk=perkList[i];
						
						GUILayout.BeginHorizontal();
							
							EditorGUILayout.Space();
						
							GUILayout.Box("", GUILayout.Width(40),  GUILayout.Height(40));
							TDE.DrawSprite(GUILayoutUtility.GetLastRect(), perk.icon, perk.desp, false);
							
							GUILayout.BeginVertical();
								EditorGUILayout.Space();
								GUILayout.Label(perk.name, GUILayout.ExpandWidth(false));
								
								GUILayout.BeginHorizontal();
						
									float cachedL=EditorGUIUtility.labelWidth;	EditorGUIUtility.labelWidth=80;
									float cachedF=EditorGUIUtility.fieldWidth;	EditorGUIUtility.fieldWidth=10;
						
									EditorGUI.BeginChangeCheck();
									bool flag=!instance.unavailablePrefabIDList.Contains(perk.prefabID) ? true : false;
									flag=EditorGUILayout.Toggle(new GUIContent(" - enabled: ", "check to enable the perk in this level"), flag);
									
									if(!Application.isPlaying && EditorGUI.EndChangeCheck()){
										if(!flag && !instance.unavailablePrefabIDList.Contains(perk.prefabID)){
											instance.unavailablePrefabIDList.Add(perk.prefabID);
											instance.purchasedPrefabIDList.Remove(perk.prefabID);
										}
										else if(flag) instance.unavailablePrefabIDList.Remove(perk.prefabID);
									}
									
									if(!instance.unavailablePrefabIDList.Contains(perk.prefabID)){
										EditorGUI.BeginChangeCheck();
										flag=instance.purchasedPrefabIDList.Contains(perk.prefabID);
										flag=EditorGUILayout.Toggle(new GUIContent(" - purchased: ", "check to set the perk as purchased right from the start"), flag);
										
										if(!Application.isPlaying && EditorGUI.EndChangeCheck()){
											if(flag) instance.purchasedPrefabIDList.Add(perk.prefabID);
											else instance.purchasedPrefabIDList.Remove(perk.prefabID);
										}
									}
									else{
										EditorGUILayout.LabelField(" - purchased: ", "- ");
									}
									
									EditorGUIUtility.labelWidth=cachedL;	EditorGUIUtility.fieldWidth=cachedF;
									
								GUILayout.EndHorizontal();
								
							GUILayout.EndVertical();
						
						GUILayout.EndHorizontal();
						
					}
					
				}
			
			EditorGUILayout.Space();
			
			DefaultInspector();
			
			if(GUI.changed) EditorUtility.SetDirty(instance);
		}
		
		
	}

}