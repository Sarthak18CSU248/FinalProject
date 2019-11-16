using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float cameraZpos = -10.0f;
    public float cameraXOffset = 5f;
    public float cameraYOffset = 1f;

    public  float HorizontalSpeed = 2.0f;

    public  float VerticalSpeed = 2.0f;

    private Transform _camera;
    private PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = player.GetComponent<PlayerController>();
        _camera = Camera.main.transform;
        _camera.position = new Vector3(player.transform.position.x + cameraXOffset, player.transform.position.y + cameraYOffset, player.transform.position.z + cameraZpos);
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerController.isFacingRight)
        {
            _camera.position = new Vector3(Mathf.Lerp(_camera.position.x, player.transform.position.x + cameraXOffset, HorizontalSpeed * Time.deltaTime),
                Mathf.Lerp(_camera.position.y, player.transform.position.y + cameraYOffset, VerticalSpeed * Time.deltaTime),
                cameraZpos);
        }
        else
        {
            _camera.position = new Vector3(Mathf.Lerp(_camera.position.x, player.transform.position.x - cameraXOffset, HorizontalSpeed * Time.deltaTime),
                Mathf.Lerp(_camera.position.y, player.transform.position.y + cameraYOffset, VerticalSpeed * Time.deltaTime),
                cameraZpos);
        }
    }
}
