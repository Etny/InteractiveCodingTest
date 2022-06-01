using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DynamicCheck.Testing;

namespace DynamicCheck.Reflection;
internal class TestMethodInvoker {

    private readonly MethodInfo _method;
    private readonly TestContext _testContext;

    private bool _hasTimeout = true;
    private TimeSpan _timeout = TimeSpan.FromMilliseconds(100);
    private object[] _args = Array.Empty<object>();
    private readonly StringWriter _outHook = new();
    private StringReader _inHook = new("");

    public TestMethodInvoker(MethodInfo method, TestContext testContext)
    {
        _method = method;
        _testContext = testContext; 
    }

    public object Invoke() 
        => _method.IsStatic ? Invoke(null) : Invoke(_testContext.CreateInstance());

    public object Invoke(object instance) {
        var stdIn = Console.In;
        var stdOut = Console.Out;

        Console.SetIn(_inHook);
        Console.SetOut(_outHook);

        try {
            var task = Task.Run(() => _method.Invoke(instance, _args));
            var result = true;

            if(_hasTimeout)
                result = task.Wait(_timeout);
            else
                task.Wait();

            if(result)
                if(task.Exception != null)
                    throw new Exception("Exception in je code: " + task.Exception.Message);
                else
                    return task.Result;
            else 
                throw new Exception("Timed-out. Zit er een infinite loop in je code?");

        } finally {
            Console.SetIn(stdIn);
            Console.SetOut(stdOut);
            _testContext.DebugOutput.AddRange(ReadStdOutHook());
        }   
    }

    private IEnumerable<string> ReadStdOutHook() 
        => _outHook.ToString().Split('\n', 9999, StringSplitOptions.RemoveEmptyEntries);
        

    public TestMethodInvoker WithArgs(params object[] args) {
        _args = args;
        return this;
    }

    public TestMethodInvoker WithStdIn(StringReader inHook) {
        _inHook = inHook;
        return this;
    }

    public TestMethodInvoker WithStdIn(IEnumerable<string> inLines) {
        _inHook = new StringReader(string.Join('\n', inLines));
        return this;
    }

    public TestMethodInvoker WithStdOutHook(out TextWriter outHook)
    {
        outHook = _outHook;
        return this;
    }

    public TestMethodInvoker WithStdOutReader(out Func<IEnumerable<string>> outReader) {
        outReader = () => ReadStdOutHook();
        return this;
    }

    public TestMethodInvoker WithTimeout(TimeSpan timeout) 
    {
        _hasTimeout = true;
        _timeout = timeout;
        return this;
    }

    public TestMethodInvoker WithTimeout(bool timeout) {
        _hasTimeout = timeout;
        return this;
    }
}