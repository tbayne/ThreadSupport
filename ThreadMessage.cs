using System;
using System.Collections;

namespace ThreadSupport
    {
    public enum TmDataTypes
        {
        Unknown,
        Bool,
        String,
        Int,
        Object
        }

    public class ThreadMessage : IEnumerable
        {
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private MsgDataCollection _data;

        public ThreadMessage(int cmd)
            {
            Cmd = cmd;
            _data = new MsgDataCollection();
            }

        public int Cmd { get; private set; }

        public string Lasterror { get; private set; }

        
        public int Count
            {
            // ReSharper disable once ConvertPropertyToExpressionBody
            get { return _data.Count; }
            }

        public bool Exists(string key)
            {

            return _data.Contains(key);
            }

        public bool Contains(string key)
            {
            return _data.Contains(key);
            }

        public bool Remove(string key)
            {
            bool rc = true;
            try
                {
                if (_data.Contains(key))
                    _data.Remove(key);
                else
                    {
                    rc = false;
                    Lasterror = "Key [" + key + "] doesn't exist";
                    }
                }
            catch (Exception ex)
                {

                rc = false;
                Lasterror = ex.Message;
                }
            return rc;
            }

        private TmDataTypes GetDataType(object o)
            {
            TmDataTypes t = TmDataTypes.Unknown;
            if (o != null)
            {
                string type = o.GetType().ToString();
                switch (type.ToUpper())
                {
                    case "SYSTEM.INT32":
                        t = TmDataTypes.Int;
                        break;
                    case "SYSTEM.STRING":
                        t = TmDataTypes.String;
                        break;
                    case "SYSTEM.BOOLEAN":
                        t = TmDataTypes.Bool;
                        break;
                    default:
                        t = TmDataTypes.Object;
                        break;
                }
            }
            return t;
            }

        public TmDataTypes GetType(string key)
            {
            TmDataTypes t = TmDataTypes.Unknown;
            if (_data.Contains(key))
                t = GetDataType(_data[key]);
            return t;
            }

        public bool Add(string key, object dataItem)
            {
            bool rc = true;
            try
                {
                _data.Add(key, dataItem);
                }
            catch (Exception ex)
                {
                Lasterror = ex.Message;
                rc = false;
                }
            return rc;
            }

        public string GetString(string key)
            {
            Lasterror = string.Empty;
            string rc;
            try
                {
                rc = _data[key].ToString();
                }
            catch (Exception ex)
                {
                Lasterror = ex.Message;
                rc = string.Empty;
                }
            return rc;
            }

        public bool GetBool(string key)
            {
            Lasterror = string.Empty;
            bool rc;
            try
                {
                rc = (bool) _data[key];
                }
            catch (Exception ex)
                {
                Lasterror = ex.Message;
                rc = false;
                }
            return rc;
            }

        public int GetInt(string key)
            {
            Lasterror = string.Empty;
            int rc;
            try
                {
                if (GetDataType(_data[key]) == TmDataTypes.Int)
                    rc = (int)_data[key];
                else
                {
                    rc = 0;
                }
                }
            catch (Exception ex)
                {
                Lasterror = ex.Message;
                rc = 0;
                }
            return rc;
            }

        public object GetObject(string key)
            {
            Lasterror = string.Empty;
            object rc;
            try
                {
                rc = _data[key];
                }
            catch (Exception ex)
                {
                Lasterror = ex.Message;
                rc = null;
                }
            return rc;
            }

        public IEnumerator GetEnumerator()
            {
            return _data.GetEnumerator();
            }
        }
    }
