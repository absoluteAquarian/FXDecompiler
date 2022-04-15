# FXDecompiler
An application written in C# for decompiling HLSL shader files compiled by [fxcompiler.exe](https://cdn.discordapp.com/attachments/271236615823687680/459821438405181470/fxcompiler.zip).  
This application was written with decompiling Terraria/tModLoader shaders in mind, though it should still work for any XNA shaders.  
(See the [Expert Shader Guide](https://github.com/tModLoader/tModLoader/wiki/Expert-Shader-Guide) for reference.)

This project also serves as a Reverse Engineering of the `C:/Windows/System32/D3DCompiler_47.dll` (DirectX 3D) library file for C++.  
The ported source to C# has been given improved error reporting, which was the main purpose behind this project's creation.

The decompiled source was obtained using [Cutter](https://cutter.re/), [Ghidra](https://ghidra-sre.org/) and [Snowman](https://github.com/yegord/snowman).  
I do not claim ownership of DirectX 3D.

# Legality
According to [17 U.S. Code § 1201](https://www.law.cornell.edu/uscode/text/17/1201), this repository is protected from copyright infringement.

**17 U.S. Code § 1201(f)(1)**
> Notwithstanding the provisions of subsection (a)(1)(A), a person who has lawfully obtained the right to use a copy of a computer program may [circumvent a technological measure](https://www.law.cornell.edu/definitions/uscode.php?width=840&height=800&iframe=true&def_id=17-USC-1838631189-2041315756&term_occur=999&term_src=title:17:chapter:12:section:1201) that effectively controls access to a particular portion of that program for the sole purpose of identifying and analyzing those elements of the program that are necessary to achieve [interoperability](https://www.law.cornell.edu/definitions/uscode.php?width=840&height=800&iframe=true&def_id=17-USC-378882880-2041310950&term_occur=999&term_src=title:17:chapter:12:section:1201) of an independently created computer program with other programs, and that have not previously been readily available to the person engaging in the circumvention, to the extent any such acts of identification and analysis do not constitute infringement under this title.

Since `C:/Windows/System32/D3DCompiler_47.dll` is packaged with all Windows 10 installations and I have bought my installation of Windows 10, I have lawfully obtained the right to use a copy of the software.

**17 U.S. Code § 1201(f)(2)**
> Notwithstanding the provisions of subsections (a)(2) and (b), a person may develop and employ technological means to [circumvent a technological measure](https://www.law.cornell.edu/definitions/uscode.php?width=840&height=800&iframe=true&def_id=17-USC-1838631189-2041315756&term_occur=999&term_src=title:17:chapter:12:section:1201), or to [circumvent protection afforded by a technological measure](https://www.law.cornell.edu/definitions/uscode.php?width=840&height=800&iframe=true&def_id=17-USC-1784525378-2041314796&term_occur=999&term_src=title:17:chapter:12:section:1201), in order to enable the identification and analysis under paragraph (1), or for the purpose of enabling [interoperability](https://www.law.cornell.edu/definitions/uscode.php?width=840&height=800&iframe=true&def_id=17-USC-378882880-2041310950&term_occur=999&term_src=title:17:chapter:12:section:1201) of an independently created computer program with other programs, if such means are necessary to achieve such [interoperability](https://www.law.cornell.edu/definitions/uscode.php?width=840&height=800&iframe=true&def_id=17-USC-378882880-2041310950&term_occur=999&term_src=title:17:chapter:12:section:1201), to the extent that doing so does not constitute infringement under this title.

**17 U.S. Code § 1201(f)(3)**
> The information acquired through the acts permitted under paragraph (1), and the means permitted under paragraph (2), may be made available to others if the person referred to in paragraph (1) or (2), as the case may be, provides such information or means solely for the purpose of enabling [interoperability](https://www.law.cornell.edu/definitions/uscode.php?width=840&height=800&iframe=true&def_id=17-USC-378882880-2041310950&term_occur=999&term_src=title:17:chapter:12:section:1201) of an independently created computer program with other programs, and to the extent that doing so does not constitute infringement under this title or violate applicable law other than this section.

As was mentioned earlier in this `README.md` file, the purpose of this project is a Reverse Engineering of `D3DCompiler_47.dll` with the added implementation of improved error reporting.  
The decompiled source present in the repository is there to serve as a reference when porting the code to C#.

**17 U.S. Code § 1201(f)(4)**
> For purposes of this subsection, the term "interoperability" means the ability of computer programs to exchange information, and of such programs mutually to use the information which has been exchanged.
