using Asteroids.SpaceObjects;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Asteroids
{
    internal class SpaceObjectSpawner
    {
        private const int MaxSmallEnergyPacksCount = 3;
        private const int MaxBigAsteroidsCount = 1;
        private const int MaxMediumAsteroidsCount = 2;
        private const int MaxSmallAsteroidsCount = 3;
        private const int WindowGridColsCount = 10;
        private const int WindowGridRowsCount = 10;

        private static int AsteroidsCountIncrement = 0;

        private int Width { get; }
        private int Height { get; }

        private Random Random = new Random();

        public SpaceObjectSpawner(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Ufo SpawnUfo(List<SpaceObject> spaceObjects)
        {
            var Ufo = new Ufo(new Point(Width / 2, 0));
            spaceObjects.Add(Ufo);

            return Ufo;
        }

        public SpaceShip SpawnSpaceShip(List<SpaceObject> spaceObjects)
        {
            var SpaceShip = new SpaceShip(new Point(0, Height / 2));
            spaceObjects.Add(SpaceShip);

            return SpaceShip;
        }

        private void SpawnAsteroids(List<SpaceObject> spaceObjects, int maxBigAsteroidsCount, int maxMediumAsteroidsCount, int maxSmallAsteroidsCount)
        {
            SpawnAsteroids((Point leftTopPosition) => { return new BigAsteroid(leftTopPosition); }, maxBigAsteroidsCount, spaceObjects);
            SpawnAsteroids((Point leftTopPosition) => { return new MediumAsteroid(leftTopPosition); }, maxMediumAsteroidsCount, spaceObjects);
            SpawnAsteroids((Point leftTopPosition) => { return new SmallAsteroid(leftTopPosition); }, maxSmallAsteroidsCount, spaceObjects);
        }

        private void SpawnAsteroids(Func<Point, Asteroid> createAsteroid, int MaxCount, List<SpaceObject> spaceObjects)
        {
            for (var i = 0; i < MaxCount; i++)
            {
                var asteroid = createAsteroid(new Point(Width, Random.Next(0, Height)));
                asteroid.AsteroidDestructionEvent += SpaceShip.CalculateScore;

                spaceObjects.Add(asteroid);
            }
        }

        public void LoadBackgroundStars(List<SpaceObject> spaceObjectList)
        {
            //TODO: optimize objects spreading... now its like a snow storm :D

            for (int startX = 1, i = 1; startX < Width; startX += Width / WindowGridColsCount, i++)
            {
                for (int startY = 1, j = 1; startY < Height; startY += Height / WindowGridRowsCount, j++)
                {
                    if ((j + i) % 3 == 0)
                    {
                        spaceObjectList.Add(
                            new PulsingConstellation(
                                new Point(startX, startY),
                                new Point(Random.Next(10, 20), Random.Next(5, 15))
                            )
                        );
                    }
                    else
                    {
                        spaceObjectList.Add(
                            new XStar(
                                new Point(startX, startY),
                                new Point(Random.Next(5, 15), Random.Next(10, 20)),
                                new Size(3, 3)
                            )
                        );
                    }
                }
            }
        }

        public void SpawnLevelAsteroids(List<SpaceObject> spaceObjectList)
        {
            SpawnAsteroids(spaceObjectList, MaxBigAsteroidsCount + AsteroidsCountIncrement, MaxMediumAsteroidsCount + AsteroidsCountIncrement, MaxSmallAsteroidsCount + AsteroidsCountIncrement);

            AsteroidsCountIncrement++;
        }

        public void SpawnEnergyPack(List<SpaceObject> spaceObjectList)
        {
            SmallEnergyPack SmallEnergyPack = new SmallEnergyPack(new Point(Width, Random.Next(0, Height)));
            spaceObjectList.Add(SmallEnergyPack);
        }
    }
}
