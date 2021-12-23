using System;
using System.Runtime.InteropServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

public class Program
{
    public static void Main (string[] args)
    {
        Console.WriteLine("Hello, World!");
        BenchmarkRunner.Run<ArrayVsMemoryPlus1>();
    }
}


[SimpleJob(RuntimeMoniker.Mono), SimpleJob(RuntimeMoniker.Net60)]
public class ArrayVsMemoryPlus1
{
    [Params(64, 512, 2048, 4096, 8192)]
    public int Size { get; set; }
    Memory<byte> memory;

    [GlobalSetup(Target = nameof(MemoryPlus1))]
    public void SetupMemory ()
    {
        memory = new Memory<byte>(new byte[sizeof(int) * Size]);
    }

    [Benchmark]
    public unsafe void MemoryPlus1 ()
    {
        var span = MemoryMarshal.Cast<byte, int>(memory.Span);
        for (int i = 0; i < span.Length; i++)
        {
            span[i] += 1;
        }
    }

    [GlobalCleanup(Target = nameof(MemoryPlus1))]
    public void MemoryCleanup ()
    {
        StringBuilder sb = new StringBuilder();
        var span = MemoryMarshal.Cast<byte, int>(memory.Span);
        for (int i = 0; i < span.Length; i++)
        {
            sb.Append(span[i]);
        }
    }

    [GlobalSetup(Target = nameof(MemoryPlus1Random))]
    public void SetupMemoryRandom ()
    {
        memory = new Memory<byte>(new byte[sizeof(int) * Size]);
    }

    [Benchmark]
    public unsafe void MemoryPlus1Random ()
    {
        var span = MemoryMarshal.Cast<byte, int>(memory.Span);
        Span<int> randSpan = stackalloc int[Size];
        switch (Size)
        {
            case 64:
                new Span<int>(RandSeqRecord.rand64, 0, Size).CopyTo(randSpan);
                break;
            case 512:
                new Span<int>(RandSeqRecord.rand512, 0, Size).CopyTo(randSpan);
                break;
            case 2048:
                new Span<int>(RandSeqRecord.rand2048, 0, Size).CopyTo(randSpan);
                break;
            case 4096:
                new Span<int>(RandSeqRecord.rand4096, 0, Size).CopyTo(randSpan);
                break;
            case 8192:
                new Span<int>(RandSeqRecord.rand8192, 0, Size).CopyTo(randSpan);
                break;
        }
        for (int i = 0; i < span.Length; i++)
        {
            span[randSpan[i]] += 1;
        }
    }

    [GlobalCleanup(Target = nameof(MemoryPlus1Random))]
    public void MemoryCleanupRandom ()
    {
        StringBuilder sb = new StringBuilder();
        var span = MemoryMarshal.Cast<byte, int>(memory.Span);
        for (int i = 0; i < span.Length; i++)
        {
            sb.Append(span[i]);
        }
    }

    int[] array;

    [GlobalSetup(Target = nameof(ArrayPlus1))]
    public void SetupArray ()
    {
        array = new int[Size];
    }

    [Benchmark]
    public void ArrayPlus1 ()
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] += 1;
        }
    }

    [GlobalCleanup(Target = nameof(ArrayPlus1))]
    public void ArrayCleanup ()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < array.Length; i++)
        {
            sb.Append(array[i]);
        }
    }


    [GlobalSetup(Target = nameof(ArrayPlus1Random))]
    public void SetupArrayRandom ()
    {
        array = new int[Size];
    }

    [Benchmark]
    public void ArrayPlus1Random ()
    {
        Span<int> randSpan = stackalloc int[Size];
        switch (Size)
        {
            case 64:
                new Span<int>(RandSeqRecord.rand64, 0, Size).CopyTo(randSpan);
                break;
            case 512:
                new Span<int>(RandSeqRecord.rand512, 0, Size).CopyTo(randSpan);
                break;
            case 2048:
                new Span<int>(RandSeqRecord.rand2048, 0, Size).CopyTo(randSpan);
                break;
            case 4096:
                new Span<int>(RandSeqRecord.rand4096, 0, Size).CopyTo(randSpan);
                break;
            case 8192:
                new Span<int>(RandSeqRecord.rand8192, 0, Size).CopyTo(randSpan);
                break;
        }
        for (int i = 0; i < array.Length; i++)
        {
            array[randSpan[i]] += 1;
        }
    }

    [GlobalCleanup(Target = nameof(ArrayPlus1Random))]
    public void ArrayCleanupRandom ()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < array.Length; i++)
        {
            sb.Append(array[i]);
        }
    }
}