using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneBasedOnEnemies : MonoBehaviour {
    public GameObject[] enemies;
    public Health playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate() {
        if (CanLoadNextScene()) {
            LoadScene(1);
        }

        if (playerHealth.currentHealthPoints <= 0) {
            LoadScene(2);
        }
    }

    private bool CanLoadNextScene() {
        bool loadScene = true;
        for (int i = 0; i < enemies.Length; i++) {
            if (enemies[i] != null) {
                loadScene = false;
            }
        }
        return loadScene;
    }

    private void LoadScene(int index) {
        SceneManager.LoadScene(index);
    }
}
