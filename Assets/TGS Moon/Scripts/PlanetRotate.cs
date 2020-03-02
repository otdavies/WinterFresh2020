using UnityEngine;
using System.Collections;

public class PlanetRotate : MonoBehaviour {
	public float RotateSpeed = .2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
    // Slowly rotate the object around its X axis at 1 degree/second.
    transform.Rotate(Vector3.down * RotateSpeed * Time.deltaTime/Time.timeScale);

	}
}
