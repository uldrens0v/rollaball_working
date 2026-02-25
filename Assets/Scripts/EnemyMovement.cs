using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour
{
    //Referencia al transform del jugador
    public Transform player;
    //Referencia al NavMeshAgent
    private NavMeshAgent navMeshAgent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Iniciamos el atributo al componente navMeshAgent del propio
        //GamerObject que ejecuta el script
        this.navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.player != null)
        {
            //Actualizamos el target del enemigo(navMeshAgent)
            //esto se hace para que el enemigo se mueva hacia player
            this.navMeshAgent.SetDestination(this.player.position);
            Debug.Log("$Posicion del jugador: {this.player.position}");
        }
    }
}
