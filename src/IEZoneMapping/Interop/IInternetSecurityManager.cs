using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace IEZoneMapping.Interop;

enum SZM_FLAGS
{
    SZM_CREATE = 0x00000000,
    SZM_DELETE = 0x00000001
}

[ComImport]
[Guid("79eac9ee-baf9-11ce-8c82-00aa004ba90b")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[CoClass(typeof(InternetSecurityManager))]
internal interface IInternetSecurityManager
{
    void SetSecuritySite(nint pSite);

    void GetSecuritySite(out nint ppSite);

    void MapUrlToZone(
        [MarshalAs(UnmanagedType.LPWStr)] string pwszUrl,
        out uint pdwZone,
        uint dwFlags);

    void GetSecurityId(
        [MarshalAs(UnmanagedType.LPWStr)] string pwszUrl,
        [MarshalAs(UnmanagedType.LPArray)] byte[] pbSecurityId,
        ref uint pcbSecurityId,
        uint dwReserved);

    void ProcessUrlAction(
        [MarshalAs(UnmanagedType.LPWStr)] string pwszUrl,
        uint dwAction,
        out byte pPolicy,
        uint cbPolicy,
        byte pContext,
        uint cbContext,
        uint dwFlags,
        uint dwReserved);

    void QueryCustomPolicy(
        [MarshalAs(UnmanagedType.LPWStr)] string pwszUrl,
        ref Guid guidKey,
        out byte ppPolicy,
        out uint pcbPolicy,
        byte pContext,
        uint cbContext,
        uint dwReserved);

    [PreserveSig]
    int SetZoneMapping(
        uint dwZone,
        [MarshalAs(UnmanagedType.LPWStr)] string lpszPattern,
        SZM_FLAGS dwFlags);

    [PreserveSig]
    int GetZoneMappings(
        uint dwZone,
        out IEnumString ppenumString,
        uint dwFlags);
}

[ComImport]
[Guid("7b8a2d94-0ac9-11d1-896c-00c04Fb6bfc4")]
internal class InternetSecurityManager
{
}
