﻿//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Stitching
{
    /// <summary>
    /// Blender for Image Stitching
    /// </summary>
    public abstract class Blender : UnmanagedObject
    {
        protected IntPtr _blenderPtr;

        /// <summary>
        /// Pointer to the native Blender object.
        /// </summary>
        public IntPtr BlenderPtr
        {
            get { return _blenderPtr; }
        }

        /// <summary>
        /// Reset the unmanaged pointer associated to this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_blenderPtr != IntPtr.Zero)
                _blenderPtr = IntPtr.Zero;
        }
    }


    /// <summary>
    /// Simple blender which mixes images at its borders.
    /// </summary>
    public class FeatherBlender : Blender
    {
        /// <summary>
        /// Create a simple blender which mixes images at its borders
        /// </summary>
        /// <param name="sharpness">Shapness</param>
        public FeatherBlender(float sharpness)
        {
            _ptr = StitchingInvoke.cveFeatherBlenderCreate(sharpness, ref _blenderPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this blender
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveFeatherBlenderRelease(ref _ptr);
            }
        }
    }

    public class MultiBandBlender : Blender
    {

        public MultiBandBlender(int tryGpu, int numBands, int weightType)
        {
            _ptr = StitchingInvoke.cveMultiBandBlenderCreate(tryGpu, numBands, weightType, ref _blenderPtr);
        }

        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveMultiBandBlenderRelease(ref _ptr);
            }
        }
    }

    public static partial class StitchingInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFeatherBlenderCreate(float sharpness, ref IntPtr blender);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFeatherBlenderRelease(ref IntPtr blender);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMultiBandBlenderCreate(int tryGpu, int numBands, int weightType, ref IntPtr blender);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMultiBandBlenderRelease(ref IntPtr blender);
    }
}
