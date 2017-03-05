var target:Transform;                           // Target to follow
var targetHeight = 1.7;                         // Vertical offset adjustment
var distance = 12.0;                            // Default Distance
var offsetFromWall = 0.1;                       // Bring camera away from any colliding objects
var maxDistance = 20;                       // Maximum zoom Distance
var minDistance = 0.6;                      // Minimum zoom Distance
var xSpeed = 200.0;                             // Orbit speed (Left/Right)
var ySpeed = 200.0;                             // Orbit speed (Up/Down)
var yMinLimit = -80;                            // Looking up limit
var yMaxLimit = 80;                             // Looking down limit
var zoomRate = 40;                          // Zoom Speed
var rotationDampening = 3.0;                // Auto Rotation speed (higher = faster)
var zoomDampening = 5.0;                    // Auto Zoom speed (Higher = faster)
var collisionLayers:LayerMask = -1;     // What the camera will collide with
var lockToRearOfTarget = false;             // Lock camera to rear of target
var allowMouseInputX = true;                // Allow player to control camera angle on the X axis (Left/Right)
var allowMouseInputY = true;                // Allow player to control camera angle on the Y axis (Up/Down)
 
private var xDeg = 0.0;
private var yDeg = 0.0;
private var currentDistance;
private var desiredDistance;
private var correctedDistance;
private var rotateBehind = false;
 
 
@script AddComponentMenu("Camera-Control/Third Person Camera Orbit (MMORPG Like)")
 
function Start ()
{
    var angles:Vector3 = transform.eulerAngles;
    xDeg = angles.x;
    yDeg = angles.y;
    currentDistance = distance;
    desiredDistance = distance;
    correctedDistance = distance;
   
    // Make the rigid body not change rotation
    if (GetComponent.<Rigidbody>())
        GetComponent.<Rigidbody>().freezeRotation = true;
       
    if (lockToRearOfTarget)
        rotateBehind = true;
 }
   
//Only Move camera after everything else has been updated
function LateUpdate ()
{
 
 
 
    // Don't do anything if target is not defined
    if (!target)
        return;
   
    var vTargetOffset:Vector3;
       
     
    // If either mouse buttons are down, let the mouse govern camera position
    if (GUIUtility.hotControl == 0)
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            //Check to see if mouse input is allowed on the axis
            if (allowMouseInputX)
                xDeg += Input.GetAxis ("Mouse X") * xSpeed * 0.02;
            else
                RotateBehindTarget();
            if (allowMouseInputY)
                yDeg -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02;
           
            //Interrupt rotating behind if mouse wants to control rotation
            if (!lockToRearOfTarget)
                rotateBehind = false;
        }
       
        // otherwise, ease behind the target if any of the directional keys are pressed
        else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || rotateBehind)
        {
            //RotateBehindTarget();
        }
    }
    yDeg = ClampAngle (yDeg, yMinLimit, yMaxLimit);
 
    // Set camera rotation
    var rotation:Quaternion = Quaternion.Euler (yDeg, xDeg, 0);
 
    // Calculate the desired distance
    desiredDistance -= Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs (desiredDistance);
    desiredDistance = Mathf.Clamp (desiredDistance, minDistance, maxDistance);
    correctedDistance = desiredDistance;
 
    // Calculate desired camera position
    vTargetOffset = Vector3 (0, -targetHeight, 0);
    var position:Vector3 = target.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset);
 
    // Check for collision using the true target's desired registration point as set by user using height
    var collisionHit:RaycastHit;
    var trueTargetPosition:Vector3 = Vector3 (target.position.x, target.position.y + targetHeight, target.position.z);
 
    // If there was a collision, correct the camera position and calculate the corrected distance
    var isCorrected = false;
    if (Physics.Linecast (trueTargetPosition, position, collisionHit, collisionLayers))
    {
        // Calculate the distance from the original estimated position to the collision location,
        // subtracting out a safety "offset" distance from the object we hit.  The offset will help
        // keep the camera from being right on top of the surface we hit, which usually shows up as
        // the surface geometry getting partially clipped by the camera's front clipping plane.
        correctedDistance = Vector3.Distance (trueTargetPosition, collisionHit.point) - offsetFromWall;
        isCorrected = true;
    }
 
    // For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance
    currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp (currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance;
 
    // Keep within limits
    currentDistance = Mathf.Clamp (currentDistance, minDistance, maxDistance);
 
    // Recalculate position based on the new currentDistance
    position = target.position - (rotation * Vector3.forward * currentDistance + vTargetOffset);
   
    //Finally Set rotation and position of camera
    transform.rotation = rotation;
    transform.position = position;
}
 
function RotateBehindTarget()
{
    var targetRotationAngle:float = target.eulerAngles.y;
    var currentRotationAngle:float = transform.eulerAngles.y;
    xDeg = Mathf.LerpAngle (currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
   
    // Stop rotating behind if not completed
    if (targetRotationAngle == currentRotationAngle)
    {
        if (!lockToRearOfTarget)
            rotateBehind = false;
    }
    else
        rotateBehind = true;
 
}
 
 
static function ClampAngle (angle : float, min : float, max : float)
{
   if (angle < -360)
      angle += 360;
   if (angle > 360)
      angle -= 360;
   return Mathf.Clamp (angle, min, max);
}