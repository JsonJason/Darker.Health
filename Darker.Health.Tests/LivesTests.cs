using System;
using NUnit.Framework;

namespace Darker.Health.Tests
{
    [TestFixture]
    public class LivesTests
    {
        [SetUp]
        public void SetUp()
        {
            _lives = new Lives(MaximumLives);
        }

        private Lives _lives;
        private const int MaximumLives = 5;

        [Test]
        public void Amount_Of_Lives_Remaining_Starts_Full()
        {
            Assert.AreEqual(MaximumLives, _lives.Remaining);
        }

        [Test]
        public void Cannot_Set_Lives_Higher_Than_Maximum()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _lives.Remaining = 10);
        }

        [Test]
        public void Cannot_Set_Negative_Maximum_Lives()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _lives.Maximum = -5);
        }

        [Test]
        public void Cannot_Set_Negative_Remaining_Lives()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _lives.Remaining = -5);
        }

        [Test]
        public void Cannot_Set_Zero_Maximum_Lives()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _lives.Maximum = 0);
        }

        [Test]
        public void Decreasing_Maximum_Lives_Caps_Remaining_To_Maximum()
        {
            _lives.Maximum = 3;
            Assert.AreEqual(_lives.Maximum, _lives.Remaining);
        }

        [Test]
        public void Increasing_Maximum_Lives_Does_Not_Effect_Remaining()
        {
            var livesBeforeChange = _lives.Remaining;
            _lives.Maximum = 10;
            Assert.AreEqual(livesBeforeChange, _lives.Remaining);
        }


        [Test]
        public void Setting_Remaining_To_Zero_Depletes_Lives()
        {
            var timesDepletedEventFired = 0;
            _lives.Depleted += (s, a) => timesDepletedEventFired++;

            _lives.Remaining = 0;
            Assert.AreEqual(1, timesDepletedEventFired);
        }
    }
}