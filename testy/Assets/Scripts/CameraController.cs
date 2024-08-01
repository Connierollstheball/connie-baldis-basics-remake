using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (this.gameObject.transform.parent.gameObject.GetComponent<PlayerScript>().caught == false)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

            yRotation += mouseX;
            yRotation = Mathf.Clamp(yRotation, -90f, 90f);

            playerBody.Rotate(Vector3.up * mouseX * 5f);
        }

        if (PlayerPrefs.HasKey("cameraSensitivity"))
        {
            mouseSensitivity = PlayerPrefs.GetFloat("cameraSensitivity") * 100f;
        }
        else
        {
            mouseSensitivity = 100f;
        }
    }
}