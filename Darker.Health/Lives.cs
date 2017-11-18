using System;

namespace Darker.Health
{
    public class Lives
    {
        private int _maximum;
        private int _remaining;

        public Lives(int maximum)
        {
            Maximum = maximum;
            Remaining = Maximum;
        }

        public int Maximum
        {
            get => _maximum;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Maximum lives must be a positive number greater than zero. Cannot Set {value}");
                _maximum = value;
                _remaining = Math.Min(_remaining, _maximum);
            }
        }

        public int Remaining
        {
            get => _remaining;
            set
            {
                if (value > Maximum)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Maximum lives is {Maximum}. Cannot Set Remaining to {value}");
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Remaining lives must be a positive number.  Cannot Set Remaining to {value}");
                _remaining = value;
                if (_remaining == 0) OnDepleted();
            }
        }

        public event EventHandler Depleted;

        protected virtual void OnDepleted()
        {
            Depleted?.Invoke(this, EventArgs.Empty);
        }
    }
}