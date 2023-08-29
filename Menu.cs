/*
 * t-P_Prog-Schafstall-Ethan-Demineur
 * Ethan Schafstall
 * 09.03.2023
 * CIN1B
 * ETML
 */

namespace t_P_Prog_Schafstall_Ethan_Demineur
{
    /// <summary>
    /// Menu class responsible for creating menu objects used for navigation/collecting game setting during the first stage of the program.
    /// </summary>
    internal class Menu
    {
        private int selectedIndex = 0;
        public string[] options;
        public string prompt;
        public Menu()
        { }
        public Menu(string menuPrompt, string[] menuOptions)
        {
            prompt = menuPrompt;
            options = menuOptions;
        }

        /// <summary>
        /// Method that writes the prompt, and options based on which option is "active", based on the selectedIndex int.
        /// </summary>
        public void DisplayOptions()
        {
            Console.SetCursorPosition(50, 12);
            Console.Write(prompt);
            Console.SetCursorPosition(50, 14);
            for (int i = 0; i < options.Length; i++)
            {
                string currentOption = options[i];
                string prefix;

                // If option equates to selectedIndex, then option is "active", change prefix, foreground and background color.
                if (selectedIndex == i)
                {
                    prefix = "*   ";
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                // Else standard prefix and colors.
                else
                {
                    prefix = " ";
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.SetCursorPosition(50, i + 14);
                Console.WriteLine($"{prefix} << {currentOption} >>");
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Method that clears menu text so that it can be updated with current selected/non selected options. Only clears out the zones used for displaying the menu.
        /// </summary>
        public void ClearScreen()
        {
            for (int i = 12; i < 21; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new String(' ', 200));
            }

        }

        /// <summary>
        /// Method that cycles through the menu options using an index.
        /// </summary>
        /// <returns>returns selectedIndex int when, if user presses "enter" key.</returns>
        public int Run()
        {
            ConsoleKey keyPressed;
            do
            {
                ClearScreen();
                Console.WriteLine();
                DisplayOptions();
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                keyPressed = keyInfo.Key;
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex == -1)
                    {
                        selectedIndex = options.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex == options.Length)
                    {
                        selectedIndex = 0;
                    }
                }

            } while (keyPressed != ConsoleKey.Enter);
            return selectedIndex;
        }
    }

}
