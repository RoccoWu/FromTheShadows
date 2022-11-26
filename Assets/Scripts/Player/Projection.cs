using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour
{
    private PhysicsScene physicsScene;
    private Scene simulationScene;
    [SerializeField] private Transform obstaclesParent;
    // Start is called before the first frame update
    void Start()
    {
        CreatePhysicsScene();
    }

    void CreatePhysicsScene()
    {
        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        physicsScene = simulationScene.GetPhysicsScene();

        foreach(Transform obj in obstaclesParent) {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObj.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField] private LineRenderer line;
    [SerializeField] private int maxPhysicsFrameIterations;
    public void SimulateTrajectory(KnifeManager knifePrefab, Vector3 pos, Vector3 velocity) 
    {
        var ghostObj = Instantiate(knifePrefab, pos, Quaternion.identity);
        ghostObj.GetComponent<Renderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, simulationScene);

        ghostObj.throwKnife();
        line.positionCount = maxPhysicsFrameIterations;
        for(int i = 0; i<maxPhysicsFrameIterations; i++)
        {
            physicsScene.Simulate(Time.fixedDeltaTime);
            line.SetPosition(i, ghostObj.transform.position);
        }
        Destroy(ghostObj.gameObject);
    }
}
