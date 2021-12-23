# MemoryMarshal VS Array

This benchmark compares performance of:
- Accessing a `Span<int>` casted from `Span<byte>` view of a `byte[]`
- Accessing a `int[]` directly

:: This is **not** about how fast Span is, but about the overhead of accessing memory via **MemoryMarshal** (so we know if it's viable option to replace arrays).
## TLDR

- .NET6 is just better than Mono in all aspects.
- `Span` on .Net6 is generally 3x faster than on Mono.
- On.Net6, `Span` is even faster than plain array access.
  - Linear access can gain 50% boost.
  - However, random access is about the same as fast as plain array access.
- On Mono, `Span` is far slower than plain array access.

## Report
``` ini
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19041.1415 (2004/May2020Update/20H1)
Intel Core i7-9700 CPU 3.00GHz, 1 CPU, 8 logical and 8 physical cores
  [Host]   : .NET Framework 4.8 (4.8.4420.0), X64 RyuJIT
  .NET 6.0 : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  Mono     : Mono 6.12.0 (Visual Studio), X64 
```
|            Method |      Job |  Runtime | Size |         Mean |     Error |    StdDev |
|------------------ |--------- |--------- |----- |-------------:|----------:|----------:|
| **.NET6 - 64** |
|       **MemoryPlus1** | **.NET 6.0** | **.NET 6.0** |   **64** |     **30.99 ns** |  **0.052 ns** |  **0.046 ns** |
| MemoryPlus1Random | .NET 6.0 | .NET 6.0 |   64 |     55.94 ns |  0.355 ns |  0.315 ns |
|        ArrayPlus1 | .NET 6.0 | .NET 6.0 |   64 |     33.33 ns |  0.048 ns |  0.045 ns |
|  ArrayPlus1Random | .NET 6.0 | .NET 6.0 |   64 |     57.42 ns |  0.788 ns |  0.615 ns |
| **Mono - 64** |
|       MemoryPlus1 |     Mono |     Mono |   64 |    116.53 ns |  0.092 ns |  0.077 ns |
| MemoryPlus1Random |     Mono |     Mono |   64 |    254.50 ns |  0.296 ns |  0.247 ns |
|        ArrayPlus1 |     Mono |     Mono |   64 |     42.38 ns |  0.248 ns |  0.232 ns |
|  ArrayPlus1Random |     Mono |     Mono |   64 |    181.89 ns |  0.199 ns |  0.187 ns |
| **.NET6 - 512** |
|       **MemoryPlus1** | **.NET 6.0** | **.NET 6.0** |  **512** |    **238.45 ns** |  **0.269 ns** |  **0.225 ns** |
| MemoryPlus1Random | .NET 6.0 | .NET 6.0 |  512 |    472.48 ns |  0.412 ns |  0.365 ns |
|        ArrayPlus1 | .NET 6.0 | .NET 6.0 |  512 |    269.21 ns |  0.697 ns |  0.618 ns |
|  ArrayPlus1Random | .NET 6.0 | .NET 6.0 |  512 |    454.11 ns |  1.155 ns |  0.965 ns |
| **Mono - 512** |
|       MemoryPlus1 |     Mono |     Mono |  512 |    776.40 ns | 12.167 ns | 10.786 ns |
| MemoryPlus1Random |     Mono |     Mono |  512 |  1,428.48 ns |  1.990 ns |  1.861 ns |
|        ArrayPlus1 |     Mono |     Mono |  512 |    326.55 ns |  0.461 ns |  0.408 ns |
|  ArrayPlus1Random |     Mono |     Mono |  512 |  1,005.22 ns |  1.129 ns |  1.056 ns |
| **.NET6 - 2048** |
|       **MemoryPlus1** | **.NET 6.0** | **.NET 6.0** | **2048** |    **933.82 ns** |  **0.985 ns** |  **0.922 ns** |
| MemoryPlus1Random | .NET 6.0 | .NET 6.0 | 2048 |  1,753.00 ns |  2.018 ns |  1.789 ns |
|        ArrayPlus1 | .NET 6.0 | .NET 6.0 | 2048 |  1,059.56 ns |  2.021 ns |  1.792 ns |
|  ArrayPlus1Random | .NET 6.0 | .NET 6.0 | 2048 |  1,882.02 ns |  1.463 ns |  1.369 ns |
| **Mono - 2048** |
|       MemoryPlus1 |     Mono |     Mono | 2048 |  3,040.32 ns | 12.504 ns | 11.085 ns |
| MemoryPlus1Random |     Mono |     Mono | 2048 |  5,365.55 ns |  5.498 ns |  4.873 ns |
|        ArrayPlus1 |     Mono |     Mono | 2048 |  1,294.10 ns |  1.750 ns |  1.366 ns |
|  ArrayPlus1Random |     Mono |     Mono | 2048 |  3,827.26 ns |  8.409 ns |  7.865 ns |
| **.NET6 - 4096** |
|       **MemoryPlus1** | **.NET 6.0** | **.NET 6.0** | **4096** |  **1,863.67 ns** |  **2.407 ns** |  **2.251 ns** |
| MemoryPlus1Random | .NET 6.0 | .NET 6.0 | 4096 |  3,540.82 ns | 10.480 ns |  9.803 ns |
|        ArrayPlus1 | .NET 6.0 | .NET 6.0 | 4096 |  2,112.11 ns |  2.500 ns |  2.216 ns |
|  ArrayPlus1Random | .NET 6.0 | .NET 6.0 | 4096 |  3,688.03 ns | 14.155 ns | 12.548 ns |
| **Mono - 4096** |
|       MemoryPlus1 |     Mono |     Mono | 4096 |  5,852.65 ns | 82.278 ns | 76.963 ns |
| MemoryPlus1Random |     Mono |     Mono | 4096 | 10,749.65 ns | 14.667 ns | 13.720 ns |
|        ArrayPlus1 |     Mono |     Mono | 4096 |  2,579.58 ns |  2.044 ns |  1.912 ns |
|  ArrayPlus1Random |     Mono |     Mono | 4096 |  7,785.77 ns | 23.392 ns | 20.736 ns |
| **.NET6 - 8192** |
|       **MemoryPlus1** | **.NET 6.0** | **.NET 6.0** | **8192** |  **3,722.81 ns** |  **3.330 ns** |  **3.115 ns** |
| MemoryPlus1Random | .NET 6.0 | .NET 6.0 | 8192 |  7,565.93 ns | 17.170 ns | 16.061 ns |
|        ArrayPlus1 | .NET 6.0 | .NET 6.0 | 8192 |  4,231.46 ns |  4.322 ns |  3.832 ns |
|  ArrayPlus1Random | .NET 6.0 | .NET 6.0 | 8192 |  7,640.85 ns | 10.831 ns |  9.602 ns |
| **Mono - 8192** |
|       MemoryPlus1 |     Mono |     Mono | 8192 | 11,852.39 ns | 58.966 ns | 55.157 ns |
| MemoryPlus1Random |     Mono |     Mono | 8192 | 21,564.19 ns | 33.788 ns | 29.952 ns |
|        ArrayPlus1 |     Mono |     Mono | 8192 |  5,219.00 ns |  8.256 ns |  7.722 ns |
|  ArrayPlus1Random |     Mono |     Mono | 8192 | 15,514.00 ns | 33.764 ns | 31.583 ns |
