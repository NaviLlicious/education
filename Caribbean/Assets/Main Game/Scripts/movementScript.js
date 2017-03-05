public var jumpSpeed:float = 18.0;
public var gravity:float = 100.0;
public var runSpeed:float = 5.0;
public var walkSpeed:float = 3.0;
public var rotateSpeed:float = 5.0;
 
private var grounded:boolean = false;
private var moveDirection:Vector3 = Vector3.zero;
private var isWalking:boolean = false;
private var moveStatus:String = "idle";
 
static var dead : boolean = false;
 
function Update ()
{
 
    if(dead == false) {
   
   
   
 
 
    // Only allow movement and jumps while grounded
    if(grounded) {
        moveDirection = new Vector3((Input.GetMouseButton(1) ? Input.GetAxis("Horizontal") : 0),0,Input.GetAxis("Vertical"));
       
        // if moving forward and to the side at the same time, compensate for distance
        // TODO: may be better way to do this?
        if(Input.GetMouseButton(1) && Input.GetAxis("Horizontal") && Input.GetAxis("Vertical")) {
            moveDirection *= .7;
           
        }
       
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= isWalking ? walkSpeed : runSpeed;
       
        moveStatus = "idle";
        if(moveDirection != Vector3.zero)
            moveStatus = isWalking ? "walking" : "running";
       
        // Jump!
        //if(Input.GetButton("Jump"))
       
        if (Input.GetKeyDown(KeyCode.Space))
       
            moveDirection.y = jumpSpeed;
    }
   
    // Allow turning at anytime. Keep the character facing in the same direction as the Camera if the right mouse button is down.
//    if(Input.GetMouseButton(1)) {
//        transform.rotation = Quaternion.Euler(0,Camera.main.transform.eulerAngles.y,0);
//    } else {
//        transform.Rotate(0,Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0);
       
//    }
   
   
       
       
       
   
    // Toggle walking/running with the T key
    //if(Input.GetKeyDown("t"))
        //isWalking = !isWalking;
 
 
   
   
    //Apply gravity
    moveDirection.y -= gravity * Time.deltaTime;
 
   
    //Move controller
    var controller:CharacterController = GetComponent(CharacterController);
    var flags = controller.Move(moveDirection * Time.deltaTime);
    grounded = (flags & CollisionFlags.Below) != 0;
   
 
    }
   
   
    if(Input.GetMouseButton(1) || Input.GetMouseButton(0)) {
        //Screen.lockCursor = true;
       
        Cursor.visible = false;
       
       
        //var mouse1 = Input.mousePosition.y;
        //var mouse2 = Input.mousePosition.x;
       
        }
       
        //Vector3 mousePos = Input.mousePosition;
    else  {
        //Screen.lockCursor = false;
        Cursor.visible = false;
       
        //Input.mousePosition.y = mouse1;
        //Input.mousePosition.x = mouse2;
       
        //Input.mousePosition = mousePos;
       
        }
   
   
}
 
@script RequireComponent(CharacterController)