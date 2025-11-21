/*
Author: ODarkN

Project: InkPulse v0.1\MainWindow.xaml.cs
This program displays dialogue lines and dynamically adds choice buttons using WPF.
It demonstrates creating a simple Visual Novel engine, handling user input, and updating the UI.
*/

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InkPulse
{
    // MainWindow class handles the main window of the InkPulse VN engine
    public partial class MainWindow : Window
    {
        // Index of the currently displayed dialogue line
        private int dialogueIndex = 0;

        // Array of dialogue lines
        private string[] dialogueLines = new string[]
        {
            "Welcome to InkPulse!",
            "This is a simple Visual Novel engine created in C# and WPF.",
            "At this stage, only dialogue lines are displayed in the text window.",
            "You can click on the window to move to the next line of dialogue.",
            "At the end of the dialogue, choice buttons appear and respond to clicks.",
            "The code is clean and easy to extend with new features.",
            "Click a choice button to exit the program or continue."
        };

        // Constructor: initializes UI components and shows first dialogue line
        public MainWindow()
        {
            InitializeComponent(); // Initialize all XAML components
            ShowDialogue();        // Display the first line of dialogue
        }

        // Displays the current dialogue line and sets up choice buttons if needed
        private void ShowDialogue()
        {
            // Check if there are more dialogue lines
            if (dialogueIndex < dialogueLines.Length)
            {
                DialogueText.Text = dialogueLines[dialogueIndex]; // Update TextBlock with current dialogue
                ChoicePanel.Children.Clear();                     // Clear previous buttons

                // If this is the last line, show choice buttons
                if (dialogueIndex == dialogueLines.Length - 1)
                {
                    // Button to close the application
                    AddChoice("Exit", (s, e) => Application.Current.Shutdown());

                    // Button to restart dialogue from the beginning
                    AddChoice("Continue", (s, e) =>
                    {
                        dialogueIndex = 0;  // Reset dialogue index
                        ShowDialogue();     // Show first line again
                    });
                }
            }
        }

        // Adds a choice button to the ChoicePanel with specified text and click action
        private void AddChoice(string text, RoutedEventHandler action)
        {
            // Create a new Button instance
            Button choiceButton = new Button
            {
                Content = text,                             // Button label
                Margin = new Thickness(5),                  // Space around button
                FontSize = 16,                              // Font size
                FontWeight = FontWeights.Bold,              // Bold text
                Style = (Style)FindResource("ChoiceButtonStyle") // Apply pre defined style
            };

            choiceButton.Click += action;                  // Assign click handler
            ChoicePanel.Children.Add(choiceButton);       // Add button to panel
        }

        // Handles mouse clicks on the window to advance dialogue
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dialogueIndex++;   // Move to next line
            ShowDialogue();    // Display it
        }
    }
}