using System;
using System.Collections.Generic;
using System.Reflection;
using CustomMacros.Areas.Infrastructure.Commands;

namespace CustomMacros.Areas.Infrastructure.Commons
{
    public class Utilities
    {
        public static string SetJqScript(string jqReady, string jqStandard)
        {
            string myRes = "<script type=\"text/javascript\"> " + jqStandard;

            if (!string.IsNullOrEmpty(jqReady))
            {
                myRes += " $(function(){ \n" + jqReady + " \n}); ";
            }
            myRes += " </script>";
            return myRes;
        }

        public static string RedirectClientJson(string myUrl)
        {
            return SetJqScript(string.Empty, "window.location = \"" + myUrl + "\"; ");
        }

        public static Type GetTypeFromName(string typeName)
        {
            Type type = Type.GetType(typeName);
            if (type != null) return type;
            // *** try to find manually
            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = ass.GetType(typeName, false);
                if (type != null) break;
            }
            return type;
        }

        public static string GetJsCommandsListener(IList<Command> commands, string macroName)
        {
            string jsStr = "";
            foreach (Command r in commands)
            {
                jsStr += "CustomMacros.commands.CommandMng.CommandTypes.set('" + r.Name + "').setListener('" + macroName + "');\n";
            }
            return jsStr;
        }

        #region // Casting Values

        public static Int64 CastInt64(object _Value, Int64 _defValue = 0, bool IsDebug = false)
        {
            Int64 _out;
            if (_Value == null) { return _defValue; }
            if (Int64.TryParse(_Value.ToString(), out _out)) { return _out; }
            else { return _defValue; }
        }

        public static Int16 CastInt16(object _Value, Int16 _defValue = 0, bool IsDebug = false)
        {
            Int16 _out;
            if (_Value == null) { return _defValue; }
            if (Int16.TryParse(_Value.ToString(), out _out)) { return _out; }
            else { return _defValue; }
        }

        public static Int32 CastInt32(object _Value, int _defValue = 0, bool IsDebug = false)
        {
            return CastInt(_Value, _defValue, IsDebug);
        }

        public static string CastString(object _Value, string _defValue = "", bool IsDebug = false)
        {
            string myCast = _defValue;
            if (_Value != null)
            {
                myCast = _Value.ToString();

            }
            return myCast;
        }

        public static bool CastBool(object _Value, bool _defValue = false, bool IsDebug = false)
        {
            bool myCast = _defValue;
            if (_Value != null && !string.IsNullOrEmpty(_Value.ToString()))
            {
                if (_Value.ToString() == "0" || _Value.ToString().ToLower() == "false") myCast = false;
                else myCast = true;
            }

            return myCast;
        }

        public static DateTime CastDateTime(object _Value, DateTime _defValue, bool IsDebug = false)
        {
            DateTime _out;
            if (_Value == null) { return _defValue; }
            if (DateTime.TryParse(_Value.ToString(), out _out)) { return _out; }
            else { return _defValue; }
        }

        public static DateTime CastDateTime(object _Value, bool IsDebug = false)
        {
            return CastDateTime(_Value, new DateTime(1900, 1, 1), IsDebug);
        }

        public static decimal CastDecimal(object _Value, decimal _defValue = 0, bool IsDebug = false)
        {
            decimal _out;
            if (_Value == null) { return _defValue; }
            if (decimal.TryParse(_Value.ToString(), out _out)) { return _out; }
            else { return _defValue; }
        }

        public static int CastInt(object _Value, int _defValue = 0, bool IsDebug = false)
        {
            int _out;
            if (_Value == null) { return _defValue; }
            if (int.TryParse(_Value.ToString(), out _out)) { return _out; }
            else { return _defValue; }
        }

        public static long CastLong(object _Value, long _defValue = 0, bool IsDebug = false)
        {
            long _out;
            if (_Value == null) { return _defValue; }
            if (long.TryParse(_Value.ToString(), out _out)) { return _out; }
            else { return _defValue; }
        }

        public static Guid CastGuid(object _Value, Guid _defValue = new Guid(), bool IsDebug = false)
        {
            Guid _out;
            if (_Value == null) { return _defValue; }
            if (Guid.TryParse(_Value.ToString(), out _out)) { return _out; }
            else { return _defValue; }
        }

        #endregion

        #region // Convert Values

        public static bool ConvertToBool(string myReqVal)
        {
            return ConvertToBool(myReqVal, false);
        }

        public static bool ConvertToBool(string myReqVal, bool defValue)
        {
            if (defValue)
            {
                if (myReqVal != "0" && myReqVal != "false")
                    return true;
                else
                    return false;
            }
            else
            {
                if (myReqVal == "0" || myReqVal == "false")
                    return false;
                else
                    return true;
            }
        }

        #endregion

    }
}