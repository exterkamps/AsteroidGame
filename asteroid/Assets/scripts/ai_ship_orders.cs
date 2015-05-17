﻿using UnityEngine;
using System.Collections;

public class ai_ship_orders : MonoBehaviour {

	public int TYPE;
	public Vector2[] waypoints;
	public GameObject followee;
	public float minDistanceFromFollowee = 6f;  //standard is to slow down right before CAM distance
	public float maxDistanceFromWaypoint;
	public float propensityToFight;
	public float propensityToFire;

	private int currentWaypoint;

	public bool DRAW_RAYS = false;


	public ai_ship_orders(int TYPE, Vector2[] ways, GameObject f, float m, float maxDFW, float ptFi, float ptFr){
		this.TYPE = TYPE;
		waypoints = ways;
		followee = f;
		minDistanceFromFollowee = m;
		currentWaypoint = 0;
		maxDistanceFromWaypoint = maxDFW;
		propensityToFight = ptFi;
		propensityToFire = ptFr;
	}

	public Vector2 nextAction(Vector2 currentPos ){
		Vector2 nextPoint = new Vector2 (0,0);

		switch (TYPE) {
		case (ship_library.AI_ORDERS_GUARD_WAYPOINT):
			nextPoint = waypoints[currentWaypoint];
			if (DRAW_RAYS){Debug.DrawLine (this.transform.position,waypoints[currentWaypoint],Color.yellow,0.2f);}
			break;
		case(ship_library.AI_ORDERS_ESCORT):
			//MAKE SURE IT ISN'T DEAD, IF IT IS, UH, PANIC?
			if (followee != null){


			//eventually need to make escort slots to fill by escorters, instead of just follow behind 
			nextPoint = followee.transform.position;
			nextPoint = nextPoint - (Vector2)followee.transform.up * minDistanceFromFollowee;
			if (DRAW_RAYS){Debug.DrawLine (this.transform.position,nextPoint,Color.yellow,0.2f);}
			}else{
				TYPE = ship_library.AI_ORDERS_GUARD_WAYPOINT;
				waypoints = new Vector2[1];
				waypoints[0] = this.transform.position;
				currentWaypoint = 0;
			}
			break;
		case (ship_library.AI_ORDERS_PATROL):
			nextPoint = waypoints[currentWaypoint];
			if (DRAW_RAYS){Debug.DrawLine (this.transform.position,nextPoint,Color.yellow,0.2f);}
			break;
		case (ship_library.AI_ORDERS_ORBIT):
			if (followee != null){
				nextPoint = followee.transform.position;
				if (DRAW_RAYS){Debug.DrawLine (this.transform.position,nextPoint,Color.yellow,0.2f);}
			}else{
				TYPE = ship_library.AI_ORDERS_GUARD_WAYPOINT;
				waypoints = new Vector2[1];
				waypoints[0] = this.transform.position;
				currentWaypoint = 0;
			}
			break;
		}

		return nextPoint;

	}

	public void incPatrol(){
		currentWaypoint = (currentWaypoint + 1 < waypoints.Length)? currentWaypoint + 1 : 0;
	}



}