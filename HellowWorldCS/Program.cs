using System.Diagnostics;
using static System.Console;
using IronPython.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HellowWorldCS
{

  class Program
  {
    static void Main(string[] args)
    {
      static void Option1_ExecProcess()
      {
        // 1) Create Process Info
        var psi = new ProcessStartInfo();
        psi.FileName = (@"C:\Program Files\Python310\python.exe");

        // 2) Provide Script and arguments
        var script = "../HelloWorlPython/HelloWorld.py";
        // var arg1 = "dummy arg1";
        // var arg2 = "dummy arg2";
        // psi.Arguments = $"\"{script}\"\"{arg1}\"\"{arg2}\"";
        psi.Arguments = $"\"{script}\"";

        // 3) Process Configurations
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;

        // 4) Execute Process and get output
        var errors = "";
        var results = "";

        using (var process = Process.Start(psi))
        {
          errors = process.StandardError.ReadToEnd();
          results = process.StandardOutput.ReadToEnd();
        }

        // 5) Display output
        WriteLine("Errors");
        WriteLine(errors);
        WriteLine();
        WriteLine("Results");
        WriteLine(results);

      }
      static void Option2_IronPython()
      {
        // 1) Create Engine
        var engine = Python.CreateEngine();

        // 2) Provide Script and arguments
        var script = "../HelloWorlPython/HelloWorld.py";
        var source = engine.CreateScriptSourceFromFile(script);
        var argv = new List<string>();
        // var arg1 = "dummy arg1";
        // var arg2 = "dummy arg2";
        // arg0 is script itself
        argv.Add("");
        // argv.Add(arg1);
        // argv.Add(arg2);

        engine.GetSysModule().SetVariable("argv", argv);

        // 3) Output redirect
        var eIO = engine.Runtime.IO;

        var errors = new MemoryStream();
        eIO.SetErrorOutput(errors, Encoding.Default);

        var results = new MemoryStream();
        eIO.SetOutput(results, Encoding.Default);

        // 4) Execute script
        var scope = engine.CreateScope();
        source.Execute(scope);

        // 5) Display output
        string str(byte[] x) => Encoding.Default.GetString(x);

        WriteLine("Errors");
        WriteLine(str(errors.ToArray()));
        WriteLine();
        WriteLine("Results");
        WriteLine(str(results.ToArray()));
      }


      // Option1_ExecProcess();
      Option2_IronPython();
    }

  }

}
