﻿using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


namespace System.Reflection
{



	[JsType(Name = "System.Reflection.MemberInfo")]
	internal abstract class JsImplMemberInfo
	{
		internal string _Name;
		public virtual string Name
		{
			get { return _Name; }
		}

		internal JsImplType _DeclaringType;
		public JsImplType DeclaringType
		{
			get
			{
				return _DeclaringType;
			}
		}


		protected virtual void VerifyCustomAttributes()
		{
			DeclaringType.VerifyCustomAttributesOnTypeAndMembers();
		}

		/// <summary>
		/// Returns the base member for custom attribute inheritance
		/// </summary>
		/// <returns></returns>
		protected virtual MemberInfo GetBaseMember()
		{
			return null;//TODO: implement for MethodInfo, EventInfo as needed.
		}

		private void AddCustomAttributes(List<object> list, JsImplType attributeType, bool inherit)
		{
			VerifyCustomAttributes();
			if (_CustomAttributes != null)
			{
				for (var i = 0; i < _CustomAttributes.length; i++)
				{
					var att = _CustomAttributes[i];
					if (attributeType.IsInstanceOfType(att))
						list.Add(att);
				}
			}
			if (inherit)
			{
				var bm = GetBaseMember();
				if (bm != null)
					bm.As<JsImplMemberInfo>().AddCustomAttributes(list, attributeType, inherit);
			}
		}

		public object[] GetCustomAttributes(JsImplType attributeType, bool inherit)
		{
			var list = new List<object>();
			AddCustomAttributes(list, attributeType, inherit);
			return list.ToArray();
		}

		public object[] GetCustomAttributes(bool inherit)
		{
            // Optimisticky to budeme přehlížet
            //if (inherit)
            //    throw new NotImplementedException("GetCustomAttributes with inherit=true is not implemented");
            
            VerifyCustomAttributes();

            if (this._CustomAttributes == null)
                this._CustomAttributes = new JsExtendedArray();

            return _CustomAttributes.As<object[]>();
		}

		internal JsExtendedArray _CustomAttributes;

	}


	[JsType(Name = "System.Reflection.MethodBase")]
	internal abstract class JsImplMethodBase : JsImplMemberInfo
	{
		public abstract object Invoke(object obj, object[] parameters);
	}


	[JsType(Name = "System.Reflection.MethodInfo")]
	internal class JsImplMethodInfo : JsImplMethodBase
	{
		internal bool _IsStatic;

        public bool IsStatic { get { return _IsStatic; } }
		public override object Invoke(object obj, object[] parameters)
		{
			JsFunction func;
			if (_IsStatic)
				func = JsFunction;
			else
			{
				if (obj == null)
					throw new Exception("Cannot invoke non static method without a target object");
				func = obj.As<JsObject>()[JsName].As<JsFunction>();//to support runtime overrides (as in .net)
			}
			object res;
			if (parameters == null)
				res = func.apply(obj);
			else
				res = func.apply(obj, parameters);
			return res;
		}
		internal JsFunction JsFunction=null;
		internal string JsName=null;

	}

    [JsType(Name = "System.Reflection.ConstructorInfo")]
    internal class JsImplConstructorInfo : JsImplMethodBase
    {
        private JsFunction func;
        private JsArray<string> parameters;

        public JsImplConstructorInfo(string methodName, JsFunction func, JsArray<string> parameters)
        {
            _Name = methodName;
            this.func = func;
            this.parameters = parameters;
        }

        [JsMethod(Name = "Invoke$$Object$Array", Code = @"
            var constructor = this.func;
            var thisValue = Object.create(constructor.prototype);
            var result = constructor.apply(thisValue, parameters);
            if (typeof result === 'object' && result !== null) {
                return result;
            }
            return thisValue;
        ")]
        public object Invoke(object[] parameters)
        {
            return null;
        }

        private JsArray<JsImplParameterInfo> _parameterInfo = null;
        public JsImplParameterInfo[] GetParameters()
        {
            if (_parameterInfo == null)
            {
                _parameterInfo = new JsArray<JsImplParameterInfo>();
                foreach (string paramTypeName in parameters)
                    _parameterInfo.Add(new JsImplParameterInfo(Type.GetType(paramTypeName)));
            }
            return _parameterInfo;
        }

        public override object Invoke(object obj, object[] parameters)
        {
            throw new NotSupportedException();
        }

        [JsMethod(Name = "op_Equality$$ConstructorInfo$$ConstructorInfo")]
        public static bool operator ==(JsImplConstructorInfo info1, JsImplConstructorInfo info2)
        {
            if ((object)info1 == null)
            {
                if ((object)info2 == null)
                    return true;
                else
                    return false;
            }
            else if ((object)info2 == null)
            {
                return false;
            }

            return info1.GetParameters() == info2.GetParameters();
        }

        [JsMethod(Name = "op_Inequality$$ConstructorInfo$$ConstructorInfo")]
        public static bool operator !=(JsImplConstructorInfo info1, JsImplConstructorInfo info2)
        {
            return !(info1 == info2);
        }
    }

    [JsType(Name = "System.Reflection.ParameterInfo")]
    internal class JsImplParameterInfo
    {
        public Type ParameterType { get; private set; }

        public JsImplParameterInfo(Type parameterType)
        {
            ParameterType = parameterType;
        }
    }

}
