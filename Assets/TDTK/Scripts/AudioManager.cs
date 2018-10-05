using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDTK{

	public class AudioManager : MonoBehaviour {

		private List<AudioSource> audioSourceList=new List<AudioSource>();
		
		private static AudioManager instance;
		
		//~ void Awake(){
			//~ if(instance!=null){
				//~ Destroy(gameObject);
				//~ return;
			//~ }
			
			//~ instance=this;
			
			//~ //DontDestroyOnLoad(gameObject);
			
			//~ //listener=gameObject.GetComponent<AudioListener>();
			
			
		//~ }
		
		public void Awake(){
			if(instance!=null) return;
			instance=this;
			
			CreateAudioSource();
		}
		
		
		/*
		public static AudioManager Init(){
			//if(instance!=null) return instance;
			
			AudioManager instance = (AudioManager)FindObjectOfType(typeof(AudioManager));
			if(instance==null){
				GameObject obj=new GameObject();
				obj.name="AudioManager";
				
				instance=obj.AddComponent<AudioManager>();
			}
			
			return instance;
		}
		*/
		
		
		//private bool init=false;
		void CreateAudioSource(){
			audioSourceList=new List<AudioSource>();
			for(int i=0; i<20; i++){
				GameObject obj=new GameObject("AudioSource"+(i+1));
				
				AudioSource src=obj.AddComponent<AudioSource>();
				src.playOnAwake=false; src.loop=false; src.volume=1; //src.spatialBlend=.75f;
				
				obj.transform.parent=transform; obj.transform.localPosition=Vector3.zero;
				
				audioSourceList.Add(src);
			}
		}
		
		
		//call to play a specific clip
		public static void PlaySound(AudioClip clip, Vector3 pos=default(Vector3)){ if(instance!=null) instance._PlaySound(clip, pos); }
		public void _PlaySound(AudioClip clip, Vector3 pos=default(Vector3)){
			if(clip==null) return;
			int Idx=GetUnusedAudioSourceIdx();
			audioSourceList[Idx].transform.position=pos;
			audioSourceList[Idx].clip=clip;		audioSourceList[Idx].Play();
		}
		
		//check for the next free, unused audioObject
		private int GetUnusedAudioSourceIdx(){
			for(int i=0; i<audioSourceList.Count; i++){ if(!audioSourceList[i].isPlaying) return i; }
			return 0;	//if everything is used up, use item number zero
		}
		
		
		
		
		[Header("Sound Effect")]
		public AudioClip playerWon;
		public static void OnPlayerWon(){ if(instance!=null && instance.playerWon!=null) PlaySound(instance.playerWon); }
		public AudioClip playerLost;
		public static void OnPlayerLost(){ if(instance!=null && instance.playerLost!=null) PlaySound(instance.playerLost); }
		
		public AudioClip lostLife;
		public static void OnLostLife(){ if(instance!=null && instance.lostLife!=null) PlaySound(instance.lostLife); }
		
		public AudioClip newWave;
		public AudioClip waveCleared;
		public static void OnNewWave(){ if(instance!=null && instance.newWave!=null) PlaySound(instance.newWave); }
		public static void OnWaveCleared(){ if(instance!=null && instance.waveCleared!=null) PlaySound(instance.waveCleared); }
		
		
		public AudioClip buildStart;
		public AudioClip buildComplete;
		public static void OnBuildStart(){ if(instance!=null && instance.buildStart!=null) PlaySound(instance.buildStart); }
		public static void OnBuildComplete(){ if(instance!=null && instance.buildComplete!=null) PlaySound(instance.buildComplete); }
		
		public AudioClip upgradeStart;
		public AudioClip upgradeComplete;
		public static void OnUpgradeStart(){ if(instance!=null && instance.upgradeStart!=null) PlaySound(instance.upgradeStart); }
		public static void OnUpgradeComplete(){ if(instance!=null && instance.upgradeComplete!=null) PlaySound(instance.upgradeComplete); }
		
		public AudioClip towerSold;
		public static void OnTowerSold(){ if(instance!=null && instance.towerSold!=null) PlaySound(instance.towerSold); }
		
		
		public AudioClip perkPurchased;
		public static void OnPerkPurchased(){ if(instance!=null && instance.perkPurchased!=null) PlaySound(instance.perkPurchased); }
		
		
		public AudioClip invalidAction;
		public static void OnInvalidAction(){ if(instance!=null && instance.invalidAction!=null) PlaySound(instance.invalidAction); }
		
		
	}

}