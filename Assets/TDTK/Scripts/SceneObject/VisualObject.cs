using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDTK {
	
	[System.Serializable]
	public class VisualObject  {
		
		public GameObject obj;
		public bool autoDestroy=true;
		public float duration=1.5f;
		
		public void Spawn(Vector3 pos){ Spawn(pos, Quaternion.identity); }
		public void Spawn(Vector3 pos, Quaternion rot){
			if(obj==null) return;
			
			if(!autoDestroy) ObjectPoolManager.Spawn(obj, pos, rot);
			else ObjectPoolManager.Spawn(obj, pos, rot, duration);
		}
		
		
		public VisualObject Clone(){
			VisualObject clone=new VisualObject();
			clone.obj=obj;
			clone.autoDestroy=autoDestroy;
			clone.duration=duration;
			return clone;
		}
	}

}