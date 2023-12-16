using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is created for the example scene. There is no support for this script.
public class CursorTrack : MonoBehaviour
{
	public Transform gun;

	private float camRayLength = 100f;          // The length of the ray from the camera into the scene.

    // Update is called once per frame
    void Update()
	{
		// Create a ray from the mouse cursor on screen in the direction of the camera.
		var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		gun.position = camRay.origin;

		// Perform the raycast and if it hits something on the floor layer...
		if (Physics.Raycast(camRay, out var hit, camRayLength))
		{
			// Create a vector from the player to the point on the floor the raycast from the mouse hit.
			var playerToMouse = hit.point - camRay.origin;

			gun.rotation = Quaternion.LookRotation(playerToMouse);
		}
	}
}
