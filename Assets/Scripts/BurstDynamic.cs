using UnityEngine;

public class ExplosionEfecto : MonoBehaviour
{
    public float tiempoDeVida = 2f;
    [Tooltip("Radio de búsqueda para encontrar el objeto original")]
    public float radioBusqueda = 0.1f; 

    void Start()
    {
        //buscamos cualquier colisionador en la posición donde apareció el efecto
        Collider[] colisionadores = Physics.OverlapSphere(transform.position, radioBusqueda);

        foreach (var col in colisionadores)
        {
            // filtramos para no copiarnos a nosotros mismos o al suelo
            // buscamos específicamente objetos con el Tag "PickUp"
            if (col.CompareTag("PickUp"))
            {
                // 3. Aplicamos la escala del objeto encontrado
                transform.localScale = col.transform.localScale;
                
                // Una vez encontrada la escala, dejamos de buscar
                break; 
            }
        }

       //destruye tras el tiempo marcado
        Destroy(gameObject, tiempoDeVida);
    }
}