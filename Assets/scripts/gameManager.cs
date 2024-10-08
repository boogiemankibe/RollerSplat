using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static gameManager singleTon;
    private GroundPiece[] allGroundPieces;
    void Start()
    {
        startUpNewLevel();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void startUpNewLevel() {
        allGroundPieces=FindObjectsOfType<GroundPiece>();
    }
    private void Awake() {
        if(singleTon == null) {
            singleTon = this;

        } else if (singleTon != this) {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

    }
    private void OnEneble() {
        SceneManager.sceneLoaded+= onLevelFinishedLoading;
    }
    private void onLevelFinishedLoading(Scene scene , LoadSceneMode mode){
        SetupNewLevel();
    }
    public void checkComplete() {
        bool isfinished = true;
        for(int i=0; i<allGroundPieces.Length; i++){
            if(allGroundPieces[i].isColor==false){
                isfinished=false;
                break;
            }
        }
        if (isfinished) {
            //next level
        }
    }


    private void SetupNewLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1) {
            SceneManager.LoadScene(0);
        }
        else{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);}
    }
}
