using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMove : MonoBehaviour
{
    public Vector2 cameraChange;
    public Vector3 playerChange;
    public bool needText;
    public string placeName;
    public GameObject text;
    public Text placeText;
    public float textFadeInTime;
    public float textFadeOutTime;

    private CameraMovement cam;
    private Color placeTextColor;

    void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
        placeTextColor = new Color(255f, 255f, 255f, 1f);
        placeText.CrossFadeColor(placeTextColor, 0f, false, true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            cam.minPosition += cameraChange;
            cam.maxPosition += cameraChange;
            other.transform.position += playerChange;

            if (needText)
            {
                StartCoroutine(PlaceNameCo());
            }
        }
    }

    IEnumerator PlaceNameCo()
    {
        placeText.text = placeName;
        text.SetActive(true);
        placeTextColor.a = 255f;
        placeText.CrossFadeColor(placeTextColor, textFadeInTime, false, true);
        yield return new WaitForSeconds(3f);
        placeTextColor.a = 1f;
        placeText.CrossFadeColor(placeTextColor, textFadeOutTime, false, true);
        yield return new WaitForSeconds(2.5f);
        text.SetActive(false);
        needText = false;
    }
}
