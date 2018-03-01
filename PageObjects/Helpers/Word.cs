using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjects.Helpers
{
    public class Word
    {
        public string Text { get; private set; }
        public bool? Valid { get; set; }

        public Word(string text, bool? valid)
        {
            this.Text = text;
            this.Valid = valid;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
