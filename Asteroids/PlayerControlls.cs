using Asteroids.Exceptions;
using Asteroids.Services;
using Asteroids.SpaceObjects;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace Asteroids
{
    internal class PlayerControlls
    {
        //Game controls state
        public bool IsPressedMoveLeft { get; private set; } = false;
        public bool IsPressedMoveRight { get; private set; } = false;
        public bool IsPressedMoveUp { get; private set; } = false;
        public bool IsPressedMoveDown { get; private set; } = false;
        public bool IsPressedFire { get; private set; } = false;
        public bool IsPressedPause { get; private set; } = false;

        public bool IsGameOnPause { get; private set; } = false;

        private static ILogger Logger = ApplicationLogging.CreateLogger<Game>();

        public PlayerControlls()
        {
        }

        public void InitGameControlls(Form form)
        {
            form.KeyDown += (object sender, KeyEventArgs e) =>
            {
                if (e.KeyCode == Keys.Space)
                {
                    IsPressedFire = true;
                }
                if (e.KeyCode == Keys.Left)
                {
                    IsPressedMoveLeft = true;
                }
                if (e.KeyCode == Keys.Right)
                {
                    IsPressedMoveRight = true;
                }
                if (e.KeyCode == Keys.Up)
                {
                    IsPressedMoveUp = true;
                }
                if (e.KeyCode == Keys.Down)
                {
                    IsPressedMoveDown = true;
                }
                if (e.KeyCode == Keys.P)
                {
                    IsPressedPause = true;
                }
            };
            form.KeyUp += (object sender, KeyEventArgs e) =>
            {
                if (e.KeyCode == Keys.Space)
                {
                    IsPressedFire = false;
                }
                if (e.KeyCode == Keys.Left)
                {
                    IsPressedMoveLeft = false;
                }
                if (e.KeyCode == Keys.Right)
                {
                    IsPressedMoveRight = false;
                }
                if (e.KeyCode == Keys.Up)
                {
                    IsPressedMoveUp = false;
                }
                if (e.KeyCode == Keys.Down)
                {
                    IsPressedMoveDown = false;
                }
                if (e.KeyCode == Keys.P)
                {
                    IsGameOnPause = !IsGameOnPause;
                }
            };
        }

        public Bullet ProcessGameControls(SpaceShip spaceShip)
        {
            Bullet Bullet = null;

            if (IsPressedFire)
            {
                try
                {
                    Bullet = spaceShip.Shoot();
                }
                catch (OutOfAmmoException exception)
                {
                    // do nothing
                    Logger.LogWarning(exception, exception.Message);
                }
            }
            if (IsPressedMoveLeft)
            {
                spaceShip.MoveHorizontal(-1);
            }
            if (IsPressedMoveRight)
            {
                spaceShip.MoveHorizontal(1);
            }
            if (IsPressedMoveUp)
            {
                spaceShip.MoveVertical(-1);
            }
            if (IsPressedMoveDown)
            {
                spaceShip.MoveVertical(1);
            }

            return Bullet;
        }
    }
}
