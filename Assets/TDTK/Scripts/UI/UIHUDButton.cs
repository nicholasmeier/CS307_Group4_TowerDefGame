using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace TDTK{

	public class UIHUDButton : MonoBehaviour {
		
		public UIButton buttonSpawn;
		public Slider sliderSpawnTimer;
		
		public UIButton buttonPerk;
		
		public UIButton buttonFF;
		
		public UIButton buttonPause;
		
		
		
		// Use this for initialization
		void Start () {
			UpdateWaveDisplay(1);
			
			buttonSpawn.Init();
			buttonSpawn.SetCallback(null, null, this.OnSpawnButton, null);
			buttonSpawn.SetActive(true);
			
			sliderSpawnTimer.gameObject.SetActive(false);
			
			if(PerkManager.IsEnabled() && buttonPerk.rootObj!=null){
				buttonPerk.Init();
				buttonPerk.SetCallback(null, null, this.OnPerkButton, null);
			}
			else{
				if(buttonPerk.rootObj!=null) buttonPerk.rootObj.SetActive(false);
			}
			
			
			buttonFF.Init();
			buttonFF.SetCallback(null, null, this.OnFFButton, null);
			
			buttonPause.Init();
			buttonPause.SetCallback(null, null, this.OnPauseButton, null);
		}
		
		void OnEnable(){ 
			TDTK.onEnableSpawnE += OnEnableSpawn;
			TDTK.onSpawnCountDownE += OnSpawnCountDown;
			TDTK.onNewWaveE += UpdateWaveDisplay;
		}
		void OnDisable(){ 
			TDTK.onEnableSpawnE -= OnEnableSpawn;
			TDTK.onSpawnCountDownE -= OnSpawnCountDown;
			TDTK.onNewWaveE -= UpdateWaveDisplay;
		}
		
		
		void OnEnableSpawn(){
			buttonSpawn.SetActive(true);
		}
		void OnSpawnButton(GameObject butObj, int pointerID=-1){
			SpawnManager.Spawn();
			buttonSpawn.SetActive(false);
		}
		
		void UpdateWaveDisplay(int wave){
			buttonSpawn.SetActive(false);
		}
		
		private bool coutingDown=false;
		void OnSpawnCountDown(){
			coutingDown=true; 
			Update();
			sliderSpawnTimer.gameObject.SetActive(true);
		}
		
		void Update(){
			if(!coutingDown) return;
			
			float time=SpawnManager.GetTimeToNextWave();
			
			if(time>0){
				//lbSpawnTimer.text="Time to next wave - "+time.ToString("f1")+"s";
				
				buttonSpawn.lbMain.text="Spawn ("+time.ToString("f1")+"s)";
				sliderSpawnTimer.value=SpawnManager.GetTimeToNextWaveRatio();
			}
			else{
				buttonSpawn.lbMain.text="Spawn";
				sliderSpawnTimer.gameObject.SetActive(false);
				
				//lbSpawnTimer.text="";
				coutingDown=false;
			}
		}
		
		
		
		void OnPerkButton(GameObject butObj, int pointerID=-1){ UIPerkScreen.Show(); }
		
		
		void OnFFButton(GameObject butObj, int pointerID=-1){
			if(Time.timeScale==1){
				Time.timeScale=3;
				buttonFF.lbMain.text=">>";
			}
			else{
				Time.timeScale=1;
				buttonFF.lbMain.text=">";
			}
		}
		
		void OnPauseButton(GameObject butObj, int pointerID=-1){ UIPauseScreen.Show(); }
		
	}

}