using System;
using System.IO;

namespace FXDecompiler {
	public static class Program {
		public static unsafe void Main(string[] args) {
			if(args.Length < 1) {
				Console.WriteLine("Usage: FXDecompiler.exe <file>");
				return;
			}

			byte[] bytecode = Decompressor.DecompressShaderEffectXNB(args[0]);

			fixed (byte* ptr = bytecode) {
				IntPtr code = new IntPtr(ptr);

				int hr = D3DCompiler.D3DDisassemble(code, (uint)bytecode.Length, 0, null, out IDxcBlob disassembly);

				if(hr != 0) {
					Console.WriteLine("Decompilation failed with error code 0x" + hr.ToString("x"));
					Environment.Exit(hr);
				} else
					Console.WriteLine("Decompilation succeeded");

				string asm = new string(disassembly.GetBufferPointer(), 0, (int)disassembly.GetBufferSize());

				using (StreamWriter writer = new StreamWriter(File.Open(Path.ChangeExtension(args[0], ".txt"), FileMode.Create)))
					writer.Write(asm);
			}
		}
	}
}
