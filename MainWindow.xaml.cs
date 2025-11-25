/*
Author: ODarkN

Project: InkPulse v0.2\MainWindow.xaml.cs
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
        private const bool MenuActive = true;

        // Index of the currently displayed dialogue line
        private int dialogueIndex = 0;

        // Flag to check if we are in the main menu
        private bool inMenu = true;

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
            ShowMenu(); // Show menu at startup
        }

        // Display the main menu with two buttons: Start to begin the dialogue, Exit to close the application
        private void ShowMenu()
        {
            inMenu = MenuActive;

            DialogueBox.Visibility = Visibility.Collapsed; // hide dialogue box

            // Configure the choice panel for the main menu
            ChoicePanel.Children.Clear();
            ChoicePanel.Orientation = Orientation.Vertical; // arrange buttons in a vertical column
            ChoicePanel.HorizontalAlignment = HorizontalAlignment.Left;  // align panel to the left side of the window
            ChoicePanel.VerticalAlignment = VerticalAlignment.Top;  // align panel to the top of the window
            ChoicePanel.Width = 200; // set fixed width for the panel
            ChoicePanel.Margin = new Thickness(10); // add spacing from window edges

            // add "Start" button to menu
            AddChoice("Start", (s, e) =>
            {
                inMenu = false; // mark that we left the menu and are starting the dialogue
                dialogueIndex = 0; // reset dialogue index to the first line
                DialogueBox.Visibility = Visibility.Visible; // show dialogue box
                ShowDialogue(); //start showing the dialogue
            });

            // add "Exit" button to menu
            AddChoice("Exit", (s, e) => Application.Current.Shutdown()); // Close the application
        }

        // Displays the current dialogue line and sets up choice buttons if needed
        private void ShowDialogue()
        {
            // Check if there are more dialogue lines
            if (dialogueIndex < dialogueLines.Length)
            {
                DialogueText.Text = dialogueLines[dialogueIndex]; // Update TextBlock with current dialogue
                ChoicePanel.Children.Clear(); // Clear previous buttons

                // If this is the last line, show choice buttons
                if (dialogueIndex == dialogueLines.Length - 1)
                {
                    // Button to restart dialogue from the beginning
                    AddChoice("Continue", (s, e) =>
                    {
                        dialogueIndex = 0; // Reset dialogue index
                        ShowDialogue(); // Show first line again
                    });

                    // Button to close the application
                    AddChoice("Back to the Menu", (s, e) =>
                    {
                        inMenu = MenuActive; // mark that we are back in menu
                        ShowMenu(); // display the main menu again
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
            if (!inMenu) // only advance dialogue if not in menu
            {
                dialogueIndex++;
                ShowDialogue();
            }
        }

    }
}