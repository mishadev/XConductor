﻿using System;

namespace XConductor.Infrastructure.CrossCutting.Seedwork.Adapter
{
    public static class TypeAdapterFactory
    {
        static ITypeAdapterFactory _currentTypeAdapterFactory = null;

        public static void SetCurrent(ITypeAdapterFactory adapterFactory)
        {
            _currentTypeAdapterFactory = adapterFactory;
        }

        public static ITypeAdapter CreateAdapter()
        {
            return _currentTypeAdapterFactory.Create();
        }
    }
}
