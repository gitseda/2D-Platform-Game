using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] bool onGround;   
    [SerializeField] float speed;      
    private float width;               
    private Rigidbody2D myBody;
    [SerializeField] LayerMask engel;  

    
    private static int totalEnemyNumber = 0; 

    void Start()
    {
        totalEnemyNumber++;            
        width = GetComponent<SpriteRenderer>().bounds.extents.x; 
        myBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (transform.right * width / 2), Vector2.down, 2f, engel); 

        if(hit.collider != null) 
        {
            onGround = true;     
        }
        else  
        {
            onGround = false;  
        }
        Flip(); 
    }

    private void onDrawGizmos()  
    {
        Gizmos.color = Color.red; 
        Vector3 playerRealPosition = transform.position + (transform.right * width / 2);  
        Gizmos.DrawLine(playerRealPosition, playerRealPosition + new Vector3(0, -2f, 0)); 
    }

    private void Flip()  
    {
        if (!onGround) 
        {
            transform.eulerAngles += new Vector3(0, 180f, 0);  
        }

        myBody.velocity = new Vector2(transform.right.x * speed, 0f);
    }
}
