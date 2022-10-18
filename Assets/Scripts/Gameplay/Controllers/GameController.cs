using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gameplay;
using Mirror;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    public bool isGameOver;
    public static GameController Instance;
    public readonly List<IPawnController> Controllers = new List<IPawnController>();
    [HideInInspector] public int activeIndexController;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
        StartCoroutine(Playing());
    }

    private IEnumerator Playing()
    {
        yield return new WaitUntil(() => Controllers.Count >= 2);
        while (!isGameOver)
        {
            for (int i = 0; i < Controllers.Count; i++)
            {
                activeIndexController = i;
                Controllers[i].Active();
                yield return new WaitUntil(() => Controllers[i].IsActive);
                if(isGameOver) break;
            }
        }

        foreach (var controller in Controllers)
        {
            controller.GameOver();
        }
    }

    public List<IPawnController> GetControllersWithoutCurrentController(IPawnController controller)
    {
        return new List<IPawnController>(Controllers.Where(i => i != controller));
    }
}
