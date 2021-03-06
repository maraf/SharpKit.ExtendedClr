﻿using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using SharpKit.JavaScript;
using SharpKit.ExtendedClr.Compilation;
using System.Reflection;
using SharpKit;

namespace System
{
	[JsType(Name = "System.Type")]
	internal class JsImplType : JsImplMemberInfo
	{
		public static JsImplType[] EmptyTypes;

		JsImplType(JsType jsType)
		{
			_JsType = jsType;
			_Name = _JsType.name;
			if (EmptyTypes == null)
				EmptyTypes = new JsImplType[0];
		}

        [JsMethod(Name = "op_Equality$$Type$$Type")]
        public static bool operator ==(JsImplType left, JsImplType right)
        {
            if ((object)left == null)
            {
                if ((object)right == null)
                    return true;
                else
                    return false;
            }
            else if ((object)right == null)
            {
                return false;
            }

            return left.FullName == right.FullName;
        }

        [JsMethod(Name = "op_Inequality$$Type$$Type")]
        public static bool operator !=(JsImplType left, JsImplType right)
        {
            return !(left == right);
        }

		public bool IsEnum
		{
			get
			{
				return _JsType.Kind==JsTypeKind.Enum;
			}
		}

        public bool IsClass
        {
            get
            {
                return _JsType.Kind == JsTypeKind.Class;
            }
        }

        public bool IsAbstract
        {
            get
            {
                return false;
            }
        }

        public bool IsInterface
        {
            get
            {
                return _JsType.Kind == JsTypeKind.Interface;
            }
        }

		protected override MemberInfo GetBaseMember()
		{
			return BaseType;
		}
		internal JsType _JsType;

		//[JsMethod(Code = "return Type.GetType(name);")]
		public static Type GetType(string name)
		{
            int commaIndex = name.IndexOf(',');
            if (commaIndex != -1)
                name = name.Substring(0, commaIndex);

			return GetType(name, false);
		}

		public static Type GetType(string name, bool throwOnError)
		{
			if (JsTypeHelper._HasTypeArguments(name))
			{
                var jsTypeAndArgs = JsTypeHelper._GetTypeWithArguments(name, throwOnError);
				if (jsTypeAndArgs == null)
					return null;
				var genericType = JsCompilerGlobal._TypeOf(jsTypeAndArgs[0].As<JsType>());
				var jsTypeArgs = jsTypeAndArgs[1].As<JsExtendedArray>();
				for (var i = 0; i < jsTypeArgs.length; i++)
				{
					jsTypeArgs[i] = JsCompilerGlobal._TypeOf(jsTypeArgs[i].As<JsType>());
				}
				var type = genericType.As<JsImplType>()._MakeGenericType(jsTypeArgs);
				return type;
			}
			var jsType = JsTypeHelper.GetType(name);
			if (jsType == null)
			{
				if(throwOnError)
					throw new Exception("Type " + name + " was not found");
				return null;
			}
			return JsCompilerGlobal._TypeOf(jsType);
		}
		public static Type GetType(string name, bool throwOnError, bool ignoreCase)
		{
			if (ignoreCase)
				throw new NotImplementedException();
			return GetType(name, throwOnError);
		}

		public Type BaseType
		{
			get
			{
				if (_JsType.baseType == null)
					return null;
				return JsCompilerGlobal._TypeOf(_JsType.baseType);
			}
		}

		#region PropertyInfo
		
		[JsMethod(NativeOverloads = true)]
		void FillProperties(JsObject def)
		{
			foreach (var funcName in def)
			{
				var isGetter = funcName.startsWith("get_");
				var isSetter = funcName.startsWith("set_");
				if(isGetter || isSetter)
					FillProperty(def, funcName, isSetter);
			}

		}

		private void FillProperty(JsObject def, string funcName, bool isSetter)
		{
			var member = def[funcName];
			var func = member.As<JsFunction>();
			var propName = funcName.Substring(4);
			var prop = _PropertiesByName[propName].As<JsImplPropertyInfo>();
			if (prop == null)
			{
				prop = new JsImplPropertyInfo();
				_PropertiesByName[propName] = prop;
				_Properties.push(prop);
				prop._Name = propName;
				prop._DeclaringType = this;
				prop._IsStatic = _JsType.staticDefinition != null && _JsType.staticDefinition[funcName] != null;
				var propTypeName = JsTypeHelper.getMemberTypeName(def, propName);
				if (propTypeName != null)
					prop._PropertyType = GetType(propTypeName).As<JsImplType>();
				else
				{
					//TODO: Fix this! Nullable<float> doenst work
				}
			}
			if (isSetter)
				prop._Setter = func;
			else
				prop._Getter = func;
		}

		bool TryFillProperty(JsObject def, string name)
		{
			if (def == null)
				return false;
			var getterName = "get_"+name;
			var setterName = "set_"+name;
			if (def.hasOwnProperty(getterName))
				FillProperty(def, getterName, false);
			if (def.hasOwnProperty(setterName))
				FillProperty(def, setterName, true);
			return _PropertiesByName[name] != null;
		}

		public JsImplPropertyInfo GetProperty(string name)
		{
			VerifyProperty(name);
			return _PropertiesByName[name].As<JsImplPropertyInfo>();
		}
        public JsImplPropertyInfo GetProperty(string name, JsImplBindingFlags bindingAttr)
        {
            //TODO: Implement properly
            if (name == null)
                throw new ArgumentNullException("name");
            return this.GetProperty(name);
        }

        internal JsImplPropertyInfo GetProperty(string name, Type returnType)
        {
            throw new NotImplementedException();
        }

		bool allPropertiesVerified;
		void VerifyProperty(string name)
		{
			if (_PropertiesByName == null)
			{
				_PropertiesByName = new JsObject();
				_Properties = new JsExtendedArray();
			}
			if (_PropertiesByName.hasOwnProperty(name))
				return;
			if (TryFillProperty(_JsType.definition, name))
				return;
			if (TryFillProperty(_JsType.staticDefinition, name))
				return;
			var baseType = BaseType;
			if (baseType != null)
			{
				var pe = baseType.GetProperty(name);
                if (pe != null)
                {
                    _PropertiesByName[name] = pe;
                    _Properties.push(pe);
                    return;
                }
			}
			_PropertiesByName[name] = null;//mark as non existent
		}
		void VerifyProperties()
		{
			if(!allPropertiesVerified)
			{
				allPropertiesVerified = true;
				if (_PropertiesByName == null)
				{
					_PropertiesByName = new JsObject();
					_Properties = new JsExtendedArray();
				}
				FillProperties(_JsType.definition);
				FillProperties(_JsType.staticDefinition);
				var baseType = BaseType;
				if (baseType != null)
				{
                    var props = baseType.GetProperties();
					foreach (var pe in props)
					{
                        string name = pe.As<JsImplPropertyInfo>()._Name;
						if (!_PropertiesByName.hasOwnProperty(name))
						{
							_PropertiesByName[name] = pe;
							_Properties.push(pe);
						}
					}
				}
			}
		}

		JsObject _PropertiesByName;
		JsExtendedArray _Properties;
		public JsImplPropertyInfo[] GetProperties()
		{
			VerifyProperties();
			return _Properties.As<JsImplPropertyInfo[]>();
		}

		#endregion

		#region MethodInfo

		void FillMethods(JsObject def)
		{
			var isStatic = def == _JsType.staticDefinition;
			foreach (var funcName in def)
			{
				if (funcName == "toString")
					continue;
				var func = def[funcName].As<JsFunction>();
				if (Js.Typeof(func) != "function")
					continue;
				var methodName = JsNamingHelper.JsFunctionNameToClrMethodName(funcName);
				var methods = _MethodsByName[methodName];
				if (methods == null)
				{
                    methods = new JsArray<MethodInfo>();
					_MethodsByName[methodName] = methods;
				}
				//TODO: check overrides and parameters
				var method = new JsImplMethodInfo();
				methods.push(method.As<MethodInfo>());
				_Methods.push(method);
				method._Name = methodName;
				method.JsName = funcName;
				method.JsFunction = func;
				method._DeclaringType = this;
				method._IsStatic = _JsType.staticDefinition != null && _JsType.staticDefinition[funcName] == func;
				//var propType = VM.resolveMemberType2(def, propName);
				//if (propType != null)
				//  prop._MethodType = _TypeOf(propType);
			}

		}

        [JsMethod(Name = "GetMethod$$String")]
		public JsImplMethodInfo GetMethod(string name)
		{
			VerifyMethods();
			var methods = _MethodsByName[name].As<JsExtendedArray>();
			if (methods == null)
				return null;
			return methods[0].As<JsImplMethodInfo>();
		}

		void VerifyMethods()
		{
			if (_MethodsByName == null)
			{
				_MethodsByName = new JsObject<JsArray<MethodInfo>>();
				_Methods = new JsExtendedArray();
				FillMethods(_JsType.definition);
				FillMethods(_JsType.staticDefinition);
				var baseType = BaseType;
				if (baseType != null)
				{
                    var methods = baseType.GetMethods();
					foreach (var me in methods)
                    {
                        if (_JsType.definition != null && _JsType.definition.hasOwnProperty(me.As<JsImplMethodInfo>().JsName))
                            continue;
                        if (_JsType.staticDefinition != null && _JsType.staticDefinition.hasOwnProperty(me.As<JsImplMethodInfo>().JsName))
                            continue;
                        var list = _MethodsByName[me.As<JsImplMethodInfo>()._Name];
                        if (list == null)
                        {
                            list = new JsArray<MethodInfo>();
                            _MethodsByName[me.As<JsImplMethodInfo>()._Name] = list;
                        }
                        list.push(me);
                        _Methods.push(me);
					}
				}
			}
		}

        JsObject<JsArray<MethodInfo>> _MethodsByName;
		JsExtendedArray _Methods;
		public JsImplMethodInfo[] GetMethods()
		{
			VerifyMethods();
			var arr = new JsArray();
			for (var i = 0; i < _Methods.length; i++)
			{
				arr.push(_Methods[i]);
			}
			return arr.As<JsImplMethodInfo[]>();
		}

		JsImplMethodInfo[] GetMethods(string name)
		{
			VerifyMethods();
			return _MethodsByName[name].As<JsImplMethodInfo[]>();
		}

		JsImplMethodInfo[] GetMethods(JsImplBindingFlags bindingFlags)
		{
			//TODO:
			return GetMethods();
		}

        void VerifyConstructors()
        {
            if (_Constructors == null)
            {
                VerifyMethods();
                _Constructors = new JsExtendedArray();
                for (int i = 0; i < _JsType.ctors.As<JsArray>().length; i++)
                {
                    JsObject ctorMeta = _JsType.ctors[i.As<string>()].As<JsObject>();
                    string ctorName = ctorMeta["name"].As<string>();
                    JsArray<string> ctorParams = ctorMeta["parameters"].As<JsArray<string>>();
                    JsFunction ctorFunc = ctorMeta["_type"].As<JsObject>()[ctorName].As<JsFunction>();

                    var method = new JsImplConstructorInfo(ctorName, ctorFunc, ctorParams);
                    method._DeclaringType = this;
                    //method.JsName = funcName;
                    //method.JsFunction = func;
                    _Constructors.push(method);
                }
            }
        }

        JsExtendedArray _Constructors;
        JsImplConstructorInfo[] GetConstructors()
        {
            VerifyConstructors();
            var arr = new JsArray();
            for (var i = 0; i < _Constructors.length; i++)
            {
                arr.push(_Constructors[i]);
            }
            return arr.As<JsImplConstructorInfo[]>();
        }

		#endregion

		protected override void VerifyCustomAttributes()
		{
			VerifyCustomAttributesOnTypeAndMembers();
		}

		public override string Name
		{
			get { return _JsType.GetName(); }
		}
		public string FullName
		{
			get { return _JsType.fullname; }
		}

        public string AssemblyQualifiedName
		{
			get { return _JsType.GetAssemblyQualifiedName(); }
		}

        private JsImplAssembly assembly;

        internal JsImplAssembly Assembly
        {
            get
            {
                if (assembly == null)
                    assembly = new JsImplAssembly(new JsImplAssemblyName() { Name = _JsType.assemblyName });

                return assembly;
            }
        }

        bool verifiedCustomAttributesOnTypeAndMembers;
		internal void VerifyCustomAttributesOnTypeAndMembers()
		{
			if(verifiedCustomAttributesOnTypeAndMembers)
				return;
			verifiedCustomAttributesOnTypeAndMembers = true;
			if (_JsType.customAttributes == null)
				return;
			for (var i = 0; i < _JsType.customAttributes.length; i++)
			{
				var attDef = _JsType.customAttributes[i].As<JsAttribute>();
                var attType = JsTypeHelper.GetType(attDef.typeName);
				var jsCtor = attType.As<JsObject>()[attDef.ctorName].As<JsFunction>();
				var att = Js.ApplyNew(jsCtor, attDef.positionalArguments);
				if (attDef.namedArguments != null)
				{
					foreach (string propName in attDef.namedArguments)
					{
						var value = attDef.namedArguments[propName];
						if (Js.Typeof(value) == "function")
							value = value.As<JsFunction>().call(null);
						att.As<JsObject>()["set_" + propName].As<JsFunction>().call(att, value);
					}
				}
				var target = GetAttributeTarget(attDef.targetType, attDef.targetMemberName);
				if (target._CustomAttributes == null)
					target._CustomAttributes = new JsExtendedArray();
				target._CustomAttributes.push(att);
			}
		}

		private JsImplMemberInfo GetAttributeTarget(string memberType, string memberName)
		{
			if (memberType == "type")
				return this;
			else if (memberType == "method")
			{
				var methodName = JsNamingHelper.JsFunctionNameToClrMethodName(memberName);
                var methods = GetMethods(methodName);
				foreach (var method in methods)
				{
					if (method.JsName == memberName) //TODO: optimize?
						return method;
				}
				return null;
			}
			else if (memberType == "property")
			{
				return GetProperty(memberName);
			}
			else
				throw new NotImplementedException("GetAttributeTarget not supported yet for memberType: "+memberType);
		}


        [JsMethod(Code = @"if (type._Name == Typeof(System.Nullable$1)._Name && type._TypeArguments != null) {
                return TypeIs(type._TypeArguments[0]._JsType, this._JsType);
            } else if (this._Name == Typeof(System.Nullable$1)._Name && this._TypeArguments != null) {
                return TypeIs(type._JsType, this._TypeArguments[0]._JsType);
            }

            return TypeIs(type._JsType, this._JsType);")]
		public bool IsAssignableFrom(JsImplType type)
		{
			throw new NotImplementedException();
		}
		[JsMethod(Code="return Is(obj, this._JsType);")]
		public bool IsInstanceOfType(object obj)
		{
			throw new NotImplementedException();
		}


		public override string ToString()
		{
			return String.Format("{Name = "+Name+" FullName = "+FullName+"}");
		}

		//public JsImplType GetInterface(string name)
		//{
		//  var jsType = _JsType;
		//  var checkedInterfaces = new JsObject();
		//  while (jsType != null)
		//  {
		//    var interfaces = this._JsType.interfaces;
		//    for (var i = 0; i < interfaces.length; i++)
		//    {
		//      var iface = interfaces[i].As<JsType>();
		//      if (iface.name == name)
		//        return _TypeOf(iface);
		//      checkedInterfaces[iface.name] = true;
		//    }
		//    jsType = jsType.baseType;
		//  }
		//  return null;
		//}

		//JsImplType GetInterface(JsType jsType, string name, JsObject checkedInterfaces)
		//{
		//  while (jsType != null)
		//  {

		//    var interfaces = this._JsType.interfaces;
		//    for (var i = 0; i < interfaces.length; i++)
		//    {
		//      var iface = interfaces[i].As<JsType>();
		//      if (iface.name == name)
		//        return _TypeOf(iface);
		//      checkedInterfaces[iface.name] = true;
		//    }
		//    jsType = jsType.baseType;
		//  }
		//  return null;

		//}


		[JsMethod(Code=@"
var x = []; 
for(var i=0;i<arguments.length;i++) 
	x.push(arguments[i]|| null);
return this._MakeGenericType(x);")]
		public JsImplType MakeGenericType(params Type[] typeArguments)
		{
			throw new NotImplementedException();
		}

		public JsImplType GetGenericTypeDefinition()
		{
			return _GenericTypeDefinition;
		}
		JsImplType _GenericTypeDefinition;

		public JsImplType[] GetGenericArguments()
		{
			if (_TypeArguments == null)
				return JsImplType.EmptyTypes;
			return _TypeArguments.As<JsImplType[]>();
		}

		JsExtendedArray _TypeArguments;

		JsObject _MakeGenericTypeCache;
		[JsMethod(NativeOverloads = true)]
		Type _MakeGenericType(JsExtendedArray typeArguments)
		{
			if (_MakeGenericTypeCache == null)
				_MakeGenericTypeCache = new JsObject();
			var key = "";
			for (var i = 0; i < typeArguments.length; i++)
			{
				var typeArg = typeArguments[i].As<JsImplType>();
				key += typeArg._Name;
			}
			var t = _MakeGenericTypeCache[key].As<JsImplType>();
			if (t == null)
			{
				t = new JsImplType(_JsType);
				_MakeGenericTypeCache[key] = t;
				t._Name = _Name;
				t._GenericTypeDefinition = this;
				t._TypeArguments = typeArguments;
				t._Properties = _Properties;
				t._PropertiesByName = _PropertiesByName;
				t._Methods = _Methods;
				t._MethodsByName = _MethodsByName;
				t._DeclaringType = _DeclaringType;
				t._CustomAttributes = _CustomAttributes;
			}
			return t.As<Type>();
		}

	}


}
