

using System;

namespace POS
{
    public interface IRequestFocus
    {
        event EventHandler<FocusRequestedEventArgs> FocusRequested;
    }
    public class FocusRequestedEventArgs:EventArgs
    {
        public FocusRequestedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; private set; }
    }
}
