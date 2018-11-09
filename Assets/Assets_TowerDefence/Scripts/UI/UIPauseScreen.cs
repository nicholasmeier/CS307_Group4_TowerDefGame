using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TDTK;

namespace TDTK{

	public class UIPauseScreen : UIScreen {
		
		public UIButton buttonResume;
		public UIButton buttonRestart;
		public UIButton buttonMainMenu;
		
		private static UIPauseScreen instance;
		
		public override void Awake(){
			base.Awake();
			
			instance=this;
		}
		
		public override void Start(){ 
			base.Start();
			
			buttonResume.Init();
			buttonResume.SetCallback(null, null, this.OnResumeButton, null);
			
			buttonRestart.Init();
			buttonRestart.SetCallback(null, null, this.OnRestartButton, null);
			
			buttonMainMenu.Init();
			buttonMainMenu.SetCallback(null, null, this.OnMenuButton, null);
			
			thisObj.SetActive(false);
		}
		
		
		public void OnResumeButton(GameObject butObj, int pointerID=-1){
			Hide();
		}
		public void OnRestartButton(GameObject butObj, int pointerID=-1){
			GameControl.RestartLevel();
		}
		public void OnMenuButton(GameObject butObj, int pointerID=-1){
			GameControl.MainMenu();
		}
		
		
		
		
		private float cachedTimeScale=1;
		
		public static void Show(){ if(instance!=null) instance._Show(); }
		public override void _Show(float duration=0.25f){
			cachedTimeScale=Time.timeScale;
			Time.timeScale=0;
			
			UIControl.BlurFadeIn();
			
			base._Show();
		}
		public static void Hide(){ if(instance!=null) instance._Hide(); }
		public override void _Hide(float duration=0.25f){
			Time.timeScale=cachedTimeScale;
			
			UIControl.BlurFadeOut();
			
			base._Hide();
		}
		
	}

}
