/*
Author: ODarkN

Project: InkPulse v0.21\MainWindow.xaml.cs
This program handles the user interface for the InkPulse Visual Novel engine.
It displays dialogue lines and dynamically adds choice buttons using WPF.
All core game mechanics and dialogue progression are managed by GameEngine.
*/

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InkPulse
{
    // MainWindow class handles the main window of the InkPulse VN engine
    public partial class MainWindow : Window
    {
        private const bool MenuActive = true;

        // Instance of the game engine, handles core mechanics
        private readonly GameEngine gameEngine;

        // Index of the currently displayed dialogue line
        private int dialogueIndex = 0;

        // Array of dialogue lines
        private string[] dialogueLines = new string[]
        {
            "Welcome to InkPulse!",
            "This is a simple Visual Novel engine created in C# and WPF.",
            "At this stage, a menu panel is displayed first, and dialogue lines appear in the text window after starting.",
            "You can click on the window to move to the next line of dialogue.",
            "At the end of the dialogues, choice buttons appear.",
            "The code is clean and easy to extend with new features.",
            "Click a choice button to return to the menu or continue."
        };

        // Constructor: initializes UI components and shows first dialogue line
        public MainWindow()
        {
            InitializeComponent(); // Initialize all XAML components
            gameEngine = new GameEngine(); // Initialize engine

            ShowMenu(); // Show menu at startup
        }

        // Display the main menu with two buttons: Start to begin the dialogue, Exit to close the application
        private void ShowMenu()
        {
            gameEngine.EnterMenu(); // Mark that we are in menu

            DialogueBox.Visibility = Visibility.Collapsed; // Hide dialogue box

            // Configure the choice panel for the main menu
            ChoicePanel.Children.Clear();
            ChoicePanel.Orientation = Orientation.Vertical; // Arrange buttons in a vertical column
            ChoicePanel.HorizontalAlignment = HorizontalAlignment.Left;  // Align panel to the left side of the window
            ChoicePanel.VerticalAlignment = VerticalAlignment.Top;  // Align panel to the top of the window
            ChoicePanel.Width = 200; // Set fixed width for the panel
            ChoicePanel.Margin = new Thickness(10); // Add spacing from window edges
            ChoicePanel.Height = 260; // Set panel height to fit all menu buttons

            AddChoice("Start Demo", (s, e) =>
            {
                // Clear previous buttons
                ChoicePanel.Children.Clear();

                // Show demo message in dialogue box
                DialogueBox.Visibility = Visibility.Visible;
                DialogueText.Text = "Demo in production. Be patient!";

                // Add a single button to return to menu
                AddChoice("Back to the Menu", (s2, e2) =>
                {
                    ShowMenu();
                });
            });

            // Add "Introduction" button to menu
            AddChoice("Introduction", (s, e) =>
            {
                gameEngine.ExitMenu(); // Mark that we left the menu and are starting the dialogue
                ShowDialogue(); // Start showing the dialogue
            });

            // Add "Exit" button to menu
            AddChoice("Exit", (s, e) => Application.Current.Shutdown()); // Close the application
        }

        // Displays the current dialogue line and sets up choice buttons if needed
        private void ShowDialogue()
        {
            dialogueIndex = gameEngine.GetDialogueIndex(); // Get current dialogue index from engine
            DialogueText.Text = dialogueLines[dialogueIndex]; // Update TextBlock with current dialogue
            ChoicePanel.Children.Clear(); // Clear previous buttons
            DialogueBox.Visibility = Visibility.Visible; // Show dialogue box

            // If dialogue has ended, show choice buttons
            if (gameEngine.DialogueEnded)
            {
                // Button to restart dialogue from the beginning
                AddChoice("Continue", (s, e) =>
                {
                    gameEngine.ResetDialogue();
                    ShowDialogue(); // Show first line again
                });

                // Button to return to the main menu
                AddChoice("Back to the Menu", (s, e) =>
                {
                    ShowMenu(); // display the main menu again
                });
            }
        }

        // Adds a choice button to the ChoicePanel with specified text and click action
        private void AddChoice(string text, RoutedEventHandler action)
        {
            // Create a new Button instance
            Button choiceButton = new Button
            {
                Content = text, // Button label
                Margin = new Thickness(10), // Space around button
                FontSize = 18, // Font size
                FontWeight = FontWeights.Bold, // Bold text
                Height = 60, //Height of the button
                Style = (Style)FindResource("ChoiceButtonStyle") // Apply pre defined style
            };

            choiceButton.Click += action; // Assign click handler
            ChoicePanel.Children.Add(choiceButton); // Add button to panel
        }

        // Handles mouse clicks on the window to advance dialogue
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!gameEngine.InMenu && !gameEngine.DialogueEnded) // only advance dialogue if not in menu and not ended
            {
                gameEngine.NextDialogue(dialogueLines.Length); // advance dialogue mechanically
                ShowDialogue();
            }
        }
    }
}