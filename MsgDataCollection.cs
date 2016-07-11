using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ThreadSupport
    {
    [Serializable]
    public class MsgDataCollection : NameObjectCollectionBase 
        {
        // You can add non-serializable types to the collection.
        // However, the serializer with throw an exception if you try to
        // serialize the collection when it contains non-serializable types.

        private IEqualityComparer _keyComparer;
        private SerializationInfo _collectionInfo;

        public MsgDataCollection()
            : this(0)
            {
            }
        public MsgDataCollection(IEqualityComparer equalityComparer)
            : this(0, equalityComparer)
            {
            }
        public MsgDataCollection(Int32 capacity)
            : base(capacity)
            {
            _keyComparer = StringComparer.InvariantCultureIgnoreCase;
            }
        public MsgDataCollection(Int32 capacity, IEqualityComparer equalityComparer)
            : base(capacity, equalityComparer)
        {
            _keyComparer = equalityComparer ?? StringComparer.InvariantCultureIgnoreCase;
        }

        protected MsgDataCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
            {
            _collectionInfo = info;
            }

        public Object this[Int32 index]
            {
            // ReSharper disable once ConvertPropertyToExpressionBody
            get { return BaseGet(index); }
            }
        public Object this[String name]
            {
            get { return BaseGet(name); }
            set
                {
                OnValidate(name, value);
                BaseSet(name, value);
                }
            }

        private Int32 IndexOfInternal(String name)
            {
            int keyIndex = -1;
            string[] names = BaseGetAllKeys();
            if (names.Length > 0)
                {
                for (int index = 0; index < names.Length; index++)
                    {
                    if (_keyComparer.Equals(names[index], name))
                        {
                        keyIndex = index;
                        break;
                        }
                    }
                }
            return keyIndex;
            }


        protected virtual void OnValidate(String name, Object value)
            {
            if (name == null)
                {
                throw new ArgumentNullException(nameof(name));
                }
            if (name.Length == 0)
                {
                throw new ArgumentException("Name cannot be empty (zero length).", nameof(name));
                }
            if (value == null)
                {
                throw new ArgumentNullException(nameof(value));
                }
            }

        public void Add(String name, Object value)
            {
            OnValidate(name, value);

            // Required IF 'name' MUST be unique...
            if (IndexOfInternal(name) > -1)
                {
                throw new ArgumentException("Duplicate name.", nameof(name));
                }
            BaseAdd(name, value);
            }
        public String[] AllKeys()
            {
            return BaseGetAllKeys();
            }
        public Object[] AllValues()
            {
            return BaseGetAllValues(typeof(Object));
            }
        public void Clear()
            {
            BaseClear();
            }
        public Boolean Contains(String name)
            {
            return (IndexOfInternal(name) > -1);
            }
        public void CopyTo(Object[] array, Int32 index)
            {
            // Copies values to destination array.
            // ICollection.CopyTo copies keys to destination array.
            Object[] sourceArray = AllValues();
            sourceArray.CopyTo(array, index);
            }
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
            base.GetObjectData(info, context);
            info.AddValue("keyComparer", _keyComparer, typeof(IEqualityComparer));
            }
        public Int32 IndexOf(string name)
            {
            return IndexOfInternal(name);
            }
        public override void OnDeserialization(Object sender)
            {
            base.OnDeserialization(sender);
            if (_collectionInfo != null)
                {
                _keyComparer = _collectionInfo.GetValue("keyComparer", typeof(IEqualityComparer)) as IEqualityComparer;
                _collectionInfo = null;
                }
            }
        public void Remove(String name)
            {
            BaseRemove(name);
            }
        public void RemoveAt(Int32 index)
            {
            BaseRemoveAt(index);
            }

        }
    }
