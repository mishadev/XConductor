﻿using System;
using System.IO;
using System.Runtime.InteropServices;

namespace XConductor.Domain.W8.MediaFoundation
{
    internal struct Blob
    {
        public int Length;
        public IntPtr Data;

        //Code Should Compile at warning level4 without any warnings, 
        //However this struct will give us Warning CS0649: Field [Fieldname] 
        //is never assigned to, and will always have its default value
        //You can disable CS0649 in the project options but that will disable
        //the warning for the whole project, it's a nice warning and we do want 
        //it in other places so we make a nice dummy function to keep the compiler
        //happy.
        private void FixCS0649()
        {
            Length = 0;
            Data = IntPtr.Zero;
        }
    }

    /// <summary>
    /// from Propidl.h.
    /// http://msdn.microsoft.com/en-us/library/aa380072(VS.85).aspx
    /// contains a union so we have to do an explicit layout
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct PropVariant
    {
        [FieldOffset(0)] private short vt;
        [FieldOffset(2)] private short wReserved1;
        [FieldOffset(4)] private short wReserved2;
        [FieldOffset(6)] private short wReserved3;
        [FieldOffset(8)] private sbyte cVal;
        [FieldOffset(8)] private byte bVal;
        [FieldOffset(8)] private short iVal;
        [FieldOffset(8)] private ushort uiVal;
        [FieldOffset(8)] private int lVal;
        [FieldOffset(8)] private uint ulVal;
        [FieldOffset(8)] private int intVal;
        [FieldOffset(8)] private uint uintVal;
        [FieldOffset(8)] private long hVal;
        [FieldOffset(8)] private long uhVal;
        [FieldOffset(8)] private float fltVal;
        [FieldOffset(8)] private double dblVal;
        [FieldOffset(8)] private bool boolVal;
        [FieldOffset(8)] private int scode;
        //CY cyVal;
        [FieldOffset(8)] private DateTime date;
        [FieldOffset(8)] private System.Runtime.InteropServices.ComTypes.FILETIME filetime;
        //CLSID* puuid;
        //CLIPDATA* pclipdata;
        //BSTR bstrVal;
        //BSTRBLOB bstrblobVal;
        [FieldOffset(8)] private Blob blobVal;
        //LPSTR pszVal;
        [FieldOffset(8)] private IntPtr pointerValue; //LPWSTR 
        //IUnknown* punkVal;
        /*IDispatch* pdispVal;
        IStream* pStream;
        IStorage* pStorage;
        LPVERSIONEDSTREAM pVersionedStream;
        LPSAFEARRAY parray;
        CAC cac;
        CAUB caub;
        CAI cai;
        CAUI caui;
        CAL cal;
        CAUL caul;
        CAH cah;
        CAUH cauh;
        CAFLT caflt;
        CADBL cadbl;
        CABOOL cabool;
        CASCODE cascode;
        CACY cacy;
        CADATE cadate;
        CAFILETIME cafiletime;
        CACLSID cauuid;
        CACLIPDATA caclipdata;
        CABSTR cabstr;
        CABSTRBLOB cabstrblob;
        CALPSTR calpstr;
        CALPWSTR calpwstr;
        CAPROPVARIANT capropvar;
        CHAR* pcVal;
        UCHAR* pbVal;
        SHORT* piVal;
        USHORT* puiVal;
        LONG* plVal;
        ULONG* pulVal;
        INT* pintVal;
        UINT* puintVal;
        FLOAT* pfltVal;
        DOUBLE* pdblVal;
        VARIANT_BOOL* pboolVal;
        DECIMAL* pdecVal;
        SCODE* pscode;
        CY* pcyVal;
        DATE* pdate;
        BSTR* pbstrVal;
        IUnknown** ppunkVal;
        IDispatch** ppdispVal;
        LPSAFEARRAY* pparray;
        PROPVARIANT* pvarVal;
        */

        /// <summary>
        /// Creates a new PropVariant containing a long value
        /// </summary>
        public static PropVariant FromLong(long value)
        {
            return new PropVariant() {vt = (short) VarEnum.VT_I8, hVal = value};
        }

        /// <summary>
        /// Helper method to gets blob data
        /// </summary>
        private byte[] GetBlob()
        {
            var blob = new byte[blobVal.Length];
            Marshal.Copy(blobVal.Data, blob, 0, blob.Length);
            return blob;
        }

        /// <summary>
        /// Interprets a blob as an array of structs
        /// </summary>
        public T[] GetBlobAsArrayOf<T>()
        {
            var blobByteLength = blobVal.Length;
            var singleInstance = (T) Activator.CreateInstance(typeof (T));
            var structSize = Marshal.SizeOf(singleInstance);
            if (blobByteLength%structSize != 0)
            {
                throw new InvalidDataException(String.Format("Blob size {0} not a multiple of struct size {1}", blobByteLength, structSize));
            }
            var items = blobByteLength/structSize;
            var array = new T[items];
            for (int n = 0; n < items; n++)
            {
                array[n] = (T) Activator.CreateInstance(typeof (T));
                Marshal.PtrToStructure(new IntPtr((long) blobVal.Data + n*structSize), array[n]);
            }
            return array;
        }

        /// <summary>
        /// Gets the type of data in this PropVariant
        /// </summary>
        public VarEnum DataType
        {
            get { return (VarEnum) vt; }
        }

    /// <summary>
        /// Property value
        /// </summary>
        public object Value
        {
            get
            {
                VarEnum ve = DataType;
                switch (ve)
                {
                    case VarEnum.VT_I1:
                        return bVal;
                    case VarEnum.VT_I2:
                        return iVal;
                    case VarEnum.VT_I4:
                        return lVal;
                    case VarEnum.VT_I8:
                        return hVal;
                    case VarEnum.VT_INT:
                        return iVal;
                    case VarEnum.VT_UI4:
                        return ulVal;
                    case VarEnum.VT_UI8:
                        return uhVal;
                    case VarEnum.VT_LPWSTR:
                        return Marshal.PtrToStringUni(pointerValue);
                    case VarEnum.VT_BLOB:
                    case VarEnum.VT_VECTOR | VarEnum.VT_UI1:
                        return GetBlob();
                    case VarEnum.VT_CLSID:
                        return (Guid)Marshal.PtrToStructure(pointerValue, typeof(Guid));
                }
                throw new NotImplementedException("PropVariant " + ve.ToString());
            }
        }

        /// <summary>
        /// allows freeing up memory, might turn this into a Dispose method?
        /// </summary>
        public void Clear()
        {
            PropVariantClear(ref this);
        }

        [DllImport("ole32.dll")]
        private static extern int PropVariantClear(ref PropVariant pvar);
    }
}

