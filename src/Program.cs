using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FXDecompiler {
	//Not all of these classes/interfaces/etc. will be used, but they're neat to have around
	public static class Program {
		private static IDxcLibrary library;

		internal static IDxcLibrary Library => library ?? (library = CreateDxcLibrary());

		public static unsafe void Main(string[] args) {
			using (Decompiler decompiler = new Decompiler(args))
				decompiler.Run();
		}

		class Decompiler : Game {
			readonly string[] args;

			public Decompiler(string[] args) {
				this.args = args;
			}

			protected override void Initialize() {
				base.Initialize();
				DecompileEffect();
				Environment.Exit(0);

				// TODO: why is the 0x80004005 error still being thrown?  And why has so much (probably unnecessary) code piled up in this file?
			}

			unsafe void DecompileEffect() {
				if (args.Length < 1) {
					Console.WriteLine("Usage: FXDecompiler.exe <file>");
					return;
				}

				byte[] bytecode = Decompressor.DecompressShaderEffectXNB(args[0]);

				fixed (byte* ptr = bytecode) {
					IntPtr code = new IntPtr(ptr);

					int hr = D3DCompiler.D3DDisassemble(code, (uint)bytecode.Length, 0, null, out IDxcBlob disassembly);

					if (hr != 0) {
						Console.WriteLine("Decompilation failed with error code 0x" + hr.ToString("x"));
						Environment.Exit(hr);
					} else
						Console.WriteLine("Decompilation succeeded");

					disassembly = Library.GetBlobAstUf16(disassembly);
					string asm = new string(disassembly.GetBufferPointer(), 0, (int)(disassembly.GetBufferSize() / 2));

					using (StreamWriter writer = new StreamWriter(File.Open(Path.ChangeExtension(args[0], ".txt"), FileMode.Create)))
						writer.Write(asm);
				}
			}
		}

		private static Guid CLSID_DxcCompiler = new Guid("73e22d93-e6ce-47f3-b5bf-f0664f39c1b0");
		private static Guid CLSID_DxcLibrary = new Guid("6245D6AF-66E0-48FD-80B4-4D271796748C");

		[DllImport(@"dxcompiler.dll", CallingConvention = CallingConvention.StdCall)]
		private static extern int DxcCreateInstance(
			ref Guid clsid,
			ref Guid iid,
			[MarshalAs(UnmanagedType.IUnknown)] out object instance);

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static IDxcLibrary CreateDxcLibrary() {
			Guid classId = CLSID_DxcLibrary;
			Guid interfaceId = typeof(IDxcLibrary).GUID;
			DxcCreateInstance(ref classId, ref interfaceId, out object result);
			return (IDxcLibrary)result;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static IDxcCompiler CreateDxcCompiler() {
			Guid classId = CLSID_DxcCompiler;
			Guid interfaceId = typeof(IDxcCompiler).GUID;
			DxcCreateInstance(ref classId, ref interfaceId, out object result);
			return (IDxcCompiler)result;
		}
	}

	[ComImport]
	[Guid("8BA5FB08-5195-40e2-AC58-0D989C3A0102")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDxcBlobEncoding : IDxcBlob {
		uint GetEncoding(out bool unknown, out uint codePage);
	}

	[ComImport]
	[Guid("e5204dc7-d18c-4c3c-bdfb-851673980fe7")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDxcLibrary {
		IDxcBlobEncoding GetBlobAstUf16(IDxcBlob blob);
	}

	[ComImport]
	[Guid("CEDB484A-D4E9-445A-B991-CA21CA157DC2")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDxcOperationResult {
		int GetStatus();
		IDxcBlob GetResult();
		IDxcBlobEncoding GetErrors();
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DXCDefine {
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pName;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pValue;
	}

	[ComImport]
	[Guid("7f61fc7d-950d-467f-b3e3-3c02fb49187c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDxcIncludeHandler {
		IDxcBlob LoadSource(string fileName);
	}

	[ComImport]
	[Guid("8c210bf3-011f-4422-8d70-6f9acb8db617")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDxcCompiler {
		IDxcOperationResult Compile(IDxcBlob source, string sourceName, string entryPoint, string targetProfile,
			[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)]
			string[] arguments,
			int argCount, DXCDefine[] defines, int defineCount, IDxcIncludeHandler includeHandler);
		IDxcOperationResult Preprocess(IDxcBlob source, string sourceName, string[] arguments, int argCount,
			DXCDefine[] defines, int defineCount, IDxcIncludeHandler includeHandler);
		IDxcBlobEncoding Disassemble(IDxcBlob source);
	}
}
