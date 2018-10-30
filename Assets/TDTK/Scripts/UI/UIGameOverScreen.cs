using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TDTK;

namespace TDTK{

	public class UIGameOverScreen : UIScreen {
		
		public Text lbGameOverMsg;
		
		public UIButton buttonContinue;
		public UIButton buttonRestart;
		public UIButton buttonMainMenu;
		
		private static UIGameOverScreen instance;
		
		public override void Awake(){
			base.Awake();
			
			instance=this;
		}
		
		public override void Start(){ 
			base.Start();
			
			buttonContinue.Init();
			buttonContinue.SetCallback(null, null, this.OnContinueButton, null);
			
			buttonRestart.Init();
			buttonRestart.SetCallback(null, null, this.OnRestartButton, null);
			
			buttonMainMenu.Init();
			buttonMainMenu.SetCallback(null, null, this.OnMenuButton, null);
			
			thisObj.SetActive(false);
		}
		
		
		public void OnContinueButton(GameObject butObj, int pointerID=-1){
			GameControl.NextLevel();
		}
		public void OnRestartButton(GameObject butObj, int pointerID=-1){
			GameControl.RestartLevel();
		}
		public void OnMenuButton(GameObject butObj, int pointerID=-1){
			GameControl.MainMenu();
		}
		
		
		
		public static void Show(bool playerWon){ if(instance!=null) instance._Show(playerWon); }
		public void _Show(bool playerWon){
			if(playerWon) lbGameOverMsg.text="Level Completed";
			else lbGameOverMsg.text="Game Over";
			
			UIControl.BlurFadeIn();
			
			base._Show();
		}
		public static void Hide(){ 
			UIControl.BlurFadeOut();
			
			if(instance!=null && instance.thisObj.activeInHierarchy) instance._Hide();
		}
		
	}

}
