using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TDTK {

	public class TowerDB : MonoBehaviour {
		
		public List<UnitTower> towerList=new List<UnitTower>();
		
		public static TowerDB LoadDB(){
			GameObject obj=Resources.Load("DB/TowerDB", typeof(GameObject)) as GameObject;
			return obj.GetComponent<TowerDB>();
		}
		
		
		
		
		#region runtime code
		public static TowerDB instance;
		public static TowerDB Init(){
			if(instance!=null) return instance;
			instance=LoadDB();
			return instance;
		}
		
		public static TowerDB GetDB(){ return Init(); }
		public static List<UnitTower> GetList(bool verify=true){
			Init();
			if(verify) VerifyList();
			return instance.towerList;
		}
		public static UnitTower GetItem(int index){ Init(); return (index>=0 && index<instance.towerList.Count) ? instance.towerList[index] : null; }
		
		public static void VerifyList(){
			for(int i=0; i<instance.towerList.Count; i++){
				if(instance.towerList[i]==null){ instance.towerList.RemoveAt(i);	i-=1; }
			}
		}
		
		public static List<int> GetPrefabIDList(){ Init();
			List<int> prefabIDList=new List<int>();
			for(int i=0; i<instance.towerList.Count; i++) prefabIDList.Add(instance.towerList[i].prefabID);
			return prefabIDList;
		}
		
		public static UnitTower GetPrefab(int pID){ Init();
			for(int i=0; i<instance.towerList.Count; i++){
				if(instance.towerList[i].prefabID==pID) return instance.towerList[i];
			}
			return null;
		}
		
		public static int GetPrefabIndex(int pID){ Init();
			for(int i=0; i<instance.towerList.Count; i++){
				//Debug.Log(i+"   "+instance.towerList[i]+"  "+instance.towerList[i].prefabID+"   "+pID);
				if(instance.towerList[i].prefabID==pID) return i;
			}
			return -1;
		}
		public static int GetPrefabIndex(UnitTower tower){
			if(tower==null) return -1;
			return GetPrefabIndex(tower.prefabID);
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
			for(int i=0; i<towerList.Count; i++){
				towerList[i].prefabID=i;
				UnityEditor.EditorUtility.SetDirty(towerList[i]);
			}
		}
		#endif
		
	}

}