using UnityEngine;
using System.Collections.Generic;
namespace Zombies
{
	public class BuildingBase : GameObjectBase
	{

		public int width;
		public int height;
		public List<BuildingParts> parts;

		/// <summary>
		/// Populates the partsOf field in all the parts of the building
		/// </summary>
		public void PopulatePartsOf(GameObjectBase Gob){
			foreach(BuildingParts part in parts){
				part.partOf = this;
				if(!(part is Wall))
					part.onTile.addOccupant(Gob);
			}
		}

		new public void KillSelf(){
			foreach(BuildingParts part in parts){
				part.KillSelf();
			}
			_UnitGod.DeReference(this);
			Destroy(gameObject);
		}

		public override Vector2 GetPixelSize(){
			Vector2 part = parts[parts.Count - 1].GetPixelSize();

			int widthX = (int)(part.x * width);
			int widthY = (int)(part.y * height);

			return new Vector2(widthX, widthY);

		}

	}
}

