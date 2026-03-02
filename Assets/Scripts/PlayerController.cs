using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private float movementX;

    private float movementY;

    public float velocidad = 10;

    //para mostrar el menu cuando acaba el juego
    public GameObject menuPrincipal;

    //Hará el conteo de objetos recibidos
    private int count;
    private int _sizeEnemies;
    public GameObject explosionFX;
    public GameObject burstFX;
    public GameObject runningFX;
    public TextMeshProUGUI countText;
    public GameObject winText;

//audioclip sirve para poner varios clips de audio en el script del player
    public AudioClip pickUpSound;
    public AudioClip defeatSound;
    public AudioClip winSound;
    public AudioClip choqueSound;

    private AudioSource audioSource;

//este es un audiosource externo, si no lo metemos tendremos problemas al querer crear el sonido de eliminacion ya que al eliminar al player tambien eliminamos al audiosource del player, esto con un externo no pasa
    public AudioSource soundManagerSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sizeEnemies = GameObject.FindGameObjectsWithTag("PickUp").Length;
        rb = GetComponent<Rigidbody>();
        this.count = 0;
        SetCountText();
        this.winText.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    /*void OnEnable()
    {
        // reseteamos la posicion si es necesario (opcional)
        // transform.position = new Vector3(0, 1, 0);

        // aseguramos que el objeto este activo visualmente
        this.gameObject.SetActive(true);

        // reseteamos el contador y los textos
        this.count = 0;


        // rehabilitamos las fisicas por si acaso
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }*/

    void OnMove(InputValue movementValue)
    {
        // Debug.Log($"Movimiento detectado: {movementValue}");

        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;

        // Debug.Log($"Valor del vector X:{movementX}");
        // Debug.Log($"Valor del vector Y:{movementY}");
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        Debug.Log($"Movimiento: {movement}");
        rb.AddForce(movement * velocidad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            Destroy(Instantiate(burstFX, other.transform.position, Quaternion.identity), 3);
            other.gameObject.SetActive(false);
            this.count++;
            Debug.Log($"Objetos recogidos:{this.count}");
            SetCountText();
            audioSource.PlayOneShot(pickUpSound);
        }
    }

    // Update is called once per frame
    //    void Update()
    //    {
    //        
    //    }
    private void SetCountText()
    {
        this.countText.text = "Count: " + this.count.ToString();
        if (this.count == _sizeEnemies)
        {
            this.winText.SetActive(true);
            Destroy(Instantiate(explosionFX, GameObject.FindGameObjectWithTag("Enemy").transform.position,
                Quaternion.identity), 2);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            soundManagerSource.PlayOneShot(winSound);
            //Invoke("MostrarMenu",2f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(explosionFX, transform.position, Quaternion.identity);
            soundManagerSource.PlayOneShot(defeatSound);
            this.gameObject.SetActive(false);
            this.winText.SetActive(true);
            this.winText.GetComponent<TextMeshProUGUI>().text = "Has perdido! D:";
            Invoke("MostrarMenu",2f);
        }

        Debug.Log($"Colision detectada con: {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Walls"))
        {
            audioSource.PlayOneShot(choqueSound);
        }
    }

    public void ActualizarVelocidad(string inputfield)
    {
        if (float.TryParse(inputfield, out float nuevaVelocidad) && nuevaVelocidad > 0)
        {
            this.velocidad = nuevaVelocidad;
        }
    }

    public void MostrarMenu()
    {
        if (menuPrincipal != null) menuPrincipal.SetActive(true);
        ReiniciarJuegoCompleto();
        if (transform.parent != null) transform.parent.gameObject.SetActive(false);
    }

    public void ReiniciarJuegoCompleto()
    {
        // recarga la escena actual de 0
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ActualizarMusica(string inputfield)
    {
    }
}