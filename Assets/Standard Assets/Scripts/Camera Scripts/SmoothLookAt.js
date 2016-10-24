var target : Transform;
var damping = 6.0;
var smooth = true;

@script AddComponentMenu("Camera-Control/Smooth Look At")

function LateUpdate () {
	if (target) {
		if (smooth)
		{
			transform.localPosition += Vector3(0.001f,0.001f,0.001f);
			// Look at and dampen the rotation
			var rotation = Quaternion.LookRotation(target.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
			transform.LookAt(target);
		}
		else
		{
			// Just lookat
		    transform.LookAt(target);
		}
	}
}

function Start () {
	// Make the rigid body not change rotation
   	if (rigidbody)
		rigidbody.freezeRotation = true;
}