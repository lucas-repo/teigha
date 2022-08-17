using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Teigha.Core;
using Teigha.TD;

namespace MultiThreadedDumper
{
    class Program
    {
        // synchro object
        private static object sync = new object();

        static void Main(string[] args)
        {
            // get memory manager and start first transaction
            MemoryManager man = MemoryManager.GetMemoryManager();
            // when working with a multithreaded application it is recommended to specify thread id
            MemoryTransaction startTr = man.StartTransaction(Thread.CurrentThread.ManagedThreadId);

            // initialize services
            ExSystemServices systemServices = new ExSystemServices();
            ExHostAppServices hostAppServices = new ExHostAppServices();
            hostAppServices.disableOutput(true);
            hostAppServices.setMtMode(1);
            TD_Db.odInitialize(systemServices);

            // obtain files to dump
            string[] dumpFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dwg", SearchOption.TopDirectoryOnly);

            // create dump tasks - the same number as dumpFiles
            Task[] dumpTasks = new Task[dumpFiles.Length];
            for (int ctr = 0; ctr < dumpFiles.Length; ctr++)
            {
                dumpTasks[ctr] = DumpFile(dumpFiles[ctr], hostAppServices);
            }

            Task.WaitAll(dumpTasks);

            // stop transactio
            man.StopTransaction(startTr);
            man.StopAll();
            // perform unitialization
            TD_Db.odUninitialize();
        }
        private static Task<int> DumpFile(string fileName, ExHostAppServices hostApp)
        {
            return Task.Factory.StartNew( () =>
            {
                MemoryTransaction dumpTr = MemoryManager.GetMemoryManager().StartTransaction(Thread.CurrentThread.ManagedThreadId);
                OdDbDatabase dumpDb = null;
                lock (sync)
                {
                    try
                    {
                        dumpDb = hostApp.readFile(fileName);
                    }
                    catch (OdError err)
                    {
                        Console.WriteLine("OdError caught {0}",err.Message);
                    }
                }
                if (null == dumpDb)
                {
                    Console.WriteLine("DWG file {0} does not exist or is incorrect", (string)fileName);
                }
                else
                {
                    // dumping will be performed to a txt file
                    // txt file name is the same as dwg file name, only extension differs
                    string txtDumpFile = (string)fileName;
                    txtDumpFile = txtDumpFile.Substring(0, txtDumpFile.LastIndexOf("."));
                    txtDumpFile += ".txt";
                    //Console.WriteLine("Dump file is {0}", txtDumpFile);
                    StreamWriter txtDump = new StreamWriter(txtDumpFile);
                    DbDumper dumper = new DbDumper(txtDump);
                    dumper.dump(dumpDb);
                }
                MemoryManager.GetMemoryManager().StopTransaction(dumpTr, Thread.CurrentThread.ManagedThreadId);
                return 0;
            });
        }
    }
}
