using Microsoft.Xna.Framework;
using System;

namespace FXDecompiler {
	public static class Program {
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
					Console.WriteLine("Usage: fxdecompiler.exe <file>");
					return;
				}

				// TODO: call reverse engineered D3DDisassemble() function
			}
		}
	}
}
