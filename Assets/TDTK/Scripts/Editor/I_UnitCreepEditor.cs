using UnityEngine;
using UnityEditor;

using System;
using System.Collections;

namespace TDTK{
	
	[CustomEditor(typeof(UnitCreep))] [CanEditMultipleObjects]
	public class I_UnitCreepEditor : _TDInspector {

		private UnitCreep instance;
		public override void Awake(){
			base.Awake();
			instance = (UnitCreep)target;
		}
		
		
		public override void OnInspectorGUI(){
			base.OnInspectorGUI();
			
			if(instance==null){ Awake(); return; }
			
			GUI.changed = false;
			
			EditorGUILayout.Space();
			
			if(!EditorApplication.isPlaying){
				PrefabType type=PrefabUtility.GetPrefabType(instance);
				
				if(type==PrefabType.Prefab || type==PrefabType.PrefabInstance){
					UnitCreep prefab=instance;
					if(type==PrefabType.PrefabInstance) prefab=(UnitCreep)PrefabUtility.GetCorrespondingObjectFromSource(instance);
					bool existInDB=CreepDB.GetPrefabIndex(prefab)>=0;
					
					if(!existInDB){
						if(instance.prefabID>=0){ instance.prefabID=-1; EditorUtility.SetDirty(instance); }
						
						EditorGUILayout.Space();
						
						EditorGUILayout.HelpBox("This prefab hasn't been added to database hence it won't be accessible to the game.", MessageType.Warning);
						GUI.color=new Color(1f, 0.7f, .2f, 1f);
						if(GUILayout.Button("Add Prefab to Database")){
							UnitCreepEditorWindow.Init();
							UnitCreepEditorWindow.NewItem(instance);
							UnitCreepEditorWindow.Init();		//call again to select the instance in editor window
						}
						GUI.color=Color.white;
					}
					else{
						EditorGUILayout.HelpBox("Editing creep using Inspector is not recommended.\nPlease use the editor window instead", MessageType.Info);
						if(GUILayout.Button("Creep Editor Window")) UnitCreepEditorWindow.Init(instance.prefabID);
					}
					
					EditorGUILayout.Space();
				}
				else{
					if(instance.prefabID>=0){ instance.prefabID=-1; EditorUtility.SetDirty(instance); }
					
					string text="Creep object won't be available to be deployed to game, or accessible in TDTK editor until it's made a prefab and added to TDTK database.";
					text+="\n\nYou can still edit the creep using default inspector. However it's not recommended";
					EditorGUILayout.HelpBox(text, MessageType.Warning);
					
					EditorGUILayout.Space();
					if(GUILayout.Button("Creep Editor Window")) UnitCreepEditorWindow.Init(instance.prefabID);
				}
			}
			
			DefaultInspector();
			
			if(GUI.changed) EditorUtility.SetDirty(instance);
		}
		
		
	}

}