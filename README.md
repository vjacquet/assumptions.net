# assumptions.net

dotnet's facts


## Benchmarks

### 
Which is faster `Array.Clear(Buffer)`, as seen in `ArrayPool.Clear`, or `Buffer.AsSpan().Clear()`, as seen in `JsonDocument.Parse`.

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1706 (21H1/May2021Update)
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.300
  [Host]     : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT
  DefaultJob : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT


```
|     Method | Strategy |     Buffer |      Mean |     Error |    StdDev | Ratio | RatioSD |
|----------- |--------- |----------- |----------:|----------:|----------:|------:|--------:|
| **ClearArray** |      **All** | **Byte[1024]** | **21.533 ns** | **0.4783 ns** | **0.6705 ns** |  **1.00** |    **0.00** |
|  ClearSpan |      All | Byte[1024] | 22.131 ns | 0.2599 ns | 0.2431 ns |  1.03 |    0.04 |
| **ClearArray** |      **All** | **Byte[2048]** | **28.100 ns** | **0.3863 ns** | **0.3424 ns** |  **1.31** |    **0.04** |
|  ClearSpan |      All | Byte[2048] | 28.538 ns | 0.4937 ns | 0.5283 ns |  1.32 |    0.05 |
| **ClearArray** |      **All** |  **Byte[256]** | **13.593 ns** | **0.1611 ns** | **0.1346 ns** |  **0.63** |    **0.02** |
|  ClearSpan |      All |  Byte[256] | 12.439 ns | 0.1708 ns | 0.1514 ns |  0.58 |    0.02 |
| **ClearArray** |      **All** | **Byte[4096]** | **47.612 ns** | **0.4016 ns** | **0.3353 ns** |  **2.22** |    **0.07** |
|  ClearSpan |      All | Byte[4096] | 47.028 ns | 0.9745 ns | 1.3976 ns |  2.19 |    0.09 |
| **ClearArray** |      **All** |  **Byte[512]** | **15.303 ns** | **0.3640 ns** | **0.4046 ns** |  **0.71** |    **0.03** |
|  ClearSpan |      All |  Byte[512] | 13.545 ns | 0.3367 ns | 0.4378 ns |  0.63 |    0.03 |
| **ClearArray** |      **All** | **Byte[8192]** | **74.953 ns** | **1.1389 ns** | **1.0096 ns** |  **3.49** |    **0.11** |
|  ClearSpan |      All | Byte[8192] | 74.637 ns | 1.2750 ns | 1.1302 ns |  3.48 |    0.10 |
|            |          |            |           |           |           |       |         |
| **ClearArray** |     **Half** | **Byte[1024]** | **17.278 ns** | **0.3303 ns** | **0.4057 ns** |  **1.00** |    **0.00** |
|  ClearSpan |     Half | Byte[1024] | 13.405 ns | 0.2342 ns | 0.2191 ns |  0.77 |    0.02 |
| **ClearArray** |     **Half** | **Byte[2048]** | **25.337 ns** | **0.3570 ns** | **0.3164 ns** |  **1.46** |    **0.05** |
|  ClearSpan |     Half | Byte[2048] | 20.260 ns | 0.4658 ns | 0.7783 ns |  1.18 |    0.07 |
| **ClearArray** |     **Half** |  **Byte[256]** |  **8.693 ns** | **0.0967 ns** | **0.0904 ns** |  **0.50** |    **0.02** |
|  ClearSpan |     Half |  Byte[256] |  5.135 ns | 0.1574 ns | 0.2450 ns |  0.30 |    0.01 |
| **ClearArray** |     **Half** | **Byte[4096]** | **29.197 ns** | **0.6069 ns** | **0.5677 ns** |  **1.68** |    **0.04** |
|  ClearSpan |     Half | Byte[4096] | 27.429 ns | 0.5297 ns | 0.4955 ns |  1.58 |    0.04 |
| **ClearArray** |     **Half** |  **Byte[512]** | **16.353 ns** | **0.2387 ns** | **0.1993 ns** |  **0.94** |    **0.03** |
|  ClearSpan |     Half |  Byte[512] | 12.686 ns | 0.3088 ns | 0.2738 ns |  0.73 |    0.03 |
| **ClearArray** |     **Half** | **Byte[8192]** | **48.642 ns** | **0.8569 ns** | **0.8016 ns** |  **2.80** |    **0.08** |
|  ClearSpan |     Half | Byte[8192] | 46.191 ns | 0.6814 ns | 0.6374 ns |  2.66 |    0.08 |


### Clone vs new [] { }

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1706 (21H1/May2021Update)
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.300
  [Host]     : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT
  DefaultJob : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT


```
|   Method |     Mean |    Error |   StdDev | Ratio | RatioSD |
|--------- |---------:|---------:|---------:|------:|--------:|
| NewArray | 11.31 ns | 0.284 ns | 0.338 ns |  1.00 |    0.00 |
|    Clone | 89.23 ns | 1.764 ns | 1.650 ns |  7.83 |    0.32 |
