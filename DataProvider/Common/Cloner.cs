using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace DataProvider
{
    public class Cloner<S,T> where T : class, new()
    {
        private readonly static Dictionary<int, Func<S, T>> _NewMap = new Dictionary<int, Func<S, T>>();
        private readonly static Dictionary<int, Action<S, T>> _CopyMap = new Dictionary<int, Action<S, T>>();      
        /// <summary>
        /// 单例模式
        /// </summary>      
        //private static readonly EntityBuilder _Instance = new EntityBuilder();     

        private const BindingFlags _Flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty;

       
        public static T CopyNew(S source)
        {
            Func<S, T> func;
            //反射的实体类型
            Type t = typeof(T);
            Type s = typeof(S);
            //string name = entity.AssemblyQualifiedName;
            int code = t.GetHashCode() | s.GetHashCode();
            if (_NewMap.ContainsKey(code))
            {
                func = _NewMap[code];
            }
            else
            {
                func = CreateMethodToNew(typeof(S), typeof(T));
                _NewMap.Add(code, func);
            }
          
            return func(source);

        }

        public static void CopyTo(S source,T targer)
        {
            Action<S, T> func;
            //反射的实体类型
            Type t = typeof(T);
            Type s = typeof(S);
            //string name = entity.AssemblyQualifiedName;
            int code = t.GetHashCode() | s.GetHashCode();
            if (_CopyMap.ContainsKey(code))
            {
                func = _CopyMap[code];
            }
            else
            {
                func = CreateMethodToCopy(typeof(S), typeof(T));
                _CopyMap.Add(code, func);
            }

            func(source,targer);
        }

        private static Func<S,T> CreateMethodToNew(Type s, Type t)
        {
            MethodInfo set;
            //PropertyInfo prop;
            //动态方法参数列表
            Type[] dmps = new Type[] { s };
            //创建动态方法
            DynamicMethod method = new DynamicMethod(string.Empty, t, dmps, t, true);
            //生成IL对象
            ILGenerator il = method.GetILGenerator();
            //声明实体本地变量
            LocalBuilder result = il.DeclareLocal(t);
            //创建实体对象
            il.Emit(OpCodes.Newobj, t.GetConstructor(Type.EmptyTypes));
            //将实体对象放入局部变量数组中
            il.Emit(OpCodes.Stloc, result);

            foreach (var p in GetProps(s, t))
            {                                
                Label endIfLabel = il.DefineLabel();
                //将局部变量实体对象加载到栈中
                il.Emit(OpCodes.Ldloc, result);
                //动态方法的第一个参数加载到栈中
                il.Emit(OpCodes.Ldarg_0);
                //将整型变量加载到栈中
                //il.Emit(OpCodes.Ldc_I4, i);
                //调用IsNull 方法，将栈中的值弹出从左到右，赋值给方法               
                il.Emit(OpCodes.Callvirt, p.GetGetMethod());               
                //将方法的返回值进行判断
                //il.Emit(OpCodes.Brtrue, endIfLabel);
                set = t.GetMethod("set_" + p.Name, _Flags);               
              
                //将get_Item方法的返回值赋给属性
                //generator.Emit(OpCodes.Stfld, property);
                il.Emit(OpCodes.Call, set);

                il.MarkLabel(endIfLabel);
            }

            il.Emit(OpCodes.Ldloc, result);
            il.Emit(OpCodes.Ret);

            return method.CreateDelegate(typeof(Func<S, T>)) as Func<S, T>;

        }

        private static Action<S, T> CreateMethodToCopy(Type s, Type t)
        {
            MethodInfo set;
            //PropertyInfo prop;
            //动态方法参数列表
            Type[] dmps = new Type[] { s,t };
            //创建动态方法
            DynamicMethod method = new DynamicMethod(string.Empty, typeof(void), dmps);
            //生成IL对象
            ILGenerator il = method.GetILGenerator();            

            foreach (var p in GetProps(s, t))
            {
                Label endIfLabel = il.DefineLabel();
                //il.Emit(OpCodes.Stloc_0);
                //il.Emit(OpCodes.Ldloc_0);
                //il.Emit(OpCodes.Ldstr, "liuhong");
                //il.Emit(OpCodes.Unbox_Any, p.PropertyType);
                //il.Emit(OpCodes.Ldc_I4, 1);
                //动态方法的第一个参数加载到栈中，需要拷贝的参数
                il.Emit(OpCodes.Ldarg_0);            
                il.Emit(OpCodes.Callvirt, p.GetGetMethod());
                if(p.PropertyType.IsValueType)
                    il.Emit(OpCodes.Box, p.PropertyType);               
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Ceq);
                il.Emit(OpCodes.Brtrue_S, endIfLabel);
                //动态方法的第二个参数加载到栈中
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Callvirt, p.GetGetMethod());
                //将方法的返回值进行判断
                //il.Emit(OpCodes.Brtrue, endIfLabel);
                set = t.GetMethod("set_" + p.Name, _Flags);

                //将get_Item方法的返回值赋给属性
                //generator.Emit(OpCodes.Stfld, property);
                il.Emit(OpCodes.Call, set);

                il.MarkLabel(endIfLabel);
            }
            //il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Ret);

            return method.CreateDelegate(typeof(Action<S, T>)) as Action<S, T>;

        }

        private static IEnumerable<PropertyInfo> GetProps(Type s, Type t)
        {
            foreach (var prop in s.GetProperties())
            {
                if (prop.PropertyType.IsValueType || "String".Equals(prop.PropertyType.Name))
                {
                    if (t.GetProperty(prop.Name, _Flags) != null)
                        yield return prop;
                }
                #region ValueType
                //switch (prop.PropertyType.Name)
                //{
                //    case "Int16":
                //    case "Int32":
                //    case "Int64":
                //    case "String":
                //    case "Byte":
                //    case "Boolean":
                //    case "DateTime":           
                //    case "Decimal":
                //    case "Double":
                //    case "Long":
                //    case "Char":
                //        if (t.GetProperty(prop.Name, _Flags) != null)
                //            yield return prop;
                //        break;  
                //    default:
                //        continue;
                //}       
                #endregion
            }
        }
    }

    public class Creator<T> where T : class, new()
    {
        private readonly static Dictionary<int, Func<T>> _MethodMap = new Dictionary<int, Func<T>>();

        public static T New()
        {
            Func<T> func;
            //反射的实体类型
            Type t = typeof(T);     
            //string name = entity.AssemblyQualifiedName;
            int code = t.GetHashCode();
            if (_MethodMap.ContainsKey(code))
            {
                func = _MethodMap[code];
            }
            else
            {
                func = CreateMethod(typeof(T));
                _MethodMap.Add(code, func);
            }

            return func();

        }

        private static Func<T> CreateMethod(Type t)
        {
            //PropertyInfo prop;
            //动态方法参数列表
            Type[] dmps = new Type[] { };
            //创建动态方法
            DynamicMethod method = new DynamicMethod(string.Empty, t, dmps, t, true);
            //生成IL对象
            ILGenerator il = method.GetILGenerator();
            //声明实体本地变量
            LocalBuilder result = il.DeclareLocal(t);
            //创建实体对象
            il.Emit(OpCodes.Newobj, t.GetConstructor(Type.EmptyTypes));
            //将实体对象放入局部变量数组中
            //il.Emit(OpCodes.Stloc, result);            

            //il.Emit(OpCodes.Ldloc, result);
            il.Emit(OpCodes.Ret);

            return method.CreateDelegate(typeof(Func<T>)) as Func<T>;

        }
    }
}
