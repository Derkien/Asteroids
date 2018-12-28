using System.Windows.Forms;

namespace Asteroids
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Form Form = new Form
            {
                Width = 800,
                Height = 600
            };

            Game.Init(Form);
            Form.Show();
            Game.Draw();

            Application.Run(Form);
        }
    }
}
