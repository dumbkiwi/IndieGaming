using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    Rigidbody2D rb;
    Transform Transform;
    SpriteRenderer SpriteControl;
    Animator Animator;

    //changeable game numbers
    public float ExtraHeights = 0.5f;
    public float PlayerSpeed = 200f;
    public float MaxJump = 100f;
    public float DashSpeed = 500f;
    Vector3 FacingVector=new Vector3(1,0);
    public int DashCount=1;
    int LiveDashCount=1;

    //Debug
    public bool RaycastDebug=false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Transform = GetComponent<Transform>();
        SpriteControl = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();

        //value assignment
        LiveDashCount = DashCount;
    }
    
    bool Dashable() {
        if (GroundCheck()) {
            LiveDashCount=DashCount;
        };
        if (LiveDashCount>0) {
            return true;
        } else {return false;};
    }

    void Update(){
        //Jump
        if (GroundCheck() && Input.GetButtonDown("Jump")) {
            rb.velocity = Vector2.up * MaxJump * Time.fixedDeltaTime;
        };

        //Dashing
        if (Dashable() && Input.GetButtonDown("Fire1")) {
            Transform.position=Transform.position + FacingVector;
            LiveDashCount-=1;
        };
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //Facing
        if (SpriteControl.flipX) {
            FacingVector.Set(-1*DashSpeed*Time.fixedDeltaTime,0,0);
        } else {
            FacingVector.Set(1*DashSpeed*Time.fixedDeltaTime,0,0);
        };

        //Left Right Movement
        if (Input.GetAxisRaw("Horizontal") != 0) {
            rb.velocity= new Vector2(PlayerSpeed*Input.GetAxisRaw("Horizontal")*Time.fixedDeltaTime,rb.velocity.y);
        };

        

        //Flip character
        switch (Input.GetAxisRaw("Horizontal")) {
            case -1:
            SpriteControl.flipX=true;
            break;
            case 1:
            SpriteControl.flipX=false;
            break;
        };

        //animator
        Animator.SetFloat("Speed",Mathf.Abs(Input.GetAxisRaw("Horizontal")));

        //RaycastDebug --WIP
        if (RaycastDebug) {
        Debug.DrawRay(Transform.position, Vector2.down * ExtraHeights,Color.red);
        };
    }

    //GroundChecking
    public bool GroundCheck() {
        RaycastHit2D Grounded = Physics2D.Raycast(Transform.position, Vector2.down, ExtraHeights, groundLayerMask);

        if (Grounded.collider != null) {return true;} else {return false;};
        
    }
}
