using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject player;

    private Vector3 offset;
    
    void Start()
    {
        offset = this.transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.position = player.transform.position + offset;
    }
}
