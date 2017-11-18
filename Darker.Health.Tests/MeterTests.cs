using System;
using NUnit.Framework;

namespace Darker.Health.Tests
{
    [TestFixture]
    public class MeterTests
    {
        [SetUp]
        public void SetUp()
        {
            _health = new Meter(MaximumHealth);
        }

        private Meter _health;
        public int MaximumHealth = 100;

        [Test]
        public void Refill_Fires_Refilled_Event()
        {
            var timesRefilledFired = 0;
            _health.Maximum = 50;
            _health.Value = 10;
            _health.Refilled += (o, a) => timesRefilledFired++;

            _health.Refill();

            Assert.AreEqual(1, timesRefilledFired);
        }

        [Test]
        public void Decrease_Lowers_Meter_Value()
        {
            _health.Value = 50;
            _health.Decrease(6);

            Assert.AreEqual(44, _health.Value);
        }

        [Test]
        public void Decrease_Of_Exact_Value_Return_Zero_Remainder()
        {
            var remainder = _health.Decrease(_health.Value);
            Assert.Zero(remainder);
        }

        [Test]
        public void Decrease_Of_More_Than_Maximum_Clamp_Value_To_Zero()
        {
            _health.Decrease(MaximumHealth + 3);
        }

        [Test]
        public void Decrease_Of_More_Than_Value_Returns_Remainder()
        {
            var remainder = _health.Decrease(_health.Value + 3);
            Assert.AreEqual(3, remainder);
        }

        [Test]
        public void Decrease_To_Zero_Depletes_Meter()
        {
            var timesDepletedFired = 0;
            _health.Value = 10;
            _health.Depleted += (o, a) => timesDepletedFired++;
            _health.Decrease(10);

            Assert.AreEqual(1, timesDepletedFired);
        }

        [Test]
        public void Decrease_Value_Cannot_Be_Negative()
        {
            _health.Value = 50;
            Assert.Throws<ArgumentOutOfRangeException>(() => _health.Decrease(-6));
        }

        [Test]
        public void Deplete_Fires_Deplete_Event()
        {
            var timesDepletedFired = 0;
            _health.Value = 10;
            _health.Depleted += (o, a) => timesDepletedFired++;
            _health.Deplete();

            Assert.AreEqual(1, timesDepletedFired);
        }

        [Test]
        public void Deplete_Returns_Amount_Depleted()
        {
            _health.Value = 38;
            var depleted = _health.Deplete();

            Assert.AreEqual(38, depleted);
        }

        [Test]
        public void Increase_Of_More_Than_Maximum_Clamps_To_Maximum()
        {
            _health.Maximum = 50;
            _health.Value = 20;
            _health.Increase(45);

            Assert.AreEqual(50, _health.Value);
        }

        [Test]
        public void Increase_Over_Maximum_Returns_Remainder()
        {
            _health.Maximum = 5;
            _health.Value = 1;

            var remainder = _health.Increase(10);

            Assert.AreEqual(6, remainder);
        }

        [Test]
        public void Increase_Raises_Value()
        {
            _health.Value = 50;
            _health.Increase(9);
            Assert.AreEqual(59, _health.Value);
        }

        [Test]
        public void Increase_To_Maximum_Returns_Zero_Remainder()
        {
            _health.Maximum = 25;
            _health.Value = 5;

            var remainder = _health.Increase(20);

            Assert.Zero(remainder);
        }

        [Test]
        public void Increase_Value_Cannot_Be_Negative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _health.Increase(-6));
        }

        [Test]
        public void Increasing_To_Full_Refills()
        {
            var timesRefilledFired = 0;
            _health.Maximum = 50;
            _health.Value = 10;
            _health.Refilled += (o, a) => timesRefilledFired++;

            _health.Increase(40);

            Assert.AreEqual(1, timesRefilledFired);
        }

        [Test]
        public void Maximum_Value_Must_Be_Greater_Than_Zero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _health.Maximum = -5);
            Assert.Throws<ArgumentOutOfRangeException>(() => _health = new Meter(-5));
        }

        [Test]
        public void Meter_Value_Cannot_Be_Less_Than_Zero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _health.Value = -6);
        }

        [Test]
        public void Meter_Value_Cannot_Be_Set_Higher_Than_Maximum()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _health.Value = MaximumHealth + 1);
        }

        [Test]
        public void Meter_Value_Reaching_Zero_Depletes_Meter()
        {
            var timesDepletedEventFired = 0;
            _health.Depleted += (o, a) => timesDepletedEventFired++;

            _health.Value = 0;

            Assert.AreEqual(1, timesDepletedEventFired);
        }

        [Test]
        public void Meter_Value_Starts_At_Maximum()
        {
            Assert.AreEqual(MaximumHealth, _health.Value);
        }

        [Test]
        public void Refill_Meter_Sets_Value_To_Maximum()
        {
            _health.Maximum = 50;
            _health.Value = 10;

            _health.Refill();

            Assert.AreEqual(_health.Maximum, _health.Value);
        }

        [Test]
        public void Refill_Returns_Amount_Required_To_Refill()
        {
            _health.Maximum = 50;
            _health.Value = 10;

            var amountUsedToRefill = _health.Refill();

            Assert.AreEqual(40, amountUsedToRefill);
        }

        [TestCaseSource(nameof(percentageCases))]
        public void FillToPercent_Sets_Value(int maximum, int desiredpercent, int expectedvalue)
        {
            _health.Maximum = maximum;
           

            _health.FillToPercent(desiredpercent);

            Assert.AreEqual(expectedvalue,_health.Value);
            Assert.AreEqual(desiredpercent, _health.PercentageFilled);
        }


        private static object[] percentageCases =
        {
            new object[] {200, 1, 2},
            new object[] { 50, 50, 25}
        };

        [TestCaseSource(nameof(percentageRemainderCases))]
        public void FillToPercent_Returns_Value_Change(int max,int value,int desiredPercent,int amountChanged)
        {
            _health.Maximum = max;
            _health.Value = value;

            var changed = _health.FillToPercent(desiredPercent);

            Assert.AreEqual(amountChanged,changed);
        }

        private static object[] percentageRemainderCases =
        {
            new object[] {100,10,50,40},
            new object[] {100, 95,50,-45}
        };

        [Test]
        public void FillToPercent_Cannot_Fill_Negative_Percentage()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _health.FillToPercent(-56));
        }

        [Test]
        public void FillToPercent_Cannot_Fill_Past_100()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _health.FillToPercent(106));
        }


    }
}