using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using IEZoneMapping.Interop;

namespace IEZoneMapping;

/// <summary>
/// This class provides working with get, add, and remove a mapping for an Internet Explorer zone.
/// </summary>
internal class InternetSecurityManager : IDisposable
{
    private IInternetSecurityManager _internetSecurityManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="InternetSecurityManager"/> class.
    /// </summary>
    public InternetSecurityManager()
    {
        _internetSecurityManager = new IInternetSecurityManager();
    }

    /// <summary>
    /// Releases all resources used by this Internet Security Manager.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by this Internet Security Manager
    /// and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true"/> to release both managed and unmanaged resources;
    /// <see langword="false"/> to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_internetSecurityManager is not null)
            {
                Marshal.FinalReleaseComObject(_internetSecurityManager);
                _internetSecurityManager = null!;
            }
        }
    }

    /// <summary>
    /// Adds a zone mapping with the specified pattern and zone type.
    /// </summary>
    /// <param name="pattern">The pattern to be added.</param>
    /// <param name="zoneType">Type of zone to map to.</param>
    /// <exception cref="FormatException">
    /// <paramref name="pattern"/> is a non-SSL site.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="pattern"/> is already mapped.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// This object was disposed.
    /// </exception>
    public void AddZoneMapping(string pattern, ZoneType zoneType)
    {
        ThrowIfDisposed();

        // TODO Add parameter validations if you want to expose this class as a global class.

        SetZoneMapping(zoneType, pattern, SZM_FLAGS.SZM_CREATE);
    }

    /// <summary>
    /// Gets zone mappings with the specified zone type.
    /// </summary>
    /// <param name="zoneType">Type of zone to get.</param>
    /// <returns>
    /// An array of <see cref="ZoneMapping"/> objects mapped to the <paramref name="zoneType"/>.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// This object was disposed.
    /// </exception>
    public ZoneMapping[] GetZoneMappings(ZoneType zoneType)
    {
        ThrowIfDisposed();

        // TODO Add parameter validations if you want to expose this class as a global class.

        int errorCode = _internetSecurityManager.GetZoneMappings((uint)zoneType, out IEnumString enumString, 0);
        if (errorCode != HResults.S_OK)
        {
            // Make assurance double sure...
            throw Marshal.GetExceptionForHR(errorCode)!;
        }

        List<ZoneMapping> zoneMappings = new();
        nint ptr = Marshal.AllocCoTaskMem(sizeof(int));

        try
        {
            string[] strings = new string[1];
            int count;

            while (enumString.Next(1, strings, ptr) != HResults.S_FALSE)
            {
                count = Marshal.ReadInt32(ptr);
                if (count == 0)
                {
                    break;
                }

                zoneMappings.Add(new ZoneMapping(strings[0], zoneType));
            }
        }
        finally
        {
            if (ptr != 0)
            {
                Marshal.FreeCoTaskMem(ptr);
            }

            Marshal.FinalReleaseComObject(enumString);
        }

        return zoneMappings.ToArray();
    }

    /// <summary>
    /// Removes a zone mapping with the specified pattern and zone type.
    /// </summary>
    /// <param name="pattern">The pattern to be removed.</param>
    /// <param name="zoneType">Type of zone to map to.</param>
    /// <exception cref="FormatException">
    /// <paramref name="pattern"/> is a non-SSL site.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="pattern"/> is already mapped.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// This object was disposed.
    /// </exception>
    public void RemoveZoneMapping(string pattern, ZoneType zoneType)
    {
        ThrowIfDisposed();

        // TODO Add parameter validations if you want to expose this class as a global class.

        SetZoneMapping(zoneType, pattern, SZM_FLAGS.SZM_DELETE);
    }

    /// <summary>
    /// Maps the specified pattern into the specified zone.
    /// </summary>
    /// <param name="zoneType">Type of zone to map to.</param>
    /// <param name="pattern">The pattern to be mapped.</param>
    /// <param name="flags">Flag that indicates whether the mappings should be added or deleted.</param>
    /// <exception cref="FormatException">
    /// <paramref name="pattern"/> is a non-SSL site.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="pattern"/> is already mapped.
    /// </exception>
    private void SetZoneMapping(ZoneType zoneType, string pattern, SZM_FLAGS flags)
    {
        int errorCode = _internetSecurityManager.SetZoneMapping((uint)zoneType, pattern, flags);
        if (errorCode != HResults.S_OK)
        {
            string message;

            switch (errorCode)
            {
                case HResults.E_ACCESSDENIED:
                    message = $"The site '{pattern}' added to {zoneType} zone must use the https:// prefix.";
                    throw new FormatException(message);

                case HResults.E_FILEEXISTS:
                    message = $"The site '{pattern}' is already in the {zoneType} zone.";
                    throw new IOException(message, errorCode);

                case HResults.E_INVALIDARG:
                    message = $"Cannot use an invalid wildcard sequence in the site '{pattern}'.";
                    throw new FormatException(message);

                default:
                    throw Marshal.GetExceptionForHR(errorCode)!;
            }
        }
    }

    /// <summary>
    /// Throws an <see cref="ObjectDisposedException"/> if this object has already been disposed.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// This object was disposed.
    /// </exception>
    private void ThrowIfDisposed()
    {
        if (_internetSecurityManager is null)
        {
            throw new ObjectDisposedException(typeof(InternetSecurityManager).FullName);
        }
    }
}
