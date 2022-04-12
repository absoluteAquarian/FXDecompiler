using Microsoft.Xna.Framework.Content;
using System;
using System.IO;

namespace FXDecompiler {
	//Copied from my MonoSound project
	internal static class Decompressor {
		const byte ContentCompressedLzx = 0x80;

		public static byte[] DecompressShaderEffectXNB(string path){
			if(Path.GetExtension(path) != ".xnb")
				throw new ArgumentException("File must be an XNB file.", "path");
			
			Stream stream = File.OpenRead(path);
			byte[] data;

			using(BinaryReader reader = new BinaryReader(stream)){
				Stream decompressedStream = Pre_ReadAsset(reader, stream);
				
				using (BinaryReader decompressedReader = new BinaryReader(decompressedStream))
					data = Simulate_ContentReader_ReadAsset(decompressedReader);
			}

			return data;
		}
		
		private static Stream Pre_ReadAsset(BinaryReader reader, Stream stream){
			// The first 4 bytes should be the "XNB" header. i use that to detect an invalid file
			byte x = reader.ReadByte();
			byte n = reader.ReadByte();
			byte b = reader.ReadByte();
			byte platform = reader.ReadByte();

			if(x != 'X' || n != 'N' || b != 'B' || (char)platform != 'w')
				throw new ContentLoadException("Asset does not appear to be a valid XNB file. Did you process your content for Windows?");

			byte version = reader.ReadByte();
			byte flags = reader.ReadByte();

			bool compressedLzx = (flags & ContentCompressedLzx) != 0;
			if(version != 5 && version != 4)
				throw new ContentLoadException("Invalid XNB version");

			// The next int32 is the length of the XNB file
			int xnbLength = reader.ReadInt32();

			Stream decompressedStream = null;
			if(compressedLzx){
				// Decompress the xnb
				int decompressedSize = reader.ReadInt32();

				if(compressedLzx){
					int compressedSize = xnbLength - 14;
					decompressedStream = new LzxDecoderStream(stream, decompressedSize, compressedSize);
				}
			}else
				decompressedStream = stream;

			return decompressedStream;
		}

		private static byte[] Simulate_ContentReader_ReadAsset(BinaryReader reader){
			//LoadAssetReaders
			Simulate_ContentTypeReaderManager_LoadAssetReaders(reader);

			//InitializeTypeReaders
			Simulate_Read7BitEncodedInt(reader);

			//InnerReadObject
			Simulate_Read7BitEncodedInt(reader);

			int count = reader.ReadInt32();
			return reader.ReadBytes(count);
		}
		
		private static void Simulate_ContentTypeReaderManager_LoadAssetReaders(BinaryReader reader){
			int numberOfReaders = Simulate_Read7BitEncodedInt(reader);

			// For each reader in the file, we read out the length of the string which contains the type of the reader,
			// then we read out the string. Finally we instantiate an instance of that reader using reflection
			for (var i = 0; i < numberOfReaders; i++)
			{
				// This string tells us what reader we need to decode the following data
				// string readerTypeString = reader.ReadString();
				string originalReaderTypeString = reader.ReadString();


				//A lot of code was deleted here.  All that matters is that the same amount of "stuff" is being read from the file


				// I think the next 4 bytes refer to the "Version" of the type reader,
				// although it always seems to be zero
				reader.ReadInt32();
			}
		}

		private static int Simulate_Read7BitEncodedInt(BinaryReader reader){
			int num = 0;
			int num2 = 0;
			byte b;

			do{
				if(num2 == 35)
					throw new FormatException("Format_Bad7BitInt32");

				b = reader.ReadByte();
				num |= (b & 0x7F) << num2;
				num2 += 7;
			}while((b & 0x80) != 0);

			return num;
		}
	}
}
