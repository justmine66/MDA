using MDA.Shared.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDA.Shared.Utils
{
    /// <summary>
    /// 参考：https://github.com/yswenli/SAEA/blob/master/Src/SAEA.Common/SAEASerialize.cs
    /// </summary>
    public static class BinarySerializerHelper1
    {
        private static readonly Type StringType = typeof(string);

        #region Serialize

        /// <summary>
        /// len+data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static byte[] Serialize(object param)
        {
            var buffer = new List<byte>();

            var length = 0;

            byte[] data = null;

            if (param == null)
            {
                length = 0;
            }
            else
            {
                switch (param)
                {
                    case string s:
                        data = Encoding.UTF8.GetBytes(s);
                        break;
                    case byte b:
                        data = new [] { b };
                        break;
                    case bool b:
                        data = BitConverter.GetBytes(b);
                        break;
                    case short s:
                        data = BitConverter.GetBytes(s);
                        break;
                    case int i:
                        data = BitConverter.GetBytes(i);
                        break;
                    case long l:
                        data = BitConverter.GetBytes(l);
                        break;
                    case float f:
                        data = BitConverter.GetBytes(f);
                        break;
                    case double d:
                        data = BitConverter.GetBytes(d);
                        break;
                    case DateTime time:
                        {
                            var str = "wl" + time.Ticks;

                            data = Encoding.UTF8.GetBytes(str);

                            break;
                        }

                    case Enum _:
                        {
                            var enumValType = Enum.GetUnderlyingType(param.GetType());

                            if (enumValType == typeof(byte))
                            {
                                data = new[] { (byte)param };
                            }
                            else if (enumValType == typeof(short))
                            {
                                data = BitConverter.GetBytes((short)param);
                            }
                            else if (enumValType == typeof(int))
                            {
                                data = BitConverter.GetBytes((int)param);
                            }
                            else
                            {
                                data = BitConverter.GetBytes((long)param);
                            }

                            break;
                        }

                    case byte[] bytes:
                        data = bytes;
                        break;
                    default:
                        {
                            var type = param.GetType();

                            if (type.IsGenericType || type.IsArray)
                            {
                                if (TypeHelper.DicTypeStrings.Contains(type.Name))
                                    data = SerializeDic((System.Collections.IDictionary)param);
                                else if (TypeHelper.ListTypeStrings.Contains(type.Name) || type.IsArray)
                                    data = SerializeList((System.Collections.IEnumerable)param);
                                else
                                    data = SerializeClass(param, type);
                            }
                            else if (type.IsClass)
                            {
                                data = SerializeClass(param, type);
                            }

                            break;
                        }
                }

                if (data != null)
                    length = data.Length;
            }

            buffer.AddRange(BitConverter.GetBytes(length));

            if (length > 0)
            {
                buffer.AddRange(data);
            }

            return buffer.ToArray();
        }

        private static byte[] SerializeClass(object obj, Type type)
        {
            if (obj == null) return null;

            var len = 0;

            byte[] data = null;

            var ps = type.GetProperties();

            if (ps.Length > 0)
            {
                var clist = new List<object>();

                foreach (var p in ps)
                {
                    clist.Add(p.GetValue(obj, null));
                }

                data = Serialize(clist.ToArray());

                len = data.Length;
            }

            if (len > 0)
            {
                return data;
            }

            return null;
        }

        private static byte[] SerializeList(IEnumerable param)
        {
            if (param != null)
            {
                var slist = new List<byte>();
                var itemType = param.AsQueryable().ElementType;

                foreach (var item in param)
                {
                    if (itemType.IsClass && itemType != StringType)
                    {
                        var ps = itemType.GetProperties();

                        if (ps.Length > 0)
                        {
                            var clist = new List<object>();

                            foreach (var p in ps)
                            {
                                clist.Add(p.GetValue(item, null));
                            }

                            var clen = 0;
                            var cdata = Serialize(clist.ToArray());
                            if (cdata != null)
                            {
                                clen = cdata.Length;
                            }

                            slist.AddRange(BitConverter.GetBytes(clen));
                            slist.AddRange(cdata);
                        }
                    }
                    else
                    {
                        var clen = 0;
                        var cdata = Serialize(item);

                        if (cdata != null)
                        {
                            clen = cdata.Length;
                        }

                        slist.AddRange(BitConverter.GetBytes(clen));
                        slist.AddRange(cdata);
                    }
                }

                if (slist.Count > 0)
                {
                    return slist.ToArray();
                }
            }
            return null;
        }

        private static byte[] SerializeDic(IDictionary param)
        {
            if (param != null && param.Count > 0)
            {
                var list = new List<byte>();

                foreach (var item in param)
                {
                    var type = item.GetType();
                    var ps = type.GetProperties();

                    if (ps.Length > 0)
                    {
                        var clist = new List<object>();
                        foreach (var p in ps)
                        {
                            clist.Add(p.GetValue(item, null));
                        }

                        var clen = 0;

                        var cdata = Serialize(clist.ToArray());

                        if (cdata != null)
                        {
                            clen = cdata.Length;
                        }

                        if (clen > 0)
                        {
                            list.AddRange(cdata);
                        }
                    }
                }

                return list.ToArray();
            }

            return null;
        }

        /// <summary>
        /// len+data
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public static byte[] Serialize(params object[] @params)
        {
            var datas = new List<byte>();

            if (@params != null)
            {
                foreach (var param in @params)
                {
                    datas.AddRange(Serialize(param));
                }
            }

            return datas.Count == 0 ? null : datas.ToArray();
        }

        #endregion

        #region Deserialize

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="types"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static object[] Deserialize(Type[] types, byte[] datas)
        {
            var list = new List<object>();

            var offset = 0;

            foreach (var t in types)
            {
                list.Add(Deserialize(t, datas, ref offset));
            }

            return list.ToArray();
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] data)
        {
            var offset = 0;

            return (T)Deserialize(typeof(T), data, ref offset);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="datas"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static object Deserialize(Type type, byte[] datas, ref int offset)
        {
            dynamic obj = null;

            var len = 0;

            len = BitConverter.ToInt32(datas, offset);
            offset += 4;
            if (len <= 0) return obj;

            var data = new byte[len];
            Buffer.BlockCopy(datas, offset, data, 0, len);
            offset += len;

            if (type == StringType)
            {
                obj = Encoding.UTF8.GetString(data);
            }
            else if (type == typeof(byte))
            {
                obj = (data);
            }
            else if (type == typeof(bool))
            {
                obj = (BitConverter.ToBoolean(data, 0));
            }
            else if (type == typeof(short))
            {
                obj = (BitConverter.ToInt16(data, 0));
            }
            else if (type == typeof(int))
            {
                obj = (BitConverter.ToInt32(data, 0));
            }
            else if (type == typeof(long))
            {
                obj = (BitConverter.ToInt64(data, 0));
            }
            else if (type == typeof(float))
            {
                obj = (BitConverter.ToSingle(data, 0));
            }
            else if (type == typeof(double))
            {
                obj = (BitConverter.ToDouble(data, 0));
            }
            else if (type == typeof(decimal))
            {
                obj = (BitConverter.ToDouble(data, 0));
            }
            else if (type == typeof(DateTime))
            {
                var dstr = Encoding.UTF8.GetString(data);
                var ticks = long.Parse(StringHelper.Substring(dstr, 2));
                obj = (new DateTime(ticks));
            }
            else if (type.BaseType == typeof(Enum))
            {
                var numType = Enum.GetUnderlyingType(type);

                if (numType == typeof(byte))
                {
                    obj = Enum.ToObject(type, data[0]);
                }
                else if (numType == typeof(short))
                {
                    obj = Enum.ToObject(type, BitConverter.ToInt16(data, 0));
                }
                else if (numType == typeof(int))
                {
                    obj = Enum.ToObject(type, BitConverter.ToInt32(data, 0));
                }
                else
                {
                    obj = Enum.ToObject(type, BitConverter.ToInt64(data, 0));
                }
            }
            else if (type == typeof(byte[]))
            {
                obj = (byte[])data;
            }
            else if (type.IsGenericType)
            {
                if (TypeHelper.ListTypeStrings.Contains(type.Name))
                {
                    obj = DeserializeList(type, data);
                }
                else if (TypeHelper.DicTypeStrings.Contains(type.Name))
                {
                    obj = DeserializeDic(type, data);
                }
                else
                {
                    obj = DeserializeClass(type, data);
                }
            }
            else if (type.IsClass)
            {
                obj = DeserializeClass(type, data);
            }
            else if (type.IsArray)
            {
                obj = DeserializeArray(type, data);
            }
            else
            {
                throw new Exception("SAEASerialize.Deserialize 未定义的类型：" + type.ToString());
            }
            return obj;
        }

        private static object DeserializeClass(Type type, byte[] data)
        {
            var tinfo = TypeHelper.GetOrAddInstance(type);

            var instance = tinfo.Instance;

            var ts = new List<Type>();

            var ps = type.GetProperties();

            foreach (var p in ps)
            {
                ts.Add(p.PropertyType);
            }

            var vas = Deserialize(ts.ToArray(), data);

            for (var j = 0; j < ps.Length; j++)
            {
                try
                {
                    if (!ps[j].PropertyType.IsGenericType)
                    {
                        ps[j].SetValue(instance, vas[j], null);
                    }
                    else
                    {
                        var genericTypeDefinition = ps[j].PropertyType.GetGenericTypeDefinition();

                        ps[j].SetValue(instance,
                            genericTypeDefinition == typeof(Nullable<>)
                                ? Convert.ChangeType(vas[j], Nullable.GetUnderlyingType(ps[j].PropertyType))
                                : vas[j], null);
                    }
                }
                catch
                {
                    throw new Exception("SAEASerialize.Deserialize 未定义的类型：" + type.ToString());
                }
            }

            return instance;
        }

        private static object DeserializeList(Type type, byte[] datas)
        {
            var info = TypeHelper.GetOrAddInstance(type);

            var instance = info.Instance;

            if (info.ArgTypes[0].IsClass && info.ArgTypes[0] != StringType)
            {
                //子项内容
                var slen = 0;
                var soffset = 0;
                while (soffset < datas.Length)
                {
                    slen = BitConverter.ToInt32(datas, soffset);
                    if (slen > 0)
                    {
                        var sobj = Deserialize(info.ArgTypes[0], datas, ref soffset);
                        if (sobj != null)
                            info.MethodInfo.Invoke(instance, new object[] { sobj });

                    }
                    else
                    {
                        info.MethodInfo.Invoke(instance, null);
                    }
                }
                return instance;
            }
            else
            {
                //子项内容
                var slen = 0;
                var soffset = 0;
                while (soffset < datas.Length)
                {
                    var len = BitConverter.ToInt32(datas, soffset);
                    var data = new byte[len];
                    Buffer.BlockCopy(datas, soffset + 4, data, 0, len);
                    soffset += 4;
                    slen = BitConverter.ToInt32(datas, soffset);
                    if (slen > 0)
                    {
                        var sobj = Deserialize(info.ArgTypes[0], datas, ref soffset);
                        if (sobj != null)
                            info.MethodInfo.Invoke(instance, new object[] { sobj });
                    }
                    else
                    {
                        info.MethodInfo.Invoke(instance, null);
                    }
                }
                return instance;
            }

        }

        private static object DeserializeArray(Type type, byte[] datas)
        {
            var obj = DeserializeList(type, datas);

            var list = obj as List<object>;

            return list?.ToArray();
        }

        private static object DeserializeDic(Type type, byte[] datas)
        {
            var tinfo = TypeHelper.GetOrAddInstance(type);

            var instance = tinfo.Instance;

            //子项内容
            var slen = 0;

            var soffset = 0;

            var m = 1;

            object key = null;

            while (soffset < datas.Length)
            {
                slen = BitConverter.ToInt32(datas, soffset);
                var sdata = new byte[slen + 4];
                Buffer.BlockCopy(datas, soffset, sdata, 0, slen + 4);
                soffset += slen + 4;
                if (m % 2 == 1)
                {
                    object v = null;
                    if (slen > 0)
                    {
                        var lloffset = 0;
                        var sobj = Deserialize(tinfo.ArgTypes[0], sdata, ref lloffset);
                        if (sobj != null)
                            v = sobj;
                    }
                    key = v;
                }
                else
                {
                    object v = null;
                    if (slen > 0)
                    {
                        var lloffset = 0;
                        var sobj = Deserialize(tinfo.ArgTypes[1], sdata, ref lloffset);
                        if (sobj != null)
                            v = sobj;
                    }
                    var val = v;
                    tinfo.MethodInfo.Invoke(instance, new[] { key, val });
                }
                m++;
            }

            return instance;
        }

        #endregion
    }
}
