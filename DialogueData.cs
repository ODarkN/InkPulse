/*
Author: ODarkN

Project: InkPulse v0.21\DialogueData.cs
This file contains all dialogue text resources for the InkPulse VN engine.
It separates dialogue content from UI and engine logic, allowing easy updates and additions.
*/

namespace InkPulse
{
    // Represents a single dialogue line, could be extended with speaker name or other metadata
    public class DialogueLine
    {
        public string Text { get; }

        public DialogueLine(string text)
        {
            Text = text;
        }
    }

    // Static class holding all dialogue sets for the game
    public static class DialogueData
    {
        // Introduction scene dialogues
        public static readonly DialogueLine[] Introduction = new DialogueLine[]
        {
            new DialogueLine("Welcome to InkPulse!"),
            new DialogueLine("This is a simple Visual Novel engine created in C# and WPF."),
            new DialogueLine("At this stage, a menu panel is displayed first, and dialogue lines appear in the text window after starting."),
            new DialogueLine("You can click on the window to move to the next line of dialogue."),
            new DialogueLine("At the end of the dialogues, choice buttons appear."),
            new DialogueLine("The code is clean and easy to extend with new features."),
            new DialogueLine("Click a choice button to return to the menu or continue.")
        };

        // Demo scene dialogues
        public static readonly DialogueLine[] Demo = new DialogueLine[]
        {
            new DialogueLine("Demo in production. Be patient!")
        };

        // You can add more scenes here as needed
        // public static readonly DialogueLine[] Scene2 = new DialogueLine[] { ... };
        // Each dialogue line is a DialogueLine object, easy to extend with speaker, emotion, or effects
        // DialogueData stores all dialogue sets as static arrays for easy loading into UI
    }
}