using System;
using System.Windows.Forms;

namespace PassSecure.Models
{
    public class KeyStroke
    {
        public KeyStroke(Keys key)
        {
            this.Key = key;
        }

        public Keys Key { get; set; }

        public TimeSpan KeyDownTime { get; set; }

        private TimeSpan keyUpTime;

        public TimeSpan KeyUpTime
        {
            get
            {
                return keyUpTime;
            }
            set
            {
                keyUpTime = value;
                this.TimeBetweenDownAndUp = KeyUpTime - KeyDownTime;
            }
        }

        public TimeSpan TimeBetweenDownAndUp { get; set; }
    }
}
