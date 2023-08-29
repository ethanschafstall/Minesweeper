/*
 * t-P_Prog-Schafstall-Ethan-Demineur
 * Ethan Schafstall
 * 09.03.2023
 * CIN1B
 * ETML
 */

using System;
using System.Text.RegularExpressions;
using WMPLib;

namespace t_P_Prog_Schafstall_Ethan_Demineur
{
    internal class Program
    {
        // Global GameParameters object, for holding game settings values.
        static GameParameters myGameSettings = new GameParameters();
        // enum of the difficulty levels associated value which determines the percentage of cells on the game board will contain a mine.
        enum GameDifficultyLevel
        {
            hard = 40,
            medium = 25,
            easy = 5,
        }
        // enum of the different languages in the game/program will be displayed in.
        enum Languages
        {
            english = 0,
            french = 1,
            spanish = 2,
        }
        // enum of the different states in which a game cell can be in.
        enum CellStates
        {
            coveredEmpty = 0,
            coveredNumber = 1,
            coveredMine = 2,
            flaggedEmpty = 3,
            flaggedNumber = 4,
            flaggedMine = 5,
            exposedEmpty = 6,
            exposedNumber = 7,
            exposedMine = 8,
        }

        /// <summary>
        /// Main method, pieces everything together.
        /// </summary>
        /// <param name="args">I don't know what this is.</param>
        static void Main(string[] args)
        {
            // current menu Index
            int currentMenu;
            //bool if menu should be running
            bool runMenu;
            // bool if game should be running
            bool runGame;
            // declare menu object array for different menu screens
            Menu[] myMenus = new Menu[4];

            do
            {
                // true so that menu runs, 0 = starting index.
                runMenu = true;
                runGame = true;
                currentMenu = 0;

                Console.Clear();
                Console.CursorVisible = false;

                // console size for menu phase
                Console.SetWindowSize(120, 30);
                Console.SetBufferSize(120, 30);

                GetLangage();
                GameSettings();
                CreateMenus(ref myMenus);
                SetTitle();
                do
                {
                    myMenus[currentMenu].DisplayOptions();
                    int currentOption = myMenus[currentMenu].Run();
                    MenuNav(currentOption, ref currentMenu, ref runMenu, ref myMenus);
                } while (runMenu);

                Cell[,] myGameBoardArray = new Cell[myGameSettings.GameHeight, myGameSettings.GameWidth];

                // calculate the amount of mines the game should have based on difficulty and gamesize. Saves that value into myGameSettings object.
                myGameSettings.MineAmount = ((myGameBoardArray.GetLength(0) * myGameBoardArray.GetLength(1)) * myGameSettings.Difficulty / 100);

                // Minimum console height 18.
                // Calculate the correct height and width for the window so that all text is properly displayed and there aren't any buffer issues.
                int consoleHeight = 18;
                if (!(myGameSettings.GameHeight * myGameSettings.CellHeight + myGameSettings.PositionFromTop < 18))
                {
                    consoleHeight = (myGameSettings.GameHeight * myGameSettings.CellHeight) + myGameSettings.PositionFromTop;
                }
                int consoleWidth = (myGameSettings.GameWidth * myGameSettings.CellWidth) + (myGameSettings.PositionFromLeft * 2) +
                    GetGameText(4)[5].Length;/*length of longest side game instruction sentence*/

                // Set window height and width based on consoleWidth consoleHeight int values.
                Console.SetWindowSize(consoleWidth + 2, consoleHeight + 2);
                Console.SetBufferSize(consoleWidth+2, consoleHeight+2);
                // create the visual gameboard that the user sees.
                CreateVisualGameBoard();

                // create the abstract gameboard that contains all the data.
                CreateAbstractGameBoard(ref myGameBoardArray);

                // game that runs a bool when game is done running.
                bool gameState = GameNav(ref myGameBoardArray);

                Console.CursorVisible = false;

                // Clear side text so that the gameover/restart text can be easily read.
                var positions = GetPositions(2);
                
                Thread.Sleep(1000);
                // for to clear game side text.
                for (int i = 0; i < 20 ; i++)
                {
                    Console.SetCursorPosition(positions.horPos, i);
                    Console.Write(new String(' ', GetGameText(4)[5].Length));
                }
                // Display gameover text, text depends on the gameState value.
                SetGameOverText(gameState);
                Thread.Sleep(1000);
                // Asks user if they would like to restart game.
                GetGameReset(ref runGame);
                Thread.Sleep(1000);
            } while (runGame);
            Environment.Exit(0);
        }

        /// <summary>
        /// Method that prints the title.
        /// </summary>
        static void SetTitle()
        {
            // ints for cursor pos for printing title
            int leftCurPos = 0;
            int topCurPos = 1;

            // change y axis cursor post based on language.
            switch (myGameSettings.Language)
            {
                case 0:
                    leftCurPos = 5;
                    break;
                case 1:
                    leftCurPos = 20;
                    break;
                case 2:
                    leftCurPos = 10;
                    break;

            }
            // for to print string array containing title.
            for (int i = 1; i < 12; i++)
            {
                Console.SetCursorPosition(leftCurPos, topCurPos);
                Console.Write(GetGameText(3)[i - 1]);
                topCurPos++;
            }

        }

        /// <summary>
        /// Method for game sounds. Didn't get to finish and add all sounds I would've liked.
        /// </summary>
        /// <param name="whichSound">string for which sound is suppose to play.</param>
        static void PlaySounds(string whichSound) 
        {
            WindowsMediaPlayer explosionSound = new WindowsMediaPlayer();
            WindowsMediaPlayer placeFlagSound = new WindowsMediaPlayer();
            WindowsMediaPlayer removeFlagSound = new WindowsMediaPlayer();
            WindowsMediaPlayer menuClickSound = new WindowsMediaPlayer();
            WindowsMediaPlayer checkCellSound = new WindowsMediaPlayer();
            WindowsMediaPlayer clearedZoneSound = new WindowsMediaPlayer();
            WindowsMediaPlayer menuSelectSound = new WindowsMediaPlayer();

            explosionSound.URL = "E://PROJETS - 777/Projets/2022-2023/3rd Trimester/P_PROG_DLS/t-P_Prog-Schafstall-Ethan-Demineur/audio/explosion5.wav";
            placeFlagSound.URL = "";
            removeFlagSound.URL = "";
            menuClickSound.URL = "";
            checkCellSound.URL = "";
            clearedZoneSound.URL = "";
            menuSelectSound.URL = "";

            // switch for whichsound string, plays sounds.
            switch (whichSound)
            {
                case "placeflag":
                    placeFlagSound.controls.play();
                    break;
                case "removeflag":
                    removeFlagSound.controls.play();
                    break;
                case "explosion":
                    explosionSound.controls.play();
                    break;
                case "menuselect":
                    menuSelectSound.controls.play();
                    break;
                case "menuclick":
                    menuClickSound.controls.play();
                    break;
                case "checkcell":
                    checkCellSound.controls.play();
                    break;
                case "clearedzone":
                    clearedZoneSound.controls.play();
                    break;

            }
        }
        /// <summary>
        /// Method that asks user if they would like to restart game.
        /// </summary>
        /// <param name="runGame">ref bool to run game</param>
        static void GetGameReset(ref bool runGame) 
        {
            var sideTextpositions = GetPositions(2);
            
            bool isYes;
            bool isNo;

            // display game restart text.
            for (int i = 0; i < GetGameText(8).Length; i++)
            {
                Console.SetCursorPosition(sideTextpositions.horPos, 6 + i);
                Console.Write(GetGameText(8)[i]);
            }

            ConsoleKeyInfo enteredChar;

            // Do to get key info from user, while user hasn't pressed a correct key.
            do
            {
                enteredChar = Console.ReadKey(true);
                isNo = (enteredChar.Key == ConsoleKey.N);
                
                // switch case changing the "yes" to restart based on language.
                switch (myGameSettings.Language)
                {
                    default:
                        isYes = (enteredChar.Key == ConsoleKey.Y);
                        break;
                    case 1:
                        isYes = (enteredChar.Key == ConsoleKey.O);
                        break;
                    case 2:
                        isYes = (enteredChar.Key == ConsoleKey.S);
                        break;
                }

            } while (!(isYes || isNo));

            // ifs for chosen key, returns rungame bool.
            if (isYes) { runGame = true; };
            if (isNo) { runGame = false; };

        }

        /// <summary>
        /// Text that is displayed after the game is over.
        /// </summary>
        /// <param name="gameWon"></param>
        static void SetGameOverText(bool gameWon)
        {
            // cursor positions.
            var sideTextpositions = GetPositions(2);

            //string array for gameover text.
            string[] gameOverText = new string[] {""};

            // two if with gameWon bool condition, changes to correct color and gets correct text.
            if (gameWon) { Console.ForegroundColor = ConsoleColor.Green; gameOverText = GetGameText(7); };
            if (!gameWon) { Console.ForegroundColor = ConsoleColor.Red; gameOverText = GetGameText(6); };
  
            // set cursor position and print gameover text from string using for.
            Console.SetCursorPosition(sideTextpositions.horPos, myGameSettings.PositionFromTop);
            for (int i = 0; i < GetGameText(7).Length; i++)
            {
                Console.SetCursorPosition(sideTextpositions.horPos, myGameSettings.PositionFromTop + i);
                Console.Write(gameOverText[i]);
            }
            Console.ResetColor();
        }

        static void PrintCredits() 
        {
            int xpos = 40;
            int ypos = 21;
            string[] creditsText = GetGameText(9);
            Console.SetCursorPosition(xpos, ypos);
            for (int i = 0; i < creditsText.Length; i++)
            {
                Console.SetCursorPosition(xpos, ypos);
                Console.WriteLine(creditsText[i]);
                ypos++;
            }
        }
        static void RemoveCredits() 
        {
            int verticalPos = 21;
            for (int i = 0; i < 8; i++)
            {
                Console.SetCursorPosition(40, verticalPos);
                Console.Write(new String(' ', 100));
                verticalPos++;
            }
        }
        /// <summary>
        /// Navigation throughout the menu screens. Using switch cases for current menu and selectedindex (chosen menu option).
        /// </summary>
        /// <param name="currentOption">Current selected menu option.</param>
        /// <param name="currentMenu">Current menu screen.</param>
        /// <param name="runMenu">Bool if the menu should be running or not, E.G. false = do not run menu</param>
        static void MenuNav(int currentOption, ref int currentMenu, ref bool runMenu, ref Menu[] myMenus)
        {
            bool exitedCustomSize = false;
            switch (currentMenu)
            {
                // Main screen.   
                case 0:
                    switch (currentOption)
                    {
                        case 0:
                            currentMenu = 2;
                            break;
                        case 1:
                            PrintCredits();
                            currentMenu = 1;
                            break;
                        case 2:
                            GetLangage();
                            CreateMenus(ref myMenus);
                            break;
                        case 3:
                            Environment.Exit(0);
                            break;
                    }
                    break;
                // Credit screen.  
                case 1:
                    switch (currentOption)
                    {
                        default:
                            RemoveCredits();
                            currentMenu = 0;
                            break;
                    }
                    break;
                // Gameboard Size screen. 
                case 2:
                    switch (currentOption)
                    {
                        case 0:
                            currentMenu = 0;
                            break;
                        case 1:
                            myGameSettings.GameHeight = 8;
                            myGameSettings.GameWidth = 8;
                            currentMenu = 3;
                            break;
                        case 2:
                            myGameSettings.GameHeight = 12;
                            myGameSettings.GameWidth = 12;
                            currentMenu = 3;
                            break;
                        case 3:
                            myGameSettings.GameHeight = 18;
                            myGameSettings.GameWidth = 18;
                            currentMenu = 3;
                            break;
                        case 4:
                            GetGameboardSize();
                            currentMenu = 3;
                            break;

                    }
                    break;
                // Game Difficulty screen.
                case 3:
                    switch (currentOption)
                    {
                        case 0:
                            currentMenu = 2;
                            break;
                        case 1:
                            myGameSettings.Difficulty = (int)GameDifficultyLevel.easy;
                            runMenu = false;
                            break;
                        case 2:
                            myGameSettings.Difficulty = (int)GameDifficultyLevel.medium;
                            runMenu = false;
                            break;
                        case 3:
                            myGameSettings.Difficulty = (int)GameDifficultyLevel.hard;
                            runMenu = false;
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Method that gets the language based on which menu option the user selects.
        /// </summary>
        static void GetLangage()
        {
            // menu object with language options.
            Menu language = new Menu("Select Language", new string[] { "English", "Français", "Español" });
           
            // run language menu. get selected index.
            language.DisplayOptions();
            int selectedIndex = language.Run();
            
            // switch to update gamesettings objet with language based on selectedindex.
            switch (selectedIndex)
            {
                case 0:
                    myGameSettings.Language = (int)Languages.english;
                    break;
                case 1:
                    myGameSettings.Language = (int)Languages.french;
                    break;
                case 2:
                    myGameSettings.Language = (int)Languages.spanish;
                    break;
            }
        }

        /// <summary>
        /// Method that sends a string array of game Text, based on the language, and reason, E.G. where the text is needed.
        /// </summary>
        /// <param name="whichText">Which text is needed</param>
        /// <returns>Returns a string of text dependant on language and where the text is needed.</returns>
        static string[] GetGameText(int whichText)
        {
            // english text string arrays.
            string[] englishMenuText = { "Start", "Credits", "Language", "Exit",
                "Credits", "Back",
                "Select game size", "Back", "8x8", "12x12", "18x18", "Custom size (min: 6 max: 30)",
                "Select difficulty level","Back", "Beginner", "Intermediate", "Expert",};

            string[] englishFakeMenuText = new string[] { "Select game size", "" ,"  << Back >>", "  << 8x8 >>",
                "  << 12x12 >>", "  << 18x18 >>", "  << Custom size (min: 6 max: 30) >>", "*    << Width :    >>", "  << Width : ",
                " >>", "*    << Height :    >>"};
            
            string[] englishInstuctionsControlsText = new string[] { "Game Instructions :",
                "",
                " - The board is divided into cells, with mines randomly distributed.", 
                " - To win, you need to open all the cells. ",
                " - The number on a cell shows the number of mines adjacent to it.", 
                " - Using this information, you can determine which cells are safe, and which contain mines.", 
                " - Cells suspected of being mines can be marked with a flag.", 
                " - If a cell containing a mine is opened, gameover.",
                "",
                "Controls :",
                "",
                " - Cursor movement using the up/down/left/right arrow keys.",
                " - Open a cell using the enter key.",
                " - Flag a cell using the spacebar key.",
                " - Close game using the escape key."};
            string[] englishGameInfoText = new string[] { "Mines(", "*", ") : ",
            "Flags(", "|", ">", ") : " };

            string[] englishGameOverText1 = new string[] { "╔═════════════════════╗", 
                                                           "║ GameOver! You Lost! ║",
                                                           "╚═════════════════════╝"};
           
            string[] englishGameOverText2 = new string[] { "╔═══════════════════════════╗",
                                                           "║ Congratulations! You Won! ║",
                                                           "╚═══════════════════════════╝"};


            string[] englishRestartText = new string[]   { "╔══════════════════════════════════╗",
                                                           "║ Would you like to play game? y/n ║",
                                                           "╚══════════════════════════════════╝"};

            string[] englishTitle = new string[] { " ██████   ██████  ███                                                                                            ",
                                                   "░░██████ ██████  ░░░                                                                                             ",
                                                   " ░███░█████░███  ████  ████████    ██████   █████  █████ ███ █████  ██████   ██████  ████████   ██████  ████████ ",
                                                   " ░███░░███ ░███ ░░███ ░░███░░███  ███░░███ ███░░  ░░███ ░███░░███  ███░░███ ███░░███░░███░░███ ███░░███░░███░░███",
                                                   " ░███ ░░░  ░███  ░███  ░███ ░███ ░███████ ░░█████  ░███ ░███ ░███ ░███████ ░███████  ░███ ░███░███████  ░███ ░░░ ",
                                                   " ░███      ░███  ░███  ░███ ░███ ░███░░░   ░░░░███ ░░███████████  ░███░░░  ░███░░░   ░███ ░███░███░░░   ░███     ",
                                                   " █████     █████ █████ ████ █████░░██████  ██████   ░░████░████   ░░██████ ░░██████  ░███████ ░░██████  █████    ",
                                                   "░░░░░     ░░░░░ ░░░░░ ░░░░ ░░░░░  ░░░░░░  ░░░░░░     ░░░░ ░░░░     ░░░░░░   ░░░░░░   ░███░░░   ░░░░░░  ░░░░░     ",
                                                   "                                                                                     ░███                        ",
                                                   "                                                                                     █████                       ",
                                                   "                                                                                    ░░░░░   "};
            string[] englishCredits = {"-----------------------------------------",
                                       "            Program Credits              ",
                                       "-----------------------------------------",
                                                  "Developed by: Ethan Aymeric Schafstall",
                                                  "Date: 5.12.22 - 31.12.22",
                                                  "Class: CIN2B",
                                                  "GitHub: github.com/ethanschafstall",
                                        "-----------------------------------------"
};
            // french text string arrays.

            string[] frenchMenuText = { "Démarrer", "Générique","Langue", "Quitter",
                "Génériques", "Retour",
                "Choisi la taille de votre plateau de jeux", "Retour", "8x8", "12x12", "18x18", "Taille personnalisé (min: 6 max: 30)",
                "Choisi le mode de difficulté","Retour", "Débutant", "Moyen", "Difficile"};

            string[] frenchFakeMenuText = new string[] { "Choisi la taille de votre plateau de jeux", "", "  << Retour >>", "  << 8x8 >>",
                "  << 12x12 >>", "  << 18x18 >>", "  << Taille personnalisé (min: 6 max: 30) >>", "*    << Largeur :    >>", "  << Largeur : ", " >>",
                "*    << Hauteur :    >>", };

            string[] frenchInstuctionsControlsText = new string[] { "Consignes de jeu :",
                "",
                " - Le plateau est divisé en cellules, avec des mines distribuées au hasard.",
                " - Pour gagner, vous devez ouvrir toutes les cellules.",
                " - Le nombre sur une cellule indique le nombre de mines qui lui sont adjacentes.",
                " - Grâce à ces infos, déterminez quelles cellules sont sûres et lesquelles contiennent des mines.",
                " - Les cellules suspectées d'être des mines peuvent être signalées par un drapeau.",
                " - Si une cellule contenant une mine est ouverte, gameover.",
                "",
                "Contrôles :",
                "",
                " - Déplacement du curseur à l'aide des touches fléchées haut/bas/gauche/droite.",
                " - Ouvrir une cellule avec la touche entrée.",
                " - Marque una celda con una badera con la tecla de la barra espaciadora.",
                " - Fermez le jeu en utilisant la touche d'échappement."};
            string[] frenchGameInfoText = new string[] { "Mines(", "*", ") : ",
            "Drapeaux(", "|", ">", ") : " };

            string[] frenchGameOverText1 = new string[] {  "╔═══════════════════════════╗",
                                                           "║ Jeu terminé! Tu as perdu! ║",
                                                           "╚═══════════════════════════╝"};

            string[] frenchGameOverText2 = new string[] {  "╔════════════════════════════╗",
                                                           "║ Félicitations! T'as gagné! ║",
                                                           "╚════════════════════════════╝"};

            string[] frenchRestartText = new string[]   { "╔════════════════════════════╗",
                                                          "║  Voulez-vous rejouer? o/n  ║",
                                                          "╚════════════════════════════╝"};

            string[] frenchTitle = new string[] { "                                                                                     ",
                                                  " ██████████                             ███                                          ",
                                                  "░░██░░░░███                           ░░░                                           ",
                                                  " ░███   ░░███  ██████  █████████████   ████  ████████    ██████  █████ ████ ████████ ",
                                                  " ░███    ░███ ███░░███░░███░░███░░███ ░░███ ░░███░░███  ███░░███░░███ ░███ ░░███░░███",
                                                  " ░███    ░███░███████  ░███ ░███ ░███  ░███  ░███ ░███ ░███████  ░███ ░███  ░███ ░░░ ",
                                                  " ░███    ███ ░███░░░   ░███ ░███ ░███  ░███  ░███ ░███ ░███░░░   ░███ ░███  ░███     ",
                                                  " ██████████  ░░██████  █████░███ █████ █████ ████ █████░░██████  ░░████████ █████    ",
                                                  "░░░░░░░░░░    ░░░░░░  ░░░░░ ░░░ ░░░░░ ░░░░░ ░░░░ ░░░░░  ░░░░░░    ░░░░░░░░ ░░░░░  ",
                                                  "                                                                                  ",
                                                  "                                                                                  "};
            string[] frenchCredits = {"-----------------------------------------",
                          "           Crédits du Programme          ",
                          "-----------------------------------------",
                          "Développé par : Ethan Aymeric Schafstall",
                          "Date : 5.12.22 - 31.12.22",
                          "Classe : CIN2B",
                          "GitHub : github.com/ethanschafstall",
                          "-----------------------------------------"};
            // spanish text string arrays.

            string[] spanishMenuText = { "Comenzar", "Créditos", "idioma", "Salir",
                "Créditos", "Regresar",
                "Seleccione el tamaño de la tabla de juego", "Regresar", "8x8", "12x12", "18x18", "Tamaño personalizado (mín: 6 máx: 30)",
                "Seleccione el nivel de dificultad","Regresar", "Principiante", "Intermedio", "Experto"};

            string[] spanishFakeMenuText = new string[] { "Seleccione el tamaño de la tabla de juego", "", "  << Regresar >>", "  << 8x8 >>",
                "  << 12x12 >>", "  << 18x18 >>", "  << Tamaño personalizado (mín: 6 máx: 30) >>", "*    << Ancho :    >>", "  << Ancho : ",
                " >>", "*    << Altura :    >>", };

            string[] spanishInstuctionsControlsText = new string[] { "Instrucciones del juego :",
                "",
                " - El tablero está dividido en celdas, con minas distribuidas al azar.",
                " - Para ganar, debes abrir todas las celdas.",
                " - El número en una celda muestra el número de minas adyacentes.",
                " - Con esta información, puede determinar qué celdas son seguras y cuáles contienen minas.",
                " - Las celdas sospechosas de ser minas se pueden marcar con una bandera.",
                " - Si se abre una celda que contiene una mina, finaliza el juego.",
                "",
                "Controles :",
                "",
                " - Movimiento del cursor utilizando las teclas de flecha arriba/abajo/izquierda/derecha.",
                " - Abra una celda con la tecla Intro.",
                " - Marque una celda con la tecla de la barra espaciadora.",
                " - Cierra el juego usando la tecla de escape."};
            string[] spanishGameInfoText = new string[] { "Minas(", "*", ") : ",
            "Banderas(", "|", ">", ") : " };

            string[] spanishGameOverText1 = new string[] { "╔══════════════════════════════╗",
                                                           "║ ¡Juego terminado! ¡Perdiste! ║",
                                                           "╚══════════════════════════════╝"};

            string[] spanishGameOverText2 = new string[] { "╔═════════════════════════╗",
                                                           "║ ¡Felicidades! ¡Ganaste! ║",
                                                           "╚═════════════════════════╝"};

            string[] spanishRestartText = new string[]   { "╔══════════════════════════════════╗",
                                                           "║ ¿Te gustaría volver a jugar? s/n ║",
                                                           "╚══════════════════════════════════╝"};

            string[] spanishTitle = new string[] { "                                                                                     ",
                                                  " ███████████                                                         ███                              ",
                                                  "░░███░░░░░███                                                       ░░░                               ",
                                                  " ░███    ░███ █████ ████  █████   ██████   ██████   █████████████   ████  ████████    ██████    █████ ",
                                                  " ░██████████ ░░███ ░███  ███░░   ███░░███ ░░░░░███ ░░███░░███░░███ ░░███ ░░███░░███  ░░░░░███  ███░░  ",
                                                  " ░███░░░░░███ ░███ ░███ ░░█████ ░███ ░░░   ███████  ░███ ░███ ░███  ░███  ░███ ░███   ███████ ░░█████ ",
                                                  " ░███    ░███ ░███ ░███  ░░░░███░███  ███ ███░░███  ░███ ░███ ░███  ░███  ░███ ░███  ███░░███  ░░░░███",
                                                  " ███████████  ░░████████ ██████ ░░██████ ░░████████ █████░███ █████ █████ ████ █████░░████████ ██████ ",
                                                  "░░░░░░░░░░░    ░░░░░░░░ ░░░░░░   ░░░░░░   ░░░░░░░░ ░░░░░ ░░░ ░░░░░ ░░░░░ ░░░░ ░░░░░  ░░░░░░░░ ░░░░░░  ",
                                                  "                                                                                  ",
                                                  "                                                                                  "};
            string[] spanishCredits = {"-----------------------------------------",
                           "          Créditos del Programa          ",
                           "-----------------------------------------",
                           "Desarrollado por: Ethan Aymeric Schafstall",
                           "Fecha: 5.12.22 - 31.12.22",
                           "Clase: CIN2B",
                           "GitHub: github.com/ethanschafstall",
                           "-----------------------------------------"};

            // double switch cases. First sorting by language, 2nd sorting by whichText int parameter.
            switch (myGameSettings.Language)
            {
                case 0:
                    switch (whichText)
                    {
                        case 0:
                            return englishMenuText;
                            break;
                        case 1:
                            return englishFakeMenuText;
                            break;
                        case 3:
                            return englishTitle;
                            break;
                        case 4:
                            return englishInstuctionsControlsText;
                            break;
                        case 5:
                            return englishGameInfoText;
                            break;
                        case 6:
                            return englishGameOverText1;
                            break;
                        case 7:
                            return englishGameOverText2;
                            break;
                        case 8:
                            return englishRestartText;
                            break;
                        case 9:
                            return englishCredits;
                            break;
                    }
                    break;
                case 1:
                    switch (whichText)
                    {
                        case 0:
                            return frenchMenuText;
                            break;
                        case 1:
                            return frenchFakeMenuText;
                            break;
                        case 3:
                            return frenchTitle;
                            break;
                        case 4:
                            return frenchInstuctionsControlsText;
                            break;
                        case 5:
                            return frenchGameInfoText;
                            break;
                        case 6:
                            return frenchGameOverText1;
                            break;
                        case 7:
                            return frenchGameOverText2;
                            break;
                        case 8:
                            return frenchRestartText;
                            break;
                        case 9:
                            return frenchCredits;
                            break;
                    }
                    break;
                case 2:
                    switch (whichText)
                    {
                        case 0:
                            return spanishMenuText;
                            break;
                        case 1:
                            return spanishFakeMenuText;
                            break;
                        case 3:
                            return spanishTitle;
                            break;
                        case 4:
                            return spanishInstuctionsControlsText;
                            break;
                        case 5:
                            return spanishGameInfoText;
                            break;
                        case 6:
                            return spanishGameOverText1;
                            break;
                        case 7:
                            return spanishGameOverText2;
                            break;
                        case 8:
                            return spanishRestartText;
                            break;
                        case 9:
                            return spanishCredits;
                            break;
                    }
                    break;
            }
            return englishMenuText;
        }

        /// <summary>
        /// Method that fills an already created 2D Cell object array with said objects, as well as asigning to each a unique visual gameboard position and a value of "coveredEmpty".
        /// </summary>
        /// <param name="myGameBoardArray">gameboard array.</param>
        static void CreateAbstractGameBoard(ref Cell[,] myGameBoardArray)
        {
            // tuple cursor positioning for the center of the cell (game starting position).
            var positions = GetPositions(1);
            int myXPos = positions.verPos;
            int myYPos = positions.horPos;

            for (int i = 0; i < myGameBoardArray.GetLength(0); i++)
            {
                for (int j = 0; j < myGameBoardArray.GetLength(1); j++)
                {
                    // Create cell object at i,j in array. Give it a unique visual gameboard position. Give it an empty state value, print said value.
                    myGameBoardArray[i, j] = new Cell();
                    myGameBoardArray[i, j].XPosition = myXPos;
                    myGameBoardArray[i, j].YPosition = myYPos;
                    myGameBoardArray[i, j].CellState = (int)CellStates.coveredEmpty;
                    myGameBoardArray[i, j].PrintState();
                    // increase y axis position.
                    myYPos += myGameSettings.CellWidth;
                }
                // reset y axis position. increase x axis position.
                myXPos += myGameSettings.CellHeight;
                myYPos = positions.horPos;
            }
        }

        /// <summary>
        /// Method that asigns prompts and options to each menu in a array. Despite the name the menus are already "created", but without value 
        /// properties, E.G. useless menus without prompts or options, this method fixes that.
        /// </summary>
        /// <param name="myMenus">Ref menu object array.</param>
        /// <param name="myMenuText">String array containing all the prompts and options needed for each menu.</param>
        static void CreateMenus(ref Menu[] myMenus)
        {
            // create the 4 menus used.
            Menu main = new Menu();
            Menu credits = new Menu();
            Menu gameSize = new Menu();
            Menu difficulty = new Menu();

            // fill ref menu array with menus.
            myMenus = new Menu[] { main, credits, gameSize, difficulty };

            // for loop using a switch and method returned string array to fill menu objets which option and prompt text.
            for (int i = 0; i < myMenus.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        myMenus[i].options = new string[] { GetGameText(0)[0], GetGameText(0)[1], GetGameText(0)[2], GetGameText(0)[3] };
                        break;
                    case 1:
                        myMenus[i].prompt = GetGameText(0)[3];
                        myMenus[i].options = new string[] { GetGameText(0)[4] };
                        break;
                    case 2:
                        myMenus[i].prompt = GetGameText(0)[5];
                        myMenus[i].options = new string[] { GetGameText(0)[6], GetGameText(0)[7], GetGameText(0)[8], GetGameText(0)[9], GetGameText(0)[10] };
                        break;
                    case 3:
                        myMenus[i].prompt = GetGameText(0)[11];
                        myMenus[i].options = new string[] { GetGameText(0)[12], GetGameText(0)[13], GetGameText(0)[14], GetGameText(0)[15] };
                        break;
                }
            }
        }

        /// <summary>
        /// Method that gets the amount of the cells the gameboard will contain vertically and horizontally.
        /// This method contains alot of what might seem like unnecessary code for what the method is suppose to do. 
        /// This is because creates a fake menu while asking for the gameboard size.
        /// This gives the illusion that the menu is dynamic, when it isn't.
        /// </summary>
        static void GetGameboardSize()
        {
            // regex of numbers, 6-30.
            Regex myRegex = new Regex(@"^([6-9]|[12][0-9]|3[0])$");
            
            // string inputs to store user data.
            string input1 = ""; string input2 = "";
            
            // ints for cursor y axis positioning.
            int cursorleft1 = 0; int cursorleft2 = 0;

            // ints for user chosen gameboard dimensions.
            int gameWidth = 0; int gameHeight = 0;

            // Adjust the cursor y axis position based on language, so that things are properly aligned.
            switch (myGameSettings.Language)
            {
                default:
                    cursorleft1 = 66;
                    cursorleft2 = 67;
                    break;
                case 1:
                    cursorleft1 = 68;
                    cursorleft2 = 68;
                    break;
            }
            // clear part of the screen, where the static menu was.
            for (int i = 12; i < 20; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new String(' ', 120));
            }
            // display "fake" menu.
            for (int i = 0; i < 8; i++)
            {
                Console.SetCursorPosition(50, i + 12);
                Console.WriteLine(GetGameText(1)[i]);
            }

            // get game width.
            do
            {
                Console.ResetColor();
                Console.SetCursorPosition(0, 19);
                Console.Write(new String(' ', 120));
                Console.SetCursorPosition(0, 19);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                Console.SetCursorPosition(50, 19);
                Console.Write(GetGameText(1)[7]);
                Console.SetCursorPosition(cursorleft1, 19);
                input1 = Console.ReadLine();
                int.TryParse(input1, out gameWidth);
            } while (!myRegex.IsMatch(input1));

            // update fake menu.
            Console.SetCursorPosition(0, 19);
            Console.ResetColor();
            Console.Write(new String(' ', 120));
            Console.SetCursorPosition(cursorleft1 - GetGameText(1)[8].Length, 19);
            Console.Write("{0}{1}{2}", GetGameText(1)[8], input1, GetGameText(1)[9]);

            // get game height.
            do
            {
                Console.ResetColor();
                Console.SetCursorPosition(0, 20);
                Console.Write(new String(' ', 120));
                Console.SetCursorPosition(50, 20);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(GetGameText(1)[10]);
                Console.SetCursorPosition(cursorleft2, 20);
                input2 = Console.ReadLine();
                int.TryParse(input2, out gameHeight);
            } while (!myRegex.IsMatch(input2));

            // save width and height to gamesettings object.
            myGameSettings.GameHeight = gameWidth;
            myGameSettings.GameWidth = gameHeight;

            Console.ResetColor();
        }

        /// <summary>
        ///  Method that creates a visual game array, by using the dimensions given by the user, or one of the  proposed default size options.
        /// </summary>
        static void CreateVisualGameBoard()
        {
            // tuple return for gameboard generation starting position.
            var positions = GetPositions(0);
            int xAxis = positions.verPos;
            int yAxis = positions.horPos;

            Console.Clear();

            // set correct cursor position.
            Console.SetCursorPosition(yAxis, xAxis);

            
            // i index for which loops through rows, j index for which loops for columns.
            for (int i = 0; i != (myGameSettings.GameHeight * myGameSettings.CellHeight) + 1; i++)
            {
                // after generating a line, return to starting y axis position.
                Console.CursorLeft = yAxis;

                for (int j = 0; j != (myGameSettings.GameWidth * myGameSettings.CellWidth) + 1; j++)
                {
                    // list of conditions comparing indexes with values for determining correct char placement, and while making sure bonds are respected.
                    if (i == 0 && j == 0)
                    {
                        Console.Write('╔');
                    }
                    else if (j == myGameSettings.GameWidth * myGameSettings.CellWidth && i == 0)
                    {
                        Console.Write('╗');
                    }
                    else if (j == 0 && i == myGameSettings.GameHeight * myGameSettings.CellHeight)
                    {
                        Console.Write('╚');
                    }
                    else if (j == myGameSettings.GameWidth * myGameSettings.CellWidth && i == myGameSettings.GameHeight * myGameSettings.CellHeight)
                    {
                        Console.Write('╝');
                    }
                    else if (j % myGameSettings.CellWidth == 0 && i == 0)
                    {
                        Console.Write('╦');
                    }
                    else if (j % myGameSettings.CellWidth == 0 && i == myGameSettings.GameHeight * myGameSettings.CellHeight)
                    {
                        Console.Write('╩');
                    }
                    else if (j == 0 && i % myGameSettings.CellHeight == 0)
                    {
                        Console.Write('╠');
                    }
                    else if (j == myGameSettings.GameWidth * myGameSettings.CellWidth && i % myGameSettings.CellHeight == 0)
                    {
                        Console.Write('╣');
                    }
                    else if ((j % myGameSettings.CellWidth == 0) && !(i % myGameSettings.CellHeight == 0))
                    {
                        Console.Write('║');
                    }
                    else if (!(j % myGameSettings.CellWidth == 0) && (i % myGameSettings.CellHeight == 0))
                    {
                        Console.Write('═');
                    }
                    else if (i % myGameSettings.CellHeight == 0 && j % myGameSettings.CellWidth == 0)
                    {
                        Console.Write('╬');
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Method responsible for player movement and actions through the visual gameboard.
        /// </summary>
        /// <param name="myGameBoardArray">Ref 2D Cell object array.</param>
        static bool GameNav(ref Cell[,] myGameBoardArray)
        {
            Console.CursorVisible = true;
            bool firstClick = true;
            bool gameWon = false;

            bool continueGame = true;

            // Amount of flags avalible to user, same amount as mines.
            int flagCount = myGameSettings.MineAmount;

            // Gets starting cursor position from tuple.
            var positions = GetPositions(1);
            int x = positions.verPos;
            int y = positions.horPos;



            // Current array position
            int xCellNum = 0;
            int yCellNum = 0;

            // set gametext.
            SetGameSideText(false);
            SetGameSideText(true, flagCount);

            // set cursor to starting position cell 0,0
            Console.SetCursorPosition(positions.horPos, positions.verPos);

            while (continueGame)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                // switch case checking which key is pressed.
                switch (keyInfo.Key)
                {

                    case ConsoleKey.Insert:
                        // Used for testing/visualization of the abstract game array.
                        //TestCellStates(ref myGameBoardArray); 
                        break;

                    case ConsoleKey.Enter:
                        // if user is checking a cell (clicking enter) for the first time
                        if (firstClick)
                        {
                            firstClick = false;

                            // add mine values to array
                            SetMineValues(ref myGameBoardArray, xCellNum, yCellNum);

                            // add number values to array (game related number values, depicting cells which are next to a mine cell)
                            FindAllNumberCells(ref myGameBoardArray);
                        }
                        
                        // if cell being check is a mine, change state, print value, expose all other coveredMines, stop game, lost.
                        if (myGameBoardArray[xCellNum, yCellNum].CellState == (int)CellStates.coveredMine)
                        {
                            myGameBoardArray[xCellNum, yCellNum].CheckCell();
                            myGameBoardArray[xCellNum, yCellNum].PrintState();
                            ExposeAllMineCells(ref myGameBoardArray);
                            continueGame = false;
                            gameWon = false;
                            break;
                        }

                        // clear/expose areas in which multiply emtpy value cells are bordering each other.
                        ClearEmptyCells(ref myGameBoardArray, xCellNum, yCellNum);
                        
                        // if to check if all cells (that aren't mines) have been checked, if true then user won the game.
                        if (CheckCells(ref myGameBoardArray))
                        {
                            continueGame = false;
                            gameWon = true;
                        }
                        break;

                    case ConsoleKey.Spacebar:
                        // bool true if cell already contains a flag.
                        bool alreadyFlagged = myGameBoardArray[xCellNum, yCellNum].CellState == (int)CellStates.flaggedEmpty ||
                                              myGameBoardArray[xCellNum, yCellNum].CellState == (int)CellStates.flaggedMine ||
                                              myGameBoardArray[xCellNum, yCellNum].CellState == (int)CellStates.flaggedNumber;

                        // if user hasn't generated the game values yet (via enter key) and the cell state isn't exposed.
                        if (!firstClick && myGameBoardArray[xCellNum, yCellNum].CellState < 6)
                        {
                            // if user has no flags available.
                            if (flagCount <= 0)
                            {
                                // if there is already a flag placed on the cell then +1 to flag counter.
                                if (alreadyFlagged)
                                {
                                    flagCount++;
                                }
                                myGameBoardArray[xCellNum, yCellNum].FlagCell(false);
                                myGameBoardArray[xCellNum, yCellNum].PrintState();
                            }
                            // if there are avalible flags
                            else
                            {
                                // if there is already a flag placed on the cell then +1 to flag counter.
                                if (alreadyFlagged)
                                {
                                    flagCount++;
                                }
                                // no current flag on cell, -1 from counter
                                else
                                {
                                    flagCount--;
                                }
                                myGameBoardArray[xCellNum, yCellNum].FlagCell(true);
                            }

                            // print cell state, update game info text.
                            myGameBoardArray[xCellNum, yCellNum].PrintState();
                            SetGameSideText(true, flagCount);
                        }
                        break;

                    // Moves cursor placement to within the first cell to the left.
                    case ConsoleKey.LeftArrow:
                        y -= myGameSettings.CellWidth;
                        yCellNum--;
                        break;

                    // Moves cursor placement to within the first cell above.
                    case ConsoleKey.UpArrow:
                        x -= myGameSettings.CellHeight;
                        xCellNum--;
                        break;

                    // Moves cursor placement to within the first cell to the right.
                    case ConsoleKey.RightArrow:
                        y += myGameSettings.CellWidth;
                        yCellNum++;
                        break;

                    // Moves cursor placement to within the first cell below.
                    case ConsoleKey.DownArrow:
                        x += myGameSettings.CellHeight;
                        xCellNum++;
                        break;
                }

                // If the cursor is in the bottom cell and trys to continue downwards, it resets vertical cursor placement to the top cell.
                if (x >= positions.verPos + (myGameSettings.CellHeight * myGameSettings.GameHeight))
                {
                    x = positions.verPos;
                    xCellNum = 0;
                }
                //// If the cursor is in the top cell and trys to continue upwards, it resets vertical cursor placement to the bottom cell.
                if (x < positions.verPos)
                {
                    x = positions.verPos + (myGameSettings.CellHeight * myGameSettings.GameHeight) - myGameSettings.CellHeight;
                    xCellNum = myGameBoardArray.GetLength(0) - 1;
                }
                ////// If the cursor is in the most left cell and trys to continue leftwards, it resets horizontal cursor placement to the most right cell.
                if (y >= positions.horPos + (myGameSettings.CellWidth * myGameSettings.GameWidth))
                {
                    y = positions.horPos;
                    yCellNum = 0;
                }
                //////If the cursor is in the most right cell and trys to continue rightwards, it resets horizontal cursor placement to the most left cell.
                if (y < positions.horPos)
                {
                    y = positions.horPos + (myGameSettings.CellWidth * myGameSettings.GameWidth) - myGameSettings.CellWidth;
                    yCellNum = myGameBoardArray.GetLength(1) - 1;
                }
                // update cursor position
                Console.SetCursorPosition(y, x);
            }
            return gameWon;
        }

        /// <summary>
        /// Method that displays the sidetext next to the gameboard during gameplay.
        /// </summary>
        /// <param name="updateText">Bool to determine if important game info needs to be updated.</param>
        /// <param name="remainingFlags">Int value which will be used for updated text.</param>
        static void SetGameSideText(bool updateText, int remainingFlags = 0) 
        {   
            // tuple positioning
            var sideTextpositions = GetPositions(2);
            var gameInfoTextpositions = GetPositions(3);
            
            // text doesn't need to be updated. So not game informational text.
            if (!updateText)
            {
                for (int i = sideTextpositions.verPos; i < GetGameText(4).Length+ sideTextpositions.verPos; i++)
                {
                    Console.SetCursorPosition(sideTextpositions.horPos, i);
                    Console.Write(GetGameText(4)[i- sideTextpositions.verPos]);
                }
            }
            else 
            {
                Console.SetCursorPosition(gameInfoTextpositions.horPos, gameInfoTextpositions.verPos);
                
                // for loop for the different stages of printing the game informational text. Text, color changes, chars, values.
                for (int i = 0; i < 7; i++)
                {
                    switch (i)
                    {
                        case 1:           
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(GetGameText(5)[i]);
                            Console.ResetColor();
                            continue;
                        case 2:
                            Console.Write(GetGameText(5)[i]);
                            Console.Write(myGameSettings.MineAmount);
                            Console.SetCursorPosition(gameInfoTextpositions.horPos+17, gameInfoTextpositions.verPos);
                            continue;
                        case 4:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(GetGameText(5)[i]);
                            Console.ResetColor();
                            continue;
                        case 5:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(GetGameText(5)[i]);
                            Console.ResetColor();
                            continue;

                    }
                    Console.Write(GetGameText(5)[i]);
                    if (i == 6)
                    {
                        Console.Write(remainingFlags);
                    }
                }
            }
        }

        /// <summary>
        /// Method that checks position and placement values to determine if the current location is valid for mine placement when user starts game
        /// and the gameboard values haven't been set.
        /// </summary>
        /// <param name="x">Int x, RNG vertical position value</param>
        /// <param name="y">Int y, RNG horizontal position value</param>
        /// <param name="currentXPos">Int currentXPos, current vertical position value</param>
        /// <param name="currentYPos">Int currentYPos, current horizontal position value</param>
        /// <returns>true if the location is valid, false if not valid</returns>
        static bool ValidMineLocation(int x, int y, int currentXPos, int currentYPos)
        {
            // if which compares randomly generated x y axis positions with the 24 surrounding current axis positions.
            if ((x == currentXPos && y == currentYPos) ||
                (x == currentXPos - 1 && y == currentYPos) ||
                (x == currentXPos - 1 && y == currentYPos - 1) ||
                (x == currentXPos - 1 && y == currentYPos + 1) ||
                (x == currentXPos && y == currentYPos - 1) ||
                (x == currentXPos + 1 && y == currentYPos) ||
                (x == currentXPos + 1 && y == currentYPos - 1) ||
                (x == currentXPos + 1 && y == currentYPos + 1) ||
                (x == currentXPos && y == currentYPos + 1) ||
                (x == currentXPos + 2 && y == currentYPos + 2) ||
                (x == currentXPos + 2 && y == currentYPos + 1) ||
                (x == currentXPos + 2 && y == currentYPos) ||
                (x == currentXPos && y == currentYPos + 2) ||
                (x == currentXPos + 1 && y == currentYPos + 2) ||
                (x == currentXPos - 2 && y == currentYPos) ||
                (x == currentXPos - 2 && y == currentYPos + 1) ||
                (x == currentXPos - 2 && y == currentYPos + 2) ||
                (x == currentXPos - 2 && y == currentYPos - 1) ||
                (x == currentXPos - 2 && y == currentYPos - 2) ||
                (x == currentXPos - 1 && y == currentYPos - 2) ||
                (x == currentXPos && y == currentYPos - 2) ||
                (x == currentXPos + 1 && y == currentYPos - 2) ||
                (x == currentXPos + 2 && y == currentYPos - 2) ||
                (x == currentXPos + 2 && y == currentYPos - 1) ||
                (x == currentXPos - 1 && y == currentYPos + 2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method that loops through each cell of the gameboard array checking for coveredEmpty or coverNumber cells.
        /// </summary>
        /// <param name="myGameBoardArray">Ref 2D Cell object array.</param>
        /// <returns>bool false if it finds any coveredEmpty or coverNumber values, meaning user hasn't exposed all cells. 
        /// true if it finds nothing, meaning all cells have been uncovered</returns>
        static bool CheckCells(ref Cell[,] myGameBoardArray)
        {
            // For loops that go through each cell of myGameBoardArray.
            for (int i = 0; i < myGameBoardArray.GetLength(0); i++)
            {
                for (int j = 0; j < myGameBoardArray.GetLength(1); j++)
                {
                    // if any non mine cells are still covered return false.
                    if (myGameBoardArray[i, j].CellState == (int)CellStates.coveredNumber || (myGameBoardArray[i, j].CellState == (int)CellStates.coveredEmpty))
                    {
                        return false;
                    }
                }
            }
            // return true if no non mine covered cells were found
            return true;
        }

        /// <summary>
        /// Method responsible for randomly setting mines throughout the gameboard array when the user presses "enter" for the first time.
        /// It doesn't place mines at the players location, or the 8 surrounding cells.
        /// </summary>
        /// <param name="myGameBoardArray">Ref 2D Cell object array.</param>
        /// <param name="currentXPos">Current X position of the game array</param>
        /// <param name="currentYPos">Current Y position of the game array</param>
        static void SetMineValues(ref Cell[,] myGameBoardArray, int currentXPos, int currentYPos)
        {
            // int counter to keep track of already placed mines.
            int placedMines = 0;
            // random for random mine placement.
            Random random = new Random();

            // ints for array x & y axis.
            int x = 0;
            int y = 0;

            // do while which generates a new random value from 0-array length, for axis ints.
            // If checks if the x and y axis positions fall into the user "starting zone", if true skip to next interation
            // if false then change state, increase counter.
            do
            {
                x = random.Next(0, myGameBoardArray.GetLength(0));
                y = random.Next(0, myGameBoardArray.GetLength(1));

                if (ValidMineLocation(x, y, currentXPos, currentYPos))
                {
                    continue;
                }
                else
                {
                    myGameBoardArray[x, y].CellState = (int)CellStates.coveredMine;
                    placedMines++;
                }
            } while (myGameSettings.MineAmount != placedMines);

        }

        /// <summary>
        /// Tuple that checks all cells surrounding cell n, looking for cells whose state value is a mine. Saves the amount of surrounding mines if any, and returns that value + an
        /// </summary>
        /// <param name="myGameBoardArray">Gameboard array.</param>
        /// <param name="x">cell's vertical position in the array.</param>
        /// <param name="y">cell's horizontal position in the array.</param>
        /// <returns>Returns a Tuple containing a bool which is true or false based on if the cell has any surrounding cells whose value is a mine, and an int containing said amount of mines, if any</returns>
        static (bool isNumCell, int adjacentMines) IsNumberCell(ref Cell[,] myGameBoardArray, int x, int y)
        {
            int adjacentMines = 0;
            bool isNumCell = false;

            // for each loops which check all cells surrounding n,n position.
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    // if checking if we are within our game array.
                    if (!(i == x && j == y) && (i >= 0 && j >= 0) && i <= myGameBoardArray.GetLength(0) - 1 && j <= myGameBoardArray.GetLength(1) - 1)
                    {
                        // if checking if the cell if cell i,j position is a mine, if true then bool + increase counter.
                        if (myGameBoardArray[i, j].CellState == (int)CellStates.coveredMine && !(myGameBoardArray[x, y].CellState == (int)CellStates.coveredMine))
                        {
                            adjacentMines++;
                            isNumCell = true;
                        }
                    }
                }
            }
            return (isNumCell, adjacentMines);

        }

        /// <summary>
        /// Method that finds all number cells in the game array and gives them their numeric value based on how many mine cells they border.
        /// </summary>
        /// <param name="myGameBoardArray">gameboard array.</param>
        static void FindAllNumberCells(ref Cell[,] myGameBoardArray)
        {
            // For loops that go through each cell of myGameBoardArray.
            for (int i = 0; i < myGameBoardArray.GetLength(0); i++)
            {
                for (int j = 0; j < myGameBoardArray.GetLength(1); j++)
                {
                    
                    // bool and int values returned from tuple based on if the current cell is considered a number cell.
                    var values = IsNumberCell(ref myGameBoardArray, i, j);

                    // if number cell = true, give the number, change cell state.
                    if (values.isNumCell)
                    {
                        myGameBoardArray[i, j].SetNumber = values.adjacentMines;
                        myGameBoardArray[i, j].CellState = (int)CellStates.coveredNumber;
                    }

                }


            }
        }

        /// <summary>
        /// Method responsible for exposing each cell which holds a mine.
        /// </summary>
        /// <param name="myGameBoardArray">gmaeboard array.</param>
        static void ExposeAllMineCells(ref Cell[,] myGameBoardArray)
        {
            PlaySounds("explosion");

            // For loops that go through each cell of myGameBoardArray

            for (int i = 0; i < myGameBoardArray.GetLength(0); i++)
            {
                for (int j = 0; j < myGameBoardArray.GetLength(1); j++)
                {
                    // if cell value is coveredMine, then change value to exposed.
                    if (myGameBoardArray[i, j].CellState == (int)CellStates.coveredMine)
                    {
                        Thread.Sleep(1);
                        myGameBoardArray[i, j].CheckCell();
                        myGameBoardArray[i, j].PrintState();
                    }

                }


            }
        }

        /// <summary>
        /// Method responsible for clearing out all empty cells which are interconnected(bordering) other empty cells.
        /// </summary>
        /// <param name="myGameBoardArray">gameboard array.</param>
        /// <param name="x">cell's vertical position in the gameboard array.</param>
        /// <param name="y">cell's horizontal position in the gameboard array.</param>
        static void ClearEmptyCells(ref Cell[,] myGameBoardArray, int x, int y)
        {
            // If x and y is not at the edge of the board.
            if (x < 0 || x >= myGameBoardArray.GetLength(0) || y < 0 || y >= myGameBoardArray.GetLength(1))
            {
                return;
            }

            int cellState = myGameBoardArray[x, y].CellState;
            // If the cell state of the cell is a mine.
            if (cellState == (int)CellStates.coveredMine || cellState == (int)CellStates.flaggedMine || cellState == (int)CellStates.exposedMine)
            {
                return;
            }
            myGameBoardArray[x, y].CheckCell();
            myGameBoardArray[x, y].PrintState();
            // If the cell state of the cell is a empty.
            if (cellState == (int)CellStates.coveredEmpty || cellState == (int)CellStates.flaggedEmpty)
            {
                for (int r = x - 1; r <= x + 1; r++)
                {
                    for (int c = y - 1; c <= y + 1; c++)
                    {
                        if (r == x && c == y)
                        {
                            // Skip the current cell.
                            continue;
                        }
                        // Recursive call to check neighboring cells.
                        ClearEmptyCells(ref myGameBoardArray, r, c);
                    }
                }
            }
        }

        /// <summary>
        /// Tuple responsible for returning cursor positioning.
        /// </summary>
        /// <param name="whichPosition">Int to determine which position is needed</param>
        /// <returns>Tuple of vertical and horizontal position</returns>
        static (int verPos, int horPos) GetPositions(int whichPosition)
        {
            // ints for positioning which is calculated based on case.
            int arrayVerPos; int arrayHorPos;

            switch (whichPosition)
            {

                // Initial Cursor position for the creation of the visual game array.
                case 0:
                    arrayHorPos = myGameSettings.PositionFromLeft;
                    arrayVerPos = myGameSettings.PositionFromTop;
                    return (arrayVerPos, arrayHorPos);
                    break;
                case 1:
                    // Cursor position for initial placement in the first (top right) cell on the visual gameboard.
                    arrayHorPos = myGameSettings.PositionFromLeft + (myGameSettings.CellWidth / 2);
                    arrayVerPos = myGameSettings.PositionFromTop + (myGameSettings.CellHeight / 2);
                    return (arrayVerPos, arrayHorPos);
                    break;
                case 2:
                    // Cursor position for placement of sideText displayed during gameplay.
                    arrayHorPos = (myGameSettings.PositionFromLeft * 2)+ (myGameSettings.CellWidth * myGameSettings.GameWidth);
                    arrayVerPos = myGameSettings.PositionFromTop;
                    return (arrayVerPos, arrayHorPos);
                    break;
                // Cursor position for placement of game information text during gameplay.
                case 3:
                    arrayHorPos = ((myGameSettings.CellWidth * myGameSettings.GameWidth) + myGameSettings.PositionFromLeft -26)/2;
                    arrayVerPos = myGameSettings.PositionFromTop-1;
                    return (arrayVerPos, arrayHorPos);
                    break;
            }
            return (0, 0);
        }

        /// <summary>
        /// // Method used to store game setting info not choosen by user in a myGameSetting object.
        /// </summary>
        static void GameSettings()
        {
            myGameSettings.PositionFromTop = 1;
            myGameSettings.PositionFromLeft = 4;
            myGameSettings.CellWidth = 4;
            myGameSettings.CellHeight = 2;
        }

        // METHOD FOR TESTING GAME ARRAY VALUES -------------------------------------->
        static void TestCellStates(ref Cell[,] myGameBoardArray)
        {
            for (int i = 0; i < myGameBoardArray.GetLength(0); i++)
            {
                Console.SetCursorPosition((myGameSettings.CellWidth * myGameSettings.GameHeight)+ (myGameSettings.PositionFromLeft * 2), 2 + i);
                for (int j = 0; j < myGameBoardArray.GetLength(1); j++)
                {
                    switch (myGameBoardArray[i, j].CellState)
                    {
                        case (int)CellStates.coveredMine:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case (int)CellStates.coveredNumber:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case (int)CellStates.exposedNumber:
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            break;
                        case (int)CellStates.flaggedMine:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                    }

                    Console.Write("{0} ", myGameBoardArray[i, j].CellState);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

        }
    }
}