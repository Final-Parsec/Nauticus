using UnityEngine;
using System.Collections.Generic;
using System;

public class Armor : Item {
	public int armor;
}

public class Tricorn_Hat : Armor{
	public Tricorn_Hat(){
		isEquip = true;
		name = "Tricorn Hat";
		classRestriction = "None";
		description = "Theres something courageous and exciting about just" +
			"wearing a Tricorn Hat and nothing else.";
		strengthBonus = 3;
		healthBonus = 10;
		constitutionBonus = 1;
	}
}

public class Pirate_Flag : Armor{
	public Pirate_Flag(){
		isEquip = true;
		name = "Pirate Flag (Jolly Roger)";
		classRestriction = "None";
		description = "Sometimes there's nothing to protect you besides what" +
			"you believe in.";
		strengthBonus = 3;
		healthBonus = 40;
		constitutionBonus = 1;
	}
}

public class Bilgewater_Rags : Armor{
	public Bilgewater_Rags(){
		isEquip = true;
		classRestriction = "None";
		name = "Bilgewater Rags";
		description = "Haven't been washed for three months!";
		healthBonus = -4;
		constitutionBonus = 2;
	}
}

public class Finest_Pirate_Regalia : Armor{
	public Finest_Pirate_Regalia(){
		name = "Finest Pirate Regalia";
		isEquip = true;
		classRestriction = "None";
		description = "Only the greatest of pirates have worn silks of this candor.";
		healthBonus = 250;
		constitutionBonus = 2;
		strengthBonus = 2;
		HPModBonus = 2;
	}
}

public class Premium_Yarrrmor : Armor{
	public Premium_Yarrrmor(){
		name = "Premium Yarrrmor";
		isEquip = true;
		classRestriction = "None";
		description = "This armor will put a hook in your enemies\' plans.";
		healthBonus = 450;
		strengthBonus = 1;
	}
}

public class Repurposed_Chain_Link_Fence : Armor{
	public Repurposed_Chain_Link_Fence(){
		name = "Repurposed Chain Link Fence";
		isEquip = true;
		classRestriction = "None";
		description = "Nobody said pirates can’t be thrifty.";
		healthBonus = 15;
		strengthBonus = 3;
	}
}

public class Neck_Beard  : Armor{
	public Neck_Beard(){
		name = "Neck Beard";
		isEquip = true;
		classRestriction = "None";
		description = "They didn’t call him Black Beard for nothing.  " +
			"These fake beards are popular among pirates to model after " +
			"one of the best: the dreaded Black Beard.";
		healthBonus = 300;
		strengthBonus = 3;
	}
}

public class armorList{
	public List<Armor> levelOne;
	public List<Armor> levelTwo;
	public List<Armor> levelThree;

	public armorList(){
		levelOne = new List<Armor>();
		levelOne.Add (new Tricorn_Hat());
		levelOne.Add (new Pirate_Flag());
		levelOne.Add (new Bilgewater_Rags());

		levelTwo = new List<Armor>();
		levelTwo.Add (new Tricorn_Hat());
		levelTwo.Add (new Pirate_Flag());
		levelTwo.Add (new Bilgewater_Rags());
		levelTwo.Add (new Finest_Pirate_Regalia());
		levelTwo.Add (new Repurposed_Chain_Link_Fence());

		levelThree = new List<Armor>();
		levelThree.Add (new Finest_Pirate_Regalia());
		levelThree.Add (new Repurposed_Chain_Link_Fence());
		levelThree.Add (new Premium_Yarrrmor());
		levelThree.Add (new Neck_Beard());
	}
	void start(){

	}
}