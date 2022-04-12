using System;
using System.Runtime.InteropServices;

namespace FXDecompiler {
	//From:  https://github.com/microsoft/DirectXShaderCompiler/blob/master/tools/clang/tools/dotnetc/D3DCompiler.cs
	[ComImport]
	[Guid("8BA5FB08-5195-40e2-AC58-0D989C3A0102")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDxcBlob {
		[PreserveSig]
		unsafe char* GetBufferPointer();
		[PreserveSig]
		uint GetBufferSize();
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D3D_SHADER_MACRO {
		[MarshalAs(UnmanagedType.LPStr)]
		public readonly string Name;
		[MarshalAs(UnmanagedType.LPStr)]
		public readonly string Definition;
	}
	
	internal static class D3DCompiler {
		//See:  https://docs.microsoft.com/en-us/windows/win32/api/d3dcompiler/nf-d3dcompiler-d3dcompile
		[DllImport("d3dcompiler_47.dll", CallingConvention = CallingConvention.Winapi, SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = true)]
		public extern static int D3DCompile(
			[MarshalAs(UnmanagedType.LPStr)] string srcData,
			int srcDataSize,
			[MarshalAs(UnmanagedType.LPStr)] string sourceName,
			[MarshalAs(UnmanagedType.LPArray)] D3D_SHADER_MACRO[] defines,
			int pInclude,
			[MarshalAs(UnmanagedType.LPStr)] string entryPoint,
			[MarshalAs(UnmanagedType.LPStr)] string target,
			uint Flags1,
			uint Flags2,
			out IDxcBlob code, out IDxcBlob errorMsgs);

		//See:  https://docs.microsoft.com/en-us/windows/win32/api/d3dcompiler/nf-d3dcompiler-d3ddisassemble
		[DllImport("d3dcompiler_47.dll", CallingConvention = CallingConvention.Winapi, SetLastError = false, CharSet = CharSet.Ansi, ExactSpelling = true)]
		public extern static int D3DDisassemble(
			IntPtr ptr,
			uint ptrSize,
			uint flags,
			[MarshalAs(UnmanagedType.LPStr)] string szComments,
			out IDxcBlob disassembly);
	}
}
