using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;

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
Console.WriteLine(ts);
Console.ReadKey();


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
