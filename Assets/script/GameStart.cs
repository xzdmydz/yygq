using UnityEngine;

public class GameStart : MonoBehaviour
{
    public Rigidbody2D CharacterRig;
    public GameObject stickman;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void qian()
    {
        stickman.transform.position = new Vector3(0, 81, 0);
    }

    public void dui()
    {
        CharacterRig.transform.position = new Vector3(-58, 58, 0);
    }

    public void li()
    {
        CharacterRig.transform.position = new Vector3(-81, 0, 0);
    }

    public void zhen()
    {
        CharacterRig.transform.position = new Vector3(-58, -58, 0);
    }

    public void kun()
    {
        CharacterRig.transform.position = new Vector3(0, -81, 0);
    }
    public void gen()
    {
        CharacterRig.transform.position = new Vector3(58, -58, 0);
    }
    public void kan()
    {
        CharacterRig.transform.position = new Vector3(81, 0, 0);
    }
    public void xun()
    {
        CharacterRig.transform.position = new Vector3(58, 58, 0);
    }

    public void boss()
    {
        CharacterRig.transform.position = new Vector3(-1, 42, 0);
    }

    public void exit()
    {
        Application.Quit();
    }

}
