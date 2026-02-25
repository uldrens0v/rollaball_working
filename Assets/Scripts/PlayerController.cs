using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private float movementX;

    private float movementY;

    public float velocidad = 10;

    //Hará el conteo de objetos recibidos
    private int  count;
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
            Destroy(Instantiate(burstFX, other.transform.position, Quaternion.identity),3);
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
                Quaternion.identity),2);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            soundManagerSource.PlayOneShot(winSound);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(explosionFX, transform.position, Quaternion.identity);
            soundManagerSource.PlayOneShot(defeatSound);
            Destroy(this.gameObject);
            this.winText.SetActive(true);
            this.winText.GetComponent<TextMeshProUGUI>().text = "Has perdido! D:";
        }

        Debug.Log($"Colision detectada con: {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Walls"))
        {
            audioSource.PlayOneShot(choqueSound);
        }
    }
}