using System.Collections;
using System.Collections.Generic;

using UnityEngine;
//using UnityEditor.Animations;


namespace TDTK{
	
	
	public class Unit : MonoBehaviour {
		
		protected Unit GetUnit(){ return this; }
		
		public enum _UnitType{ Tower, Creep }
	
		//private _UnitType type;
		public virtual _UnitType GetUnitType(){ return _UnitType.Tower; }
		public virtual bool IsTower(){ return false; }
		public virtual bool IsCreep(){ return false; }
		public virtual UnitTower GetTower(){ return null; }
		public virtual UnitCreep GetCreep(){ return null; }
		
		//for creep that use stop to attack
		public virtual void CreepAttackCount(){  }
		public virtual bool CreepIsOnAttackCD(){ return false; }
		public virtual float GetCreepSpeedMul(){ return 1; }
		
		
		public virtual bool IsPreview(){ return false; }
		public virtual bool InConstruction(){ return false; }
		
		public virtual void Destroyed(bool spawnEffDestroyed=true){ }
		
		
		
		public virtual bool IsTurret(){ return false; }
		public virtual bool IsAOE(){ return false; }
		public virtual bool IsSupport(){ return false; }
		public virtual bool IsResource(){ return false; }
		public virtual bool IsMine(){ return false; }
		public virtual bool IsSpawner(){ return false; }
		
		
		//public bool isTurret=true;
		//public bool isAOE=false;
		// public bool isSupport=false;
		//public bool isResource=false;
		//public bool isMine=false;
		//public bool isSpawner=false;
		
		[Space(5)]
		public bool resetTargetOnAttack=false;
		private bool targetReset=true;
		
		
		
		
		
		//for tower only
		public virtual UnitTower._TargetGroup GetTargetGroup(){ return UnitTower._TargetGroup.All; }
		
		//For creep only
		public virtual bool IsFlying(){ return false; }
		
		public virtual float GetPathDist(){ return 0; }
		public virtual int GetWPIdx(){ return 0; }
		public virtual int GetSubWPIdx(){ return 0; }
		public virtual float GetDistToNextWP(){ return 0; }
		public virtual float GetDistToTargetPos(){ return 0; }
		
		
		public int prefabID=-1;
		public int instanceID=-1;
		public string unitName="";
		public Sprite icon;
		public string desp="unit description";
		
		[Space(5)] 
		public float unitRadius=.25f;
		public Transform targetPoint;
		public float GetRadius(){ return unitRadius; }
		public Vector3 GetTargetPoint(){ return targetPoint!=null ? targetPoint.position : GetT().position; }
		
		[Space(10)]
		public float hp=10;
		
		public float sh=0;
		public float shStagger=0;
		
		public float cooldown=0;
		//public float cooldownAttack=0;
		//public float cooldownAOE=0;
		//public float cooldownRsc=0;
		//public float cooldownSpawner=0;
		
		public int level=0;
		public List<Stats> statsList=new List<Stats>{ new Stats() };
		
		
		public List<int> effectImmunityList=new List<int>();
		
		
		[Space(10)] public Unit attackTarget;
		public ShootObject shootObject;
		public List<Transform> shootPoint=new List<Transform>();
		public float shootPointSpacing=.2f;
		
		
		public Transform turretPivot;
		public Transform barrelPivot;
		public bool aimInXAxis=true;
		
		private Quaternion turretDefaultRot;
		private Quaternion barrelDefaultRot;
		
		
		public bool snapAiming=true;
		public float aimSpeed=20;
		public bool aimed=false;
		public void SetAim(bool flag){ aimed=(flag | snapAiming | turretPivot==null); }
		private float aimCD=0;
		
		
		
		protected GameObject thisObj;	public GameObject GetObj(){ return thisObj; }
		protected Transform thisT;		public Transform GetT(){ return thisT; }
		public Vector3 GetPos(){ return thisT!=null ? thisT.position : transform.position ; }
		
		public virtual void Awake(){
			thisT=transform;
			thisObj=gameObject;
			
			bool checkCD=IsTurret() || IsAOE() || IsResource() || IsSpawner();
			bool checkHit=IsTurret() || IsAOE() || IsMine();
			
			if(statsList.Count==0) statsList.Add(new Stats());
			for(int i=0; i<statsList.Count; i++) statsList[i].VerifyBaseStats(true, checkCD, checkHit);
			
			if(shootPoint.Count==0) shootPoint.Add(thisT);
			for(int i=0; i<shootPoint.Count; i++){ if(shootPoint[i]==null) shootPoint.RemoveAt(i); }
			
			activeEffectMod=new Effect();	activeEffectMod.SetAsModifier();
			activeEffectMul=new Effect();	activeEffectMul.SetAsMultiplier();
			
			if(snapAiming || turretPivot==null) aimed=true;
			
			if(turretPivot!=null) turretDefaultRot=turretPivot.localRotation;
			if(barrelPivot!=null) barrelDefaultRot=barrelPivot.localRotation;
			
			InitAnimation();
		}
		
		IEnumerator Start(){
			if(!IsTower()) yield break;
			yield return null;
			
			GetTower().Init();
		}
		
		
		public virtual void Update(){
			aimCD-=Time.deltaTime;
			if(!IsStunned() && aimCD<0 && IsTurret() && !IsPreview() && !InConstruction()) Aim();
		}
		
		
		public virtual void FixedUpdate(){
			if(IsPreview()) return;
			
			if(!GameControl.HasGameStarted() && GameControl.IsGamePaused()) return;
			
			cooldown-=Time.fixedDeltaTime;
			//cooldownAttack-=Time.fixedDeltaTime;
			//cooldownAOE-=Time.fixedDeltaTime;
			//cooldownRsc-=Time.fixedDeltaTime;
			//cooldownSpawner-=Time.fixedDeltaTime;
			
			IterateEffect();
			
			if(!InConstruction() && !IsDestroyed()){
				shStagger-=Time.fixedDeltaTime;
				if(shStagger<=0 && sh<GetFullSH()) sh+=GetSHRegen()*Time.fixedDeltaTime;
				
				sh+=GetEffSHRate() * Time.fixedDeltaTime;
				
				float hpRate=GetEffHPRate();
				if(hpRate>0){
					hp += hpRate * Time.fixedDeltaTime;
				}
				else{
					hpRate=ApplyShieldDamage(-hpRate);
					if(hpRate>0){
						hp -= hpRate * Time.fixedDeltaTime;
						if(hp<=0){ Destroyed(); return; }
					}
				}
				
				//hp+=GetEffHPRate() * Time.fixedDeltaTime;
				//if(hp<=0){ Destroyed(); return; }
				//if(GetEffHPRate()<0 || GetEffSHRate()<0) shStagger=GetSHStagger();
			}
			
			if(IsTurret() && !IsStunned() && !InConstruction() && !IsDestroyed()){	//if game is not paused
				ScanForTarget();
				Attack();
			}
		}
		
		
		protected bool turretShifted=false;
		protected bool resetingAim=false;
		protected IEnumerator ResetAim(){
			if(!turretShifted) yield break;
			if(turretPivot==null) yield break;
			
			Debug.Log("ResetAim");
			
			resetingAim=true;
			turretShifted=false;
			
			while(true){
				if(attackTarget!=null) break;
				
				bool aimReset=true;
				
				if(turretPivot!=thisT){
					turretPivot.localRotation=Quaternion.Lerp(turretPivot.localRotation, turretDefaultRot, aimSpeed*Time.deltaTime*.5f);
					aimReset=Quaternion.Angle(turretPivot.localRotation,turretDefaultRot)<1;
				}
				
				if(barrelPivot!=null){
					barrelPivot.localRotation=Quaternion.Lerp(barrelPivot.localRotation, barrelDefaultRot, aimSpeed*Time.deltaTime*.25f);
					aimReset=aimReset & Quaternion.Angle(barrelPivot.localRotation, barrelDefaultRot)<1;
				}
				
				if(aimReset) break;
				
				yield return null;
			}
			
			resetingAim=false;
		}
		
		public void Aim(){
			if(attackTarget==null){ SetAim(false); return; }
			if(turretPivot==null){ SetAim(true); return; }
			
			turretShifted=true;
			
			Vector3 tgtPoint=attackTarget.GetTargetPoint();
			float elevation=shootObject.GetElevationAngle(shootPoint[0].position, tgtPoint);
			
			if(!aimInXAxis || barrelPivot!=null) tgtPoint.y=turretPivot.position.y;
			Quaternion wantedRot=Quaternion.LookRotation(tgtPoint-turretPivot.position);
			
			if(elevation!=0 && aimInXAxis && barrelPivot==null) wantedRot*=Quaternion.Euler(elevation, 0, 0);
			
			if(snapAiming) turretPivot.rotation=wantedRot;
			else{
				turretPivot.rotation=Quaternion.Lerp(turretPivot.rotation, wantedRot, aimSpeed*Time.deltaTime);
				SetAim(Quaternion.Angle(turretPivot.rotation, wantedRot)<5);
			}
		
			if(!aimInXAxis || barrelPivot==null) return;
			Quaternion wantedRotX=Quaternion.LookRotation(attackTarget.GetTargetPoint()-barrelPivot.position);
			if(elevation!=0) wantedRotX*=Quaternion.Euler(elevation, 0, 0);
			if(snapAiming) barrelPivot.rotation=wantedRotX;
			else{
				barrelPivot.rotation=Quaternion.Lerp(barrelPivot.rotation, wantedRotX, aimSpeed*Time.deltaTime*2);
			}
		}
		
		
		public enum _TargetMode{ NearestToDestination, NearestToSelf, MostHP, LeastHP, Random, }
		public _TargetMode targetMode;
		public void CycleTargetMode(){ targetMode=(_TargetMode)(((int)targetMode+1)%5); }
		
		public float targetingFov=360;
		public float targetingDir=0;
		public bool UseDirectionalTargeting(){ return targetingFov>0 && targetingFov<360; }
		
		public void ScanForTarget(){
			if(attackTarget!=null){
				if(attackTarget.IsDestroyed()) attackTarget=null;
				else{
					float dist=Vector3.Distance(GetPos(), attackTarget.GetPos());
					if(dist>GetDetectionRange(attackTarget)) attackTarget=null;
					else return;
				}
			}
			
			if(CreepIsOnAttackCD()) return;	//for creep only
			//if(cooldownAttack>0) return;
			
			List<Unit> unitList=null;
			if(IsTower()) unitList=SpawnManager.GetUnitsWithinRange(this, GetAttackRange(), GetTargetGroup());
			else unitList=TowerManager.GetUnitsWithinRange(this, GetAttackRange());
			
			if(targetingFov>0 && targetingFov<360){
				Quaternion curDir=thisT.rotation*Quaternion.Euler(0, targetingDir, 0);
				for(int i=0; i<unitList.Count; i++){
					Quaternion dirToTarget=Quaternion.LookRotation(unitList[i].GetPos()-GetPos());
					if(Quaternion.Angle(curDir, dirToTarget)>targetingFov*0.5f){ unitList.RemoveAt(i);	i-=1; }
				}
			}
			
			if(unitList.Count<=0) return;
			
			if(IsCreep() && targetMode==_TargetMode.NearestToDestination) targetMode=_TargetMode.Random;
			
			int newTargetIdx=-1;
			
			if(unitList.Count==1){
				newTargetIdx=0;
			}
			else if(targetMode==_TargetMode.Random){
				newTargetIdx=Random.Range(0, unitList.Count);
			}
			else if(targetMode==_TargetMode.NearestToSelf){
				float nearest=Mathf.Infinity;
				for(int i=0; i<unitList.Count; i++){
					float dist=Vector3.Distance(GetPos(), unitList[i].GetPos());
					if(dist<nearest){ newTargetIdx=i; nearest=dist; }
				}
			}
			else if(targetMode==_TargetMode.MostHP){
				float mostHP=0;
				for(int i=0; i<unitList.Count; i++){
					if(unitList[i].hp+unitList[i].sh>mostHP){ newTargetIdx=i; mostHP=unitList[i].hp+unitList[i].sh; }
				}
			}
			else if(targetMode==_TargetMode.LeastHP){
				float leastHP=Mathf.Infinity;
				for(int i=0; i<unitList.Count; i++){
					if(unitList[i].hp+unitList[i].sh<leastHP){ newTargetIdx=i; leastHP=unitList[i].hp+unitList[i].sh; }
				}
			}
			else if(targetMode==_TargetMode.NearestToDestination){
				float pathDist=Mathf.Infinity; int furthestWP=0; int furthestSubWP=0; float distToDest=Mathf.Infinity;
				for(int i=0; i<unitList.Count; i++){
					float pDist=unitList[i].GetPathDist();
					int wpIdx=unitList[i].GetWPIdx();
					int subWpIdx=unitList[i].GetSubWPIdx();
					float tgtDistToDest=unitList[i].GetDistToTargetPos();
					
					if(pDist<pathDist){
						newTargetIdx=i; pathDist=pDist; furthestWP=wpIdx; furthestSubWP=subWpIdx; distToDest=tgtDistToDest;
					}
					else if(pDist==pathDist){
						if(furthestWP<wpIdx){
							newTargetIdx=i; pathDist=pDist; furthestWP=wpIdx; furthestSubWP=subWpIdx; distToDest=tgtDistToDest;
						}
						else if(furthestWP==wpIdx){
							if(furthestSubWP<subWpIdx){
								newTargetIdx=i; pathDist=pDist; furthestWP=wpIdx; furthestSubWP=subWpIdx; distToDest=tgtDistToDest;
							}
							else if(furthestSubWP==subWpIdx && tgtDistToDest<distToDest){
								newTargetIdx=i; pathDist=pDist; furthestWP=wpIdx; furthestSubWP=subWpIdx; distToDest=tgtDistToDest;
							}
						}
					}
				}
			}
			
			
			if(newTargetIdx>=0){
				attackTarget=unitList[newTargetIdx];
				if(snapAiming) Aim();
			}
		}
		public float GetDetectionRange(Unit tgtUnit){ return GetAttackRange()+tgtUnit.GetRadius(); }
		
		
		public void Attack(){
			//if game is not paused
			//if(!GameControl.IsGamePaused()) cooldown-=Time.fixedDeltaTime;
			
			//if(cooldownAttack>0) return;
			if(cooldown>0) return;
			
			if(resetTargetOnAttack && !targetReset){
				targetReset=true;
				attackTarget=null;
				ScanForTarget();
			}
			
			if(!aimed) return;
			if(attackTarget==null) return;
			
			targetReset=false;
			cooldown=GetCooldown();
			//cooldownAttack=GetCooldown();
			
			StartCoroutine(Shoot(new AttackInfo(this, attackTarget)));
			
			CreepAttackCount();
		}
		
		
		IEnumerator Shoot(AttackInfo aInfo){
			float attackDelay=AnimPlayAttack();
			if(attackDelay>0) yield return new WaitForSeconds(attackDelay);
			
			for(int i=0; i<shootPoint.Count; i++){
				//GameObject sObj=(GameObject)Instantiate(shootObject.gameObject, shootPoint[i].position, Quaternion.identity);
				GameObject sObj=ObjectPoolManager.Spawn(shootObject.gameObject, shootPoint[i].position, Quaternion.identity);
				ShootObject soInstance=sObj.GetComponent<ShootObject>();
				aimCD=soInstance.aimCooldown;
				if(i==shootPoint.Count-1) soInstance.InitShoot(aInfo, shootPoint[i]);
				else soInstance.InitShoot(aInfo.tgtUnit, shootPoint[i]);
				yield return new WaitForSeconds(shootPointSpacing);
			}
		}
		
		
		
		public void ApplyAttack(AttackInfo aInfo){
			if(aInfo.aoeRange>0){
				if(aInfo.srcUnit.IsTower()){
					List<Unit> tgtList=SpawnManager.GetUnitsWithinRange(this, aInfo.aoeRange);
					for(int i=0; i<tgtList.Count; i++){
						if(tgtList[i]==this) continue;
						tgtList[i].ApplyAttack(new AttackInfo(aInfo.srcUnit, tgtList[i], false));
					}
				}
			}
			
			if(IsDestroyed()) return;
			
			if(aInfo.damage>0){
				if(aInfo.hit){
					TDTK.TextOverlay(aInfo.damage.ToString(), GetTargetPoint());
					
					AnimPlayHit();
					
					//~ if(sh>0){
						//~ if(aInfo.damage<sh){
							//~ sh-=aInfo.damage;
							//~ aInfo.damage=0;
						//~ }
						//~ else{
							//~ aInfo.damage-=sh;
							//~ sh=0;
						//~ }
					//~ }
					aInfo.damage=ApplyShieldDamage(aInfo.damage);
					shStagger=GetSHStagger();
					
					hp-=aInfo.damage;
				}
				else TDTK.TextOverlay("missed", GetTargetPoint());
				
				if(hp<=0){
					Destroyed();
					return;
				}
			}
			
			if(aInfo.UseEffect()){
				for(int i=0; i<aInfo.effectList.Count; i++) ApplyEffect(aInfo.effectList[i]);
				//ApplyEffect(aInfo.effect);
			}
		}
		
		private float ApplyShieldDamage(float dmg){
			if(sh<=0) return dmg;
			
			if(dmg<sh){
				sh-=dmg;
				dmg=0;
			}
			else{
				dmg-=sh;
				sh=0;
			}
			
			return dmg;
		}
		
		
		
		#region stats
		public float GetHP(){ return hp; }
		public float GetHPRatio(){ float hpFull=GetFullHP(); return hpFull>0 ? hp/hpFull : 0;	}
		
		public float GetSH(){ return sh; }
		public float GetSHRatio(){ float shFull=GetFullSH(); return shFull>0 ? sh/shFull : 0;	}
		
		public int GetArmorType(){		return statsList[level].armorType; }
		public int GetDamageType(){	return statsList[level].damageType; }
		
		public float GetFullHP(){ 			return (statsList[level].hp + GetModHP()) * GetMulHP(); }
		public float GetFullSH(){			return (statsList[level].sh + GetModSH()) * GetMulSH(); }
		public float GetSHRegen(){ 		return (statsList[level].shRegen + GetModSHRegen()) * GetMulSHRegen(); }
		public float GetSHStagger(){	return (statsList[level].shStagger + GetModSHStagger()) * GetMulSHStagger(); }
		
		public float GetSpeed(){ 		return (statsList[level].speed + GetModSpeed()) * GetMulSpeed() * GetCreepSpeedMul();  }
		
		public float GetDamageMin(){ 	return (statsList[level].damageMin + GetModDmgMin()) * GetMulDmgMin(); }
		public float GetDamageMax(){ 	return (statsList[level].damageMax + GetModDmgMax()) * GetMulDmgMax(); }
		public float GetAttackRange(){ return (statsList[level].attackRange + GetModAttackRange()) * GetMulAttackRange(); }
		public float GetAOERange(){ 	return (statsList[level].aoeRange + GetModAOE()) * GetMulAOE(); }
		public float GetCooldown(){ 	return (statsList[level].cooldown + GetModCD()) * GetMulCD(); }
		
		public float GetHit(){ 				return (statsList[level].hit + GetModHit()) * GetMulHit(); }
		public float GetCritChance(){ 	return (statsList[level].critChance + GetModCritChance()) * GetMulCritChance(); }
		public float GetCritMultiplier(){	return (statsList[level].critMultiplier + GetModCritMul()) * GetMulCritMul(); }
		
		public float GetDodge(){ 		return Mathf.Max((statsList[level].dodge + GetModDodge()) * GetMulDodge(), 0); }
		public float GetDmgReduction(){ 	return Mathf.Clamp((statsList[level].dmgReduc+GetModDmgReduc()) * GetMulDmgReduc(), 0, 1); }
		public float GetCritReduction(){	return Mathf.Max((statsList[level].critReduc + GetModCritReduc()) * GetMulCritReduc(), 0); }
		public bool GetImmunedToCrit(){	return statsList[level].critReduc==Mathf.Infinity; }
		
		public List<float> GetRscGain(){ 
			List<float> list=RscManager.ApplyModifier(new List<float>(statsList[level].rscGain), activeEffectMod.stats.rscGain);
			list=RscManager.ApplyModifier(list, PerkManager.GetModUnitRscGain(prefabID));
			
			list=RscManager.ApplyModifier(list, activeEffectMul.stats.rscGain);
			return RscManager.ApplyModifier(list, PerkManager.GetMulUnitRscGain(prefabID));
		}
		
		
		public float GetModHP(){ return activeEffectMod.stats.hp + PerkManager.GetModUnitHP(prefabID); }
		public float GetModSH(){ return activeEffectMod.stats.sh + PerkManager.GetModUnitSH(prefabID); }
		public float GetModSHRegen(){ return activeEffectMod.stats.shRegen + PerkManager.GetModUnitSHRegen(prefabID); }
		public float GetModSHStagger(){ return activeEffectMod.stats.shStagger + PerkManager.GetModUnitSHStagger(prefabID); }
		
		public float GetModSpeed(){ return activeEffectMod.stats.speed; }// + PerkManager.GetModUnitSpeed(prefabID); }
		
		public float GetModDmgMin(){ return activeEffectMod.stats.damageMin + PerkManager.GetModUnitDmgMin(prefabID); }
		public float GetModDmgMax(){ return activeEffectMod.stats.damageMax + PerkManager.GetModUnitDmgMax(prefabID); }
		public float GetModAttackRange(){ return activeEffectMod.stats.attackRange + PerkManager.GetModUnitAttackRange(prefabID); }
		public float GetModAOE(){ return activeEffectMod.stats.aoeRange + PerkManager.GetModUnitAOE(prefabID); }
		public float GetModCD(){ return activeEffectMod.stats.cooldown + PerkManager.GetModUnitCD(prefabID); }
		
		public float GetModHit(){ return activeEffectMod.stats.hit + PerkManager.GetModUnitHit(prefabID); }
		public float GetModDodge(){ return activeEffectMod.stats.dodge + PerkManager.GetModUnitDodge(prefabID); }
		public float GetModCritChance(){ return activeEffectMod.stats.critChance + PerkManager.GetModUnitCrit(prefabID); }
		public float GetModCritMul(){ return activeEffectMod.stats.critMultiplier + PerkManager.GetModUnitCritMul(prefabID); }
		
		public float GetModDmgReduc(){ return activeEffectMod.stats.dmgReduc + PerkManager.GetModUnitDmgReduc(prefabID); }
		public float GetModCritReduc(){ return activeEffectMod.stats.critReduc + PerkManager.GetModUnitCritReduc(prefabID); }
		
		
		public float GetMulHP(){ return activeEffectMul.stats.hp * PerkManager.GetMulUnitHP(prefabID); }
		public float GetMulSH(){ return activeEffectMul.stats.sh * PerkManager.GetMulUnitSH(prefabID); }
		public float GetMulSHRegen(){ return activeEffectMul.stats.shRegen * PerkManager.GetMulUnitSHRegen(prefabID); }
		public float GetMulSHStagger(){ return activeEffectMul.stats.shStagger * PerkManager.GetMulUnitSHStagger(prefabID); }
		
		public float GetMulSpeed(){ return activeEffectMul.stats.speed; }// * PerkManager.GetMulUnitSpeed(prefabID); }
		
		public float GetMulDmgMin(){ return activeEffectMul.stats.damageMin * PerkManager.GetMulUnitDmgMin(prefabID); }
		public float GetMulDmgMax(){ return activeEffectMul.stats.damageMax * PerkManager.GetMulUnitDmgMax(prefabID); }
		public float GetMulAttackRange(){ return activeEffectMul.stats.attackRange * PerkManager.GetMulUnitAttackRange(prefabID); }
		public float GetMulAOE(){ return activeEffectMul.stats.aoeRange * PerkManager.GetMulUnitAOE(prefabID); }
		public float GetMulCD(){ return activeEffectMul.stats.cooldown * PerkManager.GetMulUnitCD(prefabID); }
		
		public float GetMulHit(){ return activeEffectMul.stats.hit * PerkManager.GetMulUnitHit(prefabID); }
		public float GetMulDodge(){ return activeEffectMul.stats.dodge * PerkManager.GetMulUnitDodge(prefabID); }
		public float GetMulCritChance(){ return activeEffectMul.stats.critChance * PerkManager.GetMulUnitCrit(prefabID); }
		public float GetMulCritMul(){ return activeEffectMul.stats.critMultiplier * PerkManager.GetMulUnitCritMul(prefabID); }
		
		public float GetMulDmgReduc(){ return activeEffectMul.stats.dmgReduc * PerkManager.GetMulUnitDmgReduc(prefabID); }
		public float GetMulCritReduc(){ return activeEffectMul.stats.critReduc * PerkManager.GetMulUnitCritReduc(prefabID); }
		
		
		
		public bool GetEffStun(){ return activeEffectMod.stun; }
		public float GetEffHPRate(){ return activeEffectMod.stats.hpRate * activeEffectMul.stats.hpRate; }
		public float GetEffSHRate(){ return activeEffectMod.stats.shRate * activeEffectMul.stats.shRate; }
		
		
		public float GetEffectOnHitChance(){ return (statsList[level].effectOnHitChance + GetModEffectOnHitC()) * GetMulEffectOnHitC(); }
		public float GetModEffectOnHitC(){ return activeEffectMod.stats.effectOnHitChance + PerkManager.GetModUnitEffOnHitChance(prefabID); }
		public float GetMulEffectOnHitC(){ return activeEffectMul.stats.effectOnHitChance * PerkManager.GetMulUnitEffOnHitChance(prefabID); }
		
		
		private List<int> cachedEffOnHitList=null;
		public List<Effect> GetEffectOnHit(){
			List<Effect> list=new List<Effect>();
			
			if(IsTower()){
				List<int> overrideIDList=PerkManager.GetUnitOverrideOnHitEff(prefabID);
				if(overrideIDList!=null){
					for(int i=0; i<overrideIDList.Count; i++){
						list.Add(EffectDB.GetPrefab(overrideIDList[i]).ModifyWithPerk());	//modify with perk would return a cloned effect
					}
					return list;
				}
				
				for(int i=0; i<statsList[level].effectOnHitIDList.Count; i++){
					list.Add(EffectDB.GetPrefab(statsList[level].effectOnHitIDList[i]).ModifyWithPerk());	//modify with perk would return a cloned effect
				}
				
				List<int> appendIDList=PerkManager.GetUnitAppendOnHitEff(prefabID);
				if(appendIDList!=null){
					for(int i=0; i<appendIDList.Count; i++){
						list.Add(EffectDB.GetPrefab(appendIDList[i]).ModifyWithPerk());	//modify with perk would return a cloned effect
					}
				}
			}
			else{
				if(statsList[level].effectOnHitIDList.Count<0) return null;
				
				if(cachedEffOnHitList==null){
					cachedEffOnHitList=new List<int>();
					for(int i=0; i<statsList[level].effectOnHitIDList.Count; i++){
						Effect effect=EffectDB.GetPrefab(statsList[level].effectOnHitIDList[i]).Clone();
						cachedEffOnHitList.Add(EffectDB.GetPrefabIndex(effect.prefabID));
					}
				}
				else{
					for(int i=0; i<cachedEffOnHitList.Count; i++) list.Add(EffectDB.GetItem(cachedEffOnHitList[i]).Clone());
				}
			}
			
			for(int i=0; i<list.Count; i++) list[i].SetType(level, this);
			return list;
		}
		
		/*
		private int cachedEffOnHitIdx=-1;
		public Effect GetEffectOnHit(){
			if(IsTower()){
				int overrideID=PerkManager.GetUnitOverrideOnHitEff(prefabID);
				if(overrideID<0 && statsList[level].effectOnHitID<0) return null;
				
				Effect effect=EffectDB.GetPrefab(overrideID>=0 ? overrideID : statsList[level].effectOnHitID).ModifyWithPerk();
				effect.SetType(level, this);
				return effect;
			}
			else{
				if(statsList[level].effectOnHitID<0) return null;
				
				Effect effect=null;
				if(cachedEffOnHitIdx<0){
					effect=EffectDB.GetPrefab(statsList[level].effectOnHitID).Clone();
					cachedEffOnHitIdx=EffectDB.GetPrefabIndex(effect.prefabID);
				}
				else{
					effect=EffectDB.GetItem(cachedEffOnHitIdx).Clone();
					//Debug.Log("use cached "+cachedEffOnHitIdx+"   "+effect.name);
				}
				
				effect.SetType(level, this);
				return effect;
			}
		}
		*/
		#endregion
		
	
		
		#region Effect
		public Effect activeEffectMod;
		public Effect activeEffectMul;
		public List<Effect> allEffectList=new List<Effect>();
		
		public void ClearAllEffect(){		//called when destroyed
			activeEffectMod=new Effect();	activeEffectMod.SetAsModifier();
			activeEffectMul=new Effect();	activeEffectMul.SetAsMultiplier();
			allEffectList=new List<Effect>();
		}
		
		public void ApplyEffect(Effect effect){
			if(effectImmunityList.Contains(effect.prefabID)) return;
			
			if(!effect.stackable){
				for(int i=0; i<allEffectList.Count; i++){
					if(Effect.FromSimilarSource(allEffectList[i], effect)){
						allEffectList[i].durationRemain=effect.duration;
						return;
					}
				}
			}
			effect.durationRemain=effect.duration;
			allEffectList.Add(effect);
			UpdateActiveEffect();
		}
		public void IterateEffect(){
			bool update=false;
			for(int i=0; i<allEffectList.Count; i++){
				allEffectList[i].durationRemain-=Time.fixedDeltaTime;
				if(allEffectList[i].durationRemain<=0){
					allEffectList.RemoveAt(i);	i-=1;
					update=true;
				}
			}
			if(update) UpdateActiveEffect();
		}
		public void UpdateActiveEffect(){
			activeEffectMod=new Effect();	activeEffectMod.SetAsModifier();
			activeEffectMul=new Effect();	activeEffectMul.SetAsMultiplier();
			
			for(int i=0; i<allEffectList.Count; i++){
				activeEffectMod.stun |= allEffectList[i].stun;
				
				if(!allEffectList[i].IsMultiplier()){
					activeEffectMod.stats.hp += allEffectList[i].stats.hp;
					activeEffectMod.stats.sh += allEffectList[i].stats.sh;
					
					activeEffectMod.stats.hpRate += allEffectList[i].stats.hpRate * DamageTable.GetModifier(GetArmorType(), allEffectList[i].stats.damageType);
					activeEffectMod.stats.shRate += allEffectList[i].stats.shRate;
					
					activeEffectMod.stats.speed += allEffectList[i].stats.speed;
					
					activeEffectMod.stats.damageMin += allEffectList[i].stats.damageMin;
					activeEffectMod.stats.damageMax += allEffectList[i].stats.damageMax;
					activeEffectMod.stats.attackRange += allEffectList[i].stats.attackRange;
					activeEffectMod.stats.aoeRange += allEffectList[i].stats.aoeRange;
					activeEffectMod.stats.cooldown += allEffectList[i].stats.cooldown;
					
					activeEffectMod.stats.hit += allEffectList[i].stats.hit;
					activeEffectMod.stats.dodge += allEffectList[i].stats.dodge;
					activeEffectMod.stats.critChance += allEffectList[i].stats.critChance;
					activeEffectMod.stats.critMultiplier += allEffectList[i].stats.critMultiplier;
					
					activeEffectMod.stats.dmgReduc += allEffectList[i].stats.dmgReduc;
					activeEffectMod.stats.critReduc += allEffectList[i].stats.critReduc;
					
					activeEffectMod.stats.effectOnHitChance += allEffectList[i].stats.effectOnHitChance;
					
					for(int n=0; n<activeEffectMod.stats.rscGain.Count; n++) 
						activeEffectMod.stats.rscGain[n] += allEffectList[i].stats.rscGain[n];
				}
				else{
					activeEffectMul.stats.hp *= allEffectList[i].stats.hp;
					activeEffectMul.stats.sh *= allEffectList[i].stats.sh;
					activeEffectMul.stats.hpRate *= allEffectList[i].stats.hpRate;
					activeEffectMul.stats.shRate *= allEffectList[i].stats.shRate;
					
					activeEffectMul.stats.speed *= allEffectList[i].stats.speed;
					
					activeEffectMul.stats.damageMin *= allEffectList[i].stats.damageMin;
					activeEffectMul.stats.damageMax *= allEffectList[i].stats.damageMax;
					activeEffectMul.stats.attackRange *= allEffectList[i].stats.attackRange;
					activeEffectMul.stats.aoeRange *= allEffectList[i].stats.aoeRange;
					activeEffectMul.stats.cooldown *= allEffectList[i].stats.cooldown;
					
					activeEffectMul.stats.hit *= allEffectList[i].stats.hit;
					activeEffectMul.stats.dodge *= allEffectList[i].stats.dodge;
					activeEffectMul.stats.critChance *= allEffectList[i].stats.critChance;
					activeEffectMul.stats.critMultiplier *= allEffectList[i].stats.critMultiplier;
					
					activeEffectMul.stats.dmgReduc *= allEffectList[i].stats.dmgReduc;
					activeEffectMul.stats.critReduc *= allEffectList[i].stats.critReduc;
					
					activeEffectMul.stats.effectOnHitChance *= allEffectList[i].stats.effectOnHitChance;
					
					for(int n=0; n<activeEffectMul.stats.rscGain.Count; n++) 
						activeEffectMul.stats.rscGain[n] *= allEffectList[i].stats.rscGain[n];
				}
			}
		}
		#endregion
		
		
		public bool IsStunned(){ return GetEffStun(); }
		
		public bool IsDestroyed(){ return hp<=0 || !thisObj.activeInHierarchy; }
		
		
		#region animation
		[Header("Animation")]
		public Transform animatorT;
		protected Animator animator;
		
		[Space(5)]
		public AnimationClip clipIdle;
		public AnimationClip clipHit;
		public AnimationClip clipDestroyed;
		
		public AnimationClip clipAttack;
		public float animationAttackDelay=0;
		
		[Space(5)]
		public AnimationClip clipMove;
		public AnimationClip clipSpawn;
		public AnimationClip clipDestination;
		
		[Space(5)]
		public AnimationClip clipConstruct;
		public AnimationClip clipDeconstruct;
		
		//private bool defaultControllerLoaded=false;
		//private static AnimatorController defaultController;
		
		
		void InitAnimation(){
			if(animatorT!=null) animator=animatorT.GetComponent<Animator>();
			if(animator==null) return;
			
			//if(!defaultControllerLoaded){
			//	defaultControllerLoaded=true;
			//	defaultController=Resources.Load("DB_TDTK/TDAnimatorController.controller", typeof(AnimatorController)) as AnimatorController;
			//}
			//if(defaultController!=null) animator.runtimeAnimatorController=defaultController;
			
			
			//AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
			AnimatorOverrideController aniOverrideController = new AnimatorOverrideController();
			aniOverrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
			animator.runtimeAnimatorController = aniOverrideController;
			
			if(clipIdle!=null) 				aniOverrideController["DummyIdle"] = clipIdle;
			if(clipHit!=null) 				aniOverrideController["DummyHit"] = clipHit;
			if(clipAttack!=null) 			aniOverrideController["DummyAttack"] = clipAttack;
			if(clipDestroyed!=null) 	aniOverrideController["DummyDestroyed"] = clipDestroyed;
			
			if(clipMove!=null) 			aniOverrideController["DummyMove"] = clipMove;
			if(clipSpawn!=null) 			aniOverrideController["DummySpawn"] = clipSpawn;
			if(clipDestination!=null) 	aniOverrideController["DummyDestination"] = clipDestination;
			
			if(clipConstruct!=null) 	aniOverrideController["DummyConstruct"] = clipConstruct;
			if(clipDeconstruct!=null) 	aniOverrideController["DummyDeconstruct"] = clipDeconstruct;
		}
		
		protected void AnimPlayMove(float speed){ 
			if(animator!=null) animator.SetFloat("Speed", speed);
		}
		protected void AnimPlayHit(){
			if(animator!=null && clipHit!=null) animator.SetTrigger("Hit");
		}
		protected float AnimPlayDestroyed(){
			if(animator==null) return 0;
			if(clipDestroyed!=null) animator.SetBool("Destroyed", true);
			return clipDestroyed!=null ? clipDestroyed.length : 0 ;
		}
		protected float AnimPlayAttack(){
			if(animator==null) return 0;
			if(clipAttack!=null) animator.SetTrigger("Attack");
			return animationAttackDelay;
		}
		
		protected void AnimPlaySpawn(){ 
			if(animator!=null && clipSpawn!=null) animator.SetTrigger("Spawn");
		}
		protected float AnimPlayDestination(){
			if(animator==null) return 0;
			if(clipDestination!=null) animator.SetBool("Destination", true);
			return clipDestination!=null ? clipDestination.length : 0 ;
		}
			
		protected void AnimPlayConstruct(){
			if(animator!=null && clipConstruct!=null) animator.SetTrigger("Construct");
		}
		protected void AnimPlayDeconstruct(){
			if(animator!=null && clipDeconstruct!=null) animator.SetTrigger("Deconstruct");
		}
		
		
		protected void AnimReset(){
			if(animator==null) return;
			if(!animator.isInitialized) return;
			animator.SetBool("Destroyed", false);
			animator.SetBool("Destination", false);
		}
		#endregion
		
		
		
		public virtual void OnDrawGizmos(){
			Gizmos.color=Color.red;
			if(attackTarget!=null){
				Debug.DrawLine(GetPos(), attackTarget.GetPos());
			}
			
			/*
			if(UseDirectionalTargeting()){
				Vector3 v1=thisT.rotation*Quaternion.Euler(0, targetingDir+targetingFov/2, 0)*new Vector3(0, 0, 3);
				Vector3 v2=thisT.rotation*Quaternion.Euler(0, targetingDir-targetingFov/2, 0)*new Vector3(0, 0, 3);
				Debug.DrawLine(GetPos(), GetPos()+v1);
				Debug.DrawLine(GetPos(), GetPos()+v2);
			}
			*/
		}
	}
	
}