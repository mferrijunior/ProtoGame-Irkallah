using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerPosition;
    public Sprite playerSprite;
    public VectorValue playerStorage;
    public SpriteValue playerStorageSprite;
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    public float fadeWait;

    void Awake()
    {
        if (fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity);
            Destroy(panel, 1f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerStorage.defaultValue = playerPosition;
            playerStorageSprite.initialValue = playerSprite;
            StartCoroutine(SceneToLoadAsync());
        }
    }

    IEnumerator SceneToLoadAsync()
    {
        if (fadeOutPanel != null)
        {
            GameObject panel = Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        }

        yield return new WaitForSeconds(fadeWait);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
