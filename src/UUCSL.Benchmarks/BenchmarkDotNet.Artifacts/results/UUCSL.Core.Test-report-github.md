``` ini

BenchmarkDotNet=v0.11.5, OS=macOS Mojave 10.14.5 (18F132) [Darwin 18.6.0]
Intel Core i7-4770HQ CPU 2.20GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100-preview6-012264
  [Host]     : .NET Core 3.0.0-preview6-27804-01 (CoreCLR 4.700.19.30373, CoreFX 4.700.19.30308), 64bit RyuJIT
  Job-VNZIVZ : .NET Core 3.0.0-preview6-27804-01 (CoreCLR 4.700.19.30373, CoreFX 4.700.19.30308), 64bit RyuJIT
  Core       : .NET Core 3.0.0-preview6-27804-01 (CoreCLR 4.700.19.30373, CoreFX 4.700.19.30308), 64bit RyuJIT

Platform=X64  Runtime=Core  

```
|           Method |     Job |     Toolchain |         Mean |
|----------------- |-------- |-------------- |-------------:|
|             List | Default | .NET Core 3.0 | 113,005.2 us |
| SortedDictionary | Default | .NET Core 3.0 |     629.9 us |
|       SortedList | Default | .NET Core 3.0 |     410.8 us |
|                  |         |               |              |
|             List |    Core |       Default | 114,387.5 us |
| SortedDictionary |    Core |       Default |     658.6 us |
|       SortedList |    Core |       Default |     415.0 us |
