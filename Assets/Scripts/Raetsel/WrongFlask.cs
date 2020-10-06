using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WrongFlask : MonoBehaviour
{
    public GameObject Explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        Invoke("LoadGameOverScene", 2);

        GameObject explosion = Instantiate(Explosion, this.transform);
        explosion.transform.position = new Vector3(explosion.transform.position.x, explosion.transform.position.y-1.5f, explosion.transform.position.z);
        Highscores.Instance.gameObject.GetComponent<Highscores>().GameLost();
    }

    private void LoadGameOverScene() {
        SceneManager.LoadScene("LooseScene");
    }
}
