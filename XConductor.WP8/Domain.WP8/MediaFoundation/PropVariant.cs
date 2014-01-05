using System;
using System.IO;
using System.Runtime.InteropServices;

namespace XConductor.Domain.W8.MediaFoundation
{
    public enum VarEnum
    {
        // Summary:
        //     Indicates that a value was not specified.
        VT_EMPTY = 0,
        //
        // Summary:
        //     Indicates a null value, similar to a null value in SQL.
        VT_NULL = 1,
        //
        // Summary:
        //     Indicates a short integer.
        VT_I2 = 2,
        //
        // Summary:
        //     Indicates a long integer.
        VT_I4 = 3,
        //
        // Summary:
        //     Indicates a float value.
        VT_R4 = 4,
        //
        // Summary:
        //     Indicates a double value.
        VT_R8 = 5,
        //
        // Summary:
        //     Indicates a currency value.
        VT_CY = 6,
        //
        // Summary:
        //     Indicates a DATE value.
        VT_DATE = 7,
        //
        // Summary:
        //     Indicates a BSTR string.
        VT_BSTR = 8,
        //
        // Summary:
        //     Indicates an IDispatch pointer.
        VT_DISPATCH = 9,
        //
        // Summary:
        //     Indicates an SCODE.
        VT_ERROR = 10,
        //
        // Summary:
        //     Indicates a Boolean value.
        VT_BOOL = 11,
        //
        // Summary:
        //     Indicates a VARIANT far pointer.
        VT_VARIANT = 12,
        //
        // Summary:
        //     Indicates an IUnknown pointer.
        VT_UNKNOWN = 13,
        //
        // Summary:
        //     Indicates a decimal value.
        VT_DECIMAL = 14,
        //
        // Summary:
        //     Indicates a char value.
        VT_I1 = 16,
        //
        // Summary:
        //     Indicates a byte.
        VT_UI1 = 17,
        //
        // Summary:
        //     Indicates an unsignedshort.
        VT_UI2 = 18,
        //
        // Summary:
        //     Indicates an unsignedlong.
        VT_UI4 = 19,
        //
        // Summary:
        //     Indicates a 64-bit integer.
        VT_I8 = 20,
        //
        // Summary:
        //     Indicates an 64-bit unsigned integer.
        VT_UI8 = 21,
        //
        // Summary:
        //     Indicates an integer value.
        VT_INT = 22,
        //
        // Summary:
        //     Indicates an unsigned integer value.
        VT_UINT = 23,
        //
        // Summary:
        //     Indicates a C style void.
        VT_VOID = 24,
        //
        // Summary:
        //     Indicates an HRESULT.
        VT_HRESULT = 25,
        //
        // Summary:
        //     Indicates a pointer type.
        VT_PTR = 26,
        //
        // Summary:
        //     Indicates a SAFEARRAY. Not valid in a VARIANT.
        VT_SAFEARRAY = 27,
        //
        // Summary:
        //     Indicates a C style array.
        VT_CARRAY = 28,
        //
        // Summary:
        //     Indicates a user defined type.
        VT_USERDEFINED = 29,
        //
        // Summary:
        //     Indicates a null-terminated string.
        VT_LPSTR = 30,
        //
        // Summary:
        //     Indicates a wide string terminated by null.
        VT_LPWSTR = 31,
        //
        // Summary:
        //     Indicates a user defined type.
        VT_RECORD = 36,
        //
        // Summary:
        //     Indicates a FILETIME value.
        VT_FILETIME = 64,
        //
        // Summary:
        //     Indicates length prefixed bytes.
        VT_BLOB = 65,
        //
        // Summary:
        //     Indicates that the name of a stream follows.
        VT_STREAM = 66,
        //
        // Summary:
        //     Indicates that the name of a storage follows.
        VT_STORAGE = 67,
        //
        // Summary:
        //     Indicates that a stream contains an object.
        VT_STREAMED_OBJECT = 68,
        //
        // Summary:
        //     Indicates that a storage contains an object.
        VT_STORED_OBJECT = 69,
        //
        // Summary:
        //     Indicates that a blob contains an object.
        VT_BLOB_OBJECT = 70,
        //
        // Summary:
        //     Indicates the clipboard format.
        VT_CF = 71,
        //
        // Summary:
        //     Indicates a class ID.
        VT_CLSID = 72,
        //
        // Summary:
        //     Indicates a simple, counted array.
        VT_VECTOR = 4096,
        //
        // Summary:
        //     Indicates a SAFEARRAY pointer.
        VT_ARRAY = 8192,
        //
        // Summary:
        //     Indicates that a value is a reference.
        VT_BYREF = 16384,
    }

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

