/*
Author: ODarkN

Project: InkPulse v0.21\MainWindow.xaml.cs
This program handles the user interface for the InkPulse Visual Novel engine.
It displays dialogue lines and dynamically adds choice buttons using WPF.
All core game mechanics and dialogue progression are managed by GameEngine and DialogueUIController.
*/

using Microsoft.VisualBasic;
using System.Drawing;
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

        // Instance of the dialogue UI controller, handles dialogue display and choice buttons
        private DialogueUIController dialogueController;

        // Initializes UI components and shows first dialogue line
        public MainWindow()
        {
            InitializeComponent(); // Initialize all XAML components

            // Retrieve the ChoiceButtonStyle from XAML resources after components are initialized
            Style choiceButtonStyle = (Style)FindResource("ChoiceButtonStyle"); // Get style resource

            gameEngine = new GameEngine(); // Initialize engine

            // Initialize dialogue controller with UI references, dialogue data and button style
            dialogueController = new DialogueUIController(
                gameEngine,
                DialogueText,
                ChoicePanel,
                DialogueBox,
                DialogueData.Introduction, // Starting dialogue set
                choiceButtonStyle // Pass the style explicitly
            );

            ShowMenu(); // Show menu at startup
                        // At this point the main menu UI is prepared and ready for interaction
        }

        // Display the main menu with Start, Introduction and Exit buttons
        public void ShowMenu()
        {
            gameEngine.EnterMenu(); // Mark that we are in menu
                                    // Set the engine to menu mode, so dialogue does not advance on clicks

            DialogueBox.Visibility = Visibility.Collapsed; // Hide dialogue box
                                                           // Hide dialogue box in menu, only buttons are displayed

            // Configure the choice panel for the main menu
            ChoicePanel.Children.Clear();
            ChoicePanel.Orientation = Orientation.Vertical; // Arrange buttons in a vertical column
            ChoicePanel.HorizontalAlignment = HorizontalAlignment.Left;  // Align panel to the left side of the window
            ChoicePanel.VerticalAlignment = VerticalAlignment.Top;  // Align panel to the top of the window
            ChoicePanel.Width = 200; // Set fixed width for the panel
            ChoicePanel.Margin = new Thickness(10); // Add spacing from window edges
            ChoicePanel.Height = 260; // Set panel height to fit all menu buttons

            // Add "Start Demo" button
            AddChoice("Start Demo", (s, e) =>
            {
                // Clear previous buttons
                ChoicePanel.Children.Clear();

                // Set dialogue controller to demo dialogues
                DialogueBox.Visibility = Visibility.Visible;
                dialogueController = new DialogueUIController(
                    gameEngine,
                    DialogueText,
                    ChoicePanel,
                    DialogueBox,
                    DialogueData.Demo,
                    (Style)FindResource("ChoiceButtonStyle") // Pass style again
                );
                // Reset dialogue state when starting a new scene
                gameEngine.ResetDialogue();

                dialogueController.ShowDialogue();

                // Add a single button to return to menu
                AddChoice("Back to the Menu", (s2, e2) =>
                {
                    ShowMenu(); // Return to the main menu after demo sequence ends
                });
            });
            // Each AddChoice creates a menu button and assigns its behavior


            // Add "Introduction" button
            AddChoice("Introduction", (s, e) =>
            {
                gameEngine.ExitMenu(); // Mark that we left the menu and are starting the dialogue
                dialogueController = new DialogueUIController(
                    gameEngine,
                    DialogueText,
                    ChoicePanel,
                    DialogueBox,
                    DialogueData.Introduction,
                    (Style)FindResource("ChoiceButtonStyle") // Pass style again
                );
                dialogueController.ShowDialogue(); // Start introduction dialogue sequence
            });

            // Add "Exit" button
            AddChoice("Exit", (s, e) => Application.Current.Shutdown()); // Closes the application when Exit is clicked
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
                Height = 60, // Height of the button
                Style = (Style)FindResource("ChoiceButtonStyle") // Apply pre defined style
            };

            choiceButton.Click += action; // Assign click handler
            ChoicePanel.Children.Add(choiceButton); // Add button to panel
        }

        // Handles mouse clicks on the window to advance dialogue
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!gameEngine.InMenu && !gameEngine.DialogueEnded) // Only advance dialogue if not in menu and not ended
            {
                dialogueController.NextDialogue(); // Advance dialogue mechanically and update UI
            }
        }
    }
}
