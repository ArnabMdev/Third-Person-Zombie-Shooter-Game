using UnityEngine;
using UnityEngine.AI;

// ReSharper disable once CheckNamespace
namespace com.Arnab.ZombieAppocalypseShooter
{
    public class ZombieController : MonoBehaviour
    {
        private Transform _player;
        private NavMeshAgent _nvAgent;
        // Start is called before the first frame update
        void Awake()
        {
            _nvAgent = GetComponent<NavMeshAgent>();
            GameManager.PlayerActive += PlayerFound;

        }

        private void PlayerFound(Transform player)
        {
            _player = player;
            _nvAgent.destination = _player.position;
        }
    }
}
