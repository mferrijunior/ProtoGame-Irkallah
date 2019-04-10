using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class TreasureChest : Interactable
{
    public Item contents;
    public Signal raiseItem;
    public Inventory playerInventory;

    private Animator anim;

    public override void DialogInteract()
    {
        if (CanInteract())
        {
            OpenChest();
        }
        else
        {
            UnloadChest();
        }

        base.DialogInteract();
    }

    public override void AnimationInteract()
    {
        base.AnimationInteract();
        anim.SetBool("open", true);
        contextClue.Raise();
        SetPlayerMove(false);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OpenChest()
    {
        // Add the item dialog
        dialog = contents.itemDescription;

        // Set player state
        player.SetState(PlayerState.interact);

        // Add contents to player inventory and as current item
        playerInventory.AddItem(contents);
        playerInventory.currentItem = contents;

        // Raise signal to animate player
        raiseItem.Raise();

        // Raise sign to close context clue
        contextClue.Raise();

        // Set chest to opened
        SetHasBeenAnimated(true);
    }

    void UnloadChest()
    {
        // Turn off player animation
        player.SetAnimatorBool("receive_item", false);

        // Set player state
        player.SetState(PlayerState.idle);

        // Set current item to empty
        playerInventory.currentItem = null;

        // Raise signal to player to nullify item sprite
        raiseItem.Raise();
    }
}
