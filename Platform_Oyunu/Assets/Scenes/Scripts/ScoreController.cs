using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] Text scoreValueText;   
    [SerializeField] float y;              
    
    private void Update()
    {
        transform.Rotate(new Vector3(0f, y, 0f));        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)  
    {
        if (collision.gameObject.CompareTag("Player"))  
        {
            GameObject.Find("Level Manager").GetComponent<LevelManager>().AddScore(50);
            Destroy(gameObject);                        
        } 
    }
}
