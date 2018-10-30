using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TDTK {

	public class CreepDB : MonoBehaviour {
		
		public List<UnitCreep> creepList=new List<UnitCreep>();
		
		public static CreepDB LoadDB(){
			GameObject obj=Resources.Load("DB/CreepDB", typeof(GameObject)) as GameObject;
			return obj.GetComponent<CreepDB>();
		}
		
		
		#region runtime code
		public static CreepDB instance;
		public static CreepDB Init(){
			if(instance!=null) return instance;
			instance=LoadDB();
			return instance;
		}
		
		public static CreepDB GetDB(){ return Init(); }
		public static List<UnitCreep> GetList(bool verify=true){
			Init();
			if(verify) VerifyList();
			return instance.creepList;
		}
		public static UnitCreep GetItem(int index){ Init(); return (index>=0 && index<instance.creepList.Count) ? instance.creepList[index] : null; }
		
		public static void VerifyList(){
			for(int i=0; i<instance.creepList.Count; i++){
				if(instance.creepList[i]==null){ instance.creepList.RemoveAt(i);	i-=1; }
			}
		}
		
		public static List<int> GetPrefabIDList(){
			Init();
			List<int> prefabIDList=new List<int>();
			for(int i=0; i<instance.creepList.Count; i++) prefabIDList.Add(instance.creepList[i].prefabID);
			return prefabIDList;
		}
		
		public static int GetPrefabIndex(int pID){
			Init();
			for(int i=0; i<instance.creepList.Count; i++){
				if(instance.creepList[i].prefabID==pID) return i;
			}
			return -1;
		}
		public static int GetPrefabIndex(UnitCreep creep){
			if(creep==null) return -1;
			return GetPrefabIndex(creep.prefabID);
		}
		
		public static string[] label;
		public static void UpdateLabel(){
			label=new string[GetList(false).Count];
			for(int i=0; i<label.Length; i++) label[i]=i+" - "+GetItem(i).unitName;
		}
		#endregion
		
		
		#if UNITY_EDITOR
		[ContextMenu ("Reset PrefabID")]
		public void ResetPrefabID(){
			for(int i=0; i<creepList.Count; i++){
				creepList[i].prefabID=i;
				UnityEditor.EditorUtility.SetDirty(creepList[i]);
			}
		}
		#endif
		
	}

}