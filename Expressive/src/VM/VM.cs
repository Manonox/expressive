using System.Runtime.InteropServices;

namespace Expressive
{
    public class VM
    {
        public Dictionary<string, Value> variables = new();

        public class Value
        {
            public ValueType Type { get; } = ValueType.Nil;

            readonly dynamic? value;

            public Value() {}

            public Value(int i)
            {
                Type = ValueType.Integer;
                value = i;
            }

            public Value(float f)
            {
                Type = ValueType.Float;
                value = f;
            }

            public Value(string s)
            {
                Type = ValueType.String;
                value = s;
            }

            public Value(bool b)
            {
                Type = ValueType.Bool;
                value = b;
            }

            public Value(Func<List<Value>, Value> func)
            {
                Type = ValueType.Function;
                value = func;
            }

            public Value Call(List<Value> args)
            {
                if (Type != ValueType.Function)
                    throw new Exception();
                var func = value as Func<List<Value>, Value> ?? throw new Exception();
                return func.Invoke(args);
            }

            public bool IsFalsy()
            {
                return Type == ValueType.Nil || (Type == ValueType.Bool && value == false);
            }

            public bool IsTruthy() => !IsFalsy();


            public static Value operator +(Value a, Value b) => new(a.value + b.value);
            public static Value operator -(Value a, Value b) => new(a.value - b.value);
            public static Value operator *(Value a, Value b) => new(a.value * b.value);
            public static Value operator /(Value a, Value b) => new(a.value / b.value);
            public static Value Or(Value a, Value b)
            {
                if (a.Type == ValueType.Nil)
                    return b;
                if (a.Type == ValueType.Bool && a.value == false)
                    return b;
                return a;
            }

            public static Value And(Value a, Value b)
            {
                if (a.Type == ValueType.Nil)
                    return a;
                if (a.Type == ValueType.Bool && a.value == false)
                    return a;
                return b;
            }

            public static Value Eq(Value a, Value b) => new(a.value == b.value);
            public static Value NotEq(Value a, Value b) => new(a.value != b.value);

            public override string ToString()
            {
                if (value == null)
                    return "nil";
                if (Type == ValueType.Bool)
                    return value.ToString().ToLower();
                if (Type == ValueType.Function)
                    return "<builtin function>";
                return value.ToString();
            }
        }

        public enum ValueType
        {
            Nil,
            Integer,
            Float,
            String,
            Bool,
            Function
        }
    }
}