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
    /// GameParameters class responible for holding/returning game setting info. Language, gameboard size, gamecell size, difficulty, etc.
    /// </summary>
    internal class GameParameters
    {
        private int gameWidth;
        private int gameHeight;
        private int cellWidth;
        private int cellHeight;
        private int positionFromLeft;
        private int positionFromTop;
        private int language;
        private int difficulty;
        private int mineAmount;
        public GameParameters() { }
        public int CellWidth
        {
            get { return cellWidth; }
            set { cellWidth = value; }
        }
        public int CellHeight
        {
            get { return cellHeight; }
            set { cellHeight = value; }
        }
        public int GameHeight
        {
            get { return gameWidth; }
            set { gameWidth = value; }
        }
        public int GameWidth
        {
            get { return gameHeight; }
            set { gameHeight = value; }
        }
        public int Difficulty
        {
            get { return difficulty; }
            set { difficulty = value; }
        }
        public int PositionFromTop
        {
            get { return positionFromTop; }
            set { positionFromTop = value; }
        }
        public int PositionFromLeft
        {
            get { return positionFromLeft; }
            set { positionFromLeft = value; }
        }
        public int Language
        {
            get { return language; }
            set { language = value; }
        }
        public int MineAmount
        {
            get { return mineAmount; }
            set { mineAmount = value; }
        }
    }
}
