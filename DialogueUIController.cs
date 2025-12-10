/*
Author: ODarkN

Project: InkPulse v0.21\DialogueUIController.cs
This script handles all dialogue UI logic.
It updates dialogue text, creates and displays choice buttons, and communicates with GameEngine.
*/

using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;

namespace InkPulse
{
    // DialogueUIController class manages dialogue text, choice buttons, and interactions
    public class DialogueUIController
    {
        private readonly GameEngine gameEngine; // Reference to GameEngine for mechanical logic
        private readonly TextBlock dialogueText; // Reference to dialogue text block in UI
        private readonly StackPanel choicePanel; // Reference to choice buttons panel in UI
        private readonly Border dialogueBox; // Reference to dialogue box container
        private readonly DialogueLine[] dialogueLines; // Holds the dialogue lines for the current scene
        private readonly Style choiceButtonStyle; // Holds the style for choice buttons

        // Assigns references to UI elements, game engine and style
        public DialogueUIController(GameEngine engine, TextBlock dialogueText, StackPanel choicePanel, Border dialogueBox, DialogueLine[] dialogueLines, Style choiceButtonStyle)
        {
            gameEngine = engine;
            this.dialogueText = dialogueText;
            this.choicePanel = choicePanel;
            this.dialogueBox = dialogueBox;
            this.dialogueLines = dialogueLines;
            this.choiceButtonStyle = choiceButtonStyle; // Assign passed style
        }

        // Displays the current dialogue line and updates UI
        public void ShowDialogue()
        {
            dialogueText.Text = dialogueLines[gameEngine.GetDialogueIndex()].Text;
            // Display the text of the current dialogue line from the dialogue array
            choicePanel.Children.Clear(); // Clear previous choice buttons
            dialogueBox.Visibility = Visibility.Visible; // Make dialogue box visible

            // If dialogue has ended, show choice buttons
            if (gameEngine.DialogueEnded)
                HandleDialogueEnd(); // Handle end of dialogue buttons in one place

        }

        // Handles creation of buttons at the end of a dialogue sequence
        private void HandleDialogueEnd()
        {
            AddChoice("Continue", (s, e) =>
            {
                gameEngine.ResetDialogue();
                ShowDialogue();
                // Restart dialogue sequence after it ends
            });

            AddChoice("Back to the Menu", (s, e) =>
            {
                ((MainWindow)Application.Current.MainWindow).ShowMenu();
                // Return to main menu from the end of dialogue choices
            });
        }

        // Advances dialogue mechanically and updates the UI
        public void NextDialogue()
        {
            if (!gameEngine.DialogueEnded) // Only advance if dialogue has not ended
            {
                gameEngine.NextDialogue(dialogueLines.Length); // Advance dialogue in engine
            }
            ShowDialogue(); // Show Dialogue refreshes UI and creates buttons if dialogue has ended
        }



        // Adds a single choice button to the choice panel
        private void AddChoice(string text, RoutedEventHandler action)
        {
            Button choiceButton = new Button
            {
                Content = text, // Button text
                Margin = new Thickness(10), // Space around button
                FontSize = 18, // Font size
                FontWeight = FontWeights.Bold, // Bold font
                Height = 60, // Button height
                Style = choiceButtonStyle // Use passed style from MainWindow
            };

            choiceButton.Click += action; // Assign click event
            choicePanel.Children.Add(choiceButton); // Add button to panel
            // All choice buttons, whether in dialogue or menu, are added dynamically here

        }
    }
}
