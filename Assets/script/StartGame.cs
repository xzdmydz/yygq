using UnityEngine;

public class StartGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject CharacterModel;



    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //CharacterModel.transform.localScale = new Vector3(1, 1, 0);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
