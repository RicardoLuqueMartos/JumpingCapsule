using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform cameraParentTransform;

    [SerializeField] float mouseX = 0.0f;
    [SerializeField] float mouseY = 0.0f;
    float x = 0.0f;
    float y = 0.0f;
    
    [SerializeField]
    private float xSpeed = 250.0f;
    [SerializeField]
    private float ySpeed = 120.0f;

    [SerializeField]
    private float yMinLimit = -20;
    [SerializeField]
    private float yMaxLimit = 80;

    [SerializeField]
    private float xMinLimit = -35;
    [SerializeField]
    private float xMaxLimit = 35;
  
    [SerializeField]
    private Transform aimingObject;

    [SerializeField]
    private Transform cameraPivotVertical;
    void Update()
    {
        RotateCamera();
        MoveCamera();
    }

    private void OnLook(InputValue Value)
    {
        Vector2 vector = Value.Get<Vector2>();
        mouseX = vector.x;
        mouseY = vector.y;
        Debug.Log(mouseX+" / "+ mouseY);
    }

    private void MoveCamera()
    {
        cameraParentTransform.position = new Vector3(playerTransform.position.x,
            playerTransform.position.y,
            playerTransform.position.z);

    }

    private void RotateCamera()
    {
        // get position gape between Mouse X and Mouse Y on screen points every frame 
        x += mouseX * xSpeed * 0.02f;
        y -= mouseY * ySpeed * 0.02f;

    //    x = ClampAngle(x, xMinLimit, xMaxLimit);
        y = ClampAngle(y, yMinLimit, yMaxLimit);

        var rotationHorizontal = Quaternion.Euler(cameraParentTransform.rotation.y, x, 0);
        var rotationVertical = Quaternion.Euler(y, cameraPivotVertical.localRotation.x, 0);

        cameraPivotVertical.localRotation = rotationVertical;
        cameraParentTransform.rotation = rotationHorizontal;
    }
    float ClampAngle(float angle, float min, float max)
    {
        // limit the canera angle values to - and + 360 degrees
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        // return camera rotation depending on min and max values 
        return Mathf.Clamp(angle, min, max);
    }

    public void PlayerMoveInCameraDirection()
    {
     /*   Vector3 targetPosition = new Vector3(playerTransform.position.x,
            aimingObject.position.y,
            playerTransform.position.x);
      */
        playerTransform.LookAt(aimingObject);
    }
}
