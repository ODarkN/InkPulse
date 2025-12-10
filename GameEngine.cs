/*
Author: ODarkN

Project: InkPulse v0.21\GameEngine.cs
This script contains the core mechanical logic of the InkPulse VN engine.
It manages game state such as menu activity and dialogue progression.
UI and dialogue data are handled elsewhere to keep this file clean.
*/

namespace InkPulse
{
    // GameEngine class manages the internal logic of the Visual Novel engine
    public class GameEngine
    {
        // Flag to check if we are currently in the main menu
        private bool inMenu = true;

        // Index of the currently displayed dialogue line, handled mechanically only
        private int dialogueIndex = 0;

        // Flag to signal that the current dialogue sequence has ended
        private bool dialogueEnded = false;

        // Property to allow UI to check if the menu is active
        public bool InMenu => inMenu;

        // Property to allow UI to check if dialogue ended
        public bool DialogueEnded => dialogueEnded;

        // Enter the main menu, reset state to menu mode
        public void EnterMenu()
        {
            inMenu = true;
            dialogueIndex = 0; // reset dialogue index when returning to menu
        }

        // Leave the main menu, gameplay or dialogue mode starts
        public void ExitMenu()
        {
            inMenu = false;
            dialogueIndex = 0; // reset dialogue index when starting new dialogue sequence
        }

        // Advance to the next dialogue line
        // Total number of dialogue lines in current sequence
        public void NextDialogue(int totalLines)
        {
            if (!dialogueEnded) // only advance if dialogue hasn't ended yet
            {
                // Check if this is the last dialogue line
                if (dialogueIndex >= totalLines - 1)
                {
                    dialogueEnded = true; // signal end of dialogue sequence
                }
                else
                {
                    dialogueIndex++; // move to next line
                }
            }
        }

        // Reset dialogue progression to the first line and clear the end ofdialogue flag
        public void ResetDialogue()
        {
            dialogueIndex = 0;
            dialogueEnded = false;
        }

        // Returns the current dialogue index, used by UI or DialogueManager
        public int GetDialogueIndex()
        {
            return dialogueIndex;
        }

        // Checks if there are more dialogue lines available, requires total line count from UI or DialogueManager
        public bool HasMoreDialogues(int totalCount)
        {
            return dialogueIndex < totalCount;
        }

        // Checks if the current dialogue line is the last one, requires total line count
        public bool IsLastDialogue(int totalCount)
        {
            return dialogueIndex == totalCount - 1;
        }
    }
}