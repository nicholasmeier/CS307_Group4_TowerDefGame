using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TDTK;

namespace TDTK{

	public class TowerManager : MonoBehaviour {
		
		
		public float gridSize=1;
		public static float GetGridSize(){ return instance!=null ? instance.gridSize : 1 ; }
		
		public bool autoAdjustTextureToGrid=true;
		
		
		[Space(5)]
		public List<int> unavailablePrefabIDList=new List<int>();
		
		[Space(5)] 
		public List<UnitTower> buildableList=new List<UnitTower>();		//tower that can be built
		public static List<UnitTower> GetBuildableList(){ return instance.buildableList; }
		
		[Space(5)] public List<UnitTower> sampleList=new List<UnitTower>();			//tower that can be built
		
		[Space(5)] public List<UnitTower> activeTowerList=new List<UnitTower>();	//active tower in the scene
		public static List<UnitTower> GetActiveTowerList(){ return instance.activeTowerList; }
		public static List<Unit> GetActiveUnitList(){ return instance.activeTowerList.ConvertAll(x => (Unit)x); }
		
		
		public List<UnitTower> supportTowerList=new List<UnitTower>();	//all the support tower in the scene
		public static List<UnitTower> GetSupportTowerList(){ return instance.supportTowerList; }
		
		
		private int towerCounter=0;	//keep count of how many tower has been built, also used to assigned instanceID on tower instance
		
		//for limited tower count for each prefab in scene
		public List<int> towerCounterPrefabID=new List<int>();	//tower prefabID used for towerCounterList
		public List<int> towerCounterList=new List<int>();		//active tower count for each unique prefabID
		public static bool CheckTowerCounterLimit(int prefabID, int limit){
			if(limit<=0) return true;
			int idx=instance.towerCounterPrefabID.IndexOf(prefabID);
			if(idx>=0) return instance.towerCounterList[idx]<limit;
			else return limit>0;
		}
		
		
		private static TowerManager instance;
		public static TowerManager GetInstance(){ return instance; }
		
		// Use this for initialization
		void Awake() {
			instance=this;
			
			List<UnitTower> dbList=TowerDB.GetList();
			for(int i=0; i<dbList.Count; i++){
				if(!unavailablePrefabIDList.Contains(dbList[i].prefabID) && !dbList[i].hideInInspector) buildableList.Add(dbList[i]);
			}
			
			for(int i=0; i<buildableList.Count; i++){
				GameObject obj=(GameObject)Instantiate(buildableList[i].gameObject);
				sampleList.Add(obj.GetComponent<UnitTower>());
				sampleList[sampleList.Count-1].isPreview=true;
				sampleList[sampleList.Count-1].gameObject.layer=TDTK.GetLayerTerrain();
				sampleList[sampleList.Count-1].transform.parent=transform;
				obj.SetActive(false);
			}
			
			//Debug.Log("remove this");
			//for(int i=0; i<buildableList.Count; i++) buildableList[i].prefabID=i;
		}
		
		public static void Init(){
			if(instance==null) instance=(TowerManager) FindObjectOfType(typeof(TowerManager));
			if(instance!=null) instance.InitPlatform();
		}
		
		
		private void InitPlatform(){
			BuildPlatform[] list = FindObjectsOfType(typeof(BuildPlatform)) as BuildPlatform[];
			for(int i=0; i<list.Length; i++) list[i].Format(gridSize, autoAdjustTextureToGrid);
		}
		
		
		public static void AddBuildable(int prefabID, int replacePID=-1){
			if(instance==null) return;
			
			UnitTower newTower=TowerDB.GetPrefab(prefabID);
			if(newTower==null){ Debug.LogWarning("Invalid prefabID?"); return; }
			
			int replaceIdx=-1;
			if(replacePID>=0){
				for(int i=0; i<instance.buildableList.Count; i++){
					if(instance.buildableList[i].prefabID==replacePID){ replaceIdx=i; break; }
				}
			}
			
			GameObject obj=(GameObject)Instantiate(newTower.gameObject);
			obj.SetActive(false);
			
			if(replaceIdx>=0){
				//TDTK.OnRemoveBuildable(instance.buildableList[replaceIdx]);
				instance.buildableList[replaceIdx]=newTower;
				instance.sampleList[replaceIdx]=obj.GetComponent<UnitTower>();
				instance.sampleList[replaceIdx].isPreview=true;
				TDTK.OnReplaceBuildable(replaceIdx, newTower);
			}
			else{
				instance.buildableList.Add(newTower);
				instance.sampleList.Add(obj.GetComponent<UnitTower>());
				instance.sampleList[instance.sampleList.Count-1].isPreview=true;
				TDTK.OnNewBuildable(newTower);
			}
			
		}
		
		
		
		// Update is called once per frame
		void Update () {
			//~ if(Input.GetMouseButtonDown(0)){
				//~ ToggleNode(Input.mousePosition, true);
			//~ }
			//~ if(Input.GetMouseButtonDown(1)){
				//~ ToggleNode(Input.mousePosition, false);
			//~ }
			
			/*
			//PointNBuild
			if(Input.GetMouseButtonUp(0)){
				sInfo=OnCursorDown(Input.mousePosition);
				
				bool select=false;
				bool build=false;
				
				if(sInfo.HasValidPoint()){
					if(sInfo.GetTower()!=null){
						select=true;
						//selectTower
						
					}
					else if(sInfo.AvailableForBuild()){
						build=true;
						//show build menu
					}
				}
				
				if(!select){}	//clear selected tower
				if(!build){}	//hide build menu
			}
			*/
			
			if(dndTower!=null && Time.unscaledTime-dndCooldown>0.15f){
				//Debug.Log("things   "+Input.GetMouseButtonUp(0)+"   "+Input.GetMouseButtonUp(1));
				
				if(lastDnDCursorPos!=Input.mousePosition){
					lastDnDCursorPos=Input.mousePosition;
					
					sInfo=GetSelectInfo(Input.mousePosition, dndInstanceID, dndTower.radius);
					if(sInfo.HasWorldPoint()) dndTower.transform.position=sInfo.GetPos();
					else dndTower.transform.position=Camera.main.ScreenToWorldPoint(Input.mousePosition+new Vector3(0, 0, 15));
				}
				
				
				bool cursorUp=Input.GetMouseButtonUp(0);
				if(cursorUp){
					if(sInfo.HasValidPoint() && sInfo.AvailableForBuild() && sInfo.CanBuildTower(dndTower.prefabID)){
						if(RscManager.HasSufficientRsc(dndTower.GetCost())){
							RscManager.SpendRsc(dndTower.GetCost());
							SelectControl.ClearUnit();
							if(!UseFreeFormMode()) AddTower(dndTower, sInfo.platform, sInfo.nodeID);
							else AddTower(dndTower, CreatePlatformForTower(dndTower, GetGridSize()), 0);
							dndTower.Build();	dndTower=null;	dndCooldown=Time.time;
						}
						else{
							GameControl.InvalidAction("Insufficient Resources");
							_ExitDragNDropPhase();
						}
					}
					else{
						GameControl.InvalidAction("Invalid Build Point");
						_ExitDragNDropPhase();
					}
				}
				else{
					//if(Input.touchCount==0) _ExitDragNDropPhase();	
				}
				
				
				/*
				if(sInfo.HasValidPoint()){
					dndTower.transform.position=sInfo.GetPos();
					
					if(sInfo.AvailableForBuild() && sInfo.CanBuildTower(dndTower.prefabID)){
						//~ if(cursorDown){
							//~ if(RscManager.HasSufficientRsc(dndTower.GetCost())){
								//~ RscManager.SpendRsc(dndTower.GetCost());
								//~ SelectControl.ClearUnit();
								//~ if(!UseFreeFormMode()) AddTower(dndTower, sInfo.platform, sInfo.nodeID);
								//~ else AddTower(dndTower, CreatePlatformForTower(dndTower, GetGridSize()), 0);
								//~ dndTower.Build();	dndTower=null;	dndCooldown=Time.time;
							//~ }
							//~ else{
								//~ GameControl.InvalidAction("Insufficient Resources");
								//~ _ExitDragNDropPhase();
							//~ }
						//~ }
					}
				}
				else{
					//Debug.Log("this will need some work ");
					dndTower.transform.position=Camera.main.ScreenToWorldPoint(Input.mousePosition+new Vector3(0, 0, 15));
					//~ if(cursorDown){
						//~ GameControl.InvalidAction("Invalid Build Point");
						//~ _ExitDragNDropPhase();
					//~ }
				}
				*/
				
				if(Input.GetMouseButtonDown(1)){
					//SelectControl.ClearUnit();
					_ExitDragNDropPhase();
				}
			}
			
		}
		
		
		private static int dndInstanceID=0;	//for optimization so path-finding wont be called every frame
		private Vector3 lastDnDCursorPos;
		
		private static SelectInfo sInfo;	//for dragNDropTower only
		public UnitTower dndTower;	//dragNDropTower
		private static float dndCooldown=0;
		public static UnitTower CreateDragNDropTower(UnitTower prefab){
			if(instance.dndTower!=null) Destroy(instance.dndTower.gameObject);
			
			//Debug.Log("CreateDragNDropTower");
			
			GameObject obj=(GameObject)Instantiate(prefab.gameObject, new Vector3(0, 999, 0), Quaternion.identity);
			instance.dndTower=obj.GetComponent<UnitTower>();
			instance.dndTower.isPreview=true;
			
			SelectControl.SelectUnit(instance.dndTower);
			
			dndCooldown=Time.unscaledTime;
			dndInstanceID+=1;
			
			return instance.dndTower;
		}
		public static bool InDragNDropPhase(){ return instance.dndTower!=null || Time.time-dndCooldown<0.15f; }
		public static UnitTower GetDragNDropTower(){ return instance.dndTower; }
		public static bool DnDHasValidPos(){ 
			return sInfo!=null && sInfo.HasValidPoint() && sInfo.CanBuildTower(instance.dndTower.prefabID);
		}
		
		public static void ExitDragNDropPhase(){ instance._ExitDragNDropPhase(); }
		public void _ExitDragNDropPhase(){
			if(dndTower==null) return;
			Destroy(dndTower.gameObject);
			SelectControl.ClearUnit();
			dndTower=null;
		}
		
		
		[Space(10)]
		public bool freeFormMode=false;
		public static bool UseFreeFormMode(){ return instance.freeFormMode; }
		public bool raycastForTerrain=true;
		public static bool RaycastForTerrain(){ return instance.raycastForTerrain; }
		
		private static LayerMask mask;
		private static LayerMask maskPlatform;
		private static LayerMask maskTerrain;
		//private static LayerMask maskObstacle;
		private static bool initMask=false;
		public static void InitMask(){
			if(initMask) return;
			initMask=true;
			
			mask=1<<TDTK.GetLayerPlatform() | 1<<TDTK.GetLayerTerrain() ;
			maskPlatform=1<<TDTK.GetLayerPlatform();
			maskTerrain=1<<TDTK.GetLayerTerrain() ;
			//maskObstacle=1<<TDTK.GetLayerTower() | 1<<TDTK.GetLayerObstacle() ;
		}
		
		private static int lastSelectID=-1;
		
		public static SelectInfo GetSelectInfo(Vector3 pointer, int ID=-1, float towerSize=1){
			InitMask();
			
			Ray ray = CameraControl.GetMainCam().ScreenPointToRay(pointer);
			RaycastHit hit;
			
			//for free from drag and drop mode
			if(UseFreeFormMode() && instance.dndTower!=null){
				if(Physics.Raycast(ray, out hit, Mathf.Infinity, maskTerrain)){
					Collider[] obstacles=Physics.OverlapSphere(hit.point, towerSize, ~maskTerrain);
					if(obstacles.Length==1 && obstacles[0].gameObject==instance.dndTower.gameObject) obstacles=new Collider[0];
					
					if(obstacles.Length==0) return new SelectInfo(hit.point);
					else return new SelectInfo("Invalid build-point!", hit.point);
				}
				return new SelectInfo("No valid build-point has been found");
			}
			
			//try to detect a platform and determine the node on it
			bool flag=Physics.Raycast(ray, out hit, Mathf.Infinity, RaycastForTerrain() ? mask : maskPlatform);
			if(flag){
				if(hit.collider.gameObject.layer==TDTK.GetLayerPlatform()){
					BuildPlatform platform=hit.collider.gameObject.GetComponent<BuildPlatform>();
					if(platform!=null){
						Vector3 pos=platform.GetTilePos(hit.point, GetGridSize());
						NodeTD node=platform.GetNearestNode(pos);
						
						if(ID>0){
							if(lastSelectID==ID && sInfo.nodeID==node.ID) return sInfo;
							else lastSelectID=ID;
						}
						
						return new SelectInfo(platform, node.ID);
					}
				}
				else if(RaycastForTerrain()) return new SelectInfo("No platform has been found", hit.point);
			}
			
			return new SelectInfo("No platform has been found");
		}
		
		
		
		
		//test function, no longer in use
		/*
		public void ToggleNode(Vector3 pointer, bool build=true){
			Camera mainCam=Camera.main;
			if(mainCam!=null){
				Ray ray = mainCam.ScreenPointToRay(pointer);
				RaycastHit hit;
				LayerMask maskPlatform=1<<TDTK.GetLayerPlatform();
				if(Physics.Raycast(ray, out hit, Mathf.Infinity, maskPlatform)){
					BuildPlatform platform=hit.collider.gameObject.GetComponent<BuildPlatform>();
					
					Vector3 pos=platform.GetTilePos(hit.point, gridSize);
					NodeTD node=platform.GetNearestNode(pos);
					
					if(build) platform.BlockNode(node.ID);
					else platform.UnblockNode(node.ID);
				}
			}
		}
		*/
		
		
		private int activeSampleTowerIdx=-1;
		public static void ShowSampleTower(int prefabID, SelectInfo sInfo){
			int idx=-1;
			for(int i=0; i<instance.sampleList.Count; i++){
				if(instance.sampleList[i].prefabID==prefabID){ idx=i; break; }
			}
			
			instance.sampleList[idx].GetT().rotation=sInfo.GetRot();
			instance.sampleList[idx].GetT().position=sInfo.GetPos();
			instance.sampleList[idx].GetObj().SetActive(true);
			instance.activeSampleTowerIdx=idx;
			
			SelectControl.SelectUnit(instance.sampleList[idx]);
		}
		public static void HideSampleTower(){
			SelectControl.ClearUnit();
			if(instance.activeSampleTowerIdx>=0) instance.sampleList[instance.activeSampleTowerIdx].GetObj().SetActive(false);
		}
		
		
		public static void BuildTower(UnitTower prefab, BuildPlatform platform, int nodeID, bool useRsc=true){
			Debug.Log("BuildTower  "+prefab);
			
			if(useRsc){
				if(!RscManager.HasSufficientRsc(prefab.GetCost())){
					Debug.Log("Insufficient resources");
					return;
				}
				//Debug.Log("Get cost "+prefab.GetCost()[0]);
				RscManager.SpendRsc(prefab.GetCost());
			}
			
			NodeTD node=platform.GetNode(nodeID);
			GameObject obj=(GameObject)Instantiate(prefab.gameObject, node.pos, platform.GetRot()*Quaternion.Euler(-90, 0, 0));
			UnitTower tower=obj.GetComponent<UnitTower>();
			
			AddTower(tower, platform, nodeID);
		}
		
		public static void PreBuildTower(UnitTower tower){
			BuildPlatform platform=null;
			LayerMask mask=1<<TDTK.GetLayerPlatform();
			Collider[] cols=Physics.OverlapSphere(tower.GetPos(), GetGridSize(), mask);
			if(cols.Length>0) platform=cols[0].gameObject.GetComponent<BuildPlatform>();
			
			if(platform!=null){
				NodeTD node=platform.GetNearestNode(tower.GetPos());
				if(Vector3.Distance(node.pos, tower.GetPos())<GetGridSize()){
					AddTower(tower, platform, node.ID);
					tower.transform.position=node.pos;
					return;
				}
			}
			
			//~ GameObject obj=new GameObject("platform");
			//~ SphereCollider collider=obj.AddComponent<SphereCollider>();
			//~ collider.radius=GetGridSize()*.5f;
			//~ obj.transform.parent=tower.transform;
			//~ obj.transform.localPosition=Vector3.zero;
			//~ obj.layer=TDTK.GetLayerPlatform();
			
			//~ platform=obj.AddComponent<BuildPlatform>();
			//~ platform.SingleNodePlatform();
			
			AddTower(tower, CreatePlatformForTower(tower, GetGridSize()), 0);
		}
		
		public static BuildPlatform CreatePlatformForTower(UnitTower tower, float size=-1){
			GameObject obj=new GameObject("platform");
			SphereCollider collider=obj.AddComponent<SphereCollider>();
			collider.radius=size*.5f;//(size<0) ? size*.5f : tower.GetSize()*.5f;
			obj.transform.parent=tower.transform;
			obj.transform.localPosition=Vector3.zero;
			obj.layer=TDTK.GetLayerPlatform();
			BuildPlatform platform=obj.AddComponent<BuildPlatform>();
			platform.SingleNodePlatform();
			return platform;
		}
		
		public static void AddTower(UnitTower tower, BuildPlatform platform=null, int nodeID=-1){
			tower.isPreview=false;
			
			tower.instanceID=instance.towerCounter;
			instance.towerCounter+=1;
			instance.activeTowerList.Add(tower);
			
			if(tower.IsSupport()) instance.supportTowerList.Add(tower);
			
			if(platform!=null && nodeID>=0){
				tower.SetBuildPoint(platform, nodeID);
				platform.BuildTower(nodeID, tower);
			}
			
			
			
			//for limiting tower count in the scene according to prefabID
			int idx=instance.towerCounterPrefabID.IndexOf(tower.prefabID);
			if(idx<0){
				instance.towerCounterPrefabID.Add(tower.prefabID);
				instance.towerCounterList.Add(1);
			}
			else instance.towerCounterList[idx]+=1;
			
			UnitTower.NewTower(tower);
			
			TDTK.OnNewTower(tower);
		}
		
		
		public static void RemoveTower(UnitTower tower){
			instance.activeTowerList.Remove(tower);
			if(tower.IsSupport()) instance.supportTowerList.Remove(tower);
			UnitTower.RemoveTower(tower);
			
			//for limiting tower count in the scene according to prefabID
			int idx=instance.towerCounterPrefabID.IndexOf(tower.prefabID);
			if(idx>=0) instance.towerCounterList[idx]-=1;
			
			TDTK.OnNewTower(null);
		}
		
		
		public static List<Unit> GetUnitsWithinRange(Unit srcUnit, float range){ return GetUnitsWithinRange(srcUnit.GetPos(), range); }
		public static List<Unit> GetUnitsWithinRange(Vector3 pos, float range){
			List<UnitTower> unitList=GetActiveTowerList();	List<Unit> tgtList=new List<Unit>();
			for(int i=0; i<unitList.Count; i++){
				if(Vector3.Distance(pos, unitList[i].GetPos())<range+unitList[i].GetRadius()) 
					tgtList.Add(unitList[i]);
			}
			return tgtList;
		}
		
	}
	
	
	
	public class SelectInfo{
		public BuildPlatform platform=null;
		public int nodeID=-1;
		
		public bool invalidPoint=false;
		public bool hasWorldPoint=false;
		public string invalidText="";
		
		public List<UnitTower> buildableList=new List<UnitTower>();
		
		public SelectInfo(string Invalid){ invalidPoint=true; invalidText=Invalid; }
		public SelectInfo(string Invalid, Vector3 point){ invalidPoint=true; invalidText=Invalid; buildPoint=point; hasWorldPoint=true;}
		public SelectInfo(BuildPlatform p, int ID){ 
			platform=p; nodeID=ID;
			
			if(AvailableForBuild()){
				buildableList=new List<UnitTower>( TowerManager.GetBuildableList() );
				
				//for limiting amount of tower in the scene according to prefabID
				for(int i=0; i<buildableList.Count; i++){
					if(TowerManager.CheckTowerCounterLimit(buildableList[i].prefabID, buildableList[i].limitInScene)) continue;
					buildableList.RemoveAt(i); i-=1;
				}
				
				for(int i=0; i<platform.unavailablePrefabIDList.Count; i++){
					for(int n=0; n<buildableList.Count; n++){
						if(buildableList[n].prefabID==platform.unavailablePrefabIDList[i]){
							buildableList.RemoveAt(n);
							break;
						}
					}
				}
				
				if(PathBlocked()){	//can only build mine
					for(int i=0; i<buildableList.Count; i++){
						if(!buildableList[i].IsMine()){
							buildableList.RemoveAt(i);	i-=1;
						}
					}
				}
			}
		}
		
		public bool HasValidPoint(){ return !invalidPoint; }
		public bool HasWorldPoint(){ return HasValidPoint() || hasWorldPoint; }
		
		public Quaternion GetRot(){ 
			if(TowerManager.UseFreeFormMode()) return Quaternion.identity;
			Quaternion rot=Quaternion.Euler(0, platform.transform.rotation.eulerAngles.y, 0);
			return rot;
		}
		public Vector3 GetPos(){ 
			if(TowerManager.UseFreeFormMode()) return buildPoint;
			return HasValidPoint() ? platform.GetNode(nodeID).pos : buildPoint;
		}
		
		public UnitTower GetTower(){ 
			return platform!=null ? platform.GetNode(nodeID).GetTower() : null ;
		}
		
		public bool AvailableForBuild(){
			if(TowerManager.UseFreeFormMode()) return !invalidPoint;
			return !platform.GetNode(nodeID).IsBlockedForTower() && GetTower()==null && !platform.GetNode(nodeID).IsOccupied();
		}
		public bool PathBlocked(){
			return !platform.CheckForNode(nodeID);
		}
		
		public bool CanBuildTower(int towerPID){
			if(TowerManager.UseFreeFormMode()) return true;
			for(int i=0; i<buildableList.Count; i++){ if(buildableList[i].prefabID==towerPID) return true; }
			return false;
		}
		
		public List<UnitTower> GetBuildableList(){ return buildableList; }
		
		
		//for freeform mode
		public Vector3 buildPoint;
		public SelectInfo(Vector3 point){ buildPoint=point; }
	}

}