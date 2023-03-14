using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float mySpeedX;                         
    [SerializeField] float speed;                   
    [SerializeField] float jumpPower;               
    private Rigidbody2D myBody;
    private Vector3 defaultLocalScale;
    public bool onGround;                           
    private bool canDoubleJump;                     
    [SerializeField] float currentAttacketTimer;    
    [SerializeField] float defaultAttackedTimer;    
    [SerializeField] GameObject arrow;              
    [SerializeField] bool attacked;                 
    private Animator myAnimator;                    
    [SerializeField] int arrowNumber;               
    [SerializeField] Text arrowNumberText;          
    [SerializeField] GameObject winPanel, losePanel;

    void Start()
    {
        attacked = false;                           
        myAnimator = GetComponent<Animator>();      
        myBody = GetComponent<Rigidbody2D>();       
        defaultLocalScale = transform.localScale;
        arrowNumberText.text = arrowNumber.ToString();  
    }

    void Update()
    {
        mySpeedX = Input.GetAxis("Horizontal");             
        myAnimator.SetFloat("Speed", Mathf.Abs(mySpeedX));  
        myBody.velocity = new Vector2(mySpeedX * speed, myBody.velocity.y);   

        #region Playerin Y�z�n� Sa� ve Sola Eksende D�nd�rme
        if (mySpeedX > 0)       
        {
            transform.localScale = new Vector3(defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);  
        }
        else if (mySpeedX < 0) 
        {
            transform.localScale = new Vector3(-defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z); 
        }
        #endregion

        #region Playerin Z�plamas�n� Sa�la
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(onGround==true)   // E�er Player Zeminin �zerindeyse
            {
                myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);  // Z�plamas� i�in
                canDoubleJump = true;                                         // Bir kez daha z�playabilsin diye  true oldu.
                myAnimator.SetTrigger("Jump");                                // Jump animasyonunu oynat

            }
            else                // E�er zemin �zerinde de�ilse
            {
                if(canDoubleJump == true)                                       // E�er ikinci defa z�plama �zelli�i aktifse
                {
                    myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);// Bir kez daha z�pla
                    canDoubleJump=false;                                        // Z�plama
                }
            }
        }
        #endregion

        #region Playerin Ok Atmas�n�n Kontrol�n� Sa�lama
        if(Input.GetMouseButtonDown(0) && arrowNumber>0) // E�er mouse t�klanm��sa ve ok say�s� 0'dan b�y�kse 
        {
            if(attacked==false)                          // Atak durumu aktif de�ilse (Ok atm�yorsa)
            {
                attacked = true;                         // Atak aktif olsun
                myAnimator.SetTrigger("Attack");         // Player ok atmaya ba�lad���nda attack true iken Animasyonu oynat.
                Invoke("Fire",0.5f);                     // Yar�m saniye sonra ok at�ls�n (Invok -> Ge� ba�lat�yor.) 
            }
           
        }
        #endregion

        #region Playerin S�reli Olarak Ok Atmas�n� Engelleme 
        if (attacked==true)  
        {
            currentAttacketTimer -= Time.deltaTime;
        }
        else
        {
            currentAttacketTimer = defaultAttackedTimer;
        }

        if(currentAttacketTimer <= 0)
        {
            attacked = false;
        }
        #endregion
    }
    void Fire() 
    {
        GameObject ok = Instantiate(arrow, transform.position, Quaternion.identity);
        ok.transform.parent = GameObject.Find("Arrows").transform; 

        if (transform.localScale.x > 0)                                              
        {
            ok.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0f);           
        }
        else                                                                        
        {
            Vector3 okScale = ok.transform.localScale;
            ok.transform.localScale = new Vector3(-okScale.x, okScale.y, okScale.z); 
            ok.GetComponent<Rigidbody2D>().velocity = new Vector2(-5f, 0f);          
        }
        arrowNumber--;                                                               
        arrowNumberText.text = arrowNumber.ToString();                               
    }

    private void OnCollisionEnter2D(Collision2D collision)   
    {
        if (collision.gameObject.CompareTag("Enemy"))        
        {
            GetComponent<TimeController>().enabled = false;  
            Die();                                            
        }
        else if(collision.gameObject.CompareTag("Finish"))    
        {
            Destroy(collision.gameObject);
            StartCoroutine(Wait(true));
        }
    }
    public void Die()
    {
        myAnimator.SetFloat("Speed", 0);                             
        myAnimator.SetTrigger("Die");                               
        
        myBody.constraints = RigidbodyConstraints2D.FreezePosition; 
        enabled = false;                                             
        StartCoroutine(Wait(false));
        GetComponent<TimeController>().enabled = false;   
    }

    IEnumerator Wait(bool win)
    {
        yield return new WaitForSecondsRealtime(1f);                
        Time.timeScale = 0;
        if (win == true)
        {
            winPanel.SetActive(true);                                
        }
        else
        { 
            losePanel.SetActive(true);                               
        }
    }
}
