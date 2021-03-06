﻿using SharpKit.JavaScript;
using System;
using System.Collections.Generic;

using System.Text;


namespace System
{

    [JsType(Name = "System.Uri")]
    internal class JsImplUri
    {
        public JsImplUri()
        {
            _OriginalString = null;
        }

        public JsImplUri(string uri)
        {
            _OriginalString = uri;
        }

        string _OriginalString;
        public string OriginalString
        {
            get
            {
                return _OriginalString;
            }
        }

        public string AbsoluteUri
        {
            get
            {
                return _OriginalString;
            }
        }


        public override string ToString()
        {
            return _OriginalString;
        }

        public override bool Equals(object obj)
        {
            return this == (JsImplUri)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //
        // Operators
        //
        public static bool operator ==(JsImplUri u1, JsImplUri u2)
        {
            if ((object)u1 == (object)u2)
            {
                return true;
            }
            if ((object)u1 == null)
            {
                return false;
            }
            if ((object)u2 == null)
            {
                return false;
            }

            return u1._OriginalString == u2._OriginalString;
        }

        public static bool operator !=(JsImplUri u1, JsImplUri u2)
        {
            return !(u1 == u2);
        }


    }
}

