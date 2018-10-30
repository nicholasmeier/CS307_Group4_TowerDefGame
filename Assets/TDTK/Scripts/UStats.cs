using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDTK{
	
	[System.Serializable]
	public class Stats {	//generic stats container for tower, creep and ability (used as multiplier for perks)
		[Header("For tower only")]
		public float buildDuration=1f;
		public float sellDuration=1f;
		public float expToNextUpgrade=10;	
		
		[Header("For Tower and Ability")]
		public List<float> cost=new List<float>();
		public List<float> sellValue=new List<float>();
		
		[Header("For Unit only")]
		public float hp=5;
		
		public float sh=0;	//shield
		public float shRegen=.5f;
		public float shStagger=3f;
		
		public float hpRate=0;	//can be negate by shield and subject to damageType 
		public float shRate=0;	//doesnt subject to stagger
		
		public int armorType=0;		//only used for base stats, not for effect
		public int damageType=0;	//only used for base stats, not for effect
		
		public float speed=3;
		
		public float dodge=0;
		public float dmgReduc=0;
		public float critReduc=0;
		
		[Header("For Everything")]
		public float hit=1;
		public float damageMin=2;
		public float damageMax=2;
		public float attackRange=2;
		public float aoeRange=0;
		public float cooldown=1;
		
		public float critChance=0;
		public float critMultiplier=0;
		
		[Space(10)] 
		public float effectOnHitChance=1;
		public int effectOnHitID=-1;
		
		public bool overrideExistingEffect=false;	//for perk only
		public List<int> effectOnHitIDList=new List<int>();
		
		
		[Header("For Rsctower only")]
		public List<float> rscGain=new List<float>();
		
		
		public virtual void VerifyBaseStats(bool checkHP, bool checkCD, bool checkHit){
			if(checkHP && hp<=0){
				Debug.LogWarning("Unit hp is set as 0. Adjust it to 1 instead");
				hp=Mathf.Max(1, hp);
			}
			
			if(checkCD && cooldown<=0){
				Debug.LogWarning("Unit cooldown is set to equal or less than 0. Adjust it to 1 instead");
				cooldown=1;
			}
			
			if(checkHit && hit<=0){
				Debug.LogWarning("Unit/Item hit chance is set to equal or less than 0. Adjust it to 1 instead");
				hit=1;
			}
			
			damageMin=Mathf.Max(0, damageMin);
			damageMax=Mathf.Max(0, damageMax);
			
			VerifyRscList(0);
		}
		
		public virtual void ResetAsBaseStat(){
			Reset(false);
			hp=1;	speed=1;
			hit=1;	damageMin=1;		damageMax=1;	attackRange=1;	cooldown=1;
		}
		
		public virtual void Reset(bool isMultiplier=false){
			cost=new List<float>();	rscGain=new List<float>();
			if(isMultiplier){
				buildDuration=1;		sellDuration=1;
				hp=1;	sh=1;		shRegen=1;		shStagger=1;			speed=1;	hpRate=1;	shRate=1;
				hit=1;	dodge=1;	damageMin=1;		damageMax=1;
				attackRange=1;		aoeRange=1;		cooldown=1;
				critChance=1;			critMultiplier=1;	dmgReduc=1;			critReduc=1;	//immunedToCrit=false;
				effectOnHitChance=1;
				VerifyRscList(1);
			}
			else{
				buildDuration=0;		sellDuration=0;
				hp=0;	sh=0;		shRegen=0;		shStagger=0;			speed=0;	hpRate=0;	shRate=0;
				hit=0;	dodge=0;	damageMin=0;		damageMax=0;
				attackRange=0;		aoeRange=0;		cooldown=0;
				critChance=0;			critMultiplier=0;	dmgReduc=0;			critReduc=0;	//immunedToCrit=false;
				effectOnHitChance=0;
				VerifyRscList(0);
			}
		}
		
		
		
		public void VerifyRscList(float fillValue){ 
			cost=RscManager.MatchRscList(cost, fillValue);
			rscGain=RscManager.MatchRscList(rscGain, fillValue);
		}
		
		
		public Stats Clone(){ return ObjectCopier.Clone(this); }
	}
	
	
	
	[System.Serializable]
	public class Effect : TDItem {//Stats{		//attack buff/debuff to be apply on target
		public int ID=0;	//assigned during runtime, unique to unit-type/ability that cast it, used to determined if the effect existed on target if it's not stackable
		
		public enum _SrcType{ Tower, Ability, Creep, Perk, None}
		public _SrcType srcType=_SrcType.None;
		public int srcPrefabID=0;
		
		public bool stackable=false;
		public float duration=1;
		[HideInInspector] 
		public float durationRemain=1;	//runtime attribute, only used on applied effect
		
		public enum _EffType{ Modifier, Multiplier }
		public _EffType effType=_EffType.Multiplier;
		public bool IsMultiplier(){ return effType==_EffType.Multiplier; }
		
		public bool stun=false;
		public Stats stats=new Stats();
		
		public Effect(){} //ResetAsEffect(); }
		
		public void SetAsModifier(bool reset=true){	effType=_EffType.Modifier;	if(reset) Reset(); }
		public void SetAsMultiplier(bool reset=true){	effType=_EffType.Multiplier;	if(reset) Reset(); }
		public void Reset(){
			stun=false;
			if(effType==_EffType.Multiplier){ stats.Reset(true);		duration=1; }
			if(effType==_EffType.Modifier)	{ stats.Reset(false);	duration=0; }
		}
		
		
		public void SetType(int id, Ability ability){
			ID=id;	srcPrefabID=ability.prefabID;
			srcType=_SrcType.Ability;
		}
		public void SetType(int id, Unit unit){
			ID=id;	srcPrefabID=unit.prefabID;
			if(unit.IsTower()) srcType=_SrcType.Tower;
			if(unit.IsCreep()) srcType=_SrcType.Creep;
		}
		
		public static bool FromSimilarSource(Effect eff1, Effect eff2){
			if(eff1.ID!=eff2.ID) return false;
			if(eff1.srcType!=eff2.srcType) return false;
			if(eff1.srcPrefabID!=eff2.srcPrefabID) return false;
			return true;
		}
		
		public bool FromTower(){ return srcType==_SrcType.Tower; }
		public bool FromCreep(){ return srcType==_SrcType.Creep; }
		
		
		public Effect ModifyWithPerk(){ return PerkManager.ModifyEffect(Clone()); }
		
		
		public void ApplyModifier(Effect effMod){	//Called by PerkManager when applying changes
			stun|=effMod.stun;
			duration+=effMod.duration;
			
			stats.hp+=effMod.stats.hp;
			stats.sh+=effMod.stats.sh;
			stats.shRegen+=effMod.stats.shRegen;
			stats.shStagger+=effMod.stats.shStagger;
			
			stats.hpRate+=effMod.stats.hpRate;
			stats.shRate+=effMod.stats.shRate;
			
			stats.speed+=effMod.stats.speed;
			
			stats.damageMin+=effMod.stats.damageMin;
			stats.damageMax+=effMod.stats.damageMax;
			stats.attackRange+=effMod.stats.attackRange;
			stats.aoeRange+=effMod.stats.aoeRange;
			stats.cooldown+=effMod.stats.cooldown;
			
			stats.hit+=effMod.stats.hit;
			stats.dodge+=effMod.stats.dodge;
			stats.critChance+=effMod.stats.critChance;
			stats.critMultiplier+=effMod.stats.critMultiplier;
			
			stats.dodge+=effMod.stats.dodge;
			stats.dmgReduc+=effMod.stats.dmgReduc;
			stats.critReduc+=effMod.stats.critReduc;
			
			stats.effectOnHitChance+=effMod.stats.effectOnHitChance;
		}
		public void ApplyMultiplier(Effect effMul){		//Called by PerkManager when applying changes
			duration*=effMul.duration;
			
			stats.hp*=effMul.stats.hp;
			stats.sh*=effMul.stats.sh;
			stats.shRegen*=effMul.stats.shRegen;
			stats.shStagger*=effMul.stats.shStagger;
			
			stats.hpRate*=effMul.stats.hpRate;
			stats.shRate*=effMul.stats.shRate;
			
			stats.speed*=effMul.stats.speed;
			
			stats.damageMin*=effMul.stats.damageMin;
			stats.damageMax*=effMul.stats.damageMax;
			stats.attackRange*=effMul.stats.attackRange;
			stats.aoeRange*=effMul.stats.aoeRange;
			stats.cooldown*=effMul.stats.cooldown;
			
			stats.hit*=effMul.stats.hit;
			stats.dodge*=effMul.stats.dodge;
			stats.critChance*=effMul.stats.critChance;
			stats.critMultiplier*=effMul.stats.critMultiplier;
			
			stats.dodge*=effMul.stats.dodge;
			stats.dmgReduc*=effMul.stats.dmgReduc;
			stats.critReduc*=effMul.stats.critReduc;
			
			stats.effectOnHitChance*=effMul.stats.effectOnHitChance;
		}
		
		
		
		
		
		public Effect Clone(){ 
			Effect clone=new Effect();
			
			base.Clone(this, clone);
			
			clone.ID=ID;
			clone.srcType=srcType;
			clone.srcPrefabID=srcPrefabID;
			
			clone.stackable=stackable;
			clone.duration=duration;
			clone.effType=effType;
			
			clone.stun=stun;
			clone.stats=stats.Clone();
			
			return clone;
		}
		
	}
	
	
	
	public class AttackInfo{
		public Unit srcUnit;
		public Unit tgtUnit;
		
		public float damageMin=0;
		public float damageMax=0;
		public float aoeRange=0;
		
		public float critChance=0;
		public float critMultiplier=0;
		
		//public Effect effect;
		public List<Effect> effectList=new List<Effect>();
		
		//actual value
		public float damage=0;
		public bool hit=false;
		public bool critical=false;
		
		public bool UseEffect(){ return effectList.Count==0; }//effect!=null; }
		
		
		public AttackInfo(float dmg){ hit=true;	damage=dmg; }
		
		public AttackInfo(Unit sUnit, Unit tUnit, bool useAOE=true){
			srcUnit=sUnit;	tgtUnit=tUnit;
			
			damageMin=srcUnit.GetDamageMin();
			damageMax=srcUnit.GetDamageMax();
			aoeRange=useAOE ? srcUnit.GetAOERange() : 0;
			critChance=srcUnit.GetCritChance();
			critMultiplier=tUnit.GetImmunedToCrit() ? 0 : srcUnit.GetCritMultiplier();
			
			//~ useEffect=srcUnit.UseEffectOnHit();
			//~ if(useEffect) effect=srcUnit.GetEffectOnHit().Clone();
			if(Random.value<srcUnit.GetEffectOnHitChance()){
				effectList=srcUnit.GetEffectOnHit();
				//effect=srcUnit.GetEffectOnHit();
			}
			
			critical=Random.value<critChance;
			damage=Mathf.Round(Random.Range(damageMin, damageMax))*(critical ? critMultiplier : 1);
			
			//Debug.Log(damageMin+"  "+damageMax+"   "+damage);
			
			damage*=DamageTable.GetModifier(tgtUnit.GetArmorType(), srcUnit.GetDamageType());
			damage*=(1-tUnit.GetDmgReduction());
			
			if(Random.value<srcUnit.GetHit()-tgtUnit.GetDodge()) hit=true;
		}
		
		public AttackInfo(Ability ability, Unit tUnit, bool useAOE=true){
			tgtUnit=tUnit;
			
			damageMin=ability.GetDamageMin();
			damageMax=ability.GetDamageMax();
			aoeRange=useAOE ? ability.GetAOERange() : 0;
			critChance=ability.GetCrit();
			critMultiplier=tUnit.GetImmunedToCrit() ? 0 : ability.GetCritMultiplier();
			
			//~ useEffect=ability.UseEffectOnHit();
			//~ if(useEffect) effect=ability.GetEffectOnHit().Clone();
			if(Random.value<ability.GetEffectOnHitChance()){
				effectList=srcUnit.GetEffectOnHit();
				//effect=ability.GetEffectOnHit();
			}
			
			critical=Random.value<critChance;
			damage=Mathf.Round(Random.Range(damageMin, damageMax))*(critical ? critMultiplier : 1);
			
			damage*=DamageTable.GetModifier(tgtUnit.GetArmorType(), ability.GetDamageType());
			damage*=(1-tUnit.GetDmgReduction());
			
			if(Random.value<ability.GetHit()-tgtUnit.GetDodge()) hit=true;
		}
	}
	
}