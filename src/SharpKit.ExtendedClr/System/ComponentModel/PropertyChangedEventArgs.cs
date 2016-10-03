using SharpKit.JavaScript;
using System;
using System.Collections.Generic;

using System.Text;


namespace System.ComponentModel
{
	[JsType(Name = "System.ComponentModel.INotifyPropertyChanging")]
	internal interface JsImplINotifyPropertyChanging
	{
	}

	[JsType(Name = "System.ComponentModel.INotifyPropertyChanged")]
	internal interface JsImplINotifyPropertyChanged
	{
	}

	[JsType(Name = "System.ComponentModel.PropertyChangedEventHandler")]
	internal delegate void JsImplPropertyChangedEventHandler(object sender, JsImplPropertyChangedEventArgs e);

	[JsType(Name = "System.ComponentModel.PropertyChangedEventArgs")]
	//[Remotable]
	internal class JsImplPropertyChangedEventArgs : JsImplEventArgs
	{
		// Fields
		private readonly string _PropertyName;

		// Methods
		public JsImplPropertyChangedEventArgs(string propertyName)
		{
			this._PropertyName = propertyName;
		}

		// Properties
		public virtual string PropertyName
		{
			get
			{
				return this._PropertyName;
			}
		}
	}

}
