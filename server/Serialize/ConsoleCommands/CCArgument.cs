using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.ConsoleCommands
{
    internal abstract class CCArgument
    {
        public CCArgument(Type type)
        {
            this._type = type;
            Value = 0;
        }

        public override string ToString()
        {
            return _type.Name;
        }

        public abstract bool TryParse(in string value);

        public object Value { get; protected set; }

        protected Type _type;
    }

    internal class CCArgumentInt32 : CCArgument
    {
        public CCArgumentInt32() : base(typeof(int))
        {

        }

        public override bool TryParse(in string value)
        {
            int parsedValue = 0;
            bool flag = int.TryParse(value, out parsedValue);
            Value = parsedValue;
            return flag;
        }
    }

    internal class CCArgumentString : CCArgument
    {
        public CCArgumentString() : base(typeof(string))
        {

        }

        public override bool TryParse(in string value)
        {
            Value = value;
            return true;
        }
    }
}
