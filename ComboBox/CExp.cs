using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Xumeijin
{
    public class CExp
    {
        private static object _Cal(object objLeft, string strOper, object objRight)
        {
            switch (strOper.ToLower())
            {
                case "+":
                    return Operators.AddObject(objLeft, objRight);

                case "-":
                    return Operators.SubtractObject(objLeft, objRight);

                case "*":
                    return Operators.MultiplyObject(objLeft, objRight);

                case "/":
                    return Operators.DivideObject(objLeft, objRight);

                case @"\":
                    return Operators.IntDivideObject(objLeft, objRight);

                case "and":
                    return Operators.AndObject(objLeft, objRight);

                case "or":
                    return Operators.OrObject(objLeft, objRight);
                case "||":
                    return Operators.OrObject(objLeft, objRight);
                case "&":
                    return Operators.ConcatenateObject(objLeft, objRight);
                case "&&":
                    return Operators.AndObject(objLeft, objRight);
                case ">=":
                    return Operators.CompareObjectGreaterEqual(objLeft, objRight, false);

                case "<=":
                    return Operators.CompareObjectLessEqual(objLeft, objRight, false);

                case "=":
                    return Operators.CompareObjectEqual(objLeft, objRight, false);

                case "<":
                    return Operators.CompareObjectLess(objLeft, objRight, false);

                case ">":
                    return Operators.CompareObjectGreater(objLeft, objRight, false);

                case "<>":
                    return Operators.CompareObjectNotEqual(objLeft, objRight, false);
                case "!=":
                    return (objLeft != objRight);
                case "==":
                    return Operators.CompareObjectEqual(objLeft, objRight, false);
                case "is":
                    return (objLeft == objRight);

                case "isnot":
                    return (objLeft != objRight);

                case "not":
                    return Operators.NotObject(objRight);
            }
            throw new ApplicationException("无效的操作符号: " + strOper);
        }

        private static int[] _GetIndices(CPkg objEnv, string strExpression, ref int iOffset)
        {
            int[] array = new int[0];
            Match match = Regex.Match(strExpression.Substring(iOffset), @"^\s*\(");
            if (!match.Success)
            {
                return array;
            }
            iOffset += match.Length;
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*(?<oper>\))");
            if (match.Success)
            {
                iOffset += match.Length;
                return array;
            }
            while (true)
            {
                object objectValue = RuntimeHelpers.GetObjectValue(GetValue(objEnv, strExpression, ref iOffset));
                Array.Resize<int>(ref array, array.Length + 1);
                array[array.Length - 1] = Conversions.ToInteger(objectValue);
                match = Regex.Match(strExpression.Substring(iOffset), @"^\s*(?<oper>\,|\))");
                if (!match.Success)
                {
                    throw new ApplicationException("错误的参数: " + strExpression.Substring(iOffset));
                }
                iOffset += match.Length;
                string str = match.Groups["oper"].Value;
                if ((str != ",") && (str == ")"))
                {
                    return array;
                }
            }
        }

        private static object _GetObject(CPkg objEnv, string strExpression, ref int iOffset)
        {
            Match match = null;
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*new\s*(?<DataType>string|integer|decimal|char)\s*\(\s*\)\s*\{", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                iOffset += match.Length;
                object obj3 = null;
                switch (match.Groups["DataType"].Value.ToLower())
                {
                    case "string":
                        obj3 = new string[0];
                        break;

                    case "integer":
                        obj3 = new int[0];
                        break;

                    case "decimal":
                        obj3 = new decimal[0];
                        break;

                    case "char":
                        obj3 = new char[0];
                        break;
                }
                while (true)
                {
                    object obj4 = RuntimeHelpers.GetObjectValue(GetValue(objEnv, strExpression, ref iOffset));
                    object[] objArray4 = new object[] { RuntimeHelpers.GetObjectValue(obj3), Operators.AddObject(NewLateBinding.LateGet(obj3, null, "Length", new object[0], null, null, null), 1) };
                    bool[] flagArray = new bool[] { true, false };
                    NewLateBinding.LateCall(null, typeof(Array), "Resize", objArray4, null, null, flagArray, true);
                    if (flagArray[0])
                    {
                        obj3 = RuntimeHelpers.GetObjectValue(objArray4[0]);
                    }
                    NewLateBinding.LateIndexSet(obj3, new object[] { Operators.SubtractObject(NewLateBinding.LateGet(obj3, null, "Length", new object[0], null, null, null), 1), RuntimeHelpers.GetObjectValue(obj4) }, null);
                    match = Regex.Match(strExpression.Substring(iOffset), @"^\s*(?<oper>\,|\})");
                    if (!match.Success)
                    {
                        throw new ApplicationException("错误的表达式: " + strExpression.Substring(iOffset));
                    }
                    iOffset += match.Length;
                    string str2 = match.Groups["oper"].Value;
                    if ((str2 != ",") && (str2 == "}"))
                    {
                        return obj3;
                    }
                }
            }
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*(?<obj>\#Value)");
            if (match.Success)
            {
                iOffset += match.Length;
                object[] objArray = _GetParam(objEnv, strExpression, ref iOffset);
                if (objArray.Length != 1)
                {
                    return null;
                }
                return objEnv[objArray[0].ToString(), 0];
            }
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*(?<obj>\#Judge)");
            if (match.Success)
            {
                iOffset += match.Length;
                object[] objArray2 = _GetParam(objEnv, strExpression, ref iOffset);
                if (objArray2.Length == 1)
                {
                    try
                    {
                        return Interaction.IIf(Operators.ConditionalCompareObjectEqual(GetValue(objEnv, Conversions.ToString(objArray2[0])), true, false), 1, 0);
                    }
                    catch (Exception exception1)
                    {
                        ProjectData.SetProjectError(exception1);
                        Exception exception = exception1;
                        ProjectData.ClearProjectError();
                    }
                }
                return -1;
            }
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*(?<obj>StringSplitOptions\.RemoveEmptyEntries)(?=(\W|$))", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                iOffset += match.Length;
                return StringSplitOptions.RemoveEmptyEntries;
            }
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*(?<obj>StringSplitOptions\.None)(?=(\W|$))", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                iOffset += match.Length;
                return StringSplitOptions.None;
            }
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*(?<obj>(\-|\+|)\d+\.\d+)");
            if (match.Success)
            {
                iOffset += match.Length;
                return decimal.Parse(match.Groups["obj"].Value);
            }
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*(?<obj>(\-|\+|)\d+)");
            if (match.Success)
            {
                iOffset += match.Length;
                return int.Parse(match.Groups["obj"].Value);
            }
            match = Regex.Match(strExpression.Substring(iOffset), "^\\s*\"(?<str>((?<!\\\\)((\\\\.)+)(?!\\\\)|[^\\\\\"])*?)\"c(?=(\\W|$))");
            if (match.Success)
            {
                iOffset += match.Length;
                return Regex.Replace(match.Groups["str"].Value, @"\\(.)", "$1")[0];
            }
            if (Regex.Match(strExpression.Substring(iOffset), @"^\s*Not(?=(\W|$))", RegexOptions.IgnoreCase).Success)
            {
                return null;
            }
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*Nothing(?=(\W|$))", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                iOffset += match.Length;
                return null;
            }
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*Null(?=(\W|$))", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                iOffset += match.Length;
                return null;
            }
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*True(?=(\W|$))", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                iOffset += match.Length;
                return true;
            }
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*False(?=(\W|$))", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                iOffset += match.Length;
                return false;
            }
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*vbCrLf(?=(\W|$))", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                iOffset += match.Length;
                return "\r\n";
            }
            match = Regex.Match(strExpression.Substring(iOffset), "^\\s*\"(?<str>((?<!\\\\)((\\\\.)+)(?!\\\\)|[^\\\\\"])*?)\"");
            if (match.Success)
            {
                iOffset += match.Length;
                return Regex.Replace(match.Groups["str"].Value, @"\\(.)", "$1");
            }
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*\(");
            if (match.Success)
            {
                iOffset += match.Length;
                object obj5 = RuntimeHelpers.GetObjectValue(GetValue(objEnv, strExpression, ref iOffset));
                match = Regex.Match(strExpression.Substring(iOffset), @"^\s*\)");
                if (!match.Success)
                {
                    throw new ApplicationException("无效的子表达式: " + strExpression.Substring(iOffset));
                }
                iOffset += match.Length;
                Regex regex = new Regex(@"^\s*(?<oper>(\.|\())");
                for (match = regex.Match(strExpression.Substring(iOffset)); match.Success; match = regex.Match(strExpression.Substring(iOffset)))
                {
                    if (match.Groups["oper"].Value == ".")
                    {
                        iOffset += match.Length;
                        obj5 = RuntimeHelpers.GetObjectValue(_Invoke(objEnv, RuntimeHelpers.GetObjectValue(obj5), strExpression, ref iOffset));
                    }
                    else if (obj5.GetType().IsArray)
                    {
                        obj5 = RuntimeHelpers.GetObjectValue(((Array)obj5).GetValue(_GetIndices(objEnv, strExpression, ref iOffset)));
                    }
                    else
                    {
                        obj5 = RuntimeHelpers.GetObjectValue(_InvokeDefault(objEnv, RuntimeHelpers.GetObjectValue(obj5), strExpression, ref iOffset));
                    }
                }
                return obj5;
            }
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*(?<obj>[$]?(\w|\\.)+)");
            if (!match.Success)
            {
                throw new ApplicationException("获取对象失败: " + strExpression.Substring(iOffset));
            }
            iOffset += match.Length;
            object objectValue = RuntimeHelpers.GetObjectValue(objEnv[Regex.Replace(match.Groups["obj"].Value, @"\\(.)", "$1"), 0]);
            Regex regex2 = new Regex(@"^\s*(?<oper>(\.|\())");
            for (match = regex2.Match(strExpression.Substring(iOffset)); match.Success; match = regex2.Match(strExpression.Substring(iOffset)))
            {
                if (match.Groups["oper"].Value == ".")
                {
                    iOffset += match.Length;
                    objectValue = RuntimeHelpers.GetObjectValue(_Invoke(objEnv, RuntimeHelpers.GetObjectValue(objectValue), strExpression, ref iOffset));
                }
                else if (objectValue.GetType().IsArray)
                {
                    objectValue = RuntimeHelpers.GetObjectValue(((Array)objectValue).GetValue(_GetIndices(objEnv, strExpression, ref iOffset)));
                }
                else
                {
                    objectValue = RuntimeHelpers.GetObjectValue(_InvokeDefault(objEnv, RuntimeHelpers.GetObjectValue(objectValue), strExpression, ref iOffset));
                }
            }
            return objectValue;
        }

        private static string _GetOper(string strExpression, ref int iOffset)
        {
            Match match = null;
            match = Regex.Match(strExpression.Substring(iOffset).ToLower(), @"^\s*(?<oper>(and|or|isnot|is|not|\+|\-|\*|\/|\\|\&\&|\&|\|\||==|\!=|\>=|\>|\<=|\<\>|\<|=|$))");
            if (match.Success)
            {
                iOffset += match.Length;
                return match.Groups["oper"].Value;
            }
            return "";
        }

        private static object[] _GetParam(CPkg objEnv, string strExpression, ref int iOffset)
        {
            object[] array = new object[0];
            Match match = Regex.Match(strExpression.Substring(iOffset), @"^\s*\(");
            if (!match.Success)
            {
                return array;
            }
            iOffset += match.Length;
            match = Regex.Match(strExpression.Substring(iOffset), @"^\s*(?<oper>\))");
            if (match.Success)
            {
                iOffset += match.Length;
                return array;
            }
            while (true)
            {
                object objectValue = RuntimeHelpers.GetObjectValue(GetValue(objEnv, strExpression, ref iOffset));
                Array.Resize<object>(ref array, array.Length + 1);
                array[array.Length - 1] = RuntimeHelpers.GetObjectValue(objectValue);
                match = Regex.Match(strExpression.Substring(iOffset), @"^\s*(?<oper>\,|\))");
                if (!match.Success)
                {
                    throw new ApplicationException("错误的参数: " + strExpression.Substring(iOffset));
                }
                iOffset += match.Length;
                string str = match.Groups["oper"].Value;
                if ((str != ",") && (str == ")"))
                {
                    return array;
                }
            }
        }

        private static object _Invoke(CPkg objEnv, object obj, string strExpression, ref int iOffset)
        {
            Match match = Regex.Match(strExpression.Substring(iOffset), @"^(?<name>\w+)");
            Type type = obj.GetType();
            if (!match.Success)
            {
                throw new ApplicationException(type.Name + "." + strExpression + "不能匹配");
            }
            iOffset += match.Length;
            object[] args = _GetParam(objEnv, strExpression, ref iOffset);
            string strChineseName = match.Groups["name"].Value;
            PropertyInfo objPropertyInfo = type.GetProperty("Item");
            if (objPropertyInfo != null)
            {
                try
                {
                    object objTemp = objPropertyInfo.GetValue(obj, new object[] { strChineseName });

                    if (objTemp != null)
                    {
                        return objTemp;
                    }
                }
                catch
                {
                }
            }

            MemberInfo[] member = type.GetMember(strChineseName);
            int num2 = member.Length - 1;
            for (int i = 0; i <= num2; i++)
            {
                try
                {
                    switch (member[i].MemberType)
                    {
                        case MemberTypes.Property:
                            return type.InvokeMember(strChineseName, BindingFlags.GetProperty, null, RuntimeHelpers.GetObjectValue(obj), args);
                        case MemberTypes.Method:
                            return type.InvokeMember(strChineseName, BindingFlags.InvokeMethod, null, RuntimeHelpers.GetObjectValue(obj), args);
                    }
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    ProjectData.ClearProjectError();
                }
            }
            throw new ApplicationException(type.Name + "." + match.Groups["name"].Value + "执行失败");
        }

        private static object _InvokeDefault(CPkg objEnv, object obj, string strExpression, ref int iOffset)
        {
            Type type = obj.GetType();
            object[] args = _GetParam(objEnv, strExpression, ref iOffset);
            MemberInfo[] defaultMembers = type.GetDefaultMembers();
            int num2 = defaultMembers.Length - 1;
            for (int i = 0; i <= num2; i++)
            {
                try
                {
                    switch (defaultMembers[i].MemberType)
                    {
                        case MemberTypes.Property:
                            return type.InvokeMember(defaultMembers[i].Name, BindingFlags.GetProperty, null, RuntimeHelpers.GetObjectValue(obj), args);
                        case MemberTypes.Method:
                            return type.InvokeMember(defaultMembers[i].Name, BindingFlags.InvokeMethod, null, RuntimeHelpers.GetObjectValue(obj), args);
                    }
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    ProjectData.ClearProjectError();
                }
            }
            throw new ApplicationException(type.Name + "执行Default失败");
        }

        private static int _Priority(string strOper)
        {
            string str = strOper.ToLower();
            switch (str)
            {
                case "":
                    return 0;
                case "or":
                case "||":
                    return 10;
                case "and":
                case "&&":
                    return 11;
            }
            if (((str == "<>") ? 1 : 0) != 0)
            {
                return 12;
            }
            if (((str == "!=") ? 1 : 0) != 0)
            {
                return 13;
            }
            if (str == "&")
            {
                return 15;
            }
            if ((((str == "+") || (str == "-")) ? 1 : 0) != 0)
            {
                return 20;
            }
            if ((str == "*") || (str == "/"))
            {
                return 25;
            }
            if (((str == @"\") ? 1 : 0) != 0)
            {
                return 0x15;
            }
            if ((((str == "is") || (str == "isnot")) ? 1 : 0) != 0)
            {
                return 30;
            }
            if ((((str == "==")) ? 1 : 0) != 0)
            {
                return 35;
            }
            if (str != "not")
            {
                throw new ApplicationException("无效的操作符: " + strOper);
            }
            return 40;
        }

        public static string Escape(string strObject)
        {
            return Regex.Replace(strObject, @"(\W)", @"\$1");
        }

        public static object GetValue(CPkg objEnv, string strExpression)
        {
            int iOffset = 0;
            object objectValue = GetValue(objEnv, strExpression, ref iOffset);
            return GetValue(objectValue);
        }

        private static object GetValue(object objectValue)
        {
            object o = objectValue;

            if (objectValue != null && objectValue.ToString() != "")
            {
                decimal dTemp = 0;
                bool bDecimal = decimal.TryParse(objectValue.ToString(), out dTemp);

                if (!bDecimal)
                {
                    bool bDate = false;
                    DateTime dDate = DateTime.Parse("1900-01-01");
                    bDate = DateTime.TryParse(objectValue.ToString(), out dDate);

                    if (bDate && dDate == DateTime.Parse("1900-01-01"))
                    {
                        o = "";
                    }
                }
            }
            return o;
        }

        public static object GetValue(CPkg objEnv, string strExpression, ref int iOffset)
        {
            Stack<object> stack2 = new Stack<object>();
            Stack<string> stack = new Stack<string>();
            object objectValue = null;
            while (strExpression.Substring(iOffset).Trim().Length > 0)
            {
                objectValue = RuntimeHelpers.GetObjectValue(_GetObject(objEnv, strExpression, ref iOffset));
                string strOper = _GetOper(strExpression, ref iOffset);
                if (strOper != "")
                {
                    goto Label_00A1;
                }
                while (stack.Count > 0)
                {
                    objectValue = RuntimeHelpers.GetObjectValue(_Cal(RuntimeHelpers.GetObjectValue(stack2.Pop()), stack.Pop(), RuntimeHelpers.GetObjectValue(objectValue)));
                }
                return objectValue;
            Label_007E:
                objectValue = RuntimeHelpers.GetObjectValue(_Cal(RuntimeHelpers.GetObjectValue(stack2.Pop()), stack.Pop(), RuntimeHelpers.GetObjectValue(objectValue)));
            Label_00A1: ;
                if ((((stack.Count > 0) && (_Priority(strOper) <= _Priority(stack.Peek()))) ? 1 : 0) != 0)
                {
                    goto Label_007E;
                }
                stack.Push(strOper);
                stack2.Push(RuntimeHelpers.GetObjectValue(objectValue));
            }

            return GetValue(objectValue);
        }
    }
}
