using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;

    [SerializeField]
    private SpawnArea spawnArea;

    [SerializeField]
    private TMP_Text scoreText;

    private GameObject currentBall;
    private SceneSystem sceneSystem = new SceneSystem();

    private int score = 0;

    private void Awake() {
        sceneSystem.init();
    }

    public void scored(){
        score += 1;
        scoreText.SetText(score.ToString());
    }

    public void startGame(){
        spawnBall();        
    }

    public Scene getScenePrediction(){
        return sceneSystem.getScenePrediction();
    }

    public PhysicsScene2D getScenePredictionPhysics(){
        return sceneSystem.getScenePredictionPhysics();
    }

    private void respawnBall(){
        destroyBall();
        spawnBall();
    }

    private void spawnBall(){
        currentBall = Instantiate(ballPrefab);
        Ball ballCommand = currentBall.GetComponent<Ball>();

        ballCommand.scoredEvent += scored;
        ballCommand.onGroundEvent += respawnBall;

        spawnArea.spawnBall(currentBall.transform);
    }

    private void destroyBall(){
        if(!currentBall) return;

        Ball ballCommand = currentBall.GetComponent<Ball>();

        ballCommand.scoredEvent -= scored;
        ballCommand.onGroundEvent -= respawnBall;

        Destroy(currentBall);
        currentBall = null;
    }

    void FixedUpdate()
    {
        sceneSystem.FixedUpdate();
    }
}
