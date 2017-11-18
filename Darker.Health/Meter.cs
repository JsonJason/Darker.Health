using System;

namespace Darker.Health
{
    public class Meter
    {
        private int _maximum;
        private int _value;

        public Meter(int maximum)
        {
            Maximum = maximum;
            Value = Maximum;
        }

        public int Maximum
        {
            get => _maximum;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Meter maximum value must be greater than zero. Cannot set to: {value}");
                _maximum = value;
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                if (value > Maximum)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Maximum Value is {Maximum}. Cannot set to: {value}");
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Meter value must be a positive number. Cannot set to: {value}");
                _value = value;
                if (_value == 0) OnDepleted();
            }
        }

        public event EventHandler Depleted;

        protected virtual void OnDepleted()
        {
            Depleted?.Invoke(this, EventArgs.Empty);
        }

        public int Decrease(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(
                    $"Meter decrease amount must be a positive number. Cannot decrease by {amount}.");
            if (amount > Value)
            {
                var remainder = amount - Value;
                Value = 0;
                return remainder;
            }
            Value -= amount;
            return 0;
        }

        public int Increase(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(
                    $"Meter increase amount must be a positive number. Cannot increase by {amount}.");

            var afterIncrease = Value + amount;

            if (afterIncrease > Maximum)
            {
                Value = Maximum;
                return afterIncrease - Maximum;
            }

            Value += amount;
            return 0;
        }
    }
}