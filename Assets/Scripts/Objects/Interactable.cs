using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Signal))]
public class Interactable : MonoBehaviour
{
    [SerializeField]
    private bool animationInteraction;
    private bool playerInRange;
    [SerializeField]
    private GameObject dialogBox;
    [SerializeField]
    private Text dialogText;
    private bool hasSeenDialog;

    public Signal contextClue;
    [HideInInspector]
    public bool hasBeenAnimated;
    [HideInInspector]
    public Player player;
    public string dialog;

    public virtual void AnimationInteract() { }

    public virtual void DialogInteract()
    {
        if (dialogBox.activeInHierarchy)
        {
            CloseDialog();
        }
        else
        {
            OpenDialog();
        }
    }

    public void SetHasBeenAnimated(bool animated)
    {
        hasBeenAnimated = animated;
    }

    public void SetPlayerMove(bool canMove)
    {
        player.SetCanMove(canMove);
    }

    public bool CanInteract()
    {
        return animationInteraction && !hasBeenAnimated;
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetButtonDown("interact") && playerInRange)
        {
            if (CanInteract())
            {
                AnimationInteract();
            }

            DialogInteract();

            if (animationInteraction)
            {
                SetHasBeenAnimated(true);
            }
        }
    }

    void CloseDialog()
    {
        if (hasSeenDialog)
        {
            dialogBox.SetActive(false);
            SetPlayerMove(true);
            player.SetState(PlayerState.walk);
        }

        if (!animationInteraction)
        {
            contextClue.Raise();
            hasSeenDialog = false;
        }
    }

    void OpenDialog()
    {
        if (!hasSeenDialog)
        {
            hasSeenDialog = true;
            dialogBox.SetActive(true);
            dialogText.text = dialog;
            SetPlayerMove(false);
            contextClue.Raise();
        }
    }

    void RaiseTriggerBasedContextClue()
    {
        if (CanInteract())
        {
            contextClue.Raise(); // single interaction items
        }
        else if (!animationInteraction)
        {
            contextClue.Raise(); // multiple interaction items
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            RaiseTriggerBasedContextClue();
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            RaiseTriggerBasedContextClue();
            playerInRange = false;
        }
    }
}
