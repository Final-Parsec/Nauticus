using UnityEngine;
using System;
using System.Collections.Generic;

namespace Zombies{
	public class HUD : MonoBehaviour {
		private int activeMenu = (int)rightButtonScreens.NoMenu;
		private List<ButtonInfo> activeMenuText;

		private static int buttonBoxWidthHeight = (int) (Screen.width*.2);
		private static int unitSelectWidth = (int) (Screen.width*.4);
		private static int unitSelectHeight = (int) (Screen.height*.2);
		private static int unitSelectIconSizeXY = (int) (unitSelectHeight*.4);
		private static int unitSelectIconSpaceingWidth = (int) (unitSelectHeight*.2);
		private static int unitSelectIconSpaceingHeight = (int) (unitSelectHeight%unitSelectIconSizeXY)/((int)(unitSelectHeight/unitSelectIconSizeXY) + 1);
		private List<Rect> boxRects = new List<Rect>(){
			new Rect (Screen.width - buttonBoxWidthHeight,
			          Screen.height - buttonBoxWidthHeight,
			          buttonBoxWidthHeight,
			          buttonBoxWidthHeight),
			new Rect (Screen.width - buttonBoxWidthHeight - unitSelectWidth,
			          Screen.height - unitSelectHeight,
			          unitSelectWidth,
			          unitSelectHeight)
		};
		private List<ButtonInfo> mainControlText = new List<ButtonInfo>(){
			//new ButtonInfo("Move", (int)rightButtonScreens.UnitOptions),
			new ButtonInfo("Build", (int)rightButtonScreens.BuildMenu, false),
			new ButtonInfo("Destroy", (int)rightButtonScreens.NoMenu)
		};
		private List<ButtonInfo> buildMenuText = new List<ButtonInfo>(){
			new ButtonInfo("House", (int)rightButtonScreens.BuildMenu),
			new ButtonInfo("Hut", (int)rightButtonScreens.BuildMenu),
			new ButtonInfo("Wall", (int)rightButtonScreens.BuildMenu),
			new ButtonInfo("Boat", (int)rightButtonScreens.BuildMenu),
			new ButtonInfo("Back", (int)rightButtonScreens.NoMenu)

		};

		private static int buttonWidthHeight = (int) (buttonBoxWidthHeight*.26);
		private static int buttonSpaceing = (int) (buttonBoxWidthHeight*.05);
		private List<Rect> rightButtonRects = new List<Rect>(){
			new Rect (Screen.width - (buttonWidthHeight * 3 + buttonSpaceing * 4), Screen.height - (buttonWidthHeight * 3 + buttonSpaceing * 4), buttonWidthHeight, buttonWidthHeight),
			new Rect (Screen.width - (buttonWidthHeight * 2 + buttonSpaceing * 3), Screen.height - (buttonWidthHeight * 3 + buttonSpaceing * 4), buttonWidthHeight, buttonWidthHeight),
			new Rect (Screen.width - (buttonWidthHeight * 1 + buttonSpaceing * 2), Screen.height - (buttonWidthHeight * 3 + buttonSpaceing * 4), buttonWidthHeight, buttonWidthHeight),
			
			new Rect (Screen.width - (buttonWidthHeight * 3 + buttonSpaceing * 4), Screen.height - (buttonWidthHeight * 2 + buttonSpaceing * 3), buttonWidthHeight, buttonWidthHeight),
			new Rect (Screen.width - (buttonWidthHeight * 2 + buttonSpaceing * 3), Screen.height - (buttonWidthHeight * 2 + buttonSpaceing * 3), buttonWidthHeight, buttonWidthHeight),
			new Rect (Screen.width - (buttonWidthHeight * 1 + buttonSpaceing * 2), Screen.height - (buttonWidthHeight * 2 + buttonSpaceing * 3), buttonWidthHeight, buttonWidthHeight),
			
			new Rect (Screen.width - (buttonWidthHeight * 3 + buttonSpaceing * 4), Screen.height - (buttonWidthHeight * 1 + buttonSpaceing * 2), buttonWidthHeight, buttonWidthHeight),
			new Rect (Screen.width - (buttonWidthHeight * 2 + buttonSpaceing * 3), Screen.height - (buttonWidthHeight * 1 + buttonSpaceing * 2), buttonWidthHeight, buttonWidthHeight),
			new Rect (Screen.width - (buttonWidthHeight * 1 + buttonSpaceing * 2), Screen.height - (buttonWidthHeight * 1 + buttonSpaceing * 2), buttonWidthHeight, buttonWidthHeight)
		};

		GUIStyle unitSelectStyle = new GUIStyle();


		public int woodResourse = 500;
		public float masterVolume = .0f; // .0 to 1.0

		public string tileMouseOver;
		public int rightButtonPressed = -1;
		private UnitGod _unitGod;
		private EventHandler _eventHandler;
		public CameraMovement _camera;
		public Texture healthTexture;

		void Start(){
			unitSelectStyle.fontSize = 12;
			
			_unitGod = UnitGod.GetInstance();
			_eventHandler = GameObject.Find("TileMap").GetComponent<EventHandler>();
			_camera = GetComponent<CameraMovement>();
		}

		/// <summary>
		/// Raises the GU event.
		/// Draws the selection box.
		/// </summary>
		void OnGUI () {
			if(_eventHandler.overlay != null){
				return;
			}

			GUI.Label (new Rect (Screen.width - 100, 10, 100, 20), tileMouseOver);
			GUI.Label (new Rect (10, 10, 100, 20), "Wood: " + woodResourse);
			GUI.Box(boxRects[0], "");
			GUI.Box(boxRects[1], "");

			DisplaySelectedUnits();

			if(activeMenu == (int)rightButtonScreens.BuildMenu){
				activeMenuText = buildMenuText;
			}else if(activeMenu == (int)rightButtonScreens.UnitOptions){
				activeMenuText = mainControlText;
			}else{
				activeMenu = (int)rightButtonScreens.UnitOptions;
			}
			DisplayRightButtons(activeMenuText);

			

			foreach(GameObjectBase Gob in _unitGod.ThingsWithHealthBars()){
				//Health Bar
				float healthRatio = (((float) Gob.health)/ ((float) Gob.maxHealth));
				if(healthRatio != 1){
					Vector2 objSize = Gob.GetPixelSize();
					float distanceRatio = 1;//(8 * _camera.GetDistanceRatio());
					float width;
					float height;
					if (distanceRatio <= 1){
						width = Gob.healthBarWidth;
						height = 5;
					}else{
						width = Gob.healthBarWidth / distanceRatio;
						height = 5 / distanceRatio;
					}
					
					
					width = width * healthRatio;
					Vector3 wantedPos =  Camera.main.WorldToScreenPoint(Gob.transform.position);
					
					GUI.color = new Color(2 * (1 - healthRatio), 2 * healthRatio, 0);
					GUI.DrawTexture(new Rect(wantedPos.x - width/2, Screen.height - wantedPos.y - objSize.y/2, width, height), healthTexture);
				}


				//Attack Bar
				if( Gob is UnitBase){
					UnitBase instance = (UnitBase)Gob;
					float attackRatio = (instance.animationCancelExperation - Time.time) / instance.animationCancelDelta;
					if(attackRatio > 0 && attackRatio < 1f && instance.target != null){
						Vector2 objSize = Gob.GetPixelSize();
						float distanceRatio = (8 * _camera.GetDistanceRatio());
						float width;
						float height;
						if (distanceRatio <= 1){
							width = 5;
							height = objSize.y;
						}else{
							width = 5 / distanceRatio;
							height = objSize.y / distanceRatio;
						}

						height = height * attackRatio;
						Vector3 wantedPos =  Camera.main.WorldToScreenPoint(Gob.transform.position);
						
						GUI.color = Color.gray;
						GUI.DrawTexture(new Rect(wantedPos.x - objSize.x/2, Screen.height - wantedPos.y - height + objSize.y/2, width, height), healthTexture);
					}
				}
			}

		}

		/// <summary>
		/// Displaies the selected units.
		/// </summary>
		private void DisplaySelectedUnits(){

			int x = 0;
			int y = 0;

			foreach (UnitBase unit in _unitGod.GetSelectedUnits()){
				Rect iconRect = new Rect (boxRects[1].x + (unitSelectIconSizeXY * x + unitSelectIconSpaceingWidth),
				                          boxRects[1].y + (unitSelectIconSizeXY * y + unitSelectIconSpaceingHeight),
								          unitSelectIconSizeXY,
								          unitSelectIconSizeXY);

				Rect textRect = new Rect (iconRect.x,
				                          iconRect.y + iconRect.height,
				                          10,
				                          iconRect.height);

				// GUI.Label (iconRect, unit.selectedImage.texture);
				GUI.Label (textRect, ""+unit.health, unitSelectStyle);

				x++;
				if(x>(int)(unitSelectWidth/unitSelectIconSizeXY)){
					x=0;
					y++;
				}
			}
		}

		/// <summary>
		/// Displaies the right buttons.
		/// </summary>
		/// <param name="buttonStrings">Button strings.</param>
		private void DisplayRightButtons(List<ButtonInfo> buttonInfo){
			if (buttonInfo == null)
				return;

			for (int x=0; x<buttonInfo.Count; x++){
				if (GUI.Button (rightButtonRects[x], buttonInfo[x].buttonText)){
					_eventHandler.DeselectButtons();

					string rightButton = buttonInfo[x].buttonText;
					activeMenu = buttonInfo[x].leadsToMenu;
					rightButtons rightButtonEnum = (rightButtons)Enum.Parse(typeof(rightButtons), rightButton);
					if (buttonInfo[x].saveAction)
						rightButtonPressed = (int)rightButtonEnum;
				}
			}
		}

		/// <summary>
		/// Returns true if the point collides with the HUD.
		/// </summary>
		/// <returns><c>true</c>, if collides with hud was pointed, <c>false</c> otherwise.</returns>
		/// <param name="point">Point.</param>
		public bool pointCollidesWithHud(Vector2 point){
		
			foreach (Rect boxRect in boxRects) 
				if(boxRect.Contains(point))
					return true;
			
			return false;
		
		}



	}

	public class ButtonInfo{
		public string buttonText;
		public int leadsToMenu;
		public bool saveAction = true;

		public ButtonInfo(string buttonText, int leadsToMenu){
			this.buttonText = buttonText;
			this.leadsToMenu = leadsToMenu;
		}
		public ButtonInfo(string buttonText, int leadsToMenu, bool saveAction){
			this.buttonText = buttonText;
			this.leadsToMenu = leadsToMenu;
			this.saveAction = saveAction;
		}
	}
}