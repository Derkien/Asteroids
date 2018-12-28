using System.Windows.Forms;

namespace Asteroids
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Form Form = new Form
            {
                Width = 1000,
                Height = 800,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                StartPosition = FormStartPosition.CenterScreen
            };

            Game.Init(Form);
            Form.Show();
            Game.Draw();

            Application.Run(Form);
        }
    }
}
