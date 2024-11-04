using UnityEngine;
using Pathfinding;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;
using Core.Audio;
using Core.Services.EventBus;
using Core.Services.SceneManagement;
using Cysharp.Threading.Tasks;
using Game.Services.Events;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class Patrol : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameOver;
    [SerializeField] List<Transform> patrolPoints;
    [SerializeField] string highPriorityTag = "Human";

    [SerializeField] float nextPatrolPointDistance = 0.05f;

    [SerializeField] Transform[] potentialPatrolPoints;

    [SerializeField] float secToLostHuman = 5f;

    private SceneTransitionService _sceneTransition;
    private AudioService _audioService;
    
    private AIDestinationSetter destinationSetter;
    private EnemyVision enemyVision;
    private int currentPointIndex = 0;
    bool isPatrol = true;

    [Inject]
    public void Construct(SceneTransitionService sceneTransitionService, AudioService audioService)
    {
        _sceneTransition = sceneTransitionService;
        _audioService = audioService;
    }
    
    void Start()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        enemyVision = GetComponent<EnemyVision>();
        destinationSetter.target = null;
        _gameOver.enabled = false;
        if (patrolPoints.Count > 0)
        {
            destinationSetter.target = patrolPoints[currentPointIndex];
        }
    }

    void Update()
    {
        if (enemyVision != null && enemyVision.TargetDetected() && enemyVision.GetDetectedTarget().gameObject.CompareTag(highPriorityTag))
        {
            if (isPatrol)
                StartCoroutine(ForgetAboutHuman());

            isPatrol = false;
            destinationSetter.target = enemyVision.GetDetectedTarget();
        }
        else if (destinationSetter.target != null)
        {
            isPatrol = true;
            float distanceToTarget = Vector3.Distance(transform.position, destinationSetter.target.position);
            if (distanceToTarget < nextPatrolPointDistance)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
                destinationSetter.target = patrolPoints[currentPointIndex];
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(highPriorityTag) || other.gameObject.CompareTag("Player"))
        {
            _gameOver.enabled = true;
            _audioService.PlaySfx("monstr golos");
            RestartWithDelay(3f).Forget();
        }
    }

    private async UniTask RestartWithDelay(float seconds)
    {
        await UniTask.WaitForSeconds(seconds);
        _sceneTransition.LoadScene("Gameplay");
    }
    

    public void addAllPoints()
    {
        patrolPoints.AddRange(potentialPatrolPoints);
    }

    public void addPatrolPoint(Transform newPp)
    {
        patrolPoints.Add(newPp);
    }

    public void removePatrolPoint(Transform newPp)
    {
        if (patrolPoints.Contains(newPp))
            patrolPoints.Remove(newPp);
    }

    IEnumerator ForgetAboutHuman()
    {
        yield return new WaitForSeconds(secToLostHuman);
        destinationSetter.target = patrolPoints[currentPointIndex];
        enemyVision.LostHuman();
        isPatrol = true;
    }
}