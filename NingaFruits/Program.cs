
namespace NingaFruits
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            bool playAgain = true;

            while (playAgain)
            {
                // ---- STEP 1: Show the Intro Screen ----
                IntroForm introForm = new IntroForm();
                introForm.ShowDialog();

                // If user closed the intro window without clicking Start, exit
                if (!introForm.StartClicked)
                {
                    break;
                }

                // ---- STEP 2: Show the Game Screen ----
                Form1 gameForm = new Form1();
                gameForm.ShowDialog();

                // If user closed the game window manually (X button), exit
                if (!gameForm.GameEndedNaturally)
                {
                    break;
                }

                // ---- STEP 3: Show the Game Over Screen ----
                GameOverForm gameOverForm = new GameOverForm(gameForm.FinalScore, gameForm.WasBombExploded, gameForm.ResourcesPath);
                gameOverForm.ShowDialog();

                // If user clicked Restart, loop again; otherwise exit
                playAgain = gameOverForm.RestartClicked;
            }
        }
    }
}