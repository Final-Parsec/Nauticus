
using System;
using UnityEngine;
namespace Zombies
{
	// Index into tiles array in TileMap
	public enum tileTypes
	{
		Grass=0,
		Water=1
	};

	public enum border
	{
		center=-1,
		Down=3,
		Up=1,
		Left=2,
		Right=4,
		upLeft=5,
		upRight=6,
		downLeft=7,
		downRight=0
	};

	public enum attackDirection
	{
		notAttacking=0,
		down=3,
		up=1,
		Right=4,
		Left=2
	};

	public enum sortingLayer
	{
		NotSelected=8,
		Selected=9
		
	};


	public enum rightButtons
	{
		noButtonPressed=-1,
		Move=0,
		Build=1,
		House=2,
		Hut=3,
		Wall=4,
		Boat=5,
		Destroy=6,
		Back=7
		
	};

	public enum rightButtonScreens
	{
		NoMenu=-1,
		UnitOptions=0,
		BuildMenu=1
		
	};

	public enum gatheringTypes
	{
		DoneGathering=-1,
		Returning=3,
		NotGathering=0,
		TravelingToWood=1,
		GatheringWood=2
		
	};

	public enum unitID
	{
		NoUnit=-1,
		Player=0,
		Zombie=1
		
	};

	public enum state
	{
		Walking=0,
		Attacking=1
		
	};


}

