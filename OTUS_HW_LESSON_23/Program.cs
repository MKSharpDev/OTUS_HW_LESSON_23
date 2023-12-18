using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

//Описание / Пошаговая инструкция выполнения домашнего задания:
//Прочитать 3 файла параллельно и вычислить количество пробелов в них (через Task).
//Написать функцию, принимающую в качестве аргумента путь к папке.
//Из этой папки параллельно прочитать все файлы и вычислить количество пробелов в них.
//Замерьте время выполнения кода (класс Stopwatch).
//7 баллов: за выполненное 1 - е задание
//+ 2 балла: за выполненное 2 - е задание
//+ 1 балл: за выполненное 3 - е задание
//Минимальное количество баллов: 7


string path = "../";
FileChecker(path);

Stopwatch stopwatch = Stopwatch.StartNew();
stopwatch.Start();
ParallelSearcher(path);
stopwatch.Stop();
TimeSpan ts = stopwatch.Elapsed;
Console.WriteLine("ParallelSearcher");
Console.WriteLine(ts);
Console.WriteLine();

stopwatch.Restart();
ParallelSearcherTaskInOneThread(path);
stopwatch.Stop();
TimeSpan ts2 = stopwatch.Elapsed;
Console.WriteLine("ParallelSearcherTaskInOneThread");
Console.WriteLine(ts2);
Console.WriteLine();

stopwatch.Restart();
ParallelSearcherTaskFactory(path);
stopwatch.Stop();
TimeSpan ts1 = stopwatch.Elapsed;
Console.WriteLine("ParallelSearcherTaskFactory");
Console.WriteLine(ts1);
Console.WriteLine();

stopwatch.Restart();
ParallelSearcherTask(path);
stopwatch.Stop();
TimeSpan ts3 = stopwatch.Elapsed;
Console.WriteLine("ParallelSearcherTask");
Console.WriteLine(ts3);
Console.WriteLine();



Console.ReadKey();

//Поток 5 поиск закончил - 145 пробелов
//Поток 1 поиск закончил - 160 пробелов
//Поток 8 поиск закончил - 116 пробелов
//ParallelSearcher
//00:00:00.0521862

//Поток 5 поиск закончил - 116 пробелов
//Поток 5 поиск закончил - 160 пробелов
//Поток 5 поиск закончил - 145 пробелов
//ParallelSearcherTaskInOneThread
//00:00:00.0026191

//Поток 8 поиск закончил - 160 пробелов
//Поток 5 поиск закончил - 116 пробелов
//Поток 9 поиск закончил - 145 пробелов
//ParallelSearcherTaskFactory
//00:00:00.0027041

//Поток 8 поиск закончил - 145 пробелов
//Поток 5 поиск закончил - 160 пробелов
//Поток 3 поиск закончил - 116 пробелов
//ParallelSearcherTask
//00:00:00.0014381



void FileChecker(string path)
{
    string[] allfiles = Directory.GetFiles(path);

    if (!allfiles.Any())
    {
        for (int i = 1; i < 4; i++)
        {
            string newText = StringGenerator(5000);
            string newPath = $"{path}NewStringFile{i}.txt";
            File.WriteAllText(newPath, newText);
        }
    }
}

async void ParallelSearcher(string searchList)
{
    string[] allfiles = Directory.GetFiles(path);

    Parallel.ForEach(allfiles, async toSearch =>
        {
            string text = File.ReadAllText(toSearch);
            int count = Searcher(text);
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} поиск закончил - {count} пробелов");
        }
    );
}
async void ParallelSearcherTaskFactory(string searchList)
{
    string[] allfiles = Directory.GetFiles(path);
    var parallelTask = new List<Task>();

    foreach (var item in allfiles)
    {
        Task newTask = Task.Factory.StartNew(() =>
        {
            string text = File.ReadAllText(item);
            int count = Searcher(text);
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} поиск закончил - {count} пробелов");
        });
        parallelTask.Add(newTask);
    }
    await Task.WhenAll(parallelTask);
}

async void ParallelSearcherTaskInOneThread(string searchList)
{
    string[] allfiles = Directory.GetFiles(path);

    foreach (var item in allfiles)
    {
        Task tsk = new Task(() =>
        {
            string text = File.ReadAllText(item);
            int count = Searcher(text);
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} поиск закончил - {count} пробелов");
        });
        tsk.Start();
        tsk.Wait();
    }
}

async void ParallelSearcherTask(string searchList)
{
    string[] allfiles = Directory.GetFiles(path);
    var parallelTask = new List<Task>();


    foreach (var item in allfiles)
    {
        Task tsk =  new Task(async () =>
        {
            string text = File.ReadAllText(item);
            int count = Searcher(text);
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} поиск закончил - {count} пробелов");
        });
        parallelTask.Add(tsk);
    }
    parallelTask.ForEach(t => t.Start());
    Task.WaitAll(Task.WhenAll(parallelTask));
}



int Searcher(string toSearch)
{
    int counter = 0;
    for (int i = 0; i < toSearch.Length; i++)
    {
        if (toSearch[i] == ' ')
        {
            counter++;
        }
    }

    return counter;
}

string StringGenerator(int length)
{
    Random random = new Random();
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
    return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[random.Next(s.Length)]).ToArray());
}

