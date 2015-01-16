using UnityEngine;
using System.Collections.Generic;

public class Weapon : Item {
}

public class Hook : Weapon{
	public Hook(){
		name = "Hook";
		isEquip = true;
		weaponAttack = 2;
		strengthBonus = 3;
		description = "A vile hook that is smattered with rust and blood. " +
			"It\'s plundered the insides of both man and wench. " +
			"This is oft a weapon of last resort, or a tool for disarming a sword.  " +
			"Also the hook is great for spearing apples.";
		classRestriction = "None";
		}
}

public class Zombie_Arm : Weapon{
	public Zombie_Arm(){
		name = "Zombie Arm";
		isEquip = true;
		weaponAttack = 2;
		strengthBonus = 0;
		description = "This arm used to belong to a person. " +
			"Then it belonged to a zombie.  Now it belongs to you.  " +
			"Who knows where it\'s been.";
		classRestriction = "None";
	}
}

//Swashbuckler
public class Kite_Shield_and_Axe : Weapon{
	public Kite_Shield_and_Axe(){
		name = "Kite Shield and Axe";
		isEquip = true;
		weaponAttack = 15;
		description = "Because of it’s heavy weight, the kite shield is," +
			"in fact, terrible for kiting enemies.  " +
			"You’ll have to stand strong while wielding this rig, " +
			"and be prepared to axe some philosophical questions.  " +
			"This kite shield is named the the Kraken, because your enemies will" +
			"be kraken their swords against it.  It is a dark blue with a blood red" +
			"skull and crossbones in the middle, and it has spikes coming " +
			"out of the eyes of the skull.  The axe included in this kit" +
			"looks like a standard hand axe. It has a smooth wooden hilt " +
			"and a bright steel blade.  On the edge of the blade is inscribed:" +
				"“Occam’s Razor.”";
		classRestriction = "Swashbuckler";
	}
}

public class Buckler_and_Rapier : Weapon{
	public Buckler_and_Rapier(){
		name = "Buckler and Rapier";
		isEquip = true;
		weaponAttack = 22;
		description = "This is a classic duo of a fighting kit. " +
				"The buckler and rapier is made for strong slicing attacks," +
				"and maximizing footwork while still having a shield to glance " +
				"off blows.  This rapier has a spiral design up it’s deadly " +
				"grey blade.  The spiral is incorporated in the fancy spiraling " +
				"steel handguard that wraps around the hilt.  The hilt is wrapped " +
				"in soft leather.  The buckler is simple, light, and small.  " +
				"It has a wooden body reinforced with a circle of steel around it." +
				"It has a dome shape to deflect blows away from the user instead " +
				"of just taking them head on.";
		classRestriction = "Swashbuckler";
	}
}

public class Targe_and_Saber : Weapon{
	public Targe_and_Saber(){
		name = "Targe and Saber";
		isEquip = true;
		weaponAttack = 25;
		description = "The targe is a deadly leather shield." +
			"It has a long spike coming right through the middle" +
			"that has ended many a short fight.  This targe has" +
			"metal studs in the shape of a nude woman reinforcing" +
			"it’s leather.  Worn like a buckler, this targe is light" +
			"and easy to wield as a weapon or a saving grace.  The" +
			"saber included in this kit is a long, slightly curved" +
			"metal machine of death.  It’s curve is made to slice " +
			"through light armor, and parry away strong attacks.  " +
			"It’s ornate handle contains a single strip of steel " +
			"connecting the handguard to the pommel that is shaped" +
			"like a nude woman to match the targe.";
		classRestriction = "Swashbuckler";
	}
}

//Buccaneer
public class Giant_Black_Cannon : Weapon{
	public Giant_Black_Cannon(){
		name = "Giant Black Cannon";
		isEquip = true;
		weaponAttack = 20;
		description = "This is a big black weapon.  It has a small handle" +
						"for moving around, and the Buccaneer takes that a little too far." +
						"Unfortunately the Buccaneer can’t find a mount or ammo, so this " +
						"cannon will have to be repurposed for bludgeoning purposes.";
		classRestriction = "Buccaneer";
	}
}

public class Lucky_Cannon : Weapon{
	public Lucky_Cannon(){
		name = "Lucky Cannon";
		isEquip = true;
		weaponAttack = 23;
		description = "This bludgeoning cannon is a family heirloom of it’s last" +
			"owner.  Unfortunately, it couldn’t provide him enough luck, or you" +
			"wouldn’t have found it.  What was once a beautiful red paint job is" +
			"flaking off to reveal a dented black ugly metal death-club.  This" +
			"cannon will never fire again, but it could still see some more " +
				"violence when wielded in the right hands.";
		classRestriction = "Buccaneer";
	}
}

public class Rune_Scimmy_2H : Weapon{
	public Rune_Scimmy_2H(){
		name = "Rune Scimmy 2H";
		isEquip = true;
		weaponAttack = 25;
		description = "This unique weapon is from a land far away.  It’s value " +
			"is known well there, and it’s metal is some of the strongest around." +
			"It glows a deep ocean blue, the perfect color for Piro’s sea-laden " +
			"eyes.  This weapon is made from the nigh unbreakable metal called " +
			"rune, and three blacksmiths died making it.  In it’s hilt is" +
			"inscribed pictures of it’s previous owners many young wives.";
		classRestriction = "Buccaneer";
	}
}

public class Two_Handed_Scimitar : Weapon{
	public Two_Handed_Scimitar(){
		name = "Two Handed Scimitar";
		isEquip = true;
		weaponAttack = 27;
		description = "A scimitar apparently made for giants.  " +
			"This scimitar is about as tall as an average pirate. " +
			"It’s blade is sharper than most pirates’ wits, but only a" +
			"buccaneer could be able to do damage with it.  Unless a pirate " +
			"accidentally tips it over or drops it on someone, this weapon " +
			"is too unwieldy for most.  It features a jagged black edge" +
			"that fades to a steel backing suggesting a unique forging " +
			"process for this huge sword.";
		classRestriction = "Buccaneer";
	}
}

//Picaroon
public class The_Dastardly_Dirk : Weapon{
	public The_Dastardly_Dirk(){
		name = "The Dastardly Dirk";
		isEquip = true;
		weaponAttack = 5;
		strengthBonus = -1;
		description = "A long dagger that isn’t quite long enough for a sword," +
			"but still short enough to be manouvered with deadly precision by a" +
			"practiced picaroon.  Made with thin blue steel, this dagger is deadly" +
			"light, and it has seen inside more throats than your doctor.";
		classRestriction = "Picaroon";
	}
}

public class Shoreline_Shiv : Weapon{
	public Shoreline_Shiv(){
		name = "Shoreline Shiv";
		isEquip = true;
		strengthBonus = 1;
		weaponAttack = 6;
		description = "This is a classic sailing knife that was made for quickly" +
			"slicing through hard fibers in high grade ropes.  The Shoreline Shiv is " +
			"great for getting the upper hand in a friendly pirate grapple due to it’s" +
			"small length.  It’s also easily hidden.  There is a misspelled swear-word" +
			"carved into the pommel.";
		classRestriction = "Picaroon";
	}
}

public class Stiletto : Weapon{
	public Stiletto(){
		name = "Stiletto";
		isEquip = true;
		weaponAttack = 9;
		description = "This is a thin retractable dagger that is terrifying to the sight," +
			"when you can see it.  It fits easily up a sleeve or in a pocket, so" +
			"oftentimes a talented user can inflict serious damage before a foe even " +
			"sees the weapon.  This weapon sports a flat profile and inlaid wood on the " +
			"hilt like a kitchen knife.  The wood is stained dark from use.";
		classRestriction = "Picaroon";
	}
}

public class Skanks_Stiletto : Weapon{
	public Skanks_Stiletto(){
		name = "Skank\'s Stiletto";
		isEquip = true;
		weaponAttack = 11;
		description = "Found after hours at Tortuga\'s Rub-N-Tug Club, this bladed" +
			"heel broke off when a dancing wench had one too many ales. This thin " +
			"blade will pierce hearts faster than her sloppy wiles as long as your " +
			"wit is equally as sharp.";
		classRestriction = "Picaroon";
	}
}

//Rapscallion
public class Hunting_Crossbow : Weapon{
	public Hunting_Crossbow(){
		name = "Hunting Crossbow";
		isEquip = true;
		weaponAttack = 12;
		description = "This crossbow was used to hunt game.  It\'s camoflaged poorly " +
			"and has seen some wear.  Lovingly etched into the side is \"Cal\'s bow\" " +
			"with13 tally marks.  Turns out zombies and pirates are as vulnerable to " +
			"this bow as any other animal.";
		classRestriction = "Rapscallion";
	}
}

public class Craven : Weapon{
	public Craven(){
		name = "Craven";
		isEquip = true;
		weaponAttack = 14;
		description = "This is a standard functional crossbow mass manufactured during the" +
			"second pirate war. The name is etched on the side by it's first owner in a chicken " +
			"scrawl.  It\'s surface is rough, but the crossbow feels sturdy enough.";
		classRestriction = "Rapscallion";
	}
}

public class Journey : Weapon{
	public Journey(){
		name = "Journey Cross 2931 Crossbow Special Edition";
		isEquip = true;
		weaponAttack = 17;
		description = "This sleek crossbow is made from walnut and oak.  It's glossy" +
			"finish shines in the light.  It's name is stamped on the butt in gold leaf." +
			"This crossbow is so pretty you might want to take out insurance on it.";
		classRestriction = "Rapscallion";
	}
}

public class Sniper_Crossbow : Weapon{
	public Sniper_Crossbow(){
		name = "Sniper Crossbow";
		isEquip = true;
		weaponAttack = 20;
		description = "X-Bow 2H-55 - A long, powerful machine of death. It's black and made of" +
			"lightweight metal.  This crossbow is military grade and is worth more than a small pirate ship.";
		classRestriction = "Rapscallion";
	}
}

public class Bolticus : Weapon{
	public Bolticus(){
		name = "Bolticus";
		isEquip = true;
		weaponAttack = 22;
		description = "This crossbow was sent to the world from the omniscient hands of the pirate" +
			"god himself.  It glows at night with an otherworldly light.The crossbow is an off-white" +
			"with gold trimming. It has a sapphire sight.";
		classRestriction = "Rapscallion";
	}
}

public class weaponList{
	public List<Weapon> levelOne;
	public List<Weapon> levelTwo;
	public List<Weapon> levelThree;
	
	public weaponList(){
		levelOne = new List<Weapon>();
		levelOne.Add (new Hook());
		levelOne.Add (new Zombie_Arm());
		levelOne.Add (new Kite_Shield_and_Axe());
		levelOne.Add (new Giant_Black_Cannon());
		levelOne.Add (new Hunting_Crossbow());
		levelOne.Add (new The_Dastardly_Dirk());

		levelTwo = new List<Weapon>();
		levelTwo.Add (new Hook());
		levelTwo.Add (new Buckler_and_Rapier());
		levelTwo.Add (new Lucky_Cannon());
		levelTwo.Add (new Rune_Scimmy_2H());
		levelTwo.Add (new Shoreline_Shiv());
		levelTwo.Add (new Stiletto());
		levelTwo.Add (new Craven());
		levelTwo.Add (new Journey());

		
		levelThree = new List<Weapon>();
		levelThree.Add (new Hook());
		levelThree.Add (new Targe_and_Saber());
		levelThree.Add (new Two_Handed_Scimitar());
		levelThree.Add (new Skanks_Stiletto());
		levelThree.Add (new Sniper_Crossbow());
		levelThree.Add (new Bolticus());
	}
}