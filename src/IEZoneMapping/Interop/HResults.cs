namespace IEZoneMapping.Interop;

internal static class HResults
{
    internal const int S_OK = 0x00000000;
    internal const int S_FALSE = 0x00000001;
    internal const int E_ACCESSDENIED = unchecked((int)0x80070005);
    /// <summary>
    /// This HRESULT originally has no name.
    /// </summary>
    internal const int E_FILEEXISTS = unchecked((int)0x80070050);
    internal const int E_INVALIDARG = unchecked((int)0x80070057);
}
