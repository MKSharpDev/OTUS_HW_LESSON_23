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


List<string> searchList = FileReaderByPath("../");

Stopwatch stopwatch = Stopwatch.StartNew();
stopwatch.Start();
ParallelSearcher(searchList);
stopwatch.Stop();
TimeSpan ts = stopwatch.Elapsed;
Console.WriteLine(ts);
Console.ReadKey();


List<string> FileReaderByPath(string path)
{
    string[] allfiles = Directory.GetFiles(path);

    if (!allfiles.Any())
    {
        for (int i = 1; i < 4; i++)
        {
            string newText = StringGenerator(500);
            string newPath = $"{path}NewStringFile{i}.txt";
            File.WriteAllText(newPath, newText);
        }
        allfiles = Directory.GetFiles(path);
    }

    List<string> searchList = new List<string>();

    foreach (string file in allfiles)
    {
        string text = File.ReadAllText(file);
        searchList.Add(text);
    }
    
    return searchList;
}

async void ParallelSearcher(List<string> searchList)
{
    Parallel.ForEach(searchList, async toSearch =>
        {
            int count = await Searcher(toSearch);
            Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} поиск закончил - {count} пробелов");
        }
    );
}


async Task<int> Searcher(string toSearch)
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
    const string chars = " ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[random.Next(s.Length)]).ToArray());
}
