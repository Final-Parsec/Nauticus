  using UnityEngine;
using System.Collections.Generic;

public class EnemyMaster{
	
	List<GameObject> enemyList;

	// Use this for initialization
	public EnemyMaster(){
		GameObject[] enemyArray = GameObject.FindGameObjectsWithTag ("Zombie");
		enemyList = new List<GameObject> ();
		enemyList.AddRange (enemyArray);
	}

	public void StartEnemyTurn(){
		foreach (GameObject enemy in enemyList) {
			enemy.GetComponent<Actor> ().Act ();
		}
	}

	public GameObject CheckTileForEnemy(int x, int y){
		foreach (GameObject enemy in enemyList) {
			if (enemy.GetComponent<Actor>().x_pos == x && enemy.GetComponent<Actor>().y_pos == y)
					return enemy;
		}
		return null;
	}

	/// <summary>
	/// Checks for dead enemies.
	/// </summary>
	public void CheckForDeadEnemies(){
		foreach (GameObject enemy in enemyList) {
			if(enemy.GetComponent<Actor>().health <= 0){
				enemyList.Remove (enemy);
				enemy.GetComponent<Actor>().killyoself ();
			}
		}
	}

	/// <summary>
	/// Checks for dead enemy.
	/// </summary>
	/// <param name="enemy">Enemy.</param>
	public void CheckForDeadEnemy(GameObject enemy){
		if (enemy.GetComponent<Actor>().health <= 0) {
				enemyList.Remove (enemy);
				enemy.GetComponent<Actor>().killyoself();
		}
	}

	public void KillAllEnemies(){
		enemyList.Clear ();
		foreach (GameObject enemy in enemyList) {
			enemy.GetComponent<Actor> ().health = 0;
		}
		for (int i = 0; i<enemyList.Count; i++) {
			enemyList[i].GetComponent<Actor>().killyoself();
		}
	}

}