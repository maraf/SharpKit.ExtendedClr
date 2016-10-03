/* Generated by SharpKit 5 v5.4.4 */
if (typeof($CreateException)=='undefined') 
{
    var $CreateException = function(ex, error) 
    {
        if(error==null)
            error = new Error();
        if(ex==null)
            ex = new System.Exception.ctor();       
        error.message = ex.message;
        for (var p in ex)
           error[p] = ex[p];
        return error;
    }
}


var AfterCompilationFunctions =  [];
var BeforeCompilationFunctions =  [];
var IsCompiled = false;
function RemoveDelegate(delOriginal, delToRemove){
    if (delToRemove == null || delOriginal == null)
        return delOriginal;
    if (delOriginal.isMulticastDelegate){
        if (delToRemove.isMulticastDelegate)
            throw $CreateException(new System.NotImplementedException.ctor$$String("Multicast to multicast delegate removal is not implemented yet"), new Error());
        var del = CreateMulticastDelegateFunction();
        for (var i = 0; i < delOriginal.delegates.length; i++){
            var del2 = delOriginal.delegates[i];
            if (del2 != delToRemove){
                if (del.delegates == null)
                    del.delegates =  [];
                del.delegates.push(del2);
            }
        }
        if (del.delegates == null)
            return null;
        if (del.delegates.length == 1)
            return del.delegates[0];
        return del;
    }
    else {
        if (delToRemove.isMulticastDelegate)
            throw $CreateException(new System.NotImplementedException.ctor$$String("single to multicast delegate removal is not supported"), new Error());
        if (delOriginal == delToRemove)
            return null;
        return delOriginal;
    }
};
function CombineDelegates(del1, del2){
    if (del1 == null)
        return del2;
    if (del2 == null)
        return del1;
    var del = CreateMulticastDelegateFunction();
    del.delegates =  [];
    if (del1.isMulticastDelegate){
        for (var i = 0; i < del1.delegates.length; i++)
            del.delegates.push(del1.delegates[i]);
    }
    else {
        del.delegates.push(del1);
    }
    if (del2.isMulticastDelegate){
        for (var i = 0; i < del2.delegates.length; i++)
            del.delegates.push(del2.delegates[i]);
    }
    else {
        del.delegates.push(del2);
    }
    return del;
};
function CreateMulticastDelegateFunction(){
    var del2 = null;
    var del = function (){
        var x = undefined;
        for (var i = 0; i < del2.delegates.length; i++){
            var del3 = del2.delegates[i];
            x = del3.apply(null, arguments);
        }
        return x;
    };
    del.isMulticastDelegate = true;
    del2 = del;
    return del;
};
function CreateClrDelegate(type, genericArgs, target, func){
    return JsTypeHelper.GetDelegate(target, func);
};
function Typeof(jsTypeOrName){
    if (jsTypeOrName == null)
        throw $CreateException(new Error("Unknown type."), new Error());
    if (typeof(jsTypeOrName) == "function"){
        jsTypeOrName = JsTypeHelper.GetType(jsTypeOrName);
    }
    if (typeof(jsTypeOrName) == "string")
        return System.Type.GetType$$String$$Boolean(jsTypeOrName, true);
    return _TypeOf(jsTypeOrName);
};
function _TypeOf(jsType){
    if (jsType == null)
        throw $CreateException(new System.Exception.ctor$$String("Cannot resovle type"), new Error());
    if (jsType._ClrType == null)
        jsType._ClrType = CreateClrType(jsType);
    return jsType._ClrType;
};
function CreateClrType(jsType){
    return new System.Type.ctor(jsType);
};
function JsTypeof(typeName){
    return JsTypeHelper.GetType(typeName, false);
};
function New(typeName, args){
    var type = JsTypeHelper.GetType(typeName, true);
    if (args == null || args.length == 0){
        var obj = SharpKit.ExtendedClr.Compilation.JsCompiler.NewByFunc(type.ctor);
        return obj;
    }
    else {
        var obj = SharpKit.ExtendedClr.Compilation.JsCompiler.NewByFuncArgs(type.ctor, args);
        return obj;
    }
};
function NewWithInitializer(type, json){
    var obj = SharpKit.ExtendedClr.Compilation.JsCompiler.NewByFunc(type.ctor);
    if (typeof(json) == "array"){
        throw $CreateException(new System.Exception.ctor$$String("not implemented"), new Error());
    }
    else {
        for (var p in json){
            var setter = obj["set_" + p];
            if (typeof(setter) == "function")
                setter.call(obj, json[p]);
            else
                obj[p] = json[p];
        }
    }
    return obj;
};
function As(obj, typeOrName){
    if (obj == null)
        return obj;
    var type = JsTypeHelper.GetType(typeOrName, true);
    if (Is(obj, type))
        return obj;
    return null;
};
function Cast(obj, typeOrName){
    if (obj == null)
        return obj;
    var type = JsTypeHelper.GetType(typeOrName, true);
    if (Is(obj, type))
        return obj;
    var converted = TryImplicitConvert(obj, type);
    if (converted != null)
        return converted;
    var objTypeName = typeof(obj);
    if (typeof(obj.getTypeName) == "function"){
        objTypeName = obj.getTypeName();
    }
    var msg = new Array("InvalidCastException: Cannot cast ", objTypeName, " to ", type.fullname, "Exception generated by JsRuntime").join("");
    throw $CreateException(new Error(msg), new Error());
};
function _TestTypeInterfacesIs(testType, iface, testedInterfaces){
    if (testedInterfaces[iface.name])
        return false;
    for (var i = 0; i < testType.interfaces.length; i++){
        var testIface = testType.interfaces[i];
        if (testIface == iface)
            return true;
        testedInterfaces[testIface.name] = true;
        if (_TestTypeInterfacesIs(testIface, iface, testedInterfaces))
            return true;
    }
    return false;
};
function TypeIs(objType, type){
    if (objType == type)
        return true;
    if (type.Kind == "Interface"){
        var testedInterfaces = new Object();
        while (objType != null){
            if (objType == type)
                return true;
            if (_TestTypeInterfacesIs(objType, type, testedInterfaces))
                return true;
            objType = objType.baseType;
        }
        return false;
    }
    if (type.Kind == "Delegate" && objType.fullname == "System.Delegate"){
        return true;
    }
    if (objType.fullname == "System.Int32"){
        if (type.fullname == "System.Decimal")
            return true;
        if (type.fullname == "System.Double")
            return true;
        if (type.fullname == "System.Single")
            return true;
        if (type.fullname == "System.Nullable$1")
            return true;
    }
    var t = objType.baseType;
    while (t != null){
        if (t == type)
            return true;
        t = t.baseType;
    }
    return false;
};
function Is(obj, typeOrName){
    if (obj == null){
        return false;
    }
    var type = JsTypeHelper.GetType(typeOrName, true);
    if (type == null){
        if (type == null && typeof(typeOrName) == "function"){
            var ctor = typeOrName;
            var i = 0;
            while (ctor != null && i < 20){
                if (obj instanceof ctor)
                    return true;
                ctor = ctor["$baseCtor"];
                i++;
            }
            return false;
        }
        throw $CreateException(new Error("type expected"), new Error());
    }
    if (type.Kind == "Enum"){
        var valueCollection = (type.staticDefinition != null ? type.staticDefinition : type);
        for (var key in valueCollection){
            if (valueCollection[key] == obj)
                return true;
        }
    }
    var objType = GetObjectType(obj);
    if (objType == null)
        return false;
    var isIt = TypeIs(objType, type);
    return isIt;
};
function Default(T){
    if (T == undefined)
        return null;
    var type = Typeof(T);
    var jsType = System.TypeExtensions.GetJsType(type);
    if (jsType != null && jsType != null && (jsType.Kind == "Struct" || (jsType.baseTypeName != null && jsType.baseTypeName.indexOf("ValueType") != -1))){
        switch (type.get_Name()){
            case "Boolean":
                return false;
            case "Byte":
            case "Int16":
            case "Int32":
            case "Int64":
            case "Double":
            case "Decimal":
                return null;
            default:
                return System.Activator.CreateInstance$$Type(jsType);
        }
    }
    return null;
};
function GetObjectType(obj){
    	var objType;	
	if(
			obj.constructor==null ||  //IE
			obj instanceof Node || //FireFox
			obj.constructor==HTMLImageElement || obj.constructor==HTMLInputElement ||								//IE & Firefox
			obj.constructor.name=='HTMLImageElement' || obj.constructor.name=='HTMLInputElement' 		//IE & Safari
		 )
	{
		var objTypeName = SharpKit.Html4.HtmlDom.GetTypeNameFromHtmlNode(obj);
		if(objTypeName==null)
			throw new Error();
		objType = JsTypeHelper.GetType(objTypeName, true);
	}
	else
	{
		objType = obj.constructor._type;
	}
	return objType === undefined ? null : objType;

};
function TryImplicitConvert(obj, type){
    	if (obj instanceof Error)
	{
		if (obj._Exception != null)
		{
			if(Is(obj._Exception, type))
				return obj._Exception;
			else
				return null;
		}
		else if (type.get_FullName() == 'System.Exception')
		{
			obj._Exception = new Exception(obj.message);
			return obj._Exception;
		}
	}
	return null;
};
function Compile(){
    SharpKit.ExtendedClr.Compilation.JsCompiler.Compile_Direct();
};
function AfterCompilation(func){
    if (IsCompiled)
        func();
    else
        AfterCompilationFunctions.push(func);
};
function AfterNextCompilation(func){
    AfterCompilationFunctions.push(func);
};
function BeforeCompilation(func){
    BeforeCompilationFunctions.push(func);
};
if (typeof(JsTypes) == "undefined")
    var JsTypes = [];
var System$ArrayExtensions = {
    fullname: "System.ArrayExtensions",
    baseTypeName: "System.Object",
    staticDefinition: {
        IsNullOrEmpty: function (array){
            return array==null || array.length==0;
        },
        IsNotNullOrEmpty: function (array){
            return array!=null && array.length>0;
        }
    },
    assemblyName: "SharpKit.ExtendedClr.Compilation",
    Kind: "Class",
    definition: {
        ctor: function (){
            System.Object.ctor.call(this);
        }
    },
    ctors: [],
    IsAbstract: true
};
JsTypes.push(System$ArrayExtensions);
var System$TypeExtensions = {
    fullname: "System.TypeExtensions",
    baseTypeName: "System.Object",
    staticDefinition: {
        GetJsType: function (type){
            return type._JsType;
        }
    },
    assemblyName: "SharpKit.ExtendedClr.Compilation",
    Kind: "Class",
    definition: {
        ctor: function (){
            System.Object.ctor.call(this);
        }
    },
    ctors: [],
    IsAbstract: true
};
JsTypes.push(System$TypeExtensions);
if (typeof(SharpKit) == "undefined")
    var SharpKit = {};
if (typeof(SharpKit.ExtendedClr) == "undefined")
    SharpKit.ExtendedClr = {};
if (typeof(SharpKit.ExtendedClr.Compilation) == "undefined")
    SharpKit.ExtendedClr.Compilation = {};
SharpKit.ExtendedClr.Compilation.JsCompiler = function (){
};
SharpKit.ExtendedClr.Compilation.JsCompiler._NewJsTypes =  [];
SharpKit.ExtendedClr.Compilation.JsCompiler.NewTypes =  [];
SharpKit.ExtendedClr.Compilation.JsCompiler.AfterCompile =  [];
SharpKit.ExtendedClr.Compilation.JsCompiler.__LastException = null;
SharpKit.ExtendedClr.Compilation.JsCompiler.Types = new Object();
SharpKit.ExtendedClr.Compilation.JsCompiler._hashKeyIndex = 0;
SharpKit.ExtendedClr.Compilation.JsCompiler._hashKeyPrefix = String.fromCharCode(1);
SharpKit.ExtendedClr.Compilation.JsCompiler.Compile_Direct = function (){
    SharpKit.ExtendedClr.Compilation.JsCompiler.Compile_Phase1();
    SharpKit.ExtendedClr.Compilation.JsCompiler.Compile_Phase2();
    SharpKit.ExtendedClr.Compilation.JsCompiler.Compile_Phase3();
};
SharpKit.ExtendedClr.Compilation.JsCompiler.Compile_Phase1 = function (){
    for (var $i2 = 0,$l2 = BeforeCompilationFunctions.length,action = BeforeCompilationFunctions[$i2]; $i2 < $l2; $i2++, action = BeforeCompilationFunctions[$i2])
        action();
    BeforeCompilationFunctions =  [];
    for (var $i3 = 0,$l3 = JsTypes.length,jsType = JsTypes[$i3]; $i3 < $l3; $i3++, jsType = JsTypes[$i3]){
        var fullName = jsType.fullname;
        var type = SharpKit.ExtendedClr.Compilation.JsCompiler.Types[fullName];
        if (type == null){
            SharpKit.ExtendedClr.Compilation.JsCompiler.Types[fullName] = jsType;
        }
        else {
            jsType.isPartial = true;
            jsType.realType = type;
        }
        if (jsType.derivedTypes == null)
            jsType.derivedTypes =  [];
        if (jsType.interfaces == null)
            jsType.interfaces =  [];
        if (jsType.definition == null)
            jsType.definition = new Object();
        var index = fullName.lastIndexOf(".");
        if (index == -1){
            jsType.name = fullName;
        }
        else {
            jsType.name = fullName.substring(index + 1);
            jsType.ns = fullName.substring(0, index);
        }
        if (jsType.Kind == "Enum"){
            if (jsType.baseTypeName == null)
                jsType.baseTypeName = "System.Object";
            if (jsType.definition["toString"] ==  Object.prototype.toString)
                jsType.definition["toString"] = new Function("return this._Name;");
        }
        else if (jsType.Kind == "Struct"){
            if (jsType.baseTypeName == null)
                jsType.baseTypeName = "System.ValueType";
        }
    }
};
SharpKit.ExtendedClr.Compilation.JsCompiler.Compile_Phase2 = function (){
    SharpKit.ExtendedClr.Compilation.JsCompiler._NewJsTypes =  [];
    for (var i = 0; i < JsTypes.length; i++){
        var jsType = JsTypes[i];
        SharpKit.ExtendedClr.Compilation.JsCompiler.Compile_Phase2_TmpType(jsType);
    }
    for (var $i4 = 0,$l4 = JsTypes.length,ce = JsTypes[$i4]; $i4 < $l4; $i4++, ce = JsTypes[$i4]){
        if (ce.cctor != null)
            ce.cctor();
    }
    JsTypes =  [];
    SharpKit.ExtendedClr.Compilation.JsCompiler.NewTypes =  [];
    for (var $i5 = 0,$t5 = SharpKit.ExtendedClr.Compilation.JsCompiler._NewJsTypes,$l5 = $t5.length,jsType = $t5[$i5]; $i5 < $l5; $i5++, jsType = $t5[$i5])
        SharpKit.ExtendedClr.Compilation.JsCompiler.NewTypes.push(System.Type.GetType$$String(jsType.fullname));
};
SharpKit.ExtendedClr.Compilation.JsCompiler.Compile_Phase2_TmpType = function (tmpType){
    var p = tmpType.fullname;
    var type = SharpKit.ExtendedClr.Compilation.JsCompiler.CompileType(tmpType);
    if (type != null){
        var result = SharpKit.ExtendedClr.Compilation.JsCompiler.CopyMemberIfNotDefined(type, type.fullname, window);
        if (result)
            SharpKit.ExtendedClr.Compilation.JsCompiler._NewJsTypes.push(type);
    }
    if (type.ns != null){
        var ns = SharpKit.ExtendedClr.Compilation.JsCompiler.ResolveNamespace(type.ns);
        if (type != null)
            ns[type.name] = type;
    }
};
SharpKit.ExtendedClr.Compilation.JsCompiler.Compile_Phase3 = function (){
    var funcs = AfterCompilationFunctions;
    AfterCompilationFunctions =  [];
    for (var $i6 = 0,$l6 = funcs.length,action = funcs[$i6]; $i6 < $l6; $i6++, action = funcs[$i6])
        action();
    IsCompiled = true;
    for (var i = 0; i < SharpKit.ExtendedClr.Compilation.JsCompiler.AfterCompile.length; i++)
        SharpKit.ExtendedClr.Compilation.JsCompiler.AfterCompile[i]();
};
SharpKit.ExtendedClr.Compilation.JsCompiler.CopyMemberIfNotDefined = function (source, name, target){
    if(target[name]===undefined) { target[name] = source; return true; } else { return false; }
};
SharpKit.ExtendedClr.Compilation.JsCompiler._CopyObject = function (source, target){
    for(var p in source)
		target[p] = source[p];
	if(source.toString!=Object.prototype.toString && target.toString==Object.prototype.toString)
		target.toString = source.toString;
};
SharpKit.ExtendedClr.Compilation.JsCompiler._SafeCopyObject = function (source, target){
    	for(var p in source)
	{
		if(typeof(target[p])!='undefined')
		{
			//TODO: Alon - unmark this. throw new Error(p+' is already defined on target object');
		}
		else
			target[p] = source[p];
	}
	if(source.toString!=Object.prototype.toString)
	{//TODO: commented out by dan-el
		//if(target.toString!=Object.prototype.toString)
			//throw new Error('toString is already defined on target object');
	}
};
SharpKit.ExtendedClr.Compilation.JsCompiler._EnumTryParse = function (name){
    return this.staticDefintion[name];
};
SharpKit.ExtendedClr.Compilation.JsCompiler.NewByFunc = function (ctor){
    return new ctor();
};
SharpKit.ExtendedClr.Compilation.JsCompiler.NewByFuncArgs = function (ctor, args){
    return new ctor.apply(null, args);
};
SharpKit.ExtendedClr.Compilation.JsCompiler.GetNativeToStringFunction = function (){
    return Object.prototype.toString;
};
SharpKit.ExtendedClr.Compilation.JsCompiler.Throw = function (exception){
    __LastException = exception || __LastException;
			var error = new Error(exception.ToString());
			error['_Exception'] = exception;
			throw error;
};
SharpKit.ExtendedClr.Compilation.JsCompiler.CreateEmptyCtor = function (){
    return function(){};
};
SharpKit.ExtendedClr.Compilation.JsCompiler.CreateBaseCtor = function (type){
    return function(){this.construct(type);};
};
if(typeof(Node)=='undefined')
	var Node = function(){};

SharpKit.ExtendedClr.Compilation.JsCompiler.ResolveNamespace = function (nsText){
    var ns = window;
    var tokens = nsText.split(".");
    for (var i = 0; i < tokens.length; i++){
        var token = tokens[i];
        if (typeof(ns[token]) == "undefined")
            ns[token] = {};
        ns[token].name = tokens.slice(0, i).join(".");
        ns = ns[token];
    }
    return ns;
};
SharpKit.ExtendedClr.Compilation.JsCompiler.ResolveBaseType = function (type, currentType){
    var baseType = JsTypeHelper.GetType(type.baseTypeName);
    if (baseType == null)
        baseType = JsTypeHelper.GetTypeIgnoreNamespace(type.baseTypeName, true);
    if (!baseType.isCompiled)
        SharpKit.ExtendedClr.Compilation.JsCompiler.CompileType(baseType);
    currentType.baseType = baseType;
    baseType.derivedTypes.push(currentType);
};
SharpKit.ExtendedClr.Compilation.JsCompiler.ResolveInterfaces = function (type, currentType){
    if (type.interfaceNames == null)
        return;
    for (var i = 0; i < type.interfaceNames.length; i++){
        var iName = type.interfaceNames[i];
        var iface = JsTypeHelper.GetType(iName);
        if (iface == null)
            iface = JsTypeHelper.GetTypeIgnoreNamespace(iName, true);
        if (!iface.isCompiled)
            SharpKit.ExtendedClr.Compilation.JsCompiler.CompileType(iface);
        currentType.interfaces.push(iface);
    }
};
SharpKit.ExtendedClr.Compilation.JsCompiler.CompileType = function (type){
    var currentType = (SharpKit.ExtendedClr.Compilation.JsCompiler.Types[type.fullname] != null ? SharpKit.ExtendedClr.Compilation.JsCompiler.Types[type.fullname] : type);
    if (currentType.ctors == null)
        currentType.ctors = new Object();
    if (!type.isCompiled){
        var baseTypeResolved = false;
        if (currentType.baseType == null && currentType.baseTypeName != null){
            SharpKit.ExtendedClr.Compilation.JsCompiler.ResolveBaseType(type, currentType);
            if (currentType.baseType != null)
                baseTypeResolved = true;
        }
        SharpKit.ExtendedClr.Compilation.JsCompiler.ResolveInterfaces(type, currentType);
        for (var p in type.definition){
            if (p.search("ctor") == 0){
                currentType[p] = type.definition[p];
                delete type.definition[p];
                if (typeof(currentType.commonPrototype) == "undefined")
                    currentType.commonPrototype = currentType[p].prototype;
                else
                    currentType[p].prototype = currentType.commonPrototype;
                currentType.ctors[p] = currentType[p];
            }
            if (p == "cctor")
                currentType.cctor = p;
        }
        if (currentType.ctor == null){
            if (currentType.ns == null || currentType.ns == ""){
                var jsCtor = window[currentType.name];
                currentType.ctor = jsCtor;
            }
            if (currentType.ctor == null && currentType.ctors != null){
                if (currentType.baseType != null)
                    currentType.ctor = SharpKit.ExtendedClr.Compilation.JsCompiler.CreateBaseCtor(currentType);
                else
                    currentType.ctor = SharpKit.ExtendedClr.Compilation.JsCompiler.CreateEmptyCtor();
            }
            if (currentType.ctor != null){
                currentType.ctors["ctor"] = currentType.ctor;
                if (typeof(currentType.commonPrototype) == "undefined")
                    currentType.commonPrototype = currentType.ctor.prototype;
                else
                    currentType.ctor.prototype = currentType.commonPrototype;
            }
        }
        for (var p in currentType.ctors){
            var ctor = currentType.ctors[p];
            if (ctor._type == null)
                ctor._type = currentType;
        }
        if (baseTypeResolved){
            SharpKit.ExtendedClr.Compilation.JsCompiler._CopyObject(currentType.baseType.commonPrototype, currentType.commonPrototype);
        }
        for (var p in type.definition){
            var member = type.definition[p];
            currentType.commonPrototype[p] = member;
            if (typeof(member) == "function"){
                member._name = p;
                member._type = currentType;
            }
        }
        if (type.definition.toString != Object.prototype.toString){
            currentType.commonPrototype.toString = type.definition.toString;
            currentType.commonPrototype.toString._type = currentType;
        }
        for (var p in type.staticDefinition){
            var member = type.staticDefinition[p];
            currentType[p] = member;
            if (typeof(member) == "function"){
                member._name = p;
                member._type = currentType;
            }
        }
        type.isCompiled = true;
    }
    SharpKit.ExtendedClr.Compilation.JsCompiler.CompileEnum(currentType);
    if (currentType != type && type.customAttributes != null){
        if (currentType.customAttributes != null){
            for (var i = 0; i < type.customAttributes.length; i++){
                currentType.customAttributes.push(type.customAttributes[i]);
            }
        }
        else {
            currentType.customAttributes = type.customAttributes;
        }
    }
    return currentType;
};
SharpKit.ExtendedClr.Compilation.JsCompiler.CompileEnum = function (currentType){
    if (currentType.Kind == "Enum"){
        currentType.tryParse = SharpKit.ExtendedClr.Compilation.JsCompiler._EnumTryParse;
        for (var p in currentType.staticDefinition){
            if (typeof(currentType.staticDefinition[p]) == "string"){
                var x = SharpKit.ExtendedClr.Compilation.JsCompiler.NewByFunc(currentType.ctor);
                x["_Name"] = p;
                currentType.staticDefinition[p] = x;
                currentType[p] = x;
            }
        }
    }
};
SharpKit.ExtendedClr.Compilation.JsCompiler.GetHashKey = function (obj){
    if (obj == undefined)
        return "undefined";
    if (obj == null)
        return "null";
    if (obj.valueOf)
        obj = obj.valueOf();
    var type = typeof(obj);
    if (type == "string")
        return obj;
    if (type == "object" || type == "function"){
        if (obj._hashKey == null){
            obj._hashKey = SharpKit.ExtendedClr.Compilation.JsCompiler._hashKeyPrefix + SharpKit.ExtendedClr.Compilation.JsCompiler._hashKeyIndex;
            SharpKit.ExtendedClr.Compilation.JsCompiler._hashKeyIndex++;
        }
        return obj._hashKey;
    }
    return obj.toString();
};
var AssemblyDoc = {
    fullname: "AssemblyDoc",
    baseTypeName: "System.Object",
    assemblyName: "SharpKit.ExtendedClr.Compilation",
    Kind: "Class",
    definition: {
        ctor: function (){
            System.Object.ctor.call(this);
        }
    },
    ctors: [{
        name: "ctor",
        parameters: []
    }
    ],
    IsAbstract: false
};
JsTypes.push(AssemblyDoc);
var SharpKit$ExtendedClr$Compilation$Extensions = {
    fullname: "SharpKit.ExtendedClr.Compilation.Extensions",
    baseTypeName: "System.Object",
    staticDefinition: {
        from: function (args, index){
            throw $CreateException(new System.NotImplementedException.ctor(), new Error());
        }
    },
    assemblyName: "SharpKit.ExtendedClr.Compilation",
    Kind: "Class",
    definition: {
        ctor: function (){
            System.Object.ctor.call(this);
        }
    },
    ctors: [],
    IsAbstract: true
};
JsTypes.push(SharpKit$ExtendedClr$Compilation$Extensions);
var SharpKit$JavaScript$JsNamingHelper = {
    fullname: "SharpKit.JavaScript.JsNamingHelper",
    baseTypeName: "System.Object",
    staticDefinition: {
        JsFunctionNameToClrMethodName: function (jsFuncName){
            var methodName = jsFuncName;
            var di = jsFuncName.indexOf("$");
            if (di > 0)
                methodName = jsFuncName.substr(0, di);
            return methodName;
        },
        ClrTypeToJsTypeRef: function (type){
            var att = System.Linq.Enumerable.FirstOrDefault$1$$IEnumerable$1(SharpKit.JavaScript.JsTypeAttribute.ctor, System.Linq.Enumerable.OfType$1(SharpKit.JavaScript.JsTypeAttribute.ctor, type.GetCustomAttributes$$Type$$Boolean(Typeof(SharpKit.JavaScript.JsTypeAttribute.ctor), false)));
            if (att != null && System.StringExtensions.IsNotNullOrEmpty$$String(att.get_Name())){
                return att.get_Name().Replace$$Char$$Char("`", "$");
            }
            return type.get_Namespace() + "." + type.get_Name().Replace$$Char$$Char("`", "$");
        },
        ClrConstructorToJsFunctionName: function (mi){
            var sb = new System.Text.StringBuilder.ctor();
            var typeRef = SharpKit.JavaScript.JsNamingHelper.ClrTypeToJsTypeRef(mi.get_DeclaringType());
            sb.Append$$String(typeRef);
            sb.Append$$String(".ctor");
            SharpKit.JavaScript.JsNamingHelper.ConvertParametersToJsFunctionName(mi.GetParameters(), sb);
            return sb.toString();
        },
        ConvertParametersToJsFunctionName: function (prms, sb){
            for (var $i2 = 0,$l2 = prms.length,prm = prms[$i2]; $i2 < $l2; $i2++, prm = prms[$i2]){
                sb.Append$$String("$$");
                sb.Append$$String(prm.get_ParameterType().get_Name());
            }
        },
        ClrMethodBaseToJsFunctionName: function (mi){
            if (mi.get_MemberType() == 1){
                return SharpKit.JavaScript.JsNamingHelper.ClrMethodToJsFunctionName(Cast(mi, System.Reflection.MethodInfo.ctor));
            }
            else if (mi.get_MemberType() == 8){
                return SharpKit.JavaScript.JsNamingHelper.ClrConstructorToJsFunctionName(Cast(mi, System.Reflection.ConstructorInfo.ctor));
            }
            else
                throw $CreateException(new System.NotImplementedException.ctor(), new Error());
        },
        ClrMethodToJsFunctionName: function (mi){
            if (SharpKit.JavaScript.JsNamingHelper.IsPropertySetter(mi)){
                return mi.get_Name();
            }
            var name = mi.get_Name();
            var type = mi.get_DeclaringType();
            if (type.get_IsGenericType()){
                type = type.GetGenericTypeDefinition();
                mi = type.GetMethod$$String(name);
            }
            var sb = new System.Text.StringBuilder.ctor();
            if (mi.get_IsStatic()){
                sb.Append$$String(SharpKit.JavaScript.JsNamingHelper.ClrTypeToJsTypeRef(type));
                var att = System.Linq.Enumerable.FirstOrDefault$1$$IEnumerable$1(SharpKit.JavaScript.JsTypeAttribute.ctor, System.Linq.Enumerable.OfType$1(SharpKit.JavaScript.JsTypeAttribute.ctor, type.GetCustomAttributes$$Type$$Boolean(Typeof(SharpKit.JavaScript.JsTypeAttribute.ctor), false)));
                if (att == null || att.get_Mode() == 2){
                    sb.Append$$String(".staticDefinition.");
                }
                else {
                    sb.Append$$String(".");
                }
            }
            sb.Append$$String(name);
            SharpKit.JavaScript.JsNamingHelper.ConvertParametersToJsFunctionName(mi.GetParameters(), sb);
            return sb.toString();
        },
        IsPropertySetter: function (mi){
            var name = mi.get_Name();
            return name.StartsWith$$String("set_") && mi.GetParameters().length == 1;
        }
    },
    assemblyName: "SharpKit.ExtendedClr.Compilation",
    Kind: "Class",
    definition: {
        ctor: function (){
            System.Object.ctor.call(this);
        }
    },
    ctors: [],
    IsAbstract: true
};
JsTypes.push(SharpKit$JavaScript$JsNamingHelper);
var JsRuntime = function (){
};
JsRuntime.Start = function (){
    Compile();
};
JsRuntime.prototype.get_Types = function (){
    return SharpKit.ExtendedClr.Compilation.JsCompiler.Types;
};
var JsTypeHelper = function (){
};
JsTypeHelper.GetTypeIgnoreNamespaceCache = null;
JsTypeHelper.GetTypeIgnoreNamespace = function (name, throwIfNotFound){
    var type = null;
    var cache = JsTypeHelper.GetTypeIgnoreNamespaceCache;
    if (cache != null){
        type = cache[name];
        if (typeof(type) != "undefined"){
            if (throwIfNotFound && type == null)
                throw $CreateException(new Error("type " + name + " was not found with (with IgnoreNamespace)."), new Error());
            return type;
        }
    }
    if (name.search(".") > -1){
        var tokens = name.split(".");
        name = tokens[tokens.length - 1];
    }
    type = SharpKit.ExtendedClr.Compilation.JsCompiler.Types[name];
    var nameAfterNs = "." + name;
    if (type == null){
        for (var p in SharpKit.ExtendedClr.Compilation.JsCompiler.Types){
            if (p == name || p.endsWith(nameAfterNs)){
                type = SharpKit.ExtendedClr.Compilation.JsCompiler.Types[p];
                break;
            }
        }
    }
    if (throwIfNotFound && type == null)
        throw $CreateException(new Error("type " + name + " was not found with (with IgnoreNamespace)."), new Error());
    if (cache != null)
        cache[name] = (type != null ? type : null);
    return type;
};
JsTypeHelper._HasTypeArguments = function (typeName){
    return typeName.indexOf("[") > -1;
};
JsTypeHelper._GetTypeWithArguments = function (typeName, throwIfNotFound){
    var name = typeName;
    var gti = name.indexOf("`");
    if (gti != -1 && name.indexOf("[") > -1){
        var args = JsTypeHelper._ParseTypeNameArgs(name);
        if (args == null)
            return null;
        var type = JsTypeHelper.GetType(args[0], throwIfNotFound);
        if (type == null)
            return null;
        var res = new Array(0);
        res.push(type);
        var typeArgs = new Array(0);
        for (var i = 0; i < args[1].length; i++){
            var typeArg = JsTypeHelper.GetType(args[1][i][0], throwIfNotFound);
            if (typeArg == null)
                return null;
            typeArgs.push(typeArg);
        }
        res.push(typeArgs);
        return res;
    }
    return null;
};
JsTypeHelper._ParseTypeNameArgs = function (name){
    	var code = name.replace(/, [a-zA-Z0-9, =.]+\]/g, ']'); //remove all the ', mscorlib, Version=1.0.0.0, publicKeyToken=xxxxxxxxx
	code = code.replace(/`([0-9])/g, '$$$1,'); //remove the `2 and replace to $2, (the comma is for array to compile)
	code = '[' + code + ']';
try
{
	var args = eval(code);
return args;
}
catch(e)
{
  //ERROR
  return null;
}
	
};
JsTypeHelper.GetType = function (typeOrNameOrCtor, throwIfNotFound){
    if (typeof(typeOrNameOrCtor) != "string"){
        if (typeof(typeOrNameOrCtor) == "function")
            return typeOrNameOrCtor._type;
        return typeOrNameOrCtor;
    }
    var name = typeOrNameOrCtor;
    var gti = name.indexOf("`");
    if (gti != -1){
        name = name.substr(0, gti + 2).replace("`", "$");
    }
    var type = SharpKit.ExtendedClr.Compilation.JsCompiler.Types[name];
    if (type == null){
        if (throwIfNotFound)
            throw $CreateException(new Error("JsType " + name + " was not found"), new Error());
        return null;
    }
    return type;
};
JsTypeHelper.FindType = function (name, throwIfNotFound){
    var type = JsTypeHelper.GetType(name, false);
    if (type == null)
        type = JsTypeHelper.GetTypeIgnoreNamespace(name, throwIfNotFound);
    return type;
};
JsTypeHelper.GetAssemblyQualifiedName = function (type){
    if (type._AssemblyQualifiedName == null){
        var name = type.fullname;
        if (type.assemblyName != null)
            name += ", " + type.assemblyName;
        type._AssemblyQualifiedName = name;
    }
    return type._AssemblyQualifiedName;
};
JsTypeHelper.GetName = function (type){
    return type.name;
};
JsTypeHelper.getMemberTypeName = function (instance, memberName){
    var signature = instance[memberName + "$$"];
    if (signature == null)
        return null;
    var tokens = signature.split(" ");
    var memberTypeName = tokens[tokens.length - 1];
    return memberTypeName;
};
JsTypeHelper.GetDelegate = function (obj, func){
    var target = obj;
    if (target == null)
        return func;
    if (typeof(func) == "string")
        func = target[func];
    var cache = target.__delegateCache;
    if (cache == null){
        cache = new Object();
        target.__delegateCache = cache;
    }
    var key = SharpKit.ExtendedClr.Compilation.JsCompiler.GetHashKey(func);
    var del = cache[key];
    if (del == null){
        del = function (){
            var del2 = arguments.callee;
            return del2.func.apply(del.target, arguments);
        };
        del.func = func;
        del.target = target;
        del.isDelegate = true;
        cache[key] = del;
    }
    return del;
};
var SharpKit$ExtendedClr$Compilation$VersionInfo = {
    fullname: "SharpKit.ExtendedClr.Compilation.VersionInfo",
    baseTypeName: "System.Object",
    staticDefinition: {
        cctor: function (){
            SharpKit.ExtendedClr.Compilation.VersionInfo.Version = "1.0.0";
        },
        GetVersion: function (){
            return new System.Version.ctor$$String("1.0.0");
        }
    },
    assemblyName: "SharpKit.ExtendedClr.Compilation",
    Kind: "Class",
    definition: {
        ctor: function (){
            System.Object.ctor.call(this);
        }
    },
    ctors: [],
    IsAbstract: true
};
JsTypes.push(SharpKit$ExtendedClr$Compilation$VersionInfo);
var System$Reflection$PropertyInfoExtensions = {
    fullname: "System.Reflection.PropertyInfoExtensions",
    baseTypeName: "System.Object",
    staticDefinition: {
        IsStatic: function (pi){
            return pi._IsStatic;
        },
        IsPublic: function (pi){
            throw new Error('Not Implemented');
        }
    },
    assemblyName: "SharpKit.ExtendedClr.Compilation",
    Kind: "Class",
    definition: {
        ctor: function (){
            System.Object.ctor.call(this);
        }
    },
    ctors: [],
    IsAbstract: true
};
JsTypes.push(System$Reflection$PropertyInfoExtensions);
var System$StringExtensions = {
    fullname: "System.StringExtensions",
    baseTypeName: "System.Object",
    staticDefinition: {
        GetValueOrDefaultIfNullOrEmpty: function (s, defaultValue){
            if(s==null || s.length==0) return defaultValue; return s;
        },
        IsNullOrEmpty$$String: function (s){
            return s == null || s.length == 0;
        },
        IsNotNullOrEmpty$$String: function (s){
            return s != null && s.length > 0;
        },
        HtmlEscape: function (s){
            return s.Replace$$String$$String("&", "&amp;").Replace$$String$$String("<", "&lt;").Replace$$String$$String(">", "&gt;").Replace$$String$$String("\n", "<br/>");
        },
        ReplaceFirst$$String$$String$$String: function (s, search, replace){
            return s.ReplaceFirst(search, replace);
        },
        ReplaceFirst$$String$$String$$String$$StringComparison: function (s, search, replace, comparisonType){
            var index = s.indexOf(search, comparisonType);
            if (index != -1){
                var finalStr = System.String.Concat$$String$$String$$String(s.substr(0, index), replace, s.substr(search.length + index));
                return finalStr;
            }
            return s;
        },
        FixCamelCasing: function (s){
            var sb = new System.Text.StringBuilder.ctor();
            var first = true;
            var $it2 = s.GetEnumerator();
            while ($it2.MoveNext()){
                var c = $it2.get_Current();
                if (System.Char.IsUpper$$Char(c) && !first){
                    sb.Append$$Char(" ");
                }
                sb.Append$$Char(c);
                first = false;
            }
            return sb.toString();
        },
        RemoveLast: function (s, count){
            return s.substr(s, s.length-count);
        },
        TrimEnd: function (s, trimText){
            if (s.EndsWith$$String(trimText))
                return System.StringExtensions.RemoveLast(s, trimText.length);
            return s;
        },
        EqualsIgnoreCase: function (s1, s2){
            return System.String.Compare$$String$$String$$Boolean(s1, s2, true) == 0;
        }
    },
    assemblyName: "SharpKit.ExtendedClr.Compilation",
    Kind: "Class",
    definition: {
        ctor: function (){
            System.Object.ctor.call(this);
        }
    },
    ctors: [],
    IsAbstract: true
};
JsTypes.push(System$StringExtensions);

